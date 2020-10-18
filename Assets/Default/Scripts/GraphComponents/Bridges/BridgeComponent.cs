using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BridgeComponent : InteractiveComponent {

    private string name;
    public static readonly int BRIDGELENGTH = 8;

    public BridgeComponent(Vector2Int position, Vector2Int size, int rotation, bool flipped, int numSenders, int numRecievers) : 
        base(position, size, rotation, flipped, numSenders, numRecievers) {

        this.name = "IDK Name LOL";
    }

    public BridgeComponent(Vector2Int position, Vector2Int size, int rotation, bool flipped, int numSenders, int numRecievers, string name) :
        base(position, size, rotation, flipped, numSenders, numRecievers) {

        this.name = name;
    }

    public override List<Tuple> getValues() {

        List<Tuple> result = new List<Tuple>();

        //result.Add(new Tuple("state",this.state + ""));
        result.Add(new Tuple("name", this.name));  

        return result;
    }

    public abstract void clearConnections();

    public string Name {
        get {
            return this.name;
        }
        set {
            this.name = value;
        }
    }
}
