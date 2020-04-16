using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassingMirror : ComponentPiece {

    private int rotation;
    private bool flipped;

    public PassingMirror(Rect rect, int rotation, bool flipped) : base(rect) {
        this.rotation = rotation;
        this.flipped = flipped;
    }
}
