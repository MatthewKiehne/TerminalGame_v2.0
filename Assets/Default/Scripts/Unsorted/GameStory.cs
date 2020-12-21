using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using System.Linq;
using RoslynCSharp.Compiler;

public enum InitStoryType { LoadStory, CreateNewStory }

public class GameStory
{


    /// <summary>
    /// 
    /// </summary>
    /// <param name="initStoryType"></param>
    /// <param name="path"></param>
    public GameStory(string path) {


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
        foreach (string folder in rootFolders) {
            Directory.CreateDirectory(destinationPath + folder);
        }

        //make the mod load order
        JObject modLoadOrder = new JObject();
        JArray order = (JArray)modLoadOrder["order"];
        JArray array = new JArray();

        string modsFolder = destinationPath + "/Mods";

        foreach (string mod in allModsPaths) {

            string modAboutFolderPath = mod + "\\About";
            string modAboutFilePath = modAboutFolderPath + "\\About.json";

            if (!Directory.Exists(mod)) {
                throw new Exception("LoadModError: " + mod + " could not be found");
            }
            if (!Directory.Exists(modAboutFolderPath)) {
                throw new Exception("AboutFolderNotFound: about folder was not found at " + modAboutFolderPath);
            }
            if (!File.Exists(modAboutFilePath)) {
                throw new Exception("AboutFileNotFound: About.json could not be found at " + modAboutFilePath);
            }

            string text = File.ReadAllText(modAboutFilePath);
            JToken root = JToken.Parse(text);
            string modName = root.SelectToken("ModName").ToString();
            string modVersion = root.SelectToken("Version").ToString();

            string currentModsFolder = modsFolder + "\\" + modName;
            Directory.CreateDirectory(currentModsFolder);

            JObject modInfo = new JObject();
            modInfo["Name"] = modName;
            modInfo["ModVersion"] = modVersion;

            array.Add(modInfo);

            //compile mods
            if (Directory.Exists(mod + "\\Scripts")) {
                Helper.compileScriptsInMod(mod,modName, currentModsFolder);
            }
        }

        JObject obj = new JObject();
        obj["array"] = array;

        Debug.Log(obj.ToString());
        File.WriteAllText(modsFolder + "\\modLoadOrder.json", obj.ToString());
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

        if (modOrderFile != null) {
            string json = File.ReadAllText(modOrderFile);
            JToken root = JObject.Parse(json);
            JToken[] scriptArray = root.SelectToken("Scripts").ToArray();

            foreach (JToken script in scriptArray) {

                string scriptPath = script.SelectToken("ScriptPath").ToString();
                JToken[] requiredArray = script.SelectToken("Requires").ToArray();


            }
        }
    }

    private void loadSript(string path) {

    }

    public static GameStory LoadStoryFromFile(string path) {
        return null;
    }

    /// <summary>
    /// A helper class so functionality can be used between the static and non-static methods
    /// </summary>
    private class Helper
    {

        /// <summary>
        /// Returns if all the requirements components have been loaded into the Creator. Adds all the Type components to the Creator
        /// </summary>
        /// <param name="requiredArray"> A JToken Array of all the required components</param>
        /// <param name="modName"> The name of the mod this script is under </param>
        /// <param name="scriptPath"> the path the script is located at under the root folder </param>
        /// <returns> Returns if all the required components have been loaded into the Creator</returns>
        public static bool foundAllScriptRequirements(JToken[] requiredArray, string modName, string scriptPath) {

            bool foundRequired = true;
            int requireCounter = 0;

            while (foundRequired && requireCounter < requiredArray.Length) {

                string requiredName = requiredArray[requireCounter].SelectToken("RequiredName").ToString();
                string requireType = requiredArray[requireCounter].SelectToken("RequiredType").ToString();

                if (requireType.Equals("Mod")) {

                    string requiredModName = requiredArray[requireCounter].SelectToken("ModName").ToString();
                    foundRequired = Creator.I.isLoaded(requiredModName, requiredName);

                } else if (requireType.Equals("Script")) {

                    foundRequired = Creator.I.isLoaded(modName, requiredName);

                } else if (requireType.Equals("Type")) {

                    Type t = Type.GetType(requiredName);
                    bool currentlyIn = Creator.I.isLoaded(t);
                    if (!currentlyIn) {
                        Creator.I.load(t);
                    }
                    foundRequired = Creator.I.isLoaded(t);

                } else {
                    throw new Exception("InvalidRequireType: " + scriptPath + " in " + modName + " has an invalid RequiredType of " + requireType);
                }

                if (!foundRequired) {
                    throw new Exception("InvalidRequire: " + scriptPath + " in " + modName + " has failed to require" + requiredName);
                }

                requireCounter++;
            }

            return foundRequired;
        }

        public static void compileScriptsInMod(string modPath, string modName, string destinationFolder) {

            string compiledScriptsFolder = destinationFolder + "\\Compiled Scripts";
            Directory.CreateDirectory(compiledScriptsFolder);
            Creator.I.SetCompileOutput(compiledScriptsFolder);

            string modLoadOrderPath = modPath + "\\Scripts\\ModLoadOrder.json";

            JToken ScriptsRoot = JToken.Parse(File.ReadAllText(modLoadOrderPath));
            JToken[] scriptsArray = ScriptsRoot.SelectToken("Scripts").Children().ToArray();

            foreach (JToken singleScript in scriptsArray) {

                string scriptPath = singleScript.SelectToken("ScriptPath").ToString();
                string fullScriptPath = modPath + "\\" + scriptPath;

                if (!File.Exists(fullScriptPath)) {
                    throw new Exception("ScriptPathNotFound: Could not find script on path " + scriptPath);
                }

                JToken[] requiresArray = singleScript.SelectToken("Requires").Children().ToArray();
                bool foundAllRequired = Helper.foundAllScriptRequirements(requiresArray, modName, scriptPath);

                if (!foundAllRequired) {
                    throw new Exception("Not all the requirements have been met to compile " + scriptPath + " in " + modName);
                }

                string allText = File.ReadAllText(fullScriptPath);
                CompilationResult result = Creator.I.compile(allText);
                //result.OutputFile

                if (result.Errors.Length != 0) {
                    string errorMessage = "";
                    for (int i = 0; i < result.Errors.Length; i++) {
                        errorMessage += i + ". " + result.Errors[i].Message + "\n";
                    }
                    Debug.Log(errorMessage);

                    throw new Exception("FailedToCompile: See errors below for more detail\n" + errorMessage);
                }

                string[] linkParts = scriptPath.Split(new char[] { '\\' });
                string[] fileParts = linkParts[linkParts.Length - 1].Split(new char[] { '.' });
                string newFileName = compiledScriptsFolder + "\\" + fileParts[0] + ".dll";
                Debug.Log(newFileName);
                string assemblyFilePath = result.OutputFile;

                File.Move(assemblyFilePath, newFileName);

                if (File.Exists(assemblyFilePath + ".pdb")) {
                    File.Delete(assemblyFilePath + ".pdb");
                }
            }
        }
    }
}