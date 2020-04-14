using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item {

    private string name;

    public Item(string name) {
        this.name = name;
    }

    public string Name {
        get {
            return this.name;
        }
    }
}
