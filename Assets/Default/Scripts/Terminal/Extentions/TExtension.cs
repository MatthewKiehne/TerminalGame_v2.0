using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TExtension {

    private List<ReceiveBridge> receiveBridges = new List<ReceiveBridge>();
    private List<SendBridge> sendBridges = new List<SendBridge>();

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
    public List<ExtensionConnection> AllBridges() {

        List<ExtensionConnection> result = new List<ExtensionConnection>();

        result.AddRange(this.sendBridges);
        result.AddRange(this.receiveBridges);

        return result;
    }
    
    public void addReceiveBridge(ReceiveBridge rec) {
        this.receiveBridges.Add(rec);
        this.OnReceiveBridgeAdd?.Invoke();
    }

    public bool removeReceiveBridge(ReceiveBridge rec) {
        bool result = this.receiveBridges.Remove(rec);
        if (result) {
            this.OnReceiveBridgeRemove?.Invoke();
        }
        return result;
    }

    public void addSendBridge(SendBridge send) {
        this.sendBridges.Add(send);
        this.OnSendBridgeAdd?.Invoke();
    }

    public bool removeSendBridge(SendBridge send) {
        bool result = this.sendBridges.Remove(send);
        if (result) {
            this.OnSendBridgeRemove?.Invoke();
        }
        return result;
    }

    public ReceiveBridge[] ReceiveBridges {
        get {
            return this.receiveBridges.ToArray();
        }
    }

    public SendBridge[] SendBridges {
        get {
            return this.sendBridges.ToArray();
        }
    }

    public string Name {
        get {
            return this.name;
        }
    }
}
