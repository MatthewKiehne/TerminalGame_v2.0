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
    private Type componentType = null;

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
        this.camera.targetTexture = this.renderTexture;
        this.rawImage.texture = this.renderTexture;
        this.renderTexture.Create();

    }

    private void destroyRenderTexture() {
        //removes any connection to the render texture

        this.camera.targetTexture = null;
        this.rawImage.texture = null;
        this.renderTexture.Release();
        GameObject.Destroy(this.renderTexture);
    }

    public override void onDestroy() {
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

        GameObject image = GameObject.Instantiate((GameObject)SceneResouces.SceneObjects[typeof(GameObject)]["GraphEditor"]);

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
        /*
        Button sendBridgeButton = basePanel.Find("SendBridgeButton").GetComponent<Button>();
        Button receiveBridgeButton = basePanel.Find("ReceiveBridgeButton").GetComponent<Button>();
        */
        Button deleteButton = basePanel.Find("DeleteButton").GetComponent<Button>();
        
        //adds the component to the button
        andButton.onClick.AddListener(() => {
            this.componentType = typeof(AndGate);
            this.changeComponent();
        });
        orButton.onClick.AddListener(() => {
            this.componentType = typeof(OrGate);
            this.changeComponent();
        });
        notButton.onClick.AddListener(() => {
            this.componentType = typeof(NotGate);
            this.changeComponent();
        });
        bufferButton.onClick.AddListener(() => {
            this.componentType = typeof(BufferGate);
            this.changeComponent();
        });
        norButton.onClick.AddListener(() => {
            this.componentType = typeof(NorGate);
            this.changeComponent();
        });
        xorButton.onClick.AddListener(() => {
            this.componentType = typeof(XorGate);
            this.changeComponent();
        });
        xnorButton.onClick.AddListener(() => {
            this.componentType = typeof(XnorGate);
            this.changeComponent();
        });
        nandButton.onClick.AddListener(() => {
            this.componentType = typeof(NandGate);
            this.changeComponent();
        });
        splitterButton.onClick.AddListener(() => {
            this.componentType = typeof(Splitter);
            this.changeComponent();
        });
        reflector.onClick.AddListener(() => {
            this.componentType = typeof(Reflector);
            this.changeComponent();
        });
        /*
        sendBridgeButton.onClick.AddListener(() => {
            this.component = new SendBridge(this.previousGridPosition, this.rotation, this.flipped, 8);
            this.changeComponent();
        });
        receiveBridgeButton.onClick.AddListener(() => {
            this.component = new ReceiveBridge(this.previousGridPosition, this.rotation, this.flipped, 8);
            this.changeComponent();
        });
        */
        deleteButton.onClick.AddListener(() => {
            this.currentState = EditorState.DeleteComponent;
            this.destroyMouseChildren();
            Debug.Log(this.currentState);
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

                this.previousGridPosition = this.getMouseLocalGridPosition();

                if (this.currentState == EditorState.AddCompoent) {

                    GraphComponent made = this.duplicateAt(this.componentType, this.previousGridPosition, this.rotation, this.flipped);
                    bool canPlace = this.logicGraphController.Graph.canPlace(made);
                   
                    this.mouseObject.transform.Find("X").gameObject.SetActive(!canPlace);
                    Vector2 worldMousePosition = this.getMouseWorldGridPosition();
                    this.mouseObject.transform.position = worldMousePosition;
                    //Debug.Log(this.mouseObject.transform.position + " " + this.mouseObject.transform.GetChild(0).position);

                } else if (this.currentState == EditorState.DeleteComponent) {
                    /*
                    GraphComponent hitComponent = this.graph.getComponentAt(gridPosition.x, gridPosition.y);
                    Debug.Log(hitComponent);
                    */
                    Vector2 worldMousePosition = this.getMouseWorldGridPosition();
                    this.mouseObject.transform.position = worldMousePosition;

                    GraphComponent graphComp = this.logicGraphController.Graph.getComponentAt(
                        this.previousGridPosition.x, this.previousGridPosition.y);

                    SpriteRenderer rend = this.mouseObject.transform.GetChild(0).GetComponent<SpriteRenderer>();

                    if(graphComp != null) {
                        rend.color = Color.red;
                    } else {
                        rend.color = Color.gray;
                    }
                }
            }
        });

        //mouse down
        this.inputs.addInput(new KeyCombination(KeyCode.Mouse0, KeyStatus.Down), () => {

            if (this.currentState == EditorState.AddCompoent) {
                //adds the component
                GraphComponent comp = this.duplicateAt(this.componentType, this.previousGridPosition, this.rotation, this.flipped);

                if (this.logicGraphController.Graph.canPlace(comp) &&
                this.inputs.CurrentFrameData.RaycastResults.Count != 0 &&
                this.inputs.CurrentFrameData.RaycastResults[0].gameObject.Equals(this.rawImage.gameObject)) {

                    this.logicGraphController.Graph.addComponentAndConnect(comp);
                    GraphComponentController gcc = this.logicGraphController.ComponentManager.createComponent(comp);
                    this.logicGraphController.ComponentManager.addComponent(gcc);
                    this.logicGraphController.ComponentManager.reconnectRays();

                }
            } else if(currentState == EditorState.DeleteComponent) {
                //removes the component

                Vector2Int gridPosition = this.getMouseLocalGridPosition();
                GraphComponent hitComponent = this.graph.getComponentAt(gridPosition.x, gridPosition.y);

                if(hitComponent != null) {

                    //Debug.Log("LogicGraphContent -> Delete: hit " + hitComponent);
                    bool removedGOState = this.logicGraphController.ComponentManager.removeComponent(hitComponent);
                    bool removeDataState = this.graph.removeComponent(hitComponent);
                    //Debug.Log("remove GO: " + removedGOState + " remove Data:" + removeDataState);

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
        } else if (worldPoint.x > graphPosition.x + this.logicGraphController.Graph.Width - 1) {
            worldPoint.x = graphPosition.x + this.logicGraphController.Graph.Width - 1;
        }
        if (worldPoint.y < graphPosition.y) {
            worldPoint.y = graphPosition.y;
        } else if (worldPoint.y > graphPosition.y + this.logicGraphController.Graph.Height - 1) {
            worldPoint.y = graphPosition.y + this.logicGraphController.Graph.Height - 1;
        }

        return new Vector2Int(Mathf.FloorToInt(worldPoint.x), Mathf.FloorToInt(worldPoint.y));
    }

    private Vector2Int getMouseLocalGridPosition() {
        //gets the local position of the mouse on the grid

        Vector2Int worldPoint = this.getMouseWorldGridPosition();
        Vector2 graphPosition = this.logicGraphController.BottomLeftWorld;

        return new Vector2Int(Mathf.FloorToInt(worldPoint.x - graphPosition.x), Mathf.FloorToInt(worldPoint.y - graphPosition.y));
    }

    private GraphComponent duplicateAt(Type type, Vector2Int position, int rotation, bool flipped) {
        //makes a duplicate of the component passed in

        GraphComponent result = null;

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
        } else if (type == typeof(SendBridge)) {
            result = new SendBridge(position, rotation, flipped, 8);
        } else if (type == typeof(ReceiveBridge)) {
            result = new ReceiveBridge(position, rotation, flipped, 8);
        } else {
            throw new System.Exception(type + " was not found when selecting the from the logic graph editor");
        }

        return result;
    }

    private void changeComponent() {
        //updates rotation, flipped, and compoent type of the visuals

        this.currentState = EditorState.AddCompoent;

        this.destroyMouseChildren();

        GraphComponent madeComponent = this.duplicateAt(this.componentType, this.previousGridPosition, this.rotation, this.flipped);

        GraphComponentController gcc = this.logicGraphController.ComponentManager.createComponent(madeComponent);
        gcc.transform.SetParent(this.mouseObject.transform);
        //gcc.transform.localPosition = gcc.transform.position;
        Rect dmn = madeComponent.getDimentions();
        gcc.transform.localPosition = new Vector3(dmn.width / 2f, dmn.height / 2f, 0);

        //deactivates the sender controllers
        if (madeComponent.GetType().IsSubclassOf(typeof(InteractiveComponent))) {

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

        if (madeComponent.GetType().IsSubclassOf(typeof(InteractiveComponent))) {
            gcc.transform.Find("MiddleBody").Find("LogicComponentSprite").GetComponent<SpriteRenderer>().sortingLayerName = "Mouse";
        }

        SpriteRenderer xGO = this.makeX();
        xGO.transform.SetParent(this.mouseObject.transform);
        xGO.color = Color.red;
        xGO.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        xGO.transform.position = gcc.transform.position;

        bool placeable = this.logicGraphController.Graph.canPlace(madeComponent);
        //Debug.Log("placable: " + placeable + " " + this.component.Position);
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
        Texture2D xTex = (Texture2D)SceneResouces.SceneObjects[typeof(Texture2D)]["X"];
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
