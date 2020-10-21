using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtensionNode {

    private string name;
    private int value;
    private ExtensionState state;

    public enum ExtensionState { SEND, RECEIVE }

    /// <summary>
    /// Gets called after the state has been set
    /// </summary>
    public event Action<int> OnSetValue;

    public ExtensionNode(string name, ExtensionState state) {
        this.name = name;
        this.state = state;
    }

    /// <summary>
    /// Sets the state of the Bridge to Zero
    /// </summary>
    public void clearValue() {
        this.value = 0;
    }

    public int Value {
        get { return this.value; } 
        set {
            this.value = value;
            this.OnSetValue?.Invoke(this.value);
        } 
    }
    
    public string Name {
        get { return this.name; } 
        set { this.name = value; }
    }

    public ExtensionState State {
        get { return this.state; }
    }
}