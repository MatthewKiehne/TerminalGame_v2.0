using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicGraphController : MonoBehaviour {
    // Start is called before the first frame update

    private LogicGraph logicGraph;
    private Vector2 bottomLeftWorld;

    private LogicGraphComponentManager componentManager;
    private LogicGraphManager logicGraphManager;

    public void setUp(LogicGraph logicGraph, Vector2 bottomLeftWorld, LogicGraphManager logicGraphManager) {

        this.logicGraph = logicGraph;
        this.bottomLeftWorld = bottomLeftWorld;
        this.transform.position = bottomLeftWorld;
        this.changeSize(logicGraph.LightGraph.Width, logicGraph.LightGraph.Height);
        this.makeComponentManager(logicGraph, bottomLeftWorld);
        this.logicGraphManager = logicGraphManager;
    }

    
    private void changeSize(int width, int height) {

        SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
        sr.drawMode = SpriteDrawMode.Tiled;
        sr.size = new Vector2(width, height);
        this.transform.position = this.transform.position + new Vector3(width / 2f, height / 2f, 0);

        Transform topBarrier = this.transform.Find("Top Barrier");
        Transform bottomBarrier = this.transform.Find("Bottom Barrier");
        Transform leftBarrier = this.transform.Find("Left Barrier");
        Transform rightBarrier = this.transform.Find("Right Barrier");

        topBarrier.localPosition = new Vector3(0, (height / 2f) + .5f, 0);
        bottomBarrier.localPosition = new Vector3(0, (-height / 2f) - .5f, 0);
        leftBarrier.localPosition = new Vector3((-width / 2f) - .5f, 0, 0);
        rightBarrier.localPosition = new Vector3((width / 2f) + .5f, 0, 0);

        BoxCollider2D topC = topBarrier.GetComponent<BoxCollider2D>();
        BoxCollider2D leftC = leftBarrier.GetComponent<BoxCollider2D>();
        BoxCollider2D rightC = rightBarrier.GetComponent<BoxCollider2D>();
        BoxCollider2D bottomC = bottomBarrier.GetComponent<BoxCollider2D>();

        topC.size = new Vector2(width, 1);
        bottomC.size = new Vector2(width, 1);
        rightC.size = new Vector2(1, height);
        leftC.size = new Vector2(1, height);
    }

    private void makeComponentManager(LogicGraph logicGraph, Vector3 bottomLeft) {

        GameObject go = new GameObject("ComponentManager");
        go.transform.SetParent(this.transform, false);

        LogicGraphComponentManager cm = go.AddComponent<LogicGraphComponentManager>();
        this.componentManager = cm;
        cm.loadComponents(logicGraph, bottomLeft);

        this.updateVisuals();
    }

    public void destroy() {
        //destroys everything
       
        if(this.componentManager != null) {
            this.componentManager.destroy();
            GameObject.Destroy(this.componentManager.gameObject);
        } else {
            throw new System.Exception("Tried to destroy the component manager but it was null");
        }
;
        GameObject.Destroy(this.gameObject);
    }

    public void updateVisuals() {
        //updates the visuals of all the components

        this.componentManager.updateComponentVisuals();
        
    }

    public LogicGraphComponentManager ComponentManager {
        get {
            return this.componentManager;
        }
    }

    public LogicGraph Graph {
        get {
            return this.logicGraph;
        }
    }

    public Vector2 BottomLeftWorld {
        get {
            return this.bottomLeftWorld;
        }
    }
}
