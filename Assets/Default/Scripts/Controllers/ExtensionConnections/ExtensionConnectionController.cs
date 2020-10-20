using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ExtensionConnectionController : MonoBehaviour {

    private Terminal terminal;

    private TerminalConnectionsContent terminalConnectionsContent;

    private Dropdown fromExtensionOptions;
    private TExtension fromExtensionSelect = null;

    private Dropdown fromBridgeOptions;
    private SendBridge fromBridgeSelect = null;

    private Dropdown toExtensionOptions;
    private TExtension toExtensionSelect = null;

    private Dropdown toBridgeOptions;
    private ReceiveBridge toBridgeSelect = null;

    private Button deleteButton;
    private Button confirmButton;

    private Color confirmColor = new Color(.2216f, 1f, 0f, 1f);

    private Image basePanel;

    private bool connected = false;

    public void setUp(Terminal terminal, TerminalConnectionsContent terminalConnectionContent) {
        this.terminal = terminal;
        this.terminalConnectionsContent = terminalConnectionContent;
        this.findComponents();
    }

    public void setUp(Terminal terminal, TerminalConnectionsContent terminalConnectionContent,
        TExtension fromExtension, SendBridge sendBridge, TExtension toExtension, ReceiveBridge receiveBridge) {

        this.terminal = terminal;
        this.terminalConnectionsContent = terminalConnectionContent;

        this.findComponents();

        int fromExtensionIndex = this.terminal.extensionIndexOf(fromExtension);
        int sendIndex = Array.IndexOf(fromExtension.SendBridges, sendBridge);
        int toExtensionIndex = this.terminal.extensionIndexOf(toExtension);
        int receiveIndex = Array.IndexOf(toExtension.ReceiveBridges, receiveBridge);

        this.fromExtensionSelect = fromExtension;
        this.fromBridgeSelect = sendBridge;
        this.toExtensionSelect = toExtension;
        this.toBridgeSelect = receiveBridge;

        this.fromExtensionOptions.AddOptions(this.extensionList());
        this.fromExtensionOptions.value = fromExtensionIndex + 1;

        this.fromBridgeOptions.value = sendIndex + 1;
        this.toExtensionOptions.value = toExtensionIndex + 1;
        this.toBridgeOptions.value = receiveIndex + 1;

        if (this.verifyAll()) {
            this.connected = true;
            this.changeBackgroundColor();
        }
    }

    private void findComponents() {
        //finds all the components and adds functionality 

        this.basePanel = this.GetComponent<Image>();

        Transform fromDropdown = this.transform.Find("From").Find("Dropdowns");
        Transform toDropdown = this.transform.Find("To").Find("Dropdowns");

        this.fromExtensionOptions = fromDropdown.Find("Extension").GetComponent<Dropdown>();
        this.fromBridgeOptions = fromDropdown.Find("Bridge").GetComponent<Dropdown>();
        this.toExtensionOptions = toDropdown.Find("Extension").GetComponent<Dropdown>();
        this.toBridgeOptions = toDropdown.Find("Bridge").GetComponent<Dropdown>();

        this.confirmButton = this.transform.Find("ConfirmButton").GetComponent<Button>();
        this.deleteButton = this.transform.Find("DeleteButton").GetComponent<Button>();

        fromExtensionOptions.ClearOptions();
        fromBridgeOptions.ClearOptions();
        toExtensionOptions.ClearOptions();
        toBridgeOptions.ClearOptions();

        if (this.transform == null) {

            throw new Exception("the global variabl Terminal has to be set before the Start funciton is called");
        }

        fromExtensionOptions.onValueChanged.AddListener(setFromExtension);
        fromBridgeOptions.onValueChanged.AddListener(setSendBridge);
        toExtensionOptions.onValueChanged.AddListener(setToExteinsion);
        toBridgeOptions.onValueChanged.AddListener(setReceiveBridge);

        this.confirmButton.onClick.AddListener(onConfirmationClick);
        this.deleteButton.onClick.AddListener(onDeletionClick);

        fromExtensionOptions.AddOptions(this.extensionList());
        fromBridgeOptions.AddOptions(this.arrowList());
        toExtensionOptions.AddOptions(this.extensionList());
        toBridgeOptions.AddOptions(this.arrowList());
    }

    private void setFromExtension(int value) {

        if (value == 0) {

            this.fromExtensionOptions.ClearOptions();
            this.fromExtensionOptions.AddOptions(this.extensionList());
            this.fromExtensionOptions.value = 0;
            this.fromExtensionSelect = null;

            this.fromBridgeOptions.ClearOptions();
            this.fromBridgeOptions.AddOptions(this.arrowList());
            this.fromBridgeOptions.value = 0;
            this.fromBridgeSelect = null;

        } else {

            if (this.validFromExtension()) {

                this.fromExtensionSelect = this.terminal.extensionAt(fromExtensionOptions.value - 1);

                fromBridgeOptions.ClearOptions();
                fromBridgeOptions.AddOptions(this.sendBridges(this.fromExtensionSelect));

            } else {

                this.fromExtensionOptions.ClearOptions();
                this.fromExtensionOptions.AddOptions(this.extensionList());
                this.fromExtensionOptions.value = 0;
                this.fromExtensionSelect = null;

                this.fromBridgeOptions.ClearOptions();
                this.fromBridgeOptions.AddOptions(this.arrowList());
                this.fromBridgeOptions.value = 0;
                this.fromBridgeSelect = null;  
            }
        }
    }

    private void setSendBridge(int value) {

        if (value == 0) {
            this.fromBridgeSelect = null;

            if (this.fromExtensionSelect != null) {
                this.fromBridgeOptions.ClearOptions();
                this.fromBridgeOptions.AddOptions(this.sendBridges(this.fromExtensionSelect));
            }

        } else {

            if (this.validFromBridge()) {

                this.fromBridgeSelect = this.fromExtensionSelect.SendBridges[fromBridgeOptions.value - 1]; ;

            } else {

                this.fromBridgeOptions.ClearOptions();
                this.fromBridgeOptions.AddOptions(this.sendBridges(this.fromExtensionSelect));
                this.fromBridgeSelect = null;
            }
        }
    }

    private void setToExteinsion(int value) {

        if (value == 0) {

            this.toExtensionOptions.ClearOptions();
            this.toExtensionOptions.AddOptions(this.extensionList());
            this.toExtensionOptions.value = 0;
            this.toExtensionSelect = null;

            this.toBridgeOptions.ClearOptions();
            this.toBridgeOptions.AddOptions(this.arrowList());
            this.toBridgeOptions.value = 0;
            this.toBridgeSelect = null;

        } else {

            if (this.validToExtension()) {

                this.toExtensionSelect = this.terminal.extensionAt(toExtensionOptions.value - 1);

                toBridgeOptions.ClearOptions();
                toBridgeOptions.AddOptions(this.receiveBridges(this.toExtensionSelect));

            } else {

                this.toExtensionOptions.ClearOptions();
                this.toExtensionOptions.AddOptions(this.extensionList());
                this.toExtensionOptions.value = 0;
                this.toExtensionSelect = null;

                this.toBridgeOptions.ClearOptions();
                this.toBridgeOptions.AddOptions(this.arrowList());
                this.toBridgeOptions.value = 0;
                this.toBridgeSelect = null;
            }
        }
    }

    private void setReceiveBridge(int value) {

        if (value == 0) {
            //does nothing
            this.toBridgeSelect = null;

            if (this.toExtensionSelect != null) {
                this.toBridgeOptions.ClearOptions();
                this.toBridgeOptions.AddOptions(this.receiveBridges(this.toExtensionSelect));
            }

        } else {

            if (this.validToBridge()) {

                this.toBridgeSelect = this.toExtensionSelect.ReceiveBridges[toBridgeOptions.value - 1]; ;

            } else {

                this.toBridgeOptions.ClearOptions();
                this.toBridgeOptions.AddOptions(this.receiveBridges(this.toExtensionSelect));
                this.toBridgeSelect = null;
            }
        }
    }

    private bool validFromExtension() {
        //verifies if the from extension is correct
        bool result = false;

        if (fromExtensionOptions.value - 1 < this.terminal.extensionLength()) {

            TExtension ext = this.terminal.extensionAt(fromExtensionOptions.value - 1);

            if (ext.Name.Equals(fromExtensionOptions.options[fromExtensionOptions.value].text)) {

                result = true;
            }
        }

        return result;
    }

    private bool validFromBridge() {
        //verifies if the from bridge is correct
        bool result = false;

        if (fromBridgeOptions.value - 1 < this.fromExtensionSelect.SendBridges.Length) {

            SendBridge sendBridge = this.fromExtensionSelect.SendBridges[fromBridgeOptions.value - 1];

            if (sendBridge.Name.Equals(this.fromBridgeOptions.options[this.fromBridgeOptions.value].text)) {

                result = true;
            }
        }

        return result;
    }

    private bool validToExtension() {
        //verifies if the to extension is correct
        bool result = false;

        if (toExtensionOptions.value - 1 < this.terminal.extensionLength()) {

            TExtension ext = this.terminal.extensionAt(toExtensionOptions.value - 1);

            if (ext.Name.Equals(toExtensionOptions.options[toExtensionOptions.value].text)) {

                result = true;
            }
        }

        return result;
    }

    private bool validToBridge() {
        //verifies if the from bridge is correct
        bool result = false;

        if (toBridgeOptions.value - 1 < this.toExtensionSelect.ReceiveBridges.Length) {

            ReceiveBridge receiveBridge = this.toExtensionSelect.ReceiveBridges[toBridgeOptions.value - 1];

            if (receiveBridge.Name.Equals(this.toBridgeOptions.options[this.toBridgeOptions.value].text)) {
                result = true;
            }
        }

        return result;
    }

    private bool extensionInTerminal(TExtension ext) {
        bool found = false;

        int counter = 0;
        while(!found && counter < this.terminal.extensionLength()) {

            if (this.terminal.extensionAt(counter).Equals(ext)) {
                found = true;
            }

            counter++;
        }

        return found;
    }

    private bool bridgeInExtension(ExtensionConnection bridge, TExtension ext) {
        bool found = false;

        if(bridge.GetType() == typeof(SendBridge)) {
            found = Array.Exists(ext.SendBridges, b => b.Equals(bridge));
        } else {
            found = Array.Exists(ext.ReceiveBridges, b => b.Equals(bridge));
        }

        return found;
    }

    private bool verifyAll() {
        //verifies the information is still accurate and changes the ones that are not

        return this.validFromExtension() && this.validFromBridge() && 
            this.validToExtension() && this.validToBridge() &&
            this.extensionInTerminal(this.fromExtensionSelect) && this.bridgeInExtension(this.fromBridgeSelect, this.fromExtensionSelect) &&
            this.extensionInTerminal(this.toExtensionSelect) && this.bridgeInExtension(this.toBridgeSelect, this.toExtensionSelect);
    }

    private void onConfirmationClick() {
        //changes the color of the base a connection is made

        if (this.verifyAll()) {

            this.connected = true;

            this.fromBridgeSelect.ReceiveBridges.Add(this.toBridgeSelect);
            this.toBridgeSelect.SendBridges.Add(this.fromBridgeSelect);

            this.changeBackgroundColor();
            
        } else {

            this.toBridgeOptions.value = this.toBridgeOptions.value;
            this.toExtensionOptions.value = this.toExtensionOptions.value;
            this.fromBridgeOptions.value = this.fromBridgeOptions.value;
            this.fromExtensionOptions.value = this.fromExtensionOptions.value;
        }
    }

    private void changeBackgroundColor() {
        //reads the state of the connection and changes the background
        if (this.connected) {
            this.basePanel.color = this.confirmColor;
        } 
    }
    
    private void onDeletionClick() {

        if (connected) {

            TExtension from = null;
            SendBridge send = null;

            TExtension to = null;
            ReceiveBridge rec = null;

            try {
                from = this.findExtensionByName(this.terminal, fromExtensionSelect.Name);
                send = this.findSendBridgeByName(from.SendBridges, fromBridgeSelect.Name);
                to = this.findExtensionByName(this.terminal, toExtensionSelect.Name);
                rec = this.findReceiveBridgeByName(to.ReceiveBridges, toBridgeSelect.Name);

            } catch (Exception ex) {
                throw ex;
            }

            if (send != null && rec != null) {

                bool one = send.ReceiveBridges.Remove(rec);
                bool two = rec.SendBridges.Remove(send);

            } else {
                this.terminalConnectionsContent.onRefreshClick();
            }
        }

        Destroy(this.gameObject);
    }

    private TExtension findExtensionByName(Terminal terminal, string name) {

        TExtension result = null;

        int counter = 0;

        while (counter < terminal.extensionLength() && result == null) {

            if (terminal.extensionAt(counter).Name.Equals(name)) {
                result = terminal.extensionAt(counter);
            }

            counter++;
        }

        return result;
    }

    private SendBridge findSendBridgeByName(SendBridge[] list, string name) {

        SendBridge result = null;
        int counter = 0;
        while (counter < list.Length && result == null) {
            if (list[counter].Name.Equals(name)) {
                result = list[counter];
            }
            counter++;
        }
        return result;
    }

    private ReceiveBridge findReceiveBridgeByName(ReceiveBridge[] list, string name) {

        ReceiveBridge result = null;
        int counter = 0;
        while (counter < list.Length && result == null) {
            if (list[counter].Name.Equals(name)) {
                result = list[counter];
            }
            counter++;
        }
        return result;
    }

    private List<string> selectList() {
        return new List<string>() { "Select" };
    }

    private List<string> arrowList() {
        return new List<string>() { "<=" };
    }

    private List<string> extensionList() {
        List<String> extList = this.selectList();
        for (int i = 0; i < this.terminal.extensionLength(); i++) {
            extList.Add(this.terminal.extensionAt(i).Name);
        }
        return extList;
    }

    private List<string> sendBridges(TExtension extension) {

        List<string> result = this.selectList();

        foreach (SendBridge send in extension.SendBridges) {
            result.Add(send.Name);
        }

        return result;
    }

    private List<string> receiveBridges(TExtension extension) {

        List<string> result = this.selectList();

        foreach (ReceiveBridge rec in extension.ReceiveBridges) {
            result.Add(rec.Name);
        }

        return result;
    }

}