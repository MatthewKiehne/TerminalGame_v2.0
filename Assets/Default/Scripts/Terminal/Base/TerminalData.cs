using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[Serializable]
public class TerminalData
{

    public string Name;

    public TerminalData(Terminal ter) {
        this.Name = ter.Name;
    }

    /// <summary>
    /// Creates a Terminal from the path 
    /// </summary>
    public Terminal getTerminal(string terminalPath) {

        Terminal result = new Terminal(this.Name);

        string logicGraphPath = terminalPath + "/LogicGraphs";
        string[] logicPaths = Directory.GetFiles(logicGraphPath);

        foreach (string logicPath in logicPaths) {

            if (logicPath.EndsWith(".json")) {

                LogicGraphData lgd = Save.loadJson<LogicGraphData>(logicPath);
                result.addExtension(lgd.getLogicGraph());
            }
        }

        //connects the TExtensions
        string TExtConnectionPath = terminalPath + "/" + "TExtensionConnectionsData.json";
        TExtensionConnectionsData connectionData = Save.loadJson<TExtensionConnectionsData>(TExtConnectionPath);
        connectionData.buildConnections(result);

        return result;
    }
}