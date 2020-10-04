using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BridgeComponentConnectionData {

    public string fromExtension;
    public string fromBridge;

    public string toExtension;
    public string toBridge;

    public BridgeComponentConnectionData(string fromExtension, string fromBridge, string toExtension, string toBridge) {

        this.fromExtension = fromExtension;
        this.fromBridge = fromBridge;

        this.toExtension = toExtension;
        this.toBridge = toBridge;
    }
}