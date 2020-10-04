using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sender : ComponentPiece {

    private List<Receiver> targets;

    public Sender(Rect rect) : base(rect) {
        this.targets = new List<Receiver>();
    }

    public void setTargetsActive() {

        //Debug.Log("Number of targets: " + this.targets.Count);

        for(int i = 0; i < targets.Count; i++) {
            targets[i].setActive(true);
        }
    }

    public int getTargetCount() {
        return this.targets.Count;
    }

    public void clearTargets() {
        //clears the targets
        this.targets.Clear();
    }

    public void addTarget(Receiver receiver) {
        this.targets.Add(receiver);
    }

    public void setTargets(List<Receiver> receivers) {
        this.targets = receivers;
    }
}
