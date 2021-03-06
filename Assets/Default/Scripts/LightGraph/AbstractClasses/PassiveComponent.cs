﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveComponent : LightComponent {

    public PassiveComponent(Vector2Int position , Vector2Int size, int rotation, bool flipped) : base(position, size, rotation, flipped) {
    }

    public override List<Tuple> getValues() {
        //does nothing
        return new List<Tuple>();
    }

    public override void setValues(List<Tuple> values) {
        //does nothing
    }
}
