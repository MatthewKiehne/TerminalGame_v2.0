using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicGraph : TExtension
{
    private LightGraph lightGraph;

    public LogicGraph(int width, int height, string name) : base(name) {

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

        List<ExtensionConnection> allBridges = this.AllBridges();

        for (int i = 0; i < allBridges.Count; i++) {
            allBridges[i].clearState();
        }

        this.lightGraph.clearReceivers();
    }

    public LightGraph LightGraph {
        get {
            return this.lightGraph;
        }
    }
}
