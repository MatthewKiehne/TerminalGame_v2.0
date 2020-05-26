using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GraphComponentData
{

    public int[] Position;
    public int[] Size;
    public int Rotaiton;
    public bool Flipped;
    public string Type;

    public List<Tuple> Values = new List<Tuple>();

    public GraphComponentData(GraphComponent comp) {
        this.Position = new int[2] { comp.Position.x, comp.Position.y };
        this.Size = new int[2] { comp.Size.x, comp.Size.y };
        this.Rotaiton = comp.Rotation;
        this.Flipped = comp.Flipped;
        this.Type = comp.GetType().ToString();

        this.Values = comp.getValues();
    }
}