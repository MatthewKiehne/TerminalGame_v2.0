using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ExtensionConnection {

    private string name;
    private int state;

    /// <summary>
    /// Gets called after the state has been set
    /// </summary>
    public event Action<int> OnSetState;

    public ExtensionConnection(string name) {
        this.name = name;
    }

    public abstract void clearConnections();

    /// <summary>
    /// Sets the state of the Bridge to Zero
    /// </summary>
    public void clearState() {
        this.State = 0;
    }

    public int State {
        get {
            return this.state;
        } 
        set {
            this.state = value;
            this.OnSetState?.Invoke(this.state);
        } 
    }
    
    public string Name {
        get {
            return this.name;
        } set {
            this.name = value;
        }
    }
}