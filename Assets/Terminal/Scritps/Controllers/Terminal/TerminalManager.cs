using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalManager : TimeDependent {

    //private List<TerminalController> terminalControllers = new List<TerminalController>();
    private List<TerminalController> terminalControllers = new List<TerminalController>();

    public void displayTerminal(Terminal terminal) {

        
        GameObject go = new GameObject(terminal.Name);
        TerminalController tc = go.AddComponent<TerminalController>();
        tc.setUp(terminal);
        this.terminalControllers.Add(tc);
        go.transform.position = Vector3.zero;
        go.transform.SetParent(this.transform, true);
        
    }

    public void clearTerminal() {
        //destroys all the terminals

        foreach(TerminalController terCon in this.terminalControllers) {
            GameObject.Destroy(terCon.gameObject);
        }
        this.terminalControllers.Clear();
    }

    public void updateAllTerminalVisuals() {
        foreach (TerminalController ter in this.terminalControllers) {
            ter.updateVisuals();
        }
    }

    public override void updateTime(float time) {
        
        for(int i = 0; i < this.terminalControllers.Count; i++) {
            this.terminalControllers[i].updateTime(time);
        }
        
    }

    public List<TerminalController> TerminalControllers {
        get {
            return this.terminalControllers;
        }
    }
}
