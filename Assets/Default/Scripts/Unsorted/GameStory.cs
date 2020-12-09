using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using System.Linq;

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
            this.loadAllScripts(scriptsPath, "testName");
        }
    }

    /// <summary>
    /// Reads all the 
    /// </summary>
    /// <param name="path"></param>
    private void loadAllScripts(string path, string modName) {

        string[] files = Directory.GetFiles(path);
        string modOrderFile = Array.Find(files, element => element.EndsWith("ModLoadOrder.json"));
        
        if(modOrderFile != null) {
            string json = File.ReadAllText(modOrderFile);
            JToken root = JObject.Parse(json);
            JToken[] modArray = root.SelectToken("Mods").ToArray();

            foreach(JToken mod in modArray) {

                string scriptPath = mod.SelectToken("Path").ToString();
                JToken[] requiredArray = mod.SelectToken("Requires").ToArray();

                foreach(JToken require in requiredArray) {

                    string requiredName = require.SelectToken("Name").ToString();
                    JToken requireType = require.SelectToken("Type");

                    if(requireType.Children().ToArray().Length != 0) {

                        string requiredMod = requireType.SelectToken("Mod").ToString();
                        string requiredScript = requireType.SelectToken("Script").ToString();

                    } else {
                        string t = requireType.ToString();

                        if (t.Equals("Script")) {

                        } else if (t.Equals("Type")) {

                        } else {
                            throw new Exception("TypeNotSupported: in script: " + scriptPath + " type " + t + " is not supported");
                        }
                    }
                }
            }
        }
    }

    private void loadSript(string path) {

    }

    public static GameStory LoadStoryFromFile(string path) {
        return null;
    }
}