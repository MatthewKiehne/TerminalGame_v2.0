using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NandGate : LogicComponent
{
    public NandGate(Vector2Int position, int rotation, bool flipped) : base(position, new Vector2Int(2,2), rotation, flipped, 1, 2) {

    }

    public override void setState() {
        
        if(this.getReceiverAt(0).getActive() && this.getReceiverAt(1).getActive()) {
            this.state = 0;
        } else {
            this.state = 1;
        }
    }
}
