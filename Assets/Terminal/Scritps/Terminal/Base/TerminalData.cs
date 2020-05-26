using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TerminalData
{

    public string Name;

    public TerminalData(Terminal ter) {
        this.Name = ter.Name;
    }
}