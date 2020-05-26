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

    public LogicGraphData(LogicGraph graph) {

        this.Name = graph.Name;
        this.Width = graph.Width;
        this.Height = graph.Height;

        this.position[0] = graph.Position.x;
        this.position[1] = graph.Position.y;

        List<LightComponent> comps = graph.getAllGraphComponents();
        foreach(LightComponent comp in comps) {
            Components.Add(new GraphComponentData(comp));
        }
    }
}