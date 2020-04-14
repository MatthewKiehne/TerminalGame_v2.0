﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotGate : LogicComponent {

    public NotGate(Vector2Int position, int rotation, bool flipped) : base(position, new Vector2Int(1,1), rotation,flipped,1,1) { 
    }

    public override void setState() {

        if (this.getReceiverAt(0).getActive()) {
            this.state = 0;
        } else {
            this.state = 1;
        }
    }
}
