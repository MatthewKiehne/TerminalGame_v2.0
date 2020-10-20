using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageContent : WindowContent {

    private Sprite sprite;

    public ImageContent(Sprite sprite) {
        this.sprite = sprite;
    }


    public override void changeWindowSize(int width, int height) {
        //deos nothing
    }

    public override void receiveBroadcast(string message) {
        //does nothing
    }

    public override bool sameContent(WindowContent content) {
        return this.GetType() == content.GetType();
    }

    public override void spawnContents(WindowController windowController, Transform contentPanel, Canvas canvas) {

        GameObject imagePrefab = (GameObject)SceneResouces.SceneObjects["Default"][typeof(GameObject)]["Image"];
        GameObject imageGO = GameObject.Instantiate(imagePrefab, contentPanel.transform, false);
        imageGO.GetComponent<Image>().sprite = this.sprite;
    }

    protected override void destroyContent() {
        //does nothing
    }
}