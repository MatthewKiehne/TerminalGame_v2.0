using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TInput : TExtension {

    private int state = 0;

    public TInput(string name) : base(name) {

        this.addSendBridge(new SendBridge("Keyboard Output"));
    }

    public override void clearReceivers() {

        for (int i = 0; i < this.ReceiveBridges.Length; i++) {
            this.ReceiveBridges[i].clearState();
        }
    }

    public override void sendSignal() {

        foreach(SendBridge send in this.SendBridges) {
            send.sendData(this.state);
        }
        
    }

    public override void setState() {

        KeyCode chosenKey = KeyCode.H;

        if (Input.GetKey(chosenKey)) {
            this.state = 1;
        } else {
            state = 0;
        }
    }
}