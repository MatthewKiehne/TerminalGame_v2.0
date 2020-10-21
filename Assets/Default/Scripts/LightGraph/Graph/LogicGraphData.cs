using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LogicGraphData
{
    public string Name;

    public int Width;
    public int Height;

    public int[] position = new int[2];

    public List<GraphComponentData> Components = new List<GraphComponentData>();

    /// <summary>
    /// Creates savable data from a LogicGraph
    /// </summary>
    public LogicGraphData(LogicChip graph) {

        this.Name = graph.Name;
        this.Width = graph.LightGraph.Width;
        this.Height = graph.LightGraph.Height;

        List<LightComponent> comps = graph.LightGraph.getAllGraphComponents();
        foreach(LightComponent comp in comps) {

            Components.Add(new GraphComponentData(comp));
        }
    }

    /// <summary>
    /// Creates a Logic Graph from the data in this Class
    /// </summary>
    public LogicChip getLogicGraph() {

        LogicChip result = new LogicChip(this.Width, this.Height, this.Name);

        foreach (GraphComponentData gcd in this.Components) {

            Type type = Type.GetType(gcd.Type);
            LightComponent lightComponent = null;

            if (type.IsSubclassOf(typeof(LightComponent))) {

                if (!type.IsSubclassOf(typeof(LinkComponent))) {

                    lightComponent = (LightComponent)Activator.CreateInstance(
                        type, new object[] {
                            new Vector2Int(gcd.Position[0], gcd.Position[1]),
                            gcd.Rotaiton,
                            gcd.Flipped
                        });

                } else {
                    if (type == typeof(GraphOutput)) {

                        lightComponent = new GraphOutput(new Vector2Int(gcd.Position[0], gcd.Position[1]),
                            gcd.Rotaiton,
                            gcd.Flipped, new ExtensionNode("Blank", ExtensionNode.ExtensionState.SEND));

                    } else if (type == typeof(GraphInput)) {

                        lightComponent = new GraphInput(new Vector2Int(gcd.Position[0], gcd.Position[1]),
                            gcd.Rotaiton,
                            gcd.Flipped, new ExtensionNode("Blank", ExtensionNode.ExtensionState.RECEIVE));
                    } else {
                        throw new Exception(type + " is not supported when loading data");
                    }
                }

                if (type.IsSubclassOf(typeof(LogicComponent))) {

                    LogicComponent logic = (LogicComponent)lightComponent;
                    logic.setState();
                }

            } else {
                throw new Exception("Type " + type + " is not accepted from LogicGraphData");
            }

            if (lightComponent != null) {

                lightComponent.setValues(gcd.Values);
                result.LightGraph.addComponent(lightComponent);
            }
        }

        return result;
    }
}