using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TConstant : TExtension {

    private int number;

    public TConstant( int number) : base("Constant: " + number) {
        this.number = number;

        this.sendBridges.Add(new SendBridge(Vector2Int.zero, 0, false, 1, "Constant Output"));
    }

    public override void clearReceivers() {
        for (int i = 0; i < this.ReceiveBridges.Count; i++) {
            this.receiveBridges[i].clearState();
        }
    }

    public override void sendSignal() {

        for (int i = 0; i < this.sendBridges.Count; i++) {
            this.sendBridges[i].sendData(this.number);
        }
    }

    public override void setState() {
        //does nothing
    }
}
