using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NorGate : LogicComponent {

    public NorGate(Vector2Int position, int rotation, bool flipped) : base(position, new Vector2Int(2,2), rotation, flipped, 1, 2) {

    }

    public override void setState() {
        
        if(!this.getReceiverAt(0).getActive() && !this.getReceiverAt(1).getActive()) {
            this.state = 1;
        } else {
            this.state = 0;
        }
    }
}
