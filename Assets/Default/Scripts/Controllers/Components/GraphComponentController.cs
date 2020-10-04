using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GraphComponentController : MonoBehaviour {

    protected GraphComponent graphComponent;

    public abstract void setUp(GraphComponent graphComponent);

    public abstract void tickUpdate();

    public abstract void createNewRays();

    public GraphComponent GetGraphComponent {
        get {
            return this.graphComponent;
        }
    }
}
