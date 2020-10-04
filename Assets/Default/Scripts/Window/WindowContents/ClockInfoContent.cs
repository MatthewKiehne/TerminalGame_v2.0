using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockInfoContent : WindowContent {

	private Clock clock;

	public ClockInfoContent(Clock clock){
		this.clock = clock;
	}

	public override void spawnContents (WindowController windowController, Transform contentPanel, Canvas canvas) {

        /*

		//makes the gui and adds it to the content panel
		GameObject guiPrefab = Resources.Load<GameObject> ("Prefabs/WindowGUI/ClockInfo");
		GameObject gui = GameObject.Instantiate (guiPrefab);
		gui.transform.SetParent (contentPanel, false);

		//gets the window manager
		WindowManager windowManager = GameObject.Find ("WindowManager").GetComponent<WindowManager> ();

		//makes the graph buttons for the display
		GameObject  buttonPrefab = Resources.Load<GameObject> ("Prefabs/BasicGUI/Button");
		Transform graphPanel = gui.transform.Find ("GraphList").Find ("Mask").Find ("Display");

		List<Graph> graphs = clock.Graphs;

		for (int i = 0; i < graphs.Count; i++) {

			//holds the index in a local variable
			int index = i;

			//makes the button
			GameObject button = GameObject.Instantiate (buttonPrefab);
			button.transform.SetParent (graphPanel, false);

			//sets the text for the button
			button.transform.Find("Text").GetComponent<Text>().text = graphs[index].Name;

			//makes it a LayoutElement
			LayoutElement le = button.AddComponent<LayoutElement> ();
			le.minHeight = 30;
			le.flexibleWidth = 1;

			//sets up the input
			Button b = button.GetComponent<Button> ();
			b.onClick.AddListener (() => {

				//spawns a new window based on the graph
				WindowContent graph = new GraphContent(graphs[index]);
				Window window = new Window(graphs[index].Name, 300, 300, false, graph);
				windowManager.spawnWindow(window);
			});
		}
        */
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