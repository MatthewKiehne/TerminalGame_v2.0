using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphOutput : LinkComponent {

    private ExtensionNode sendNode;

    public GraphOutput(Vector2Int position, int rotation, bool flipped, ExtensionNode sendBridge) :
        base(position, new Vector2Int(LinkComponent.BRIDGELENGTH, 1), rotation, flipped, 0, LinkComponent.BRIDGELENGTH) {

        this.sendNode = sendBridge;
    }

    public override void setValues(List<Tuple> values) {
        //sets the receivers to the correct value

        this.sendData(int.Parse(values[0].Value));
        this.sendNode.Name = values[1].Value;
    }

    public override string getName() {
        return this.sendNode.Name;
    }

    /// <summary>
    /// Sends the data of this component's receivers to the the SendBridge
    /// </summary>
    public void sendData(int data) {
        this.sendNode.Value = data;
    }

    public override ExtensionNode getExtensionConnection() {
        return this.sendNode;
    }
}