using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LReceiver : Receiver {

    private bool active = false;


    public LReceiver(Rect rect) : base(rect) {
    }

    public override bool getActive() {
        return this.active;
    }

    public override void setActive(bool active) {
        this.active = active;
    }
}
