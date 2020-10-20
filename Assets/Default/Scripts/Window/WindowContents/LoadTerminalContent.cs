using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LoadTerminalContent : WindowContent {

    private WindowManager windowManager;
    private TerminalManager terminalManager;

    public LoadTerminalContent(WindowManager windowManager, TerminalManager terminalManager) {
        this.windowManager = windowManager;
        this.terminalManager = terminalManager;
        this.movable = false;
        this.fixxedSize = true;
    }

    public override void changeWindowSize(int width, int height) {
        //does nothing
    }

    protected override void destroyContent() {
        this.windowManager.AllowSpawnWindows = true;
        this.windowManager.setActivityofCurrentWindows(true);
    }

    public override void receiveBroadcast(string message) {
        //does nothing
    }

    public override bool sameContent(WindowContent content) {
        return this.GetType() == content.GetType();
    }

    public override void spawnContents(WindowController windowController, Transform contentPanel, Canvas canvas) {

        this.windowManager.AllowSpawnWindows = false;

        //loads and instantiates the gui
        GameObject guiPrefab = (GameObject)SceneResouces.SceneObjects["Default"][typeof(GameObject)]["EmptyList"];
        GameObject gui = GameObject.Instantiate(guiPrefab);
        gui.transform.SetParent(contentPanel, false);

        //gets the directories in order of creation
        string path = Application.dataPath + "/Default/Resources/Default/Saves";
        DirectoryInfo di = new DirectoryInfo(path);
        List<string> order = di.EnumerateDirectories().OrderBy(d => d.CreationTime).Select(d => d.Name).ToList();

        //makes a button for each save
        GameObject buttonPrefab = (GameObject)SceneResouces.SceneObjects["Default"][typeof(GameObject)]["BasicButton"];
        Transform display = gui.transform.Find("Mask").Find("Display");

        

        List<Terminal> loadedTerminals = new List<Terminal>();

        //loops through all the terminals
        for (int i = 0; i < order.Count; i++) {

            int index = i;

            GameObject button = GameObject.Instantiate(buttonPrefab);
            button.transform.SetParent(display, false);

            LayoutElement le = button.AddComponent<LayoutElement>();
            le.minHeight = 40;
            le.flexibleWidth = 1;

            Text text = button.transform.Find("Text").GetComponent<Text>();
            text.text = order[index];

            Button b = button.GetComponent<Button>();

            b.onClick.AddListener(() => {

                //gets rid of all the prexisting terminals
                this.terminalManager.clearTerminal();

                string selectedSave = path + "/" + order[index];
                selectedSave += "/Terminals";
                string[] terminals = Directory.GetDirectories(selectedSave);

                foreach (string ter in terminals) {

                    //terminal creation
                    DirectoryInfo info = new DirectoryInfo(ter);

                    string terJsonPath = ter + "/" + info.Name + ".json";
                    TerminalData terData = Save.loadJson<TerminalData>(terJsonPath);
                    Terminal terminal = new Terminal(terData);
                    loadedTerminals.Add(terminal);

                    string logicGraphPath = ter + "/LogicGraphs";
                    string[] logicPaths = Directory.GetFiles(logicGraphPath);

                    foreach(string logicPath in logicPaths) {

                        if (logicPath.EndsWith(".json")) {

                            LogicGraphData lgd = Save.loadJson<LogicGraphData>(logicPath);
                            //Debug.Log(lgd.Name);

                            LogicGraph lg = new LogicGraph(lgd);
                            terminal.addExtension(lg);
                        }
                    }

                    //connects the TExtensions
                    string TExtConnectionPath = ter + "/" + "TExtensionConnectionsData.json";
                    TExtensionConnectionsData connectionData = Save.loadJson<TExtensionConnectionsData>(TExtConnectionPath);
                    connectionData.buildConnections(terminal);

                    //terminalManager.displayTerminal(terminal);
                }

                //clear all the windows
                windowManager.removeAllWindows();
            });
        }
    }
}