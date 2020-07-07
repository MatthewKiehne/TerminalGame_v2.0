using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BReceiver : Receiver {

    private SendBridge sendBridge;
    private int stateNumber;

    public BReceiver(Rect rect, SendBridge sendBridge, int stateNumber) : base(rect) {
        this.stateNumber = stateNumber;
        this.sendBridge = sendBridge;
    }

    public override bool getActive() {
        return true;
    }

    public override void setActive(bool active) {
        //sends this receiver's value to the receiver as soon as it is activated

        if (active) {
            this.sendBridge.sendData(this.stateNumber);
        }
    }
}
