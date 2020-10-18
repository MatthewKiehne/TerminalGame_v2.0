using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicGraphComponentManager : MonoBehaviour {

    private List<GraphComponentController> componentControllers = new List<GraphComponentController>();
    private Vector3 bottomLeftofGraph = Vector3.zero;

    private void Start() {
        //connects all the rays when the graph loads
        Invoke("reconnectRays", .02f);
    }

    public void loadComponents(LogicGraph logicGraph, Vector3 bottomLeft) {
        //loads all of the components on to the graph
        this.bottomLeftofGraph = bottomLeft;

        List<LightComponent> graphComponents = logicGraph.lightGraph.getAllGraphComponents();

        //makes all the components
        foreach (GraphComponent gc in graphComponents) {

            //creates the gameobject 
            GraphComponentController gcc = createComponent(gc);
            this.addComponent(gcc);
        }

        this.reconnectRays();
    }

    public void addComponent(GraphComponentController comp) {
        this.componentControllers.Add(comp);
    }

    public bool removeComponent(GraphComponent component) {

        bool result = false;
        int counter = 0;

        while(counter < this.componentControllers.Count && !result) {

            if (this.componentControllers[counter].GetGraphComponent.Equals(component)) {

                GraphComponentController controller = this.componentControllers[counter];
                this.componentControllers.RemoveAt(counter);
                GameObject.Destroy(controller.gameObject);
                result = true;
            }

            counter++;
        }

        return result;
    }

    public GraphComponentController createComponent(GraphComponent comp) {
        //creates the component

        //find the x and y of the component
        Vector2Int pos = comp.Position;
        Vector2Int size = comp.Size;
        Vector3 basePosition = new Vector3(pos.x, pos.y, 0);
        Vector3 offset = new Vector3(size.x / 2f, size.y / 2f, 0);
        if (comp.Rotation % 2 == 1) {
            float newX = offset.y;
            offset.y = offset.x;
            offset.x = newX;
        }

        //rotates the component
        Vector2 position = this.bottomLeftofGraph + basePosition + offset;

        Vector3 componentRotation = new Vector3(0, 0, -90 * comp.Rotation);
        if (comp.Flipped) {
            componentRotation = new Vector3(0, 180, 90 * comp.Rotation);
        }

        Type resultType = null;
        GameObject prefab = null;

        //logic compoents
        if (comp.GetType().BaseType == typeof(LogicComponent)) {

            prefab = (GameObject)SceneResouces.SceneObjects[typeof(GameObject)]["BasicLogicComponent"];
            resultType = typeof(LogicComponentController);

            //passive components
        } else if (comp.GetType().BaseType == typeof(PassiveComponent)) {

            prefab = (GameObject)SceneResouces.SceneObjects[typeof(GameObject)]["PassingMirror"];
            resultType = typeof(PassiveComponentController);

        } else if (comp.GetType().BaseType == typeof(LinkComponent)) {

            prefab = (GameObject)SceneResouces.SceneObjects[typeof(GameObject)]["BasicLogicComponent"];
            resultType = typeof(BridgeComponentController);

        } else {
            throw new System.Exception("This Component is not supported while making the GameObject");
        }

        GameObject go = Instantiate(prefab);
        GraphComponentController gcc = (GraphComponentController)go.AddComponent(Type.GetType(resultType.ToString()));
        go.transform.SetParent(this.transform, true);
        go.transform.position = position;
        go.transform.eulerAngles = componentRotation;
        gcc.setUp(comp);

        return gcc;
    }

    public void updateComponentVisuals() {
        //updates the visuals for the components

        foreach(GraphComponentController gcc in this.componentControllers) {
            gcc.tickUpdate();
        }
    }

    public void reconnectRays() {
        //deletes all the rays and then reconnects them all

        foreach (GraphComponentController gcc in this.componentControllers) {
            gcc.createNewRays();
        }

        this.updateComponentVisuals();
    }

    public void destroy() {

        foreach (GraphComponentController gcc in this.componentControllers) {
            GameObject.Destroy(gcc.gameObject);
        }
        this.componentControllers.Clear();
        this.componentControllers = new List<GraphComponentController>();

        GameObject.Destroy(this.gameObject);
    }
}
