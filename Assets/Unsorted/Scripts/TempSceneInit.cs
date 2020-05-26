using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class TempSceneInit : MonoBehaviour {

    //private List<Terminal> terminals = new List<Terminal>();
    private TerminalManager terminalManager;

    private List<Terminal> allTerminals;

    [SerializeField]
    private Sprite truthTableSprite;

    [SerializeField]
    private Sprite logicGraphControls;

    void Start() {

        //reads the game objects into memory
        TextAsset text = Resources.Load<TextAsset>("Text/InitSceneLoad/GameScene");

        if (text != null) {
            StringReader strReader = new StringReader(text.text);

            string line = strReader.ReadLine();

            while (line != null) {
                SceneResouces.loadResources(line);
                line = strReader.ReadLine();
            }
        } else {
            Debug.Log("txt not found");
        }

        //Sprite sprite = Resources.Load<Sprite>("Sprites/LogicGates/And");

        Terminal firstTerminal = new Terminal("First Test Terminal");

        Clock newClock = new Clock(5, 0, true, true);
        firstTerminal.Clock = newClock;

        LogicGraph emptyGraph = new LogicGraph(100, 100, "Empty Graph",Vector2Int.zero);
        LogicGraph testGraph = new LogicGraph(100, 100, "Test Graph", Vector2Int.zero);

        testGraph.addComponent(new Reflector(new Vector2Int(1, 6), 0, false));
        testGraph.addComponent(new NotGate(new Vector2Int(1, 4), 0, false));
        testGraph.addComponent(new NotGate(new Vector2Int(1, 1), 0, false));
        testGraph.connectGraph();

        firstTerminal.addComponent(testGraph);
        firstTerminal.addComponent(emptyGraph);

        Terminal secondTerminal = new Terminal("Second Terminal");
        secondTerminal.addComponent(new LogicGraph(20, 20, "Mini Graph", Vector2Int.zero));

        Terminal emptyTerminal = new Terminal("Blank Graphs");
        for(int i = 0; i < 10; i++) {
            LogicGraph tempEmptyGraph = new LogicGraph(500, 500, "Graph No." + (i + 1), Vector2Int.zero);
            emptyTerminal.addComponent(tempEmptyGraph);
        }

        this.allTerminals = new List<Terminal>();
        this.allTerminals.Add(firstTerminal);
        //this.allTerminals.Add(secondTerminal);
        //this.allTerminals.Add(emptyTerminal);

        GameObject terminalManagerGO = new GameObject("Terminal Manager");
        this.terminalManager = terminalManagerGO.AddComponent<TerminalManager>();
        
        foreach(Terminal ter in this.allTerminals) {
            this.terminalManager.displayTerminal(ter);
        }
    }

    public void allTerminalWindow() {

        //window test
        WindowContent testContents = new TerminalListContent(this.terminalManager);
        Window win = new Window("All Terminals", 200, 200, testContents);

        GameObject windowManagerGO = GameObject.Find("WindowManager");
        WindowManager wm = windowManagerGO.GetComponent<WindowManager>();

        wm.spawnWindow(win);
    }

    public void updateAllTerminals() {
        //updates all the terminals

        this.updateData();
        this.updateVisuals();
        
    }

    public void saveButton() {

        string path = Application.dataPath + "/Saves/Resources/Saves";

        string illegal = System.DateTime.Now.ToString();
        string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
        Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
        illegal = r.Replace(illegal, " ");

        Save.makeDirectory(path, illegal);

        path += "/" + illegal + "/";

        string terminalFolder = "Terminals";
        Save.makeDirectory(path, terminalFolder);

        path += "/" + terminalFolder;


        foreach(Terminal ter in this.allTerminals) {

            //makes terminal directory
            Save.makeDirectory(path, ter.Name);
            string tempPath = path + "/" + ter.Name + "/";

            //saves terminal json
            Save.saveJson<TerminalData>(new TerminalData(ter), tempPath, ter.Name + ".json");

            //makes directory for logic graphs
            Save.makeDirectory(tempPath, "LogicGraphs");
            tempPath += "/LogicGraphs";

            for (int i = 0; i < ter.getExtentionLength(); i++) {

                TExtension extension = ter.getExtentionAt(i);
                if(extension.GetType() == typeof(LogicGraph)) {

                    string name = extension.Name + ".json";

                    LogicGraph lg = (LogicGraph)extension;
                    Save.saveJson<LogicGraphData>(new LogicGraphData(lg), tempPath, name);
                }
            }
        }
    }

    public void loadButton() {

        //hide all the windows
        GameObject windowManagerGO = GameObject.Find("WindowManager");
        WindowManager wm = windowManagerGO.GetComponent<WindowManager>();
        wm.setActivityofCurrentWindows(false);

        //spawn load window
        wm.spawnWindow(new Window("Load Save", 300, 250, new LoadTerminalContent(wm, this.terminalManager)));

        //disable other window from spawning
        wm.AllowSpawnWindows = false;
    }

    private void updateVisuals() {
        this.terminalManager.updateAllTerminalVisuals();
    }

    private void updateData() {

        foreach(TerminalController terCon in this.terminalManager.TerminalControllers) {
            terCon.Terminal.update();
        }
    }
}
