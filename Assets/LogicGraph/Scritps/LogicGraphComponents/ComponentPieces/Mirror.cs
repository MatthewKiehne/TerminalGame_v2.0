using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : ComponentPiece {

    private bool flipped;
    private int rotation;

    public Mirror(Rect rect, int rotation, bool flipped) : base(rect) {

        this.rotation = rotation;
        this.flipped = flipped;
    }
}
