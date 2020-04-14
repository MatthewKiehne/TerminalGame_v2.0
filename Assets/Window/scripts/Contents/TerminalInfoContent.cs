using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerminalInfoContent : WindowContent {

    private TerminalController terminalController;

    public TerminalInfoContent(TerminalController terminalController) {

        this.terminalController = terminalController;
    }

    public override void changeWindowSize(int width, int height) {
    }

    public override void onDestroy() {
    }

    public override bool sameContent(WindowContent content) {
        return this.GetType() == content.GetType() && ((TerminalInfoContent)content).getTerminal().Equals(this.terminalController);
    }

    public override void spawnContents(WindowController windowController, Transform contentPanel, Canvas canvas) {

        GameObject guiPrefab = (GameObject)SceneResouces.SceneObjects[typeof(GameObject)]["TerminalInfo"];
        GameObject gui = GameObject.Instantiate(guiPrefab);
        gui.transform.SetParent(contentPanel, false);

        //makes a button for each clock
        GameObject buttonPrefab = (GameObject)SceneResouces.SceneObjects[typeof(GameObject)]["BasicButton"];
        Transform display = gui.transform.Find("ExtensionList").Find("Mask").Find("Display");
        WindowManager windowManager = GameObject.Find("WindowManager").GetComponent<WindowManager>();

        CameraManager cameraManager = GameObject.Find("CameraManager").GetComponent<CameraManager>();

        for (int i = 0; i < this.terminalController.Terminal.getExtentionLength(); i++) {

            int index = i;

            GameObject buttonGO = GameObject.Instantiate(buttonPrefab);
            buttonGO.transform.SetParent(display, false);
            Button button = buttonGO.GetComponent<Button>();

            LayoutElement le = buttonGO.AddComponent<LayoutElement>();
            le.minHeight = 40;
            le.flexibleWidth = 1;

            Text text = button.transform.Find("Text").GetComponent<Text>();
            text.text = this.terminalController.Terminal.getExtentionAt(index).Name;

            button.onClick.AddListener( () => {

                if(this.terminalController.Terminal.getExtentionAt(index).GetType() == typeof(LogicGraph)) {

                    LogicGraph graph = (LogicGraph)this.terminalController.Terminal.getExtentionAt(index);
                    LogicGraphContent lgContent = new LogicGraphContent(graph, terminalController.GraphManager);
                    Window win = new Window(graph.Name, 200, 200, false, lgContent);

                    windowManager.spawnWindow(win);
                }
            });
        }
    }

    public TerminalController getTerminal() {
        return this.terminalController;
    }
}