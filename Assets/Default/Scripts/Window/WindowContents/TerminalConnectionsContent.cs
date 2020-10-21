using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerminalConnectionsContent : WindowContent {

    private GameObject connectionPrefab;

    private Terminal terminal;

    private GameObject baseContentPanel;

    public TerminalConnectionsContent(Terminal terminal) {
        this.terminal = terminal;
    }

    public override void changeWindowSize(int width, int height) {
        //does nothing
    }

    public override bool sameContent(WindowContent content) {
        return content.GetType() == this.GetType() && ((TerminalConnectionsContent)content).terminal.Equals(this.terminal);
    }

    public override void spawnContents(WindowController windowController, Transform contentPanel, Canvas canvas) {

        this.connectionPrefab = (GameObject)SceneResouces.SceneObjects["Default"][typeof(GameObject)]["Connection"];

        GameObject guiPrefab = (GameObject)SceneResouces.SceneObjects["Default"][typeof(GameObject)]["TerminalConnections"];
        this.baseContentPanel = GameObject.Instantiate(guiPrefab);
        this.baseContentPanel.transform.SetParent(contentPanel, false);

        Transform list = this.baseContentPanel.transform.Find("EmptyList").Find("Mask").Find("Display");

        //add new connection
        Transform header = this.baseContentPanel.transform.Find("Header");
        Button addConnection = header.Find("Add").GetComponent<Button>();
        Button refresh = header.Find("Refresh").GetComponent<Button>();
        addConnection.onClick.AddListener(() => {

            GameObject go = GameObject.Instantiate(connectionPrefab, list);
            ExtensionConnectionController ecc = go.GetComponent<ExtensionConnectionController>();
            ecc.setUp(this.terminal, this);
        });

        refresh.onClick.AddListener(onRefreshClick);

        this.populateConnections();
    }

    public void onRefreshClick() {
        //refreshes all the connections being desplayed
        this.clearConnections();
        this.populateConnections();
    }

    private void populateConnections() {
        //creates a list of all the connections
        /*

        Transform list = this.baseContentPanel.transform.Find("EmptyList").Find("Mask").Find("Display");

        for (int x = 0; x < this.terminal.extensionLength(); x++) {
            TExtension extension = this.terminal.extensionAt(x);

            foreach (ExtensionNode send in extension.SendBridges) {

                int total = send.ReceiveBridges.Count;
                for (int i = 0; i < total; i++) {

                    ReceiveBridge rec = send.ReceiveBridges[i];

                    GameObject go = GameObject.Instantiate(connectionPrefab, list);
                    ExtensionConnectionController ecc = go.GetComponent<ExtensionConnectionController>();
                    TExtension recExtension = this.findReceiveBridge(rec);
                    ecc.setUp(this.terminal, this, extension, send, recExtension, rec);
                }
            }
        }
        */
    }

    private void clearConnections() {
        //clears all the connections out

        Transform list = this.baseContentPanel.transform.Find("EmptyList").Find("Mask").Find("Display");

        foreach (Transform child in list.transform) {
            GameObject.Destroy(child.gameObject);
        }
    }

    private TExtension findReceiveBridge(ExtensionNode input) {
        //finds the extension that the receiveBridge belongs to

        TExtension result = null;

        int extensionCounter = 0;
        while (extensionCounter < this.terminal.extensionLength() && result == null) {

            int receiveCoutner = 0;
            while (receiveCoutner < this.terminal.extensionAt(extensionCounter).ReceiveNodes.Length && result == null) {

                if (input.Equals(this.terminal.extensionAt(extensionCounter).ReceiveNodes[receiveCoutner])) {
                    result = this.terminal.extensionAt(extensionCounter);
                }

                receiveCoutner++;
            }

            extensionCounter++;
        }

        return result;
    }

    public override void receiveBroadcast(string message) {
        //does nothing
    }

    protected override void destroyContent() {
        //does nothing
    }

    public Terminal Terminal {
        get {
            return this.terminal;
        }
    }
}