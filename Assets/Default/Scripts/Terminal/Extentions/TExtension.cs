using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TExtension {

    private List<ExtensionNode> receiveNodes = new List<ExtensionNode>();
    private List<ExtensionNode> sendNodes = new List<ExtensionNode>();

    private string name;

    public event Action OnReceiveBridgeAdd;
    public event Action OnReceiveBridgeRemove;

    public event Action OnSendBridgeAdd;
    public event Action OnSendBridgeRemove;


    public TExtension(string name) {
        this.name = name;
    }

    //sets the state of the TExtension from the input from the TSignalReceive and then should clear the TSignalReceive
    public abstract void setState();

    //this sends the signal from the TSignalSend to all the TSignalReceives for the next round
    public abstract void sendSignal();

    //clears the receivers of all data they have received
    public abstract void clearReceivers();

    /// <summary>
    /// Returns a list of all the ExtensionConnections
    /// </summary>
    public ExtensionNode[] AllBridges() {

        List<ExtensionNode> result = new List<ExtensionNode>();

        result.AddRange(this.sendNodes);
        result.AddRange(this.receiveNodes);

        return result.ToArray();
    }
    
    public void addReceiveBridge(ExtensionNode rec) {
        this.receiveNodes.Add(rec);
        this.OnReceiveBridgeAdd?.Invoke();
    }

    public bool removeReceiveBridge(ExtensionNode rec) {
        bool result = this.receiveNodes.Remove(rec);
        if (result) {
            this.OnReceiveBridgeRemove?.Invoke();
        }
        return result;
    }

    public void addSendBridge(ExtensionNode send) {
        this.sendNodes.Add(send);
        this.OnSendBridgeAdd?.Invoke();
    }

    public bool removeSendBridge(ExtensionNode send) {
        bool result = this.sendNodes.Remove(send);
        if (result) {
            this.OnSendBridgeRemove?.Invoke();
        }
        return result;
    }

    public ExtensionNode[] ReceiveNodes {
        get { return this.receiveNodes.ToArray(); }
    }

    public ExtensionNode[] SendNodes {
        get { return this.receiveNodes.ToArray(); }
    }

    public string Name {
        get { return this.name; }
    }
}
