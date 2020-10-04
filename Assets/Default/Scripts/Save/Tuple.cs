using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Tuple
{

    public string Name;
    public string Value;

    public Tuple(string name, string value) {
        this.Name = name;
        this.Value = value;
    }

    public static List<Tuple> DictionaryToTuple<T>(Dictionary<string, T> dic) {

        List<Tuple> result = new List<Tuple>();

        string[] keys = dic.Keys.ToArray();

        foreach(string key in keys) {
            result.Add(new Tuple (key, dic[key].ToString()));
        }

        return result;
    }
}