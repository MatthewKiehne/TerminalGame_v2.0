using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveBridge : BridgeComponent {

    public ReceiveBridge(Vector2Int position, int rotation, bool flipped, int length) : 
        base(position, new Vector2Int(length,1), rotation, flipped, length, 0) {

        this.length = length;
    }

    public ReceiveBridge(Vector2Int position, int rotation, bool flipped, int length, string name) : 
        base(position, new Vector2Int(length, 1), rotation, flipped, length, 0, name) {

        this.length = length;
    }

    public void setState(int signal) {
        //updates the state and sends the signal


        int checkState = this.state | signal;

        //Debug.Log("ReceiveBridge - setState - check state:" + checkState);

        if(checkState > this.state) {

            this.state = checkState;

            for (int i = 0; i < this.senders.Count; i++) {

                if (((this.state >> i) & 1) == 1) {
                    this.senders[i].setTargetsActive();
                }
            }
        }
    }

    public void clearState() {
        this.state = 0;
    }
}
