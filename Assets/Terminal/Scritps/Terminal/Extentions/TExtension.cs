using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TExtension : GraphComponent {

    protected List<ReceiveBridge> receiveBridges = new List<ReceiveBridge>();
    protected List<SendBridge> sendBridges = new List<SendBridge>();

    private string name;

    public TExtension(string name, Vector2Int position) : base(position, new Vector2Int(5, 5), 0, false) {
        this.name = name;
    }

    //sets the state of the TExtension from the input from the TSignalReceive and then should clear the TSignalReceive
    public abstract void setState();

    //this sends the signal from the TSignalSend to all the TSignalReceives for the next round
    public abstract void sendSignal();

    //clears the receivers of all data they have received
    public abstract void clearReceivers();

    public List<BridgeComponent> AllBridges() {

        List<BridgeComponent> result = new List<BridgeComponent>();

        result.AddRange(this.sendBridges);
        result.AddRange(this.receiveBridges);

        return result;
    }     

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

    public string Name {
        get {
            return this.name;
        }
    }
}
