using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GraphComponent {

    //public enum ComponentCategory { Logic, Passive, Bridge}

    private Vector2Int position;
    private Vector2Int size;
    private int rotaiton;
    private bool flipped;

    //private ComponentCategory compType;

    protected List<ComponentPiece> componentPieces = new List<ComponentPiece>();

    public GraphComponent(Vector2Int position, Vector2Int size, int rotation, bool flipped) {
        this.position = position;
        this.size = size;
        this.rotaiton = rotation;
        this.flipped = flipped;
        //this.compType = type;
    }

    public Rect getDimentions() {
        //gets the basic dimentions of the shape

        Vector2Int rotatedSize = size;
        if ((this.rotaiton % 2) == 1) {
            rotatedSize = new Vector2Int(size.y, size.x);
        }

        return new Rect(this.position, rotatedSize);
    }

    public Vector2Int Position {
        get {
            return this.position;
        }
    }

    public Vector2Int Size {
        get {
            return this.size;
        }
    }

    public int Rotation {
        get {
            return this.rotaiton;
        }
    }

    public bool Flipped {
        get {
            return this.flipped;
        }
    }

    public List<ComponentPiece> ComponentPieces {
        get {
            return this.componentPieces;
        }
    }

    /*
    public ComponentCategory ComponentType {
        get {
            return this.compType;
        }
    }
    */
}
