using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TExtensionConnectionsData
{

    public List<BridgeComponentConnectionData> connections = new List<BridgeComponentConnectionData>();

    public TExtensionConnectionsData(Terminal terminal) {

        Dictionary<ExtensionConnection, TExtension> recDic = new Dictionary<ExtensionConnection, TExtension>();

        /*
        for (int i = 0; i < terminal.extensionLength(); i++) {

            TExtension ext = terminal.extensionAt(i);

            foreach (ReceiveBridge rec in ext.ReceiveBridges) {
                recDic.Add(rec, ext);
            }
        }

        for (int i = 0; i < terminal.extensionLength(); i++) {

            TExtension ext = terminal.extensionAt(i);

            foreach (SendBridge send in ext.SendBridges) {

                for (int r = 0; r < send.ReceiveBridges.Count; r++) {

                    ReceiveBridge toBridge = send.ReceiveBridges[r];
                    TExtension toExtension = recDic[toBridge];

                    BridgeComponentConnectionData connection = new BridgeComponentConnectionData(
                        ext.Name, send.Name, toExtension.Name, toBridge.Name);

                    this.connections.Add(connection);

                }
            }
        }
        */
    }

    /// <summary>
    /// Links two bridges together in the Terminal passed in
    /// </summary>
    public void buildConnections(Terminal terminal) {

        /*

        foreach (BridgeComponentConnectionData connection in this.connections) {

            TExtension fromExtension = terminal.findExtension(connection.fromExtension);
            ExtensionConnection fromNode = Array.Find(fromExtension.SendBridges, (x) => x.Name.Equals(connection.fromBridge));
            TExtension toExtension = terminal.findExtension(connection.toExtension);
            ExtensionConnection toNode = Array.Find(toExtension.ReceiveBridges, (x) => x.Name.Equals(connection.toBridge));

            if (fromExtension != null && fromBridge != null && toExtension != null && toBridge != null) {
                fromBridge.ReceiveBridges.Add(toBridge);
                toBridge.SendBridges.Add(fromBridge);
            }
        }
        */
    }
}