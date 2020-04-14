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

        Terminal firstTerminal = new Terminal("First Test Terminal", 0, 0);

        Clock newClock = new Clock(5, 0, true, true);
        firstTerminal.Clock = newClock;

        LogicGraph emptyGraph = new LogicGraph(100, 100, "Empty Graph");
        LogicGraph testGraph = new LogicGraph(100, 100, "Test Graph");

        testGraph.addComponent(new NotGate(new Vector2Int(1, 4), 0, false));
        testGraph.addComponent(new NotGate(new Vector2Int(1, 1), 0, false));
        testGraph.connectGraph();

        firstTerminal.addExtension(testGraph);
        firstTerminal.addExtension(emptyGraph);

        Terminal secondTerminal = new Terminal("Second Terminal", 0, 0);
        secondTerminal.addExtension(new LogicGraph(20, 20, "Mini Graph"));

        Terminal emptyTerminal = new Terminal("Blank Graphs", 0, 0);
        for(int i = 0; i < 10; i++) {
            LogicGraph tempEmptyGraph = new LogicGraph(500, 500, "Graph No." + (i + 1));
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


        /*
        TConstant firstCon = new TConstant(3);
        terminal.addExtension(firstCon);
        TConstant secondCon = new TConstant(11);
        terminal.addExtension(secondCon);

        
        //make the receiver
        ReceiveBridge firstReceive = new ReceiveBridge(new Vector2Int(2, 0), 0, false, 8);
        graph.addComponent(firstReceive);

        ReceiveBridge secondReceive = new ReceiveBridge(new Vector2Int(12, 0), 0, false, 8);
        graph.addComponent(secondReceive);
        

        //connects the the receiver bridge
        firstCon.SendBridges[0].addBridge(firstReceive);
        secondCon.SendBridges[0].addBridge(secondReceive);

        Reflector oneOne = new Reflector(new Vector2Int(2, 1), 0, false);
        Reflector oneTwo = new Reflector(new Vector2Int(3, 2), 0, false);
        Reflector oneThree = new Reflector(new Vector2Int(4, 3), 0, false);
        Reflector oneFour = new Reflector(new Vector2Int(5, 4), 0, false);
        Reflector oneFive = new Reflector(new Vector2Int(6, 5), 0, false);
        Reflector oneSix = new Reflector(new Vector2Int(7, 6), 0, false);
        Reflector oneSeven = new Reflector(new Vector2Int(8, 7), 0, false);
        Reflector oneEight = new Reflector(new Vector2Int(9, 8), 0, false);
        graph.addComponent(oneOne);
        graph.addComponent(oneTwo);
        graph.addComponent(oneThree);
        graph.addComponent(oneFour);
        graph.addComponent(oneFive);
        graph.addComponent(oneSix);
        graph.addComponent(oneSeven);
        graph.addComponent(oneEight);

        Reflector twoOne = new Reflector(new Vector2Int(12, 10), 0, false);
        Reflector twoTwo = new Reflector(new Vector2Int(13, 11), 0, false);
        Reflector twoThree = new Reflector(new Vector2Int(14, 12), 0, false);
        Reflector twoFour = new Reflector(new Vector2Int(15, 13), 0, false);
        Reflector twoFive = new Reflector(new Vector2Int(16, 14), 0, false);
        Reflector twoSix = new Reflector(new Vector2Int(17, 15), 0, false);
        Reflector twoSeven = new Reflector(new Vector2Int(18, 16), 0, false);
        Reflector twoEight = new Reflector(new Vector2Int(19, 17), 0, false);
        graph.addComponent(twoOne);
        graph.addComponent(twoTwo);
        graph.addComponent(twoThree);
        graph.addComponent(twoFour);
        graph.addComponent(twoFive);
        graph.addComponent(twoSix);
        graph.addComponent(twoSeven);
        graph.addComponent(twoEight);



        for (int i = 0; i < 8; i++) {
            Vector2Int startPos = new Vector2Int((i * 8) + 20, 14);

            Reflector oneIn = new Reflector(startPos + new Vector2Int(2, i - 13 ), 2, false);
            graph.addComponent(oneIn);

            Reflector twoIn = new Reflector(startPos + new Vector2Int(3, i - 4), 2, false);
            graph.addComponent(twoIn);

            Splitter aSplit = new Splitter(startPos + new Vector2Int(2, 9), 0, false);
            graph.addComponent(aSplit);

            Splitter bSplit = new Splitter(startPos + new Vector2Int(3, 8), 0, false);
            graph.addComponent(bSplit);

            LogicComponent firstXor = new XorGate(startPos + new Vector2Int(2, 10), 0, false);
            graph.addComponent(firstXor);

            Splitter afterXorSplit = new Splitter(startPos + new Vector2Int(2, 12), 0, false);
            graph.addComponent(afterXorSplit);

            Splitter cInSplit = new Splitter(startPos + new Vector2Int(3, 13), 0, false);
            graph.addComponent(cInSplit);

            XorGate secondXor = new XorGate(startPos + new Vector2Int(2, 15), 0, false);
            graph.addComponent(secondXor);

            AndGate firstCalc = new AndGate(startPos + new Vector2Int(4, 12), 1, false);
            graph.addComponent(firstCalc);

            AndGate secondCalc = new AndGate(startPos + new Vector2Int(4, 8), 1, false);
            graph.addComponent(secondCalc);

            Reflector firstCalcRef = new Reflector(startPos + new Vector2Int(6, 9), 2, false);
            graph.addComponent(firstCalcRef);

            Reflector secondCalfRef = new Reflector(startPos + new Vector2Int(6, 12), 0, false);
            graph.addComponent(secondCalfRef);

            OrGate cout = new OrGate(startPos + new Vector2Int(7, 12), 1, false);
            graph.addComponent(cout);

            Reflector reflectOut = new Reflector(startPos + new Vector2Int(2, 17 + i), 1, false);
            graph.addComponent(reflectOut);
        }

        



        //connects the graph
        graph.connectGraph();

        */

        /*
        Vector3 offSet = new Vector3(8, 12, 0);
        GameObject logicGraphManager = new GameObject("LogicGraphManager");
        LogicGraphManager lgm = logicGraphManager.AddComponent<LogicGraphManager>();
        lgm.displayLogicGraph(graph, offSet, Camera.main);

        LogicGraphCameraController mainCameraController = Camera.main.GetComponent<LogicGraphCameraController>();
        mainCameraController.setUp(offSet, graph);

        this.terminal = terminal;
        this.logicGraphManager = lgm;
        */

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
        Window win = new Window("All Terminals", 200, 200, false, testContents);

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


    private Spaceship loadShip() {
        Spaceship ship = null;

        TextAsset text = Resources.Load<TextAsset>("Text/Saves/ShipData");

        if (text != null) {

            ship = new Spaceship();

            //gets the current line
            StringReader reader = new StringReader(text.text);
            string line = reader.ReadLine();
            char[] whiteSpace = new char[] { ' ' };

            if (!line.Equals("~SP")) {
                throw new Exception("ShipData.txt does not start with: ~Sp");
            }

            line = reader.ReadLine();

            //read in ship data
            while (!line.Equals("~TR")) {
                line = reader.ReadLine();
            }

            line = reader.ReadLine();

            //read in terminal data
            while (!line.Equals("~CK")) {

                string[] parts = line.Split(whiteSpace);

                if (parts[0].Equals("t")) {
                    int x = int.Parse(parts[1]);
                    int y = int.Parse(parts[2]);

                    Terminal ter = new Terminal("Temp Terminal Name", x, y);
                    ship.Terminals.Add(ter);
                }

                line = reader.ReadLine();
            }

            line = reader.ReadLine();

            //read in clock data
            while (!line.Equals("~LG")) {

                string[] parts = line.Split(whiteSpace);

                int indexOfTerminal = int.Parse(parts[0]);
                int updatesPerSecond = int.Parse(parts[1]);
                float timePassed = float.Parse(parts[2]);

                bool canStep = false;
                if (parts[3].Equals("1")) {
                    canStep = true;
                }
                bool canSlowDown = false;
                if (parts[4].Equals("1")) {
                    canSlowDown = true;
                }
                Clock currentClock = new Clock(updatesPerSecond, timePassed, canStep, canSlowDown);
                ship.Terminals[indexOfTerminal].Clock = currentClock;

                line = reader.ReadLine();
            }

            line = reader.ReadLine();

            //read in Logic Graph data
            while (!line.Equals("~LC")) {

                string[] parts = line.Split(whiteSpace);
                int terminalIndex = int.Parse(parts[0]);
                int width = int.Parse(parts[1]);
                int height = int.Parse(parts[2]);

                LogicGraph lg = new LogicGraph(width, height, "Place Holder Graph Name");
                ship.Terminals[terminalIndex].addExtension(lg);

                line = reader.ReadLine();
            }

            line = reader.ReadLine();

            //read in logic components data
            while (!line.Equals("~CC")) {

                GraphComponent gc = null;

                string[] parts = line.Split(whiteSpace);

                //gets the terminal and graph in the terminal
                int terminalIndex = int.Parse(parts[0]);
                int logicGraphIndex = int.Parse(parts[1]);

                //gets the data from the line
                string componentName = parts[2];
                int xPos = int.Parse(parts[3]);
                int yPos = int.Parse(parts[4]);
                int numRotaions = int.Parse(parts[5]);
                bool flipped = false;
                if (parts[6].Equals("1")) {
                    flipped = true;
                }

                if (componentName.Equals("notGate")) {
                    gc = new NotGate(new Vector2Int(xPos, yPos), numRotaions, flipped);
                } else if (componentName.Equals("andGate")) {
                    gc = new AndGate(new Vector2Int(xPos, yPos), numRotaions, flipped);
                } else if (componentName.Equals("orGate")) {
                    gc = new OrGate(new Vector2Int(xPos, yPos), numRotaions, flipped);
                } else if (componentName.Equals("xorGate")) {
                    gc = new XorGate(new Vector2Int(xPos, yPos), numRotaions, flipped);
                } else if (componentName.Equals("xnorGate")) {
                    gc = new XnorGate(new Vector2Int(xPos, yPos), numRotaions, flipped);
                } else if (componentName.Equals("bufferGate")) {
                    gc = new BufferGate(new Vector2Int(xPos, yPos), numRotaions, flipped);
                } else if (componentName.Equals("norGate")) {
                    gc = new NorGate(new Vector2Int(xPos, yPos), numRotaions, flipped);
                } else if (componentName.Equals("nandGate")) {
                    gc = new NandGate(new Vector2Int(xPos, yPos), numRotaions, flipped);
                } else if (componentName.Equals("passingMirror")) {
                    gc = new Splitter(new Vector2Int(xPos, yPos), numRotaions, flipped);
                } else if (componentName.Equals("Mirror")) {
                    gc = new Reflector(new Vector2Int(xPos, yPos), numRotaions, flipped);
                }

                if (gc == null) {
                    throw new Exception("the name of the logic componet:" + componentName + " was not found");
                }

                ((LogicGraph)(ship.Terminals[terminalIndex].getExtentionAt(logicGraphIndex))).addComponent(gc);

                line = reader.ReadLine();
            }

            line = reader.ReadLine();
            //read in locic compoent connections
            while (line != null) {

                string[] parts = line.Split(whiteSpace);

                //gets the data from the line
                int terminalIndex = int.Parse(parts[0]);
                int logicGraphIndex = int.Parse(parts[1]);
                int senderIndex = int.Parse(parts[2]);
                int senderSenderIndex = int.Parse(parts[3]);
                int recieverIndex = int.Parse(parts[4]);
                int recieverRecieverIndex = int.Parse(parts[5]);

                //connects the components
                LogicGraph lg = (LogicGraph)ship.Terminals[terminalIndex].getExtentionAt(logicGraphIndex);
                LogicComponent sender = lg.getLogicComponentAt(senderIndex);
                LogicComponent receiver = lg.getLogicComponentAt(recieverIndex);
                sender.getSenderAt(senderSenderIndex).addTarget(receiver.getReceiverAt(recieverRecieverIndex));

                line = reader.ReadLine();
            }
        }

        return ship;
    }

    private void makeSave() {

        DateTime dt = System.DateTime.Now;
        string dateTimeName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        string baseDirectoryPath = Application.dataPath + "/Saves/" + dateTimeName;
        Directory.CreateDirectory(baseDirectoryPath);

        string[] fileNames = new string[] { "/LogicGraphs", "/SaveData" };

        foreach (string filePath in fileNames) {
            Directory.CreateDirectory(baseDirectoryPath + filePath);
        }
    }

    private string getNewestSave() {

        string[] saves = Directory.GetDirectories(Application.dataPath + "/Saves");

        string result = "";

        if (saves.Length > 0) {

            Regex rx = new Regex(@"(\d+)$");

            foreach (string s in saves) {
                if (s.CompareTo(result) > 0) {
                    result = s;
                }
            }

        }

        return result;
    }

    private void loadSave(string path) {

        //parse all files into new graphs
        string logicGraphsPath = path + "/LogicGraphs";
        List<LogicGraph> logicGraphs = new List<LogicGraph>();

        string[] logicGraphFiles = Directory.GetFiles(logicGraphsPath);

        foreach (string logicFile in logicGraphFiles) {
        }
    }
}
