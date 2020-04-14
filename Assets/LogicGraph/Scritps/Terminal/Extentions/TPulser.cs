using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPulser : TExtension {

    private int pulseMax;
    private int pulseCurrent;

    private int singnalNumber = 3;

    private bool pulseThisUpdate = false;

    public TPulser(int pulseEveryXTicks) : base("Pulser " + pulseEveryXTicks) {
        this.pulseCurrent = 0;
        this.pulseMax = pulseEveryXTicks;

        this.sendBridges.Add(new SendBridge(Vector2Int.zero, 0, false, 1, "Pulser Output"));
    }

    public override void setState() {
        //increases the clock and sets the state

        //overly cautious code below
        pulseCurrent = pulseCurrent + 1;
        //overly cautious code above

        if (pulseCurrent % pulseMax == 0) {
            this.pulseThisUpdate = true;
            this.pulseCurrent = 0;
        }
    }

    public override void sendSignal() {

        //send the signal if it is time to update
        if (pulseThisUpdate) {

            //Debug.Log("pulse this update: " + pulseThisUpdate);

            for (int i = 0; i < this.sendBridges.Count; i++) {
                this.sendBridges[i].sendData(singnalNumber);
            }

            this.pulseThisUpdate = false;
            this.pulseCurrent = 0;
        }
    }

    public override void clearReceivers() {
        //clears all the receiver bridges

        for(int i = 0; i < this.ReceiveBridges.Count; i++) {
            this.receiveBridges[i].clearState();
        }
    }
}
