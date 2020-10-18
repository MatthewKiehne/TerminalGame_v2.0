using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicGraph : TExtension
{
    public LightGraph lightGraph;

    public LogicGraph(int width, int height, string name) : base(name) {

        this.lightGraph = new LightGraph(this,width, height);
    }

    public LogicGraph(LogicGraphData data) : base(data.Name) {
        //loads the logic graph from data

        this.lightGraph = new LightGraph(this,data.Width, data.Height);

        foreach (GraphComponentData gcd in data.Components) {

            Type type = Type.GetType(gcd.Type);
            LightComponent lightComponent = null;

            if (type.IsSubclassOf(typeof(LightComponent))) {

                lightComponent = (LightComponent)Activator.CreateInstance(
                    type,
                    new object[] {
                        new Vector2Int(gcd.Position[0], gcd.Position[1]),
                        gcd.Rotaiton,
                        gcd.Flipped
                    });

                if (type.IsSubclassOf(typeof(LogicComponent))) {

                    LogicComponent logic = (LogicComponent)lightComponent;
                    logic.setState();
                }

            } else {
                throw new Exception("Type " + type + " is not accepted from LogicGraphData");
            }

            if (lightComponent != null) {

                lightComponent.setValues(gcd.Values);
                this.lightGraph.addComponent(lightComponent);
            }
        }
    }

    #region TExtension

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

    #endregion
}
