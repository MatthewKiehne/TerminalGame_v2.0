using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphInput : LinkComponent
{

    private ReceiveBridge reveivedBridge;

    public GraphInput(Vector2Int position, int rotation, bool flipped, ReceiveBridge receiveBridge) :
        base(position, new Vector2Int(LinkComponent.BRIDGELENGTH, 1), rotation, flipped, LinkComponent.BRIDGELENGTH, 0) {

        this.reveivedBridge = receiveBridge;
        this.reveivedBridge.OnSetState += this.onStateChange;
    }

    public override void setValues(List<Tuple> values) {
        //sets the receivers to the correct value

        this.reveivedBridge.setState(int.Parse(values[0].Value));
        this.reveivedBridge.Name = values[1].Value;
    }

    /// <summary>
    /// this function get passed to ReceiveBridge OnStateChange
    /// it will update all the sneders
    /// </summary>
    public void onStateChange(int newState) {

        if (newState != 0) {
            for (int i = 0; i < this.senders.Count; i++) {

                int index = (this.senders.Count - 1 - i);

                if (((newState >> index) & 1) == 1) {
                    this.senders[i].setTargetsActive();
                }

            }
        }
    }

    public override string getName() {
        return this.reveivedBridge.Name;
    }

    public override ExtensionConnection getExtensionConnection() {
        return this.reveivedBridge;
    }
}