using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicChip : TExtension
{
    private LightGraph lightGraph;

    public LogicChip(int width, int height, string name) : base(name) {

        this.lightGraph = new LightGraph(this, width, height);
    }

    public override void setState() {
        this.lightGraph.setState();
    }

    public override void sendSignal() {
        this.lightGraph.sendSignal();
    }

    public override void clearReceivers() {
        //clears all the receiver bridges and the interactive components

        ExtensionNode[] allBridges = this.AllBridges();

        for (int i = 0; i < allBridges.Length; i++) {
            allBridges[i].clearValue();
        }

        this.lightGraph.clearReceivers();
    }

    public LightGraph LightGraph {
        get {
            return this.lightGraph;
        }
    }
}
