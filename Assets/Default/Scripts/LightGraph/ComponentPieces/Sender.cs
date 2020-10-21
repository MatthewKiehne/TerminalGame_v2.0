using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sender : ComponentPiece {

    private List<Receiver> receivers;

    public Sender(Rect rect) : base(rect) {
        this.receivers = new List<Receiver>();
    }

    public void setTargetsActive() {

        for(int i = 0; i < receivers.Count; i++) {
            receivers[i].setActive(true);
        }
    }

    public int getTargetCount() {
        return this.receivers.Count;
    }

    public void clearTargets() {
        //clears the targets
        this.receivers.Clear();
    }

    public void addTarget(Receiver receiver) {
        this.receivers.Add(receiver);
    }

    public void setTargets(List<Receiver> receivers) {
        this.receivers = receivers;
    }
}
