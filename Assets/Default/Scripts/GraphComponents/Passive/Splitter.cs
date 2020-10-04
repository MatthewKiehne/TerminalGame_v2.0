using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : PassiveComponent {

    public Splitter(Vector2Int position, int rotation, bool flipped) : base(position, new Vector2Int(1, 1), rotation, flipped) {

        Vector2 sensorSize = new Vector2(.6f, .6f);
        Vector2 offset = (Vector2.one - sensorSize) / 2f;

        this.componentPieces.Add(new PassingMirror(new Rect(position + offset, sensorSize), rotation, flipped));
    }
}
