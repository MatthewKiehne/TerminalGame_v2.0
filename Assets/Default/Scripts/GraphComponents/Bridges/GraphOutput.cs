using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphOutput : LinkComponent {

    private SendBridge sendBridge;

    public GraphOutput(Vector2Int position, int rotation, bool flipped, SendBridge sendBridge) :
        base(position, new Vector2Int(BridgeComponent.BRIDGELENGTH, 1), rotation, flipped, 0, BridgeComponent.BRIDGELENGTH) {

        this.sendBridge = sendBridge;
    }

    public override void setValues(List<Tuple> values) {
        //sets the receivers to the correct value

        //this.sendData(int.Parse(values[0].Value));
        //this.Name = values[1].Value;
    }

    public override string getName() {
        return this.sendBridge.Name;
    }

    /// <summary>
    /// Sends the data of this component's receivers to the the SendBridge
    /// </summary>
    public void sendData(int data) {
        this.sendBridge.sendData(data);
    }

    public override ExtensionConnection getExtensionConnection() {
        return this.sendBridge;
    }
}