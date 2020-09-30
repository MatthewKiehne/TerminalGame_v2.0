using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TInput : TExtension {

    private int state = 0;

    public TInput(string name) : base(name) {

        this.sendBridges.Add(new SendBridge(Vector2Int.zero, 0, false, "Keyboard Output"));
    }

    public override void clearReceivers() {

        for (int i = 0; i < this.receiveBridges.Count; i++) {
            this.receiveBridges[i].clearState();
        }
    }

    public override void sendSignal() {

        foreach(SendBridge send in this.sendBridges) {
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