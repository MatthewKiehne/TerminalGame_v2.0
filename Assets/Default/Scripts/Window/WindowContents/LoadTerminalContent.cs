using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LoadTerminalContent : WindowContent {

    private TerminalManager terminalManager;

    public LoadTerminalContent( TerminalManager terminalManager) {
        this.terminalManager = terminalManager;
        this.movable = false;
        this.fixxedSize = true;
    }

    public override void changeWindowSize(int width, int height) {
        //does nothing
    }

    protected override void destroyContent() {
        WindowManager.Instance.AllowSpawnWindows = true;
        WindowManager.Instance.setActivityofCurrentWindows(true);
    }

    public override void receiveBroadcast(string message) {
        //does nothing
    }

    public override bool sameContent(WindowContent content) {
        return this.GetType() == content.GetType();
    }

    public override void spawnContents(WindowController windowController, Transform contentPanel, Canvas canvas) {

        WindowManager.Instance.AllowSpawnWindows = false;

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

                foreach (string terminalPath in terminals) {

                    //terminal creation
                    DirectoryInfo info = new DirectoryInfo(terminalPath);

                    string terJsonPath = terminalPath + "/" + info.Name + ".json";
                    TerminalData terData = Save.loadJson<TerminalData>(terJsonPath);

                    Terminal terminal = terData.getTerminal(terminalPath);

                    //adds the terminal to the Terminal Manager
                    GameObject go = new GameObject(terminalPath);
                    TerminalController terminalController = go.AddComponent<TerminalController>();
                    terminalController.setUp(terminal);
                    terminalManager.addTerminalController(terminalController);
                }

                //clear all the windows
                WindowManager.Instance.removeAllWindows();
            });
        }
    }
}