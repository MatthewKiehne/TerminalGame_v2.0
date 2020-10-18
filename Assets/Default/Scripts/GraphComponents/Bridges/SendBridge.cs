using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendBridge : ExtensionConnection
{

    private List<ReceiveBridge> receiveBridges = new List<ReceiveBridge>();

    public SendBridge(string name) : base (name) {

    }

    public void sendData(int state) {
        //sends the state to all the receivers

        this.State = state | this.State;
        for (int i = 0; i < receiveBridges.Count; i++) {
            receiveBridges[i].setState(state);
        }
    }

    public override void clearConnections() {
        //clears all the connections to this bridge

        foreach (ReceiveBridge rec in this.receiveBridges) {
            bool removed = rec.SendBridges.Remove(this);
        }
        this.receiveBridges.Clear();
    }

    public List<ReceiveBridge> ReceiveBridges {
        get {
            return this.receiveBridges;
        }
    }
}
