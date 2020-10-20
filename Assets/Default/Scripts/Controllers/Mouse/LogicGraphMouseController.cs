using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicGraphMouseController : MonoBehaviour {
    // Start is called before the first frame update
    
    public void setUp(Camera camera) {

    }

    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetKeyDown(KeyCode.Mouse0)) {

            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if(hit.collider != null) {
                this.clickOnComponent(hit);
            }      
        }        
    }

    private void clickOnComponent(RaycastHit2D hit) {

        Transform hitTrans = hit.collider.transform;

        if (!hitTrans.name.Equals("BasicLogicComponent")) {
            hitTrans = hitTrans.parent;
        }

        LogicComponentController logicComponentController = hitTrans.GetComponent<LogicComponentController>();
        LogicComponent logicComponent = logicComponentController.LogicComponent;

    }
}
