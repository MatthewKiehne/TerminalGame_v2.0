using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendBridge : BridgeComponent {

    private List<ReceiveBridge> receiveBridges = new List<ReceiveBridge>();

    public SendBridge(Vector2Int position, int rotation, bool flipped, int length) : 
        base(position, new Vector2Int(length, 1), rotation, flipped, 0, length) {

        this.length = length;
    }

    public SendBridge(Vector2Int position, int rotation, bool flipped, int length, string name) :
        base(position, new Vector2Int(length, 1), rotation, flipped, 0, length, name) {

        this.length = length;
    }

    public void sendData(int state) {
        //sends the state to all the receivers

        this.state = state;
        for(int i = 0; i < receiveBridges.Count; i++) {
            receiveBridges[i].setState(state);
        }
    }

    public void addBridge(ReceiveBridge bridge) {
        this.receiveBridges.Add(bridge);
    }

    public ReceiveBridge receiveBridgeGetAt(int index) {
        return this.receiveBridges[index];
    }

    public int receiveBridgeCount() {
        return this.receiveBridges.Count;
    }
}
