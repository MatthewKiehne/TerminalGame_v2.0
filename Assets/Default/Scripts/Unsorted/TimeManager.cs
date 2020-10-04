using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    public List<TimeDependent> frameDependencies = new List<TimeDependent>();
    public List<TimeDependent> fixedDependencies = new List<TimeDependent>();

    private float timeMultiplier = 1f;

    private void Update() {
        //updates all the time dependents

        for (int i = 0; i < frameDependencies.Count; i++) {
            frameDependencies[i].updateTime(Time.deltaTime);
        }
    }

    private void FixedUpdate() {
        //updates all the fixed dependencies

        for (int i = 0; i < fixedDependencies.Count; i++) {
            fixedDependencies[i].updateTime(Time.fixedDeltaTime);
        }
    }

    public float TimeMultiplier {
        get {
            return this.timeMultiplier;
        }
        set {
            this.timeMultiplier = value;
        }
    }

}
