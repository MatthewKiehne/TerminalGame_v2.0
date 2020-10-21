using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TInput : TExtension {

    private int state = 0;

    public TInput(string name) : base(name) {

        this.addSendBridge(new ExtensionNode("Keyboard Output",ExtensionNode.ExtensionState.SEND));
    }

    public override void clearReceivers() {

        for (int i = 0; i < this.ReceiveNodes.Length; i++) {
            this.ReceiveNodes[i].clearValue();
        }
    }

    public override void sendSignal() {

        foreach(ExtensionNode send in this.SendNodes) {
            send.Value = this.state;
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