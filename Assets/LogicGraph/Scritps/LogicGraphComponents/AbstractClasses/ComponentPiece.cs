using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ComponentPiece {

    private Rect rect;


    public ComponentPiece(Rect rect){
        this.rect = rect;
    }

    public Rect Rect {
        get {
            return this.rect;
        }
    }

}
