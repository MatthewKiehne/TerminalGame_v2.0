using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BridgeComponentController : InteractiveComponentController {

    public override void setUp(GraphComponent graphComponent) {

        this.graphComponent = graphComponent;
        LinkComponent link = (LinkComponent)graphComponent;

        this.formBasicShape((InteractiveComponent)graphComponent);
        this.displaySprite("Square");


        Transform middle = this.transform.Find("MiddleBody");

        //makes the middle a little bit wider
        Vector3 localScale = middle.transform.localScale;
        middle.transform.localScale = new Vector3(localScale.x, 1.5f, localScale.z);

        Vector3 moveDirection  = new Vector3(0, -.45f, 0);
        if(graphComponent.GetType() == typeof(GraphOutput)) {
            moveDirection.y = -moveDirection.y;
        }
        middle.localPosition = moveDirection;

        //adds the canvas to the component
        SpriteRenderer rend = middle.GetChild(0).GetComponent<SpriteRenderer>();
        rend.transform.localScale = Vector3.one;
        string labelPrefabName = "LGBridgeLabel";
        GameObject labelPrefab = (GameObject)SceneResouces.SceneObjects["Default"][typeof(GameObject)][labelPrefabName];


        GameObject labelGameObject = GameObject.Instantiate(labelPrefab);
        labelGameObject.transform.SetParent(rend.transform, false);
        labelGameObject.transform.localPosition = Vector3.zero;

        RectTransform labelRect = labelGameObject.GetComponent<RectTransform>();
        labelRect.sizeDelta = new Vector2(rend.bounds.size.x, rend.bounds.size.y);

        labelGameObject.transform.GetChild(0).GetComponent<Text>().text = link.getName();

        float panelWidth = labelRect.rect.width;
        float panelHeight = labelRect.rect.height;


        //so the text is not upside down
        if (link.Rotation > 1) {
            labelGameObject.transform.Rotate(new Vector3(0, 0, 180));
        }
    }
}
