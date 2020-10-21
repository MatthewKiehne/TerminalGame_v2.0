using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtensionConnection {

    private Terminal fromTerminal;
    private TExtension fromExtension;
    private ExtensionNode fromNode;
    private Terminal toTerminal;
    private TExtension toExtension;
    private ExtensionNode toNode;

    public ExtensionConnection (Terminal fromTerminal, TExtension fromExtension, ExtensionNode fromNode,
                                Terminal toTerminal, TExtension toExtension, ExtensionNode toNode) {

        this.fromTerminal = fromTerminal;
        this.fromExtension = fromExtension;
        this.fromNode = fromNode;
        this.toTerminal = toTerminal;
        this.toExtension = toExtension;
        this.toNode = toNode;

        if(fromNode.State == ExtensionNode.ExtensionState.RECEIVE) {
            throw new System.Exception("fromNode can not have a state of Receive");
        }

        if(toNode.State == ExtensionNode.ExtensionState.SEND) {
            throw new System.Exception("toNode can not have a state of Send");
        }

        this.fromNode.OnSetValue += updateState;
    }

    private void updateState(int value) {
        this.toNode.Value = value;
    }

    public void destroy() {
        this.fromNode.OnSetValue -= updateState;
    }

    //getter for each value
    public Terminal FromTerminal {
        get { return this.fromTerminal; }
    }
    public TExtension FromExtension {
        get { return this.FromExtension;  }
    }
    public ExtensionNode FromNode {
        get { return this.fromNode;  }
    }
    public Terminal ToTerminal {
        get { return this.ToTerminal; }
    }
    public TExtension ToExtension {
        get { return this.toExtension; }
    }
    public ExtensionNode ToNode{
        get{ return this.toNode; }
    }
}