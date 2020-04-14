using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BridgeComponent : InteractiveComponent {

    private string name;
    protected int length;

    public BridgeComponent(Vector2Int position, Vector2Int size, int rotation, bool flipped, int numSenders, int numRecievers) : 
        base(position, size, rotation, flipped, numSenders, numRecievers, ComponentCategory.Bridge) {

        this.name = "IDK Name LOL";
    }

    public BridgeComponent(Vector2Int position, Vector2Int size, int rotation, bool flipped, int numSenders, int numRecievers, string name) :
        base(position, size, rotation, flipped, numSenders, numRecievers, ComponentCategory.Bridge) {

        this.name = name;
    }

    public string Name {
        get {
            return this.name;
        }
        set {
            this.name = value;
        }
    }

    public int Length {
        get {
            return this.length;
        }
    }
}
