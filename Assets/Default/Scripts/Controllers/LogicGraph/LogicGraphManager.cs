using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text.RegularExpressions;

public class LogicGraphManager : MonoBehaviour {
    // Start is called before the first frame update

    private List<LogicGraphController> graphControllers = new List<LogicGraphController>();

    public LogicGraphController displayLogicGraph(LogicGraph graph, Vector3 bottomLeft) {

        GameObject graphPrefab = (GameObject)SceneResouces.SceneObjects["Default"][typeof(GameObject)]["Graph"];
        GameObject go = Instantiate(graphPrefab).gameObject;

        Vector3 offset = new Vector3(graph.LightGraph.Width / 2, graph.LightGraph.Height / 2, 0);
        go.transform.position = bottomLeft + offset;

        LogicGraphController lgc = go.GetComponent<LogicGraphController>();
        lgc.setUp(graph, bottomLeft,this);
        this.graphControllers.Add(lgc);

        return lgc;
    }

    public void updateAllLogicGraphVisuals() {
        //updates all of the logic graphs with the time
        foreach (LogicGraphController lgc in this.graphControllers) {
            lgc.updateVisuals();
        }
    }

    public bool removeGraphController(LogicGraphController controller) {
        return this.graphControllers.Remove(controller);
    }

    public List<LogicGraphController> GraphControllers {
        get {
            return this.graphControllers;
        }
    }

}
