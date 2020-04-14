using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIContents : WindowContent {

    private GameObject gui;

    public GUIContents(GameObject gui) {

        this.gui = gui;
    }

    public override void spawnContents(WindowController windowController, Transform contentPanel, Canvas canvas) {

        //makes the gui
        GameObject panel = GameObject.Instantiate(gui);
        panel.transform.SetParent(contentPanel, false);
    }

    public override void changeWindowSize(int width, int height) {
        //does nothing
    }

    public override void onDestroy() {

    }

    public override bool sameContent(WindowContent content) {
        return content.GetType() == this.GetType() && ((GUIContents)content).getGUI().Equals(this.gui);
    }

    public GameObject getGUI() {
        return this.gui;
    }
}
