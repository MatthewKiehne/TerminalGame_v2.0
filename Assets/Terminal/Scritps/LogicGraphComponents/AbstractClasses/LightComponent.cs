using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LightComponent : GraphComponent{
    public LightComponent(Vector2Int position, Vector2Int size, int rotation, bool flipped) : base(position, size, rotation, flipped) {

    }
}