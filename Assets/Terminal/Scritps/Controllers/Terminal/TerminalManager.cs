using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalManager : MonoBehaviour {

    private List<TerminalController> terminalControllers = new List<TerminalController>();

    public void clearTerminal() {
        //destroys all the terminals

        foreach(TerminalController terCon in this.terminalControllers) {
            GameObject.Destroy(terCon.gameObject);
        }
        this.terminalControllers.Clear();
    }

    public void Update() {
        
        foreach(TerminalController ter in this.terminalControllers) {
            ter.updateTime(Time.deltaTime);
        }
    }

    public List<TerminalController> TerminalControllers {
        get {
            return this.terminalControllers;
        }
    }
}
