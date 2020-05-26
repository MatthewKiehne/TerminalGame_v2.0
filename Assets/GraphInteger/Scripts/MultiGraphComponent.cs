using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiGraphComponent : GraphComponent {

    public MultiGraphComponent(Vector2Int position, Vector2Int size, int rotation, bool flipped) : base(position, size, rotation, flipped) {
    }

    public override List<Tuple> getValues() {
        throw new System.NotImplementedException();
    }

    public override void setValues(List<Tuple> values) {
        throw new System.NotImplementedException();
    }
}