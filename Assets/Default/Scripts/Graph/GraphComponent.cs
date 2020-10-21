using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GraphComponent {

    private Vector2Int position;
    private Vector2Int size;
    private int rotaiton;
    private bool flipped;

    protected List<ComponentPiece> componentPieces = new List<ComponentPiece>();

    public GraphComponent(Vector2Int position, Vector2Int size, int rotation, bool flipped) {
        this.position = position;
        this.size = size;
        this.rotaiton = rotation;
        this.flipped = flipped;
    }

    ///<summary>
    /// Returns the shape of the component after it has been rotated
    /// </summary>
    public Rect getDimentions() {

        Vector2Int rotatedSize = size;
        if ((this.rotaiton % 2) == 1) {
            rotatedSize = new Vector2Int(size.y, size.x);
        }

        return new Rect(this.position, rotatedSize);
    }

    public abstract List<Tuple> getValues();

    public abstract void setValues(List<Tuple> values);

    public Vector2Int Position {
        get {
            return this.position;
        } set {
            this.position = value;
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
}
