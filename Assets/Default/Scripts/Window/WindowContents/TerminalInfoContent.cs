using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerminalInfoContent : WindowContent
{

    private TerminalController terminalController;

    private Transform listDisplay;

    public TerminalInfoContent(TerminalController terminalController) {

        this.terminalController = terminalController;
    }

    public override void changeWindowSize(int width, int height) {
        //does nothing
    }

    public override bool sameContent(WindowContent content) {
        return this.GetType() == content.GetType() &&
            ((TerminalInfoContent)content).getTerminal().Equals(this.terminalController);
    }

    public override void spawnContents(WindowController windowController, Transform contentPanel, Canvas canvas) {

        WindowManager windowManager = GameObject.Find("WindowManager").GetComponent<WindowManager>();

        GameObject guiPrefab = (GameObject)SceneResouces.SceneObjects["Default"][typeof(GameObject)]["TerminalInfo"];
        GameObject gui = GameObject.Instantiate(guiPrefab);
        gui.transform.SetParent(contentPanel, false);

        //opens the terminal connections window
        Button linkButton = gui.transform.Find("Info").Find("BasicButton").GetComponent<Button>();
        linkButton.onClick.AddListener(() =>
        {
            windowManager.spawnWindow(new Window(
                 this.terminalController.Terminal.Name + "'s Connections",
                300, 300, new TerminalConnectionsContent(this.terminalController.Terminal)));
        });

        //makes a button for each clock
        Transform leftPanel = gui.transform.Find("LeftPanel");
        this.listDisplay = leftPanel.Find("ExtensionList").Find("Mask").Find("Display");

        Button addButton = leftPanel.Find("HeaderPanel").Find("AddButton").GetComponent<Button>();
        addButton.onClick.AddListener(() =>
        {
            this.onAddGraphClick();
        });

        this.populateExtensions();
    }

    private void onAddGraphClick() {
        //when the user wants to add a graph to the terminal

        EnterTextContent content = new EnterTextContent("Give a unique name to the graph", (string enteredText) =>
        {

            this.terminalController.Terminal.addExtension(new LogicGraph(100, 100, enteredText));
            this.refreshExtensionList();

        }, () => { }, 50);

        content.addErrorCheck((string value) =>
        {

            bool result = true;
            int counter = 0;

            while (result && counter < this.terminalController.Terminal.extensionLength()) {

                if (this.terminalController.Terminal.extensionAt(counter).Name.Equals(value)) {
                    result = false;
                }

                counter++;
            }

            return result;

        }, "That name already exists in this terminal");

        Window window = new Window("Add new Graph to " + this.terminalController.Terminal.Name, 200, 200, content);
        this.spawnChildWindow(window);

    }

    private void populateExtensions() {
        //populates the list with all the extensions

        GameObject itemPrefab = (GameObject)SceneResouces.SceneObjects["Default"][typeof(GameObject)]["ItemButton"];

        for (int i = 0; i < this.terminalController.Terminal.extensionLength(); i++) {

            int index = i;

            GameObject item = GameObject.Instantiate(itemPrefab);
            item.transform.SetParent(this.listDisplay, false);

            Transform graphButton = item.transform.Find("SelectButton");
            Transform deleteButton = item.transform.Find("DeleteButton");

            graphButton.Find("Text").GetComponent<Text>().text = this.terminalController.Terminal.extensionAt(index).Name;

            graphButton.GetComponent<Button>().onClick.AddListener(() =>
            {

                if (this.terminalController.Terminal.extensionAt(index).GetType() == typeof(LogicGraph)) {

                    LogicGraph graph = (LogicGraph)this.terminalController.Terminal.extensionAt(index);
                    LogicGraphContent lgContent = new LogicGraphContent(graph, terminalController.GraphManager);
                    Window win = new Window(graph.Name, 200, 200, lgContent);

                    WindowManager.Instance.spawnWindow(win);
                }
            });

            deleteButton.GetComponent<Button>().onClick.AddListener(() =>
            {

                //removes the logic graph window
                if (this.terminalController.Terminal.extensionAt(index).GetType() == typeof(LogicGraph)) {

                    LogicGraph graph = (LogicGraph)this.terminalController.Terminal.extensionAt(index);
                    LogicGraphContent lgContent = new LogicGraphContent(graph, terminalController.GraphManager);

                    WindowController wc = WindowManager.Instance.getControllerByData(lgContent);
                    if (wc != null) {
                        WindowManager.Instance.destroyWindow(wc);
                    }
                }

                this.terminalController.Terminal.removeExtension(this.terminalController.Terminal.extensionAt(index));
                this.refreshExtensionList();
            });
        }
    }

    public override void receiveBroadcast(string message) {
        //does nothing
    }

    protected override void destroyContent() {

        //removes the functions from the buttons
        foreach (Transform child in this.listDisplay) {
            child.Find("SelectButton").GetComponent<Button>().onClick.RemoveAllListeners();
            child.Find("DeleteButton").GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }

    private void deleteExtensions() {
        //clears the extension list

        foreach (Transform child in this.listDisplay.transform) {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void refreshExtensionList() {
        //refresh the list of extensions

        this.deleteExtensions();
        this.populateExtensions();
    }

    public TerminalController getTerminal() {
        return this.terminalController;
    }
}