using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal
{

    private string name;
    private Clock clock;

    /// <summary>
    /// Holds all the TExtensions in this Terminal
    /// </summary>
    private List<TExtension> extensions = new List<TExtension>();

    /// <summary>
    /// holds all the connections between connections this Terminal contains
    /// </summary>
    public List<ExtensionConnection> interiorConnections;

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

    /// <summary>
    /// Gets called after an ExtensionConnection has been added to InteriorConnections
    /// </summary>
    public event Action OnConnectionAdd;

    /// <summary>
    /// Called after an ExtensionConnection ahs been removed from InteriorConnections
    /// </summary>
    public event Action OnConnectionRemove;

    public Terminal(string name) {
        this.name = name;
        this.interiorConnections = new List<ExtensionConnection>();
        this.clock = new Clock(1f);
    }

    public Terminal(string name, Clock clock) {
        this.name = name;
        this.clock = clock;
        this.interiorConnections = new List<ExtensionConnection>();
    }

    public TExtension findExtension(string name) {
        return this.extensions.Find(x => x.Name.Equals(name));
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

        /*
        //disconnect 
        foreach (SendBridge send in extension.SendBridges) {
            send.clearConnections();
        }
        foreach (ReceiveBridge rec in extension.ReceiveBridges) {
            rec.clearConnections();
        }
        */

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

    /// <summary>
    /// Returns all the ExtensionConnections between TExtensions within this Terminal
    /// </summary>
    public ExtensionConnection[] InteriorConnections() {
        return this.interiorConnections.ToArray();
    }

    /// <summary>
    /// Add an ExtensionConnection to InteriorConnections
    /// </summary>
    public bool addConnection(ExtensionConnection connection) {

        bool result = false;
        TExtension fromExtension = Array.Find(this.Extensions, extension => extension.Equals(connection.FromExtension));
        TExtension toExtension = Array.Find(this.Extensions, extension => extension.Equals(connection.ToExtension));


        if (connection.FromTerminal.Equals(this) &&
            fromExtension != null &&
            Array.Exists(fromExtension.SendNodes, bridge => bridge.Equals(connection.FromNode)) &&

            connection.ToTerminal.Equals(this) &&
            toExtension != null &&
            Array.Exists(toExtension.ReceiveNodes, bridge => bridge.Equals(connection.ToNode))) {

            this.interiorConnections.Add(connection);
            result = true;
            this.OnConnectionAdd?.Invoke();
        }

        return result;
    }

    /// <summary>
    /// Removes the ExtensionConnection from InteriorConnections
    /// </summary>
    public bool removeConnection(ExtensionConnection connection) {

        ExtensionConnection found = this.interiorConnections.Find(con => con.Equals(connection));

        if(found != null) {
            found.destroy();
            this.interiorConnections.Remove(found);
            this.OnConnectionRemove?.Invoke();
        }

        return found != null;
    }

    public Clock Clock {
        get { return this.clock; }
        set { this.clock = value; }
    }
    public string Name {
        get { return this.name; }
    }
}
