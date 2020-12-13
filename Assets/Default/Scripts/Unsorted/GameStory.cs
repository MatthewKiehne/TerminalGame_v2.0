using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using System.Linq;

public enum InitStoryType { LoadStory, CreateNewStory}

public class GameStory {


    /// <summary>
    /// 
    /// </summary>
    /// <param name="initStoryType"></param>
    /// <param name="path"></param>
    public GameStory(string path) {

        this.loadStory(path);
    }

    /// <summary>
    /// Create a new file system which a new GameStory can read from
    /// </summary>
    /// <param name="destinationPath"></param>
    /// <param name="allModsPaths"></param>
    public static void CreateNewStory(string destinationPath, string[] allModsPaths) {

        if (Directory.Exists(destinationPath)) {
            Directory.Delete(destinationPath, true);
        }
        Directory.CreateDirectory(destinationPath);

        string[] rootFolders = { "/Mods", "/About", "/Saves" };
        foreach(string folder in rootFolders) {
            Directory.CreateDirectory(destinationPath + folder);
        }

        string modsFolder = destinationPath + "/Mods";

        foreach (string mod in allModsPaths) {

            if (Directory.Exists(mod)) {

                string modAboutFolderPath = mod + "\\About";

                if (Directory.Exists(modAboutFolderPath)) {
                    
                    string modAboutFilePath = modAboutFolderPath + "\\About.json";
                    Debug.Log(Directory.GetFiles(modAboutFolderPath)[0]);

                    //string text = File.ReadAllText(modAboutFolderPath);

                    //Debug.Log(text);

                    if (File.Exists(modAboutFilePath)) {

                        Debug.Log(File.GetAttributes(Directory.GetFiles(modAboutFolderPath)[0]));
                        //FileStream stream = new FileStream(Directory.GetFiles(modAboutFolderPath)[0], FileMode.Open);
                        string text = File.ReadAllText(modAboutFilePath);
                        JToken root = JToken.Parse(text);
                        string modName = root.SelectToken("ModName").ToString();
                        Debug.Log(modName);

                        string currentModsFolder = modsFolder + "\\" + modName;
                        Directory.CreateDirectory(currentModsFolder);

                        string compiledScriptsFolder = currentModsFolder + "\\Compiled Scripts";
                        Directory.CreateDirectory(compiledScriptsFolder);


                        
                        
                        
                        /*
                        JToken root = JObject.Parse(aboutFileText);
                        string modName = root.SelectToken("ModName").ToString();
                        Debug.Log(modName);
                        */

                    } else {
                        throw new Exception("AboutFileNotFound: About.json could not be found at " + modAboutFilePath);
                    }
                } else {
                    throw new Exception("AboutFolderNotFound: about folder was not found at " + modAboutFolderPath);
                }
            } else {
                throw new Exception("LoadModError: " + mod + " could not be found");
            }
        }
    }

    /// <summary>
    /// Will attempt to load a 
    /// </summary>
    /// <param name="path"></param>
    private void loadStory(string path) {

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
            JToken[] scriptArray = root.SelectToken("Scripts").ToArray();

            foreach(JToken script in scriptArray) {

                string scriptPath = script.SelectToken("ScriptPath").ToString();
                JToken[] requiredArray = script.SelectToken("Requires").ToArray();

                bool foundRequired = true;
                int requireCounter = 0;

                while(foundRequired && requireCounter < requiredArray.Length) {

                    string requiredName = requiredArray[requireCounter].SelectToken("RequiredName").ToString();
                    string requireType = requiredArray[requireCounter].SelectToken("RequiredType").ToString();

                    if (requireType.Equals("Mod")) {

                        string requiredModName = requiredArray[requireCounter].SelectToken("ModName").ToString();
                        foundRequired = Creator.I.isLoaded(requiredModName, requiredName);

                    } else if (requireType.Equals("Script")) {

                        foundRequired = Creator.I.isLoaded(modName, requiredName);

                    } else if (requireType.Equals("Type")) {
                        foundRequired = Creator.I.isLoaded(Type.GetType(requiredName));
                    } else {
                        throw new Exception("InvalidRequireType: " + scriptPath + " in " + modName + " has an invalid RequiredType of " + requireType);
                    }

                    if (!foundRequired) {
                        throw new Exception("InvalidRequire: " + scriptPath + " in " + modName + " has failed to require" + requiredName);
                    }

                    requireCounter++;
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