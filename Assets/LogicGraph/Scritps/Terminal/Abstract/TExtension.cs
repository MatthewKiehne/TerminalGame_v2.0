using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TExtension : Item {

    protected List<ReceiveBridge> receiveBridges = new List<ReceiveBridge>();
    protected List<SendBridge> sendBridges = new List<SendBridge>();

    public TExtension(string name) : base(name) {
    }

    //sets the state of the TExtension from the input from the TSignalReceive and then should clear the TSignalReceive
    public abstract void setState();

    //this sends the signal from the TSignalSend to all the TSignalReceives for the next round
    public abstract void sendSignal();

    //clears the receivers of all data they have received
    public abstract void clearReceivers();
        

    public List<ReceiveBridge> ReceiveBridges {
        get {
            return this.receiveBridges;
        }
    }

    public List<SendBridge> SendBridges {
        get {
            return this.sendBridges;
        }
    }
}
