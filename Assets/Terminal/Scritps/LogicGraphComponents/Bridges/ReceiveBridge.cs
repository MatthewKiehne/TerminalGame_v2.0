using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveBridge : BridgeComponent {

    private List<SendBridge> sendBridges = new List<SendBridge>();

    public ReceiveBridge(Vector2Int position, int rotation, bool flipped) : 
        base(position, new Vector2Int(BridgeComponent.BRIDGELENGTH,1), rotation, flipped, BridgeComponent.BRIDGELENGTH, 0) {
;
    }

    public ReceiveBridge(Vector2Int position, int rotation, bool flipped, string name) : 
        base(position, new Vector2Int(BridgeComponent.BRIDGELENGTH, 1), rotation, flipped, BridgeComponent.BRIDGELENGTH, 0, name) {
    }

    public void setState(int signal) {
        //updates the state and sends the signal


        int checkState = this.state | signal;

        //Debug.Log("ReceiveBridge - setState - check state:" + checkState);

        if(checkState != this.state) {

            this.state = checkState;

            for (int i = 0; i < this.senders.Count; i++) {

                int index = (this.senders.Count - 1 - i);

                if (((this.state >> index) & 1) == 1) {
                    this.senders[i].setTargetsActive();
                }
            }
        }
    }

    public override void setValues(List<Tuple> values) {
        //sets the receivers to the correct value

        this.setState(int.Parse(values[0].Value));
        this.Name = values[1].Value;
    }

    public void clearState() {
        this.state = 0;
    }

    public override void clearConnections() {
        //clears all the connections

        foreach(SendBridge send in this.sendBridges) {
            bool removed = send.ReceiveBridges.Remove(this);
            Debug.Log("removed " + send + " " + removed);
        }
        this.sendBridges.Clear();
    }

    public List<SendBridge> SendBridges {
        get {
            return this.sendBridges;
        }
    }
}
