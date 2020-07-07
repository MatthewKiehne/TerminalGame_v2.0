using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Receiver : ComponentPiece {

    public Receiver(Rect rect) : base(rect) {

    }

    public abstract void setActive(bool active);

    public abstract bool getActive();
}
