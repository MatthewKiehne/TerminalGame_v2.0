using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal {

    private string name;
    private Clock clock;

    private List<TExtension> extensions = new List<TExtension>();

    /// <summary>
    /// Gets called after an Extension has been added to the Terminal
    /// </summary>
    public event Action OnExtensionAdd;

    /// <summary>
    /// Gets called after an Extension has been removed to the Terminal
    /// </summary>
    public event Action OnExtensionRemove;

    /// <summary>
    /// Called after the Terminal has finished an updating all the Extensions
    /// </summary>
    public event Action OnTerminalUpdate;

    public Terminal(string name) {
        this.name = name;
    }

    public Terminal(string name, Clock clock) {
        this.name = name;
        this.clock = clock;
    }

    public TExtension findExtension(string name) {
        return this.extensions.Find(x => x.Name.Equals(name));
    }

    public Terminal(TerminalData data) {
        this.name = data.Name;
    }

    /// <summary>
    /// Updates the extensions based on the number of periods the clock has passed 
    /// </summary>
    public int updateTime(float timePassed) {

        int numUpdates = 0;

        if (this.clock != null) {

            numUpdates = this.clock.updateTime(timePassed);
            for (int i = 0; i < numUpdates; i++) {
                this.updateExtensions();
            }
        }
        return numUpdates;
    }

    /// <summary>
    /// Updates all the Extensions in the Terminal
    /// </summary>
    private void updateExtensions() {

        //sets the state of each extension then clears the extensions
        for (int i = 0; i < this.extensions.Count; i++) {
            this.extensions[i].setState();
            this.extensions[i].clearReceivers();
        }

        //sends the signal to the extensions
        for (int i = 0; i < this.extensions.Count; i++) {
            this.extensions[i].sendSignal();
        }

        this.OnTerminalUpdate?.Invoke();
    }

    /// <summary>
    /// Removes an Extension from the Terminal
    /// </summary>
    public bool removeExtension(TExtension extension) {

        bool result = true;

        //disconnect 
        foreach(SendBridge send in extension.SendBridges) {
            send.clearConnections();
        }
        foreach(ReceiveBridge rec in extension.ReceiveBridges) {
            rec.clearConnections();
        }

        result = this.extensions.Remove(extension);

        this.OnExtensionRemove?.Invoke();

        return result;
    }

    /// <summary>
    /// Adds the extension to the Terminal
    /// </summary>
    public void addExtension(TExtension extension) {
        this.extensions.Add(extension);
        this.OnExtensionAdd?.Invoke();
    }

    public TExtension extensionAt(int index) {
        return this.extensions[index];
    }

    public int extensionLength() {
        return this.extensions.Count;
    }

    public int extensionIndexOf(TExtension extension) {
        return this.extensions.IndexOf(extension);
    }

    public TExtension[] Extensions {
        get {
            return this.extensions.ToArray();
        }
    }

    public Clock Clock {
        get {
            return this.clock;
        }
        set {
            this.clock = value;
        }
    }
    public string Name {
        get {
            return this.name;
        }
    }
}
