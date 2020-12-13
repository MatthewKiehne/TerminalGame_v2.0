using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class TempSceneInit : MonoBehaviour {

    private TerminalManager terminalManager;

    [SerializeField]
    private Sprite logicGraphControls;

    void Start() {

        string t = File.ReadAllText("C:/Users/New_User/Desktop/test.txt");
        Debug.Log(t);

        string path = Application.persistentDataPath + "/Stories/MyStory";

        Debug.Log(path);
        string[] mods = { "C:\\Users\\New_User\\AppData\\LocalLow\\MyCompany\\Logic Gate Simulator\\Mods\\ExampleMod" };
        GameStory.CreateNewStory(path, mods);

        //story.loadMod("C:\\Users\\New_User\\Documents\\Unity Programs\\TerminalGame_v2.0\\Assets\\.Mods\\ExampleMod");

        //reads the game objects into memory
        TextAsset text = Resources.Load<TextAsset>("Default/Text/InitSceneLoad/GameScene");

        if (text != null) {
            StringReader strReader = new StringReader(text.text);

            string line = strReader.ReadLine();

            while (line != null) {

                if (!line.Equals("") && !line.StartsWith("~") ) {
                    SceneResouces.loadResources(line);
                }
                
                line = strReader.ReadLine();
            }
        } else {
            throw new Exception("txt not found");
        }

        Terminal firstTerminal = new Terminal("First Test Terminal", new Clock(1f));

        LogicChip emptyGraph = new LogicChip(100, 100, "Empty Graph");
        firstTerminal.addExtension(emptyGraph);


        Terminal emptyTerminal = new Terminal("Blank Graphs", new Clock(2f));
        for(int i = 0; i < 4; i++) {
            LogicChip tempEmptyGraph = new LogicChip(500, 500, "Graph No." + (i + 1));
            emptyTerminal.addExtension(tempEmptyGraph);
        }

        //emptyTerminal.addExtension(new TInput("H"));

        List<Terminal> terms = new List<Terminal>();
        terms.Add(firstTerminal);
        terms.Add(emptyTerminal);

        GameObject terminalManagerGO = new GameObject("Terminal Manager");
        this.terminalManager = terminalManagerGO.AddComponent<TerminalManager>();

        Terminal[] terminals = new Terminal[] { firstTerminal, emptyTerminal };

        foreach(Terminal terminal in terminals) {
            GameObject go = new GameObject(terminal.Name);
            TerminalController terminalController = go.AddComponent<TerminalController>();
            terminalController.setUp(terminal);
            this.terminalManager.addTerminalController(terminalController);
        }
    }

    public void updateAllTerminals() {

        foreach(TerminalController controller in terminalManager.TerminalControllers) {
            controller.updateTime(controller.Terminal.Clock.MaxTime);
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

    public void saveButton() {
        //saves the current state of all the terminals

        string path = Application.dataPath + "/Default/Resources/Default/Saves";

        EnterTextContent saveContent = new EnterTextContent("Enter in the name for your save", (string enteredText) => {

            string illegal = enteredText + "~" + System.DateTime.Now.ToString();
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            illegal = r.Replace(illegal, " ");

            Save.makeDirectory(path, illegal);

            path += "/" + illegal + "/";

            string terminalFolder = "Terminals";
            Save.makeDirectory(path, terminalFolder);

            path += "/" + terminalFolder;


            foreach (TerminalController ter in this.terminalManager.TerminalControllers) {

                //makes terminal directory
                Save.makeDirectory(path, ter.Terminal.Name);
                string tempPath = path + "/" + ter.Terminal.Name + "/";

                //saves terminal json
                Save.saveJson<TerminalData>(new TerminalData(ter.Terminal), tempPath, ter.Terminal.Name + ".json");

                //saves the connections 
                Save.saveJson<TExtensionConnectionsData>(new TExtensionConnectionsData(ter.Terminal), tempPath, typeof(TExtensionConnectionsData).ToString() + ".json");

                //makes directory for logic graphs
                Save.makeDirectory(tempPath, "LogicGraphs");
                tempPath += "/LogicGraphs";

                for (int i = 0; i < ter.Terminal.extensionLength(); i++) {

                    TExtension extension = ter.Terminal.extensionAt(i);
                    if (extension.GetType() == typeof(LogicChip)) {

                        string name = extension.Name + ".json";

                        LogicChip lg = (LogicChip)extension;
                        Save.saveJson<LogicGraphData>(new LogicGraphData(lg), tempPath, name);
                    }
                }
            }

        }, () => { }, 50);

        WindowManager.Instance.spawnWindow(new Window("Save Game", 200, 200, saveContent));
    }

    public void loadButton() {

        //hide all the windows
        GameObject windowManagerGO = GameObject.Find("WindowManager");
        WindowManager wm = windowManagerGO.GetComponent<WindowManager>();
        wm.setActivityofCurrentWindows(false);

        //spawn load window
        wm.spawnWindow(new Window("Load Save", 300, 250, new LoadTerminalContent(this.terminalManager)));

        //disable other window from spawning
        wm.AllowSpawnWindows = false;
    }
}
