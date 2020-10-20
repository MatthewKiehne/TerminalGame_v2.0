using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LinkComponent : InteractiveComponent {

    public static readonly int BRIDGELENGTH = 8;

    public LinkComponent(Vector2Int position, Vector2Int size, int rotation, bool flipped, int numSenders, int numRecievers) : 
        base(position, size, rotation, flipped, numSenders, numRecievers) {
    }

    public override List<Tuple> getValues() {

        List<Tuple> result = new List<Tuple>();

        result.Add(new Tuple("state",this.getExtensionConnection().State + ""));
        result.Add(new Tuple("name", this.getExtensionConnection().Name));  

        return result;
    }

    public abstract ExtensionConnection getExtensionConnection();

    public abstract string getName();
}
