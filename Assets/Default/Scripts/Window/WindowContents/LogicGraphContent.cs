using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogicGraphContent : WindowContent {

    //tells what state the editor is in
    private enum EditorState { AddCompoent, DeleteComponent }
    private EditorState currentState = EditorState.AddCompoent;

    //manager and controller
    private LogicGraphManager logicGraphManager;
    private LogicGraphController logicGraphController;
    private LogicGraph graph;

    //ui stuff
    private Camera camera;
    private RenderTexture renderTexture;
    private RawImage rawImage;
    private RectTransform rawImageRect;

    //keeps track of component info
    private Vector2Int previousGridPosition = Vector2Int.zero;
    private int rotation = 0;
    private bool flipped = false;

    //this is the component being show on the screen
    private LightComponent component = null;

    //the mouse objet
    private GameObject mouseObject = null;

    public LogicGraphContent(LogicGraph graph, LogicGraphManager logicGraphManager) {
        this.logicGraphManager = logicGraphManager;
        this.graph = graph;
    }

    public override void changeWindowSize(int width, int height) {
        //change the size of the render texture

        //get rid of old
        this.destroyRenderTexture();

        //create the new
        this.renderTexture = new RenderTexture(width, height, 16);
        this.renderTexture.antiAliasing = 1;
        this.camera.targetTexture = this.renderTexture;
        this.rawImage.texture = this.renderTexture;
        this.renderTexture.Create();
    }

    public override void receiveBroadcast(string message) {
        //should do something
    }

    private void destroyRenderTexture() {
        //removes any connection to the render texture

        this.camera.targetTexture = null;
        this.rawImage.texture = null;
        this.renderTexture.Release();
        GameObject.Destroy(this.renderTexture);
    }

    /// <summary>
    /// Destroys the propper things
    /// </summary>
    protected override void destroyContent() {
        //when the window is closed

        //destroy mouse Object
        GameObject.Destroy(this.mouseObject);

        //destroy logic graph
        this.logicGraphController.destroy();

        //release camera target texture
        this.destroyRenderTexture();

        //remove camera from camera manager
        GameObject.Find("CameraManager").GetComponent<CameraManager>().removeCamera(this.camera);
        //this.cameraManager.removeCamera(this.camera);
    }

    public override void spawnContents(WindowController windowController, Transform contentPanel, Canvas canvas) {

        CameraManager cameraManager = GameObject.Find("CameraManager").GetComponent<CameraManager>();
        Camera cam = cameraManager.makeNewCamera(false);
        this.camera = cam;

        Vector2 graphPosition = this.camera.transform.position;
        this.logicGraphController = this.logicGraphManager.displayLogicGraph(this.graph, graphPosition);

        GameObject image = GameObject.Instantiate((GameObject)SceneResouces.SceneObjects["Default"][typeof(GameObject)]["GraphEditor"]);

        this.rawImage = image.GetComponent<RawImage>();
        image.transform.SetParent(contentPanel.transform, false);
        this.rawImageRect = image.GetComponent<RectTransform>();

        //only for testing
        this.mouseObject = new GameObject("Mouse Graph Position");
        this.mouseObject.transform.position = new Vector3(this.camera.transform.position.x, this.camera.transform.position.y, 0);

        this.renderTexture = new RenderTexture(512, 512, 16);
        this.rawImage.texture = this.renderTexture;

        this.camera.targetTexture = this.renderTexture;
        this.renderTexture.Create();

        LogicGraphCameraController lgcc = this.camera.gameObject.AddComponent<LogicGraphCameraController>();
        lgcc.setUp(graphPosition, this.logicGraphController.Graph, this.inputs, this.rawImage);

        #region GUIButtons

        Transform basePanel = image.transform.Find("Panel");

        Button andButton = basePanel.Find("AndButton").GetComponent<Button>();
        Button orButton = basePanel.Find("OrButton").GetComponent<Button>();
        Button notButton = basePanel.Find("NotButton").GetComponent<Button>();
        Button bufferButton = basePanel.Find("BufferButton").GetComponent<Button>();
        Button nandButton = basePanel.Find("NandButton").GetComponent<Button>();
        Button norButton = basePanel.Find("NorButton").GetComponent<Button>();
        Button xorButton = basePanel.Find("XorButton").GetComponent<Button>();
        Button xnorButton = basePanel.Find("XnorButton").GetComponent<Button>();
        Button reflector = basePanel.Find("ReflectorButton").GetComponent<Button>();
        Button splitterButton = basePanel.Find("SplitterButton").GetComponent<Button>();
        Button sendBridgeButton = basePanel.Find("SendBridgeButton").GetComponent<Button>();
        Button receiveBridgeButton = basePanel.Find("ReceiveBridgeButton").GetComponent<Button>();
        Button deleteButton = basePanel.Find("DeleteButton").GetComponent<Button>();

        //adds the component to the button
        andButton.onClick.AddListener(() => {
            this.component = new AndGate(this.previousGridPosition, this.rotation, this.flipped);
            this.changeComponent();
        });
        orButton.onClick.AddListener(() => {
            this.component = new OrGate(this.previousGridPosition, this.rotation, this.flipped);
            this.changeComponent();
        });
        notButton.onClick.AddListener(() => {
            this.component = new NotGate(this.previousGridPosition, this.rotation, this.flipped);
            this.changeComponent();
        });
        bufferButton.onClick.AddListener(() => {
            this.component = new BufferGate(this.previousGridPosition, this.rotation, this.flipped);
            this.changeComponent();
        });
        norButton.onClick.AddListener(() => {
            this.component = new NorGate(this.previousGridPosition, this.rotation, this.flipped);
            this.changeComponent();
        });
        xorButton.onClick.AddListener(() => {
            this.component = new XorGate(this.previousGridPosition, this.rotation, this.flipped);
            this.changeComponent();
        });
        xnorButton.onClick.AddListener(() => {
            this.component = new XnorGate(this.previousGridPosition, this.rotation, this.flipped);
            this.changeComponent();
        });
        nandButton.onClick.AddListener(() => {
            this.component = new NandGate(this.previousGridPosition, this.rotation, this.flipped);
            this.changeComponent();
        });
        splitterButton.onClick.AddListener(() => {
            this.component = new Splitter(this.previousGridPosition, this.rotation, this.flipped);
            this.changeComponent();
        });
        reflector.onClick.AddListener(() => {
            this.component = new Reflector(this.previousGridPosition, this.rotation, this.flipped);
            this.changeComponent();
        });
        sendBridgeButton.onClick.AddListener(() => {
            this.component = new GraphOutput(this.previousGridPosition, this.rotation, this.flipped, new SendBridge("Blank"));
            this.changeComponent();
        });
        receiveBridgeButton.onClick.AddListener(() => {
            this.component = new GraphInput(this.previousGridPosition, this.rotation, this.flipped, new ReceiveBridge("Blank"));
            this.changeComponent();
        });

        deleteButton.onClick.AddListener(() => {
            this.currentState = EditorState.DeleteComponent;
            this.destroyMouseChildren();
            GameObject xSprite = this.makeX().gameObject;
            xSprite.transform.SetParent(this.mouseObject.transform);
            xSprite.transform.localPosition = new Vector2(.5f, .5f);

        });

        //selects the first button
        andButton.onClick.Invoke();
        andButton.Select();

        #endregion

        #region KeyboardInputs
        //rotations
        this.inputs.addInput(new KeyCombination(KeyCode.Q, KeyStatus.Down), () => {
            this.rotation = (rotation + 1) % 4;
            this.changeComponent();
        });
        this.inputs.addInput(new KeyCombination(KeyCode.E, KeyStatus.Down), () => {
            this.rotation = this.rotation - 1;
            if (this.rotation < 0) {
                rotation = 3;
            }
            this.changeComponent();
        });

        //flip
        this.inputs.addInput(new KeyCombination(KeyCode.F, KeyStatus.Down), () => {
            this.flipped = !flipped;
            this.changeComponent();
        });

        //mouse rest
        this.inputs.addInput(new KeyCombination(KeyCode.Mouse0, KeyStatus.Rest), () => {


            if (this.logicGraphController.BottomLeftWorld != this.previousGridPosition) {

                Vector2Int currentMousePos = this.getMouseLocalGridPosition();

                if (!this.previousGridPosition.Equals(currentMousePos)) {

                    this.previousGridPosition = currentMousePos;

                    if (this.currentState == EditorState.AddCompoent) {

                        this.component.Position = this.previousGridPosition;
                        bool canPlace = this.logicGraphController.Graph.LightGraph.canPlace(this.component);

                        this.mouseObject.transform.Find("X").gameObject.SetActive(!canPlace);
                        Vector2 worldMousePosition = this.getMouseWorldGridPosition();
                        this.mouseObject.transform.position = worldMousePosition;

                    } else if (this.currentState == EditorState.DeleteComponent) {

                        Vector2 worldMousePosition = this.getMouseWorldGridPosition();
                        this.mouseObject.transform.position = worldMousePosition;

                        GraphComponent graphComp = this.logicGraphController.Graph.LightGraph.getComponentAt(
                            this.previousGridPosition.x, this.previousGridPosition.y);

                        SpriteRenderer rend = this.mouseObject.transform.GetChild(0).GetComponent<SpriteRenderer>();

                        if (graphComp != null) {
                            rend.color = Color.red;
                        } else {
                            rend.color = Color.gray;
                        }
                    }
                }
            }
        });

        //mouse down
        this.inputs.addInput(new KeyCombination(KeyCode.Mouse0, KeyStatus.Down), () => {

            if (this.currentState == EditorState.AddCompoent) {
                //adds the component
                LightComponent comp = this.duplicateAt(this.component.GetType(), this.previousGridPosition, this.rotation, this.flipped);

                if (this.logicGraphController.Graph.LightGraph.canPlace(comp) &&
                this.inputs.CurrentFrameData.RaycastResults.Count != 0 &&
                this.inputs.CurrentFrameData.RaycastResults[0].gameObject.Equals(this.rawImage.gameObject)) {

                    if (comp.GetType().IsSubclassOf(typeof(LinkComponent))) {

                        EnterTextContent etc = new EnterTextContent(
                            "Type in a unique name for the Bridge." +
                            " The name can not be the same as another bridge on this Graph", 
                            (string inputFieldText) => {
                                //confrim
                                ((LinkComponent)comp).getExtensionConnection().Name = inputFieldText;
                                addComponentToGraph(comp);
                            },
                            () => {}, 
                            15);

                        //names exits
                        etc.addErrorCheck((string value) => {

                            bool result = true;
                            int counter = 0;

                            List<ExtensionConnection> checkBridges = this.logicGraphController.Graph.AllBridges();

                            while(result && counter < checkBridges.Count) {

                                if (value.Equals(checkBridges[counter].Name)) {
                                    result = false;
                                }

                                counter++;
                            }

                            return result;

                        }, "Name Already Exits");

                        //can place component after entering value
                        etc.addErrorCheck((string value) => {
                            return this.logicGraphController.Graph.LightGraph.canPlace(comp);
                        }, "Can not Place there");

                        this.spawnChildWindow(new Window(comp.GetType() + " Name", 200, 200, etc));

                    } else {

                        this.addComponentToGraph(comp);
                    }
                }
            } else if (currentState == EditorState.DeleteComponent) {
                //removes the component

                Vector2Int gridPosition = this.getMouseLocalGridPosition();
                LightComponent hitComponent = this.graph.LightGraph.getComponentAt(gridPosition.x, gridPosition.y);

                if (hitComponent != null) {

                    bool removedGOState = this.logicGraphController.ComponentManager.removeComponent(hitComponent);
                    bool removeDataState = this.graph.LightGraph.removeComponent(hitComponent);

                    if (removedGOState && removeDataState) {
                        this.logicGraphController.ComponentManager.reconnectRays();
                    } else {
                        throw new System.Exception("ComponentNotFound: GameObjectRemoval:" + removedGOState + " DataRemoval" + removeDataState);
                    }
                }
            }
        });

        #endregion
    }

    /// <summary>
    /// Add a Light Component to the Logic Graph
    /// </summary>
    private void addComponentToGraph(LightComponent comp) {
        this.logicGraphController.Graph.LightGraph.addComponent(comp);
        GraphComponentController gcc = this.logicGraphController.ComponentManager.createComponent(comp);
        this.logicGraphController.ComponentManager.addComponent(gcc);
        this.logicGraphController.ComponentManager.reconnectRays();
    }

    private Vector2 getMouseWorldPoint() {
        //gets the world point for the mouse

        Vector2 graphPosition = this.logicGraphController.BottomLeftWorld;
        Vector2 mousePos = inputs.CurrentFrameData.MousePosition;

        Vector2 bottomLeftImage = (Vector2)this.rawImageRect.position -
            new Vector2(this.rawImageRect.sizeDelta.x / 2f, this.rawImageRect.sizeDelta.y / 2f);

        Vector2 veiwPort = (mousePos - bottomLeftImage) / this.rawImageRect.sizeDelta;

        return this.camera.ViewportToWorldPoint(veiwPort);

        //return worldPoint = new Vector2(Mathf.Floor(worldPoint.x), Mathf.Floor(worldPoint.y));
    }

    private Vector2Int getMouseWorldGridPosition() {
        //gets the world position of the mouse on the grid

        Vector3 worldPoint = this.getMouseWorldPoint();
        Vector2 graphPosition = this.logicGraphController.BottomLeftWorld;

        worldPoint = new Vector3(Mathf.Floor(worldPoint.x), Mathf.Floor(worldPoint.y), 0);

        if (worldPoint.x < graphPosition.x) {
            worldPoint.x = graphPosition.x;
        } else if (worldPoint.x > graphPosition.x + this.logicGraphController.Graph.LightGraph.Width - 1) {
            worldPoint.x = graphPosition.x + this.logicGraphController.Graph.LightGraph.Width - 1;
        }
        if (worldPoint.y < graphPosition.y) {
            worldPoint.y = graphPosition.y;
        } else if (worldPoint.y > graphPosition.y + this.logicGraphController.Graph.LightGraph.Height - 1) {
            worldPoint.y = graphPosition.y + this.logicGraphController.Graph.LightGraph.Height - 1;
        }

        return new Vector2Int(Mathf.FloorToInt(worldPoint.x), Mathf.FloorToInt(worldPoint.y));
    }

    private Vector2Int getMouseLocalGridPosition() {
        //gets the local position of the mouse on the grid

        Vector2Int worldPoint = this.getMouseWorldGridPosition();
        Vector2 graphPosition = this.logicGraphController.BottomLeftWorld;

        return new Vector2Int(Mathf.FloorToInt(worldPoint.x - graphPosition.x), Mathf.FloorToInt(worldPoint.y - graphPosition.y));
    }

    private LightComponent duplicateAt(Type type, Vector2Int position, int rotation, bool flipped) {
        //makes a duplicate of the component passed in

        LightComponent result = null;

        if (type == typeof(AndGate)) {
            result = new AndGate(position, rotation, flipped);
        } else if (type == typeof(OrGate)) {
            result = new OrGate(position, rotation, flipped);
        } else if (type == typeof(NotGate)) {
            result = new NotGate(position, rotation, flipped);
        } else if (type == typeof(BufferGate)) {
            result = new BufferGate(position, rotation, flipped);
        } else if (type == typeof(NandGate)) {
            result = new NandGate(position, rotation, flipped);
        } else if (type == typeof(XorGate)) {
            result = new XorGate(position, rotation, flipped);
        } else if (type == typeof(XnorGate)) {
            result = new XnorGate(position, rotation, flipped);
        } else if (type == typeof(NorGate)) {
            result = new NorGate(position, rotation, flipped);
        } else if (type == typeof(Splitter)) {
            result = new Splitter(position, rotation, flipped);
        } else if (type == typeof(Reflector)) {
            result = new Reflector(position, rotation, flipped);
        } else if (type == typeof(GraphOutput)) {
            result = new GraphOutput(position, rotation, flipped, new SendBridge("Blank"));
        } else if (type == typeof(GraphInput)) {
            result = new GraphInput(position, rotation, flipped, new ReceiveBridge("Blank"));
        } else {
            throw new System.Exception(type + " was not found when selecting the from the logic graph editor");
        }

        return result;
    }

    private void changeComponent() {
        //updates rotation, flipped, and compoent type of the visuals

        this.currentState = EditorState.AddCompoent;

        this.destroyMouseChildren();

        this.component = this.duplicateAt(this.component.GetType(), this.previousGridPosition, this.rotation, this.flipped);

        GraphComponentController gcc = this.logicGraphController.ComponentManager.createComponent(this.component);
        gcc.transform.SetParent(this.mouseObject.transform);
        Rect dmn = this.component.getDimentions();
        gcc.transform.localPosition = new Vector3(dmn.width / 2f, dmn.height / 2f, 0);

        //deactivates the sender controllers
        if (this.component.GetType().IsSubclassOf(typeof(InteractiveComponent))) {

            foreach (Transform child in gcc.transform) {

                SpriteRenderer rend = child.GetComponent<SpriteRenderer>();

                if (rend != null) {
                    rend.sortingLayerName = "Mouse";
                }

                SenderController sendCon = child.GetComponent<SenderController>();

                if (sendCon != null) {
                    GameObject.Destroy(sendCon);
                }
            }
        }

        if (this.component.GetType().IsSubclassOf(typeof(InteractiveComponent))) {
            gcc.transform.Find("MiddleBody").Find("LogicComponentSprite").GetComponent<SpriteRenderer>().sortingLayerName = "Mouse";
        }

        //adds the X to the sprite
        SpriteRenderer xGO = this.makeX();
        xGO.transform.SetParent(this.mouseObject.transform);
        xGO.color = Color.red;
        xGO.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        xGO.transform.position = gcc.transform.position;

        bool placeable = this.logicGraphController.Graph.LightGraph.canPlace(this.component);

        if (placeable) {
            xGO.gameObject.SetActive(false);
        }
    }

    private void destroyMouseChildren() {
        //destroys all children
        foreach (Transform child in this.mouseObject.transform) {
            GameObject.Destroy(child.gameObject);
        }
    }

    private SpriteRenderer makeX() {
        //makes the red "X" for the graph

        GameObject xGO = new GameObject("X");
        Texture2D xTex = (Texture2D)SceneResouces.SceneObjects["Default"][typeof(Texture2D)]["X"];
        SpriteRenderer xRende = xGO.AddComponent<SpriteRenderer>();
        Sprite xSprite = Sprite.Create(xTex, new Rect(0, 0, xTex.width, xTex.height), Vector2.one * .5f, xTex.width);
        xRende.sprite = xSprite;
        xRende.sortingLayerName = "Mouse";
        xRende.sortingOrder = 15;

        return xRende;
    }

    public override bool sameContent(WindowContent content) {
        return content.GetType() == this.GetType() &&
            ((LogicGraphContent)content).graph.Equals(this.graph);
    }
}
