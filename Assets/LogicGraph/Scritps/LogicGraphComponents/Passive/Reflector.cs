using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflector : PassiveComponent {

    public Reflector(Vector2Int position, int rotation, bool flipped) : base(position, new Vector2Int(1, 1), rotation, flipped) {

        Vector2 mirrorOffset = new Vector2(.3f, .1f);
        Vector2 bodyOffset = new Vector2(.1f, .3f);
        Vector2 componentSize = new Vector2(.6f, .6f);

        //the format of the layout
        int format = rotation;

        //if flipped move to the format index
        if (flipped) {
            format = (rotation + 1) % 4;
        }

        //update the offsets
        if (format == 0) {
            //does nothing
        } else if (format == 1) {
            mirrorOffset = new Vector2(.1f, .1f);
            bodyOffset = new Vector2(.3f, .3f);

        } else if (format == 2) {

            mirrorOffset = new Vector2(.1f, .3f);
            bodyOffset = new Vector2(.3f, .1f);

        } else if (format == 3) {

            mirrorOffset = new Vector2(.3f, .3f);
            bodyOffset = new Vector2(.1f, .1f);
        }

        Mirror mir = new Mirror(new Rect(position + mirrorOffset, componentSize), rotation, flipped);
        ComponentBody body = new ComponentBody(new Rect(position + bodyOffset, componentSize));

        this.componentPieces.Add(body);
        this.componentPieces.Add(mir);
    }
}
