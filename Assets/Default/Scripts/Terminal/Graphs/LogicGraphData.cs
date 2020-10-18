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

        //Debug.Log(graph.Name);

        this.Name = graph.Name;
        this.Width = graph.lightGraph.Width;
        this.Height = graph.lightGraph.Height;

        List<LightComponent> comps = graph.lightGraph.getAllGraphComponents();
        foreach(LightComponent comp in comps) {

            //Debug.Log(comp.GetType());
            Components.Add(new GraphComponentData(comp));
        }
    }
}