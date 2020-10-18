using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BReceiver : Receiver {

    private GraphOutput graphOutput;
    private int stateNumber;

    public BReceiver(Rect rect, GraphOutput output, int stateNumber) : base(rect) {
        this.stateNumber = stateNumber;
        this.graphOutput = output;
    }

    public override bool getActive() {
        return true;
    }

    public override void setActive(bool active) {
        //sends this receiver's value to the receiver as soon as it is activated

        if (active) {
            this.graphOutput.sendData(this.stateNumber);
        }
    }
}
