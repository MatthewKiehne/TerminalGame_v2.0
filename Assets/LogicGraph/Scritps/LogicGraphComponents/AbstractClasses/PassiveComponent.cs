using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveComponent : GraphComponent {

    public PassiveComponent(Vector2Int position , Vector2Int size, int rotation, bool flipped) : base(position, size, rotation, flipped, ComponentCategory.Passive) {
    }

}
