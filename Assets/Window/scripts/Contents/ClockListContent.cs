using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockListContent : WindowContent {

	public override void spawnContents (WindowController windowController, Transform contentPanel, Canvas canvas) {


        //loads and instantiates the gui
        GameObject guiPrefab = (GameObject)SceneResouces.SceneObjects[typeof(GameObject)]["TerminalList"];
		GameObject gui = GameObject.Instantiate (guiPrefab);
		gui.transform.SetParent (contentPanel, false);

		//makes a button for each clock
		GameObject  buttonPrefab = (GameObject)SceneResouces.SceneObjects[typeof(GameObject)]["BasicButton"];
        Transform display = gui.transform.Find ("Mask").Find ("Display");
		WindowManager windowManager = GameObject.Find ("WindowManager").GetComponent<WindowManager> ();


		//loops through all the clocks
		for (int i = 0; i < 20; i++) {

			int index = i;

			GameObject button = GameObject.Instantiate (buttonPrefab);
			button.transform.SetParent (display, false);

			LayoutElement le = button.AddComponent<LayoutElement> ();
			le.minHeight = 40;
			le.flexibleWidth = 1;

            Text text = button.transform.Find("Text").GetComponent<Text>();
            text.text = "TExtention: " + i;
            text.fontSize = 16;


			Button b = button.GetComponent<Button> ();

            /*

			b.onClick.AddListener (() => {

				WindowContent content = new ClockInfoContent(allClocks[index]);
				Window win = new Window("clock info", 150, 150, false, content);
				windowManager.spawnWindow(win);
			});
            */
		}
	}

	public override void changeWindowSize (int width, int height) {
		//does nothing
	}

	public override void onDestroy () {
		
	}

    public override bool sameContent(WindowContent content) {
        return false;
    }
}
