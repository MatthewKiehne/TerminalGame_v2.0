using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveBridge : ExtensionConnection
{

    private List<SendBridge> sendBridges = new List<SendBridge>();

    public ReceiveBridge(string name) : base(name) {
    }

    public void setState(int signal) {
        //updates the state and sends the signal


        int checkState = this.State | signal;

        if (checkState != this.State) {

            this.State = checkState;
        }
    }

    public override void clearConnections() {
        //clears all the connections

        foreach (SendBridge output in this.sendBridges) {
            bool removed = output.ReceiveBridges.Remove(this);
        }
        this.sendBridges.Clear();
    }

    public List<SendBridge> SendBridges {
        get {
            return this.sendBridges;
        }
    }
}
