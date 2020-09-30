using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalController : MonoBehaviour {

    private Terminal terminal;
    private LogicGraphManager logicGraphManager;

    public void setUp(Terminal terminal) {
        this.terminal = terminal;

        GameObject go = new GameObject("Logic Graph Manager");
        this.logicGraphManager = go.AddComponent<LogicGraphManager>();
        go.transform.position = Vector3.zero;
        go.transform.SetParent(this.transform, true);
    }


    public void updateTime(float timePassed) {
        if (this.terminal != null) {
            int numUpdates = this.terminal.updateTime(timePassed);
            if(numUpdates != 0) {
                this.updateVisuals();
            }
        }
    }

    private void updateVisuals() {

        logicGraphManager.updateAllLogicGraphVisuals();
    }

    public LogicGraphManager GraphManager {
        get {
            return this.logicGraphManager;
        }
    }

    public Terminal Terminal {
        get {
            return this.terminal;
        }
    }
}
