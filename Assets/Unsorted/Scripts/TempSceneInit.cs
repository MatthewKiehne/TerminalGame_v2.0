using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class TempSceneInit : MonoBehaviour {

    //private List<Terminal> terminals = new List<Terminal>();
    private TerminalManager terminalManager;

    [SerializeField]
    private Sprite truthTableSprite;

    [SerializeField]
    private Sprite logicGraphControls;


    private void showComponent(GraphComponent component, Sprite sprite) {

        List<ComponentPiece> pieces = component.ComponentPieces;

        foreach (ComponentPiece piece in pieces) {
            showRect(piece.Rect, sprite);
        }
    }

    private void showRect(Rect rect, Sprite sprite) {
        GameObject go = new GameObject();
        go.transform.position = rect.center;
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        go.transform.localScale = rect.size;
    }


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

        testGraph.addComponent(new NotGate(new Vector2Int(1, 4), 0, false));
        testGraph.addComponent(new NotGate(new Vector2Int(1, 1), 0, false));
        testGraph.connectGraph();

        firstTerminal.addExtension(testGraph);
        firstTerminal.addExtension(emptyGraph);

        Terminal secondTerminal = new Terminal("Second Terminal");
        secondTerminal.addExtension(new LogicGraph(20, 20, "Mini Graph", Vector2Int.zero));

        Terminal emptyTerminal = new Terminal("Blank Graphs");
        for(int i = 0; i < 10; i++) {
            LogicGraph tempEmptyGraph = new LogicGraph(500, 500, "Graph No." + (i + 1), Vector2Int.zero);
            emptyTerminal.addExtension(tempEmptyGraph);
        }

        List<Terminal> terminalList = new List<Terminal>();
        terminalList.Add(firstTerminal);
        terminalList.Add(secondTerminal);
        terminalList.Add(emptyTerminal);

        GameObject terminalManagerGO = new GameObject("Terminal Manager");
        this.terminalManager = terminalManagerGO.AddComponent<TerminalManager>();
        
        foreach(Terminal ter in terminalList) {
            this.terminalManager.displayTerminal(ter);
        }
    }

    public void allTerminalWindow() {

        //window test
        WindowContent testContents = new TerminalListContent(this.terminalManager);
        Window win = new Window("All Terminals", 200, 200, false, testContents);

        GameObject windowManagerGO = GameObject.Find("WindowManager");
        WindowManager wm = windowManagerGO.GetComponent<WindowManager>();

        wm.spawnWindow(win);
    }

    public void truthTableWindow() {

        ImageContent testContents = new ImageContent(this.truthTableSprite);
        Window win = new Window("Truth Tables", 200, 200, false, testContents);

        GameObject windowManagerGO = GameObject.Find("WindowManager");
        WindowManager wm = windowManagerGO.GetComponent<WindowManager>();

        wm.spawnWindow(win);
    }

    public void logicGraphControlWindow() {

        ImageContent testContents = new ImageContent(this.logicGraphControls);
        Window win = new Window("Logic Graph Controls", 200, 200, false, testContents);

        GameObject windowManagerGO = GameObject.Find("WindowManager");
        WindowManager wm = windowManagerGO.GetComponent<WindowManager>();

        wm.spawnWindow(win);

    }

    public void clickLink(string str1, string str2, int num) {
        Debug.Log(str1 + "," + str2 + "," + num);
    }

    public void updateAllTerminals() {
        //updates all the terminals

        this.updateData();
        this.updateVisuals();
        
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
