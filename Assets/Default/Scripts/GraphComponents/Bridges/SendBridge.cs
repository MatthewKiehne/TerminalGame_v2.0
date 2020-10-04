using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendBridge : BridgeComponent {

    private List<ReceiveBridge> receiveBridges = new List<ReceiveBridge>();

    public SendBridge(Vector2Int position, int rotation, bool flipped) : 
        base(position, new Vector2Int(BridgeComponent.BRIDGELENGTH, 1), rotation, flipped, 0, BridgeComponent.BRIDGELENGTH) {
    }

    public SendBridge(Vector2Int position, int rotation, bool flipped, string name) :
        base(position, new Vector2Int(BridgeComponent.BRIDGELENGTH, 1), rotation, flipped, 0, BridgeComponent.BRIDGELENGTH, name) {
    }

    public void sendData(int state) {
        //sends the state to all the receivers

        this.state = state | this.state;
        for (int i = 0; i < receiveBridges.Count; i++) {
            receiveBridges[i].setState(state);
        }
    }

    public override void setValues(List<Tuple> values) {
        //sets the receivers to the correct value

        this.sendData(int.Parse(values[0].Value));
        this.Name = values[1].Value;
    }

    public override void clearConnections() {
        //clears all the connections to this bridge

        foreach (ReceiveBridge rec in this.receiveBridges) {
            bool removed = rec.SendBridges.Remove(this);
            Debug.Log("removed " + rec + " " + removed);
        }
        this.receiveBridges.Clear();
    }

    public List<ReceiveBridge> ReceiveBridges {
        get {
            return this.receiveBridges;
        }
    }

}
