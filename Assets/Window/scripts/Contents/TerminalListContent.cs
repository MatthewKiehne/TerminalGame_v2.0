using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerminalListContent : WindowContent {

    private TerminalManager terminalManager;

    public TerminalListContent(TerminalManager terminalManager) {
        this.terminalManager = terminalManager;
    }

    public override void spawnContents(WindowController windowController, Transform contentPanel, Canvas canvas) {


        //loads and instantiates the gui
        GameObject guiPrefab = (GameObject)SceneResouces.SceneObjects[typeof(GameObject)]["EmptyList"];
        GameObject gui = GameObject.Instantiate(guiPrefab);
        gui.transform.SetParent(contentPanel, false);

        //makes a button for each clock
        GameObject buttonPrefab = (GameObject)SceneResouces.SceneObjects[typeof(GameObject)]["BasicButton"];
        Transform display = gui.transform.Find("Mask").Find("Display");
        WindowManager windowManager = GameObject.Find("WindowManager").GetComponent<WindowManager>();

        //loops through all the terminals
        for (int i = 0; i < this.terminalManager.TerminalControllers.Count; i++) {

            int index = i;

            GameObject button = GameObject.Instantiate(buttonPrefab);
            button.transform.SetParent(display, false);

            LayoutElement le = button.AddComponent<LayoutElement>();
            le.minHeight = 40;
            le.flexibleWidth = 1;

            Text text = button.transform.Find("Text").GetComponent<Text>();
            text.text = this.terminalManager.TerminalControllers[i].Terminal.Name;

            Button b = button.GetComponent<Button>();

			b.onClick.AddListener (() => {

                WindowContent content = new TerminalInfoContent(this.terminalManager.TerminalControllers[index]);
				Window win = new Window( this.terminalManager.TerminalControllers[index].Terminal.Name + " - Terminal Info", 
                    150, 150, content);
				windowManager.spawnWindow(win);
			});
            
        }
    }

    public override void changeWindowSize(int width, int height) {
        //does nothing
    }

    public override void onDestroy() {
        //does nothing
    }

    public override bool sameContent(WindowContent content) {

        return content.GetType() == this.GetType() && this.terminalManager.Equals(((TerminalListContent)content).terminalManager);
    }
}
