using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal {

    private string name;
    private Clock clock;

    private List<TExtension> extensions = new List<TExtension>();

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

    public int updateTime(float timePassed) {
        //updates the extensions based on the number of periods the clock has passed 

        int numUpdates = 0;

        if (this.clock != null) {

            numUpdates = this.clock.updateTime(timePassed);
            for (int i = 0; i < numUpdates; i++) {
                this.updateExtensions();
            }
        }
        return numUpdates;
    }

    private void updateExtensions() {
        //updates the terminal and all of the TExtensions in it

        //Debug.Log("Terminal Update Extensions " + this.Name);

        //sets the state of each extension then clears the extensions
        for (int i = 0; i < this.extensions.Count; i++) {
            this.extensions[i].setState();
            this.extensions[i].clearReceivers();

            //Debug.Log("extension Update: " + this.extensions[i].Name);
        }

        //sends the signal to the extensions
        for (int i = 0; i < this.extensions.Count; i++) {
            this.extensions[i].sendSignal();
        }
    }

    #region GraphInteger
    public bool addComponent(TExtension component) {
        this.extensions.Add(component);
        return true;
    }

    public bool removeComponent(TExtension component) {

        bool result = true;

        //disconnect 
        foreach(SendBridge send in component.SendBridges) {
            send.clearConnections();
        }
        foreach(ReceiveBridge rec in component.ReceiveBridges) {
            rec.clearConnections();
        }

        result = this.extensions.Remove(component);

        return result;
    }

    public TExtension getComponentAt(int x, int y) {
        throw new System.NotImplementedException();
    }

    public List<TExtension> getAllGraphComponents() {
        throw new System.NotImplementedException();
    }
    #endregion

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

    public void addExtension(TExtension extension) {
        this.extensions.Add(extension);
    }

    public bool removeExtension(TExtension extension) {
        //removes the connections and then removes the component
        bool result = this.extensions.Remove(extension);
        if (result) {
            foreach (SendBridge send in extension.SendBridges) {
                send.clearConnections();
            }
            foreach (ReceiveBridge rec in extension.ReceiveBridges) {
                rec.clearConnections();
            }
        }
        return result;
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
}
