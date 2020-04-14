using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveComponentController : GraphComponentController {

    public override void setUp(GraphComponent passiveComponent) {

        this.graphComponent = passiveComponent;

        //Debug.Log("set up " + passiveComponent.GetType().ToString() + " " + Time.frameCount);

        GameObject middle = this.transform.Find("Middle").gameObject;

        if(passiveComponent.GetType() == typeof(Reflector)) {
            GameObject mirrorBasePrefab = (GameObject)SceneResouces.SceneObjects[typeof(GameObject)]["MirrorBase"];

            GameObject back = GameObject.Instantiate(mirrorBasePrefab);
            //back.GetComponent<PolygonCollider2D>().enabled = true;
            back.transform.SetParent(this.transform, false);

            middle.AddComponent<MirrorController>();
        } else {
            middle.AddComponent<PassingMirrorController>();
        }

        this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -45) + this.transform.rotation.eulerAngles);
    }

    public override void tickUpdate() {
        //does nothing
    }

    public override void createNewRays() {
        //does nothing
    }
}
