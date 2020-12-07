using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json;
using Newtonsoft;
using Newtonsoft.Json.Linq;

public class GameStory {

    /// <summary>
    /// 
    /// </summary>
    public GameStory() {

    }

    public void loadMod(string path) {

        string dataPath = Application.dataPath;

        string[] folders = Directory.GetDirectories(path);
        Debug.Log(folders.Length);
        string scriptsPath = Array.Find(folders, element => element.EndsWith("Scripts"));


        if (scriptsPath != null) {
            this.loadAllScripts(scriptsPath);
        }
    }

    /// <summary>
    /// Reads all the 
    /// </summary>
    /// <param name="path"></param>
    private void loadAllScripts(string path) {

        string[] files = Directory.GetFiles(path);
        string modOrderFile = Array.Find(files, element => element.EndsWith("ModLoadOrder.json"));
        
        if(modOrderFile != null) {
            string json = File.ReadAllText(modOrderFile);
            JToken root = JObject.Parse(json);
            Debug.Log(root);
            Debug.Log(new JArray(root).Count);
        }
    }

    private void loadSript(string path) {

    }

    public static GameStory LoadStoryFromFile(string path) {
        return null;
    }
}