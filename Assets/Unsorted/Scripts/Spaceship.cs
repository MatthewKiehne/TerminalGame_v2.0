using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : SpaceObject{

    private List<Terminal> terminals = new List<Terminal>();

    public Spaceship() {

    }

    public List<Terminal> Terminals {
        get {
            return this.terminals;
        }
    }

}
