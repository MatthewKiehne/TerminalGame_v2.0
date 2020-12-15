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

                    if (File.Exists(modAboutFilePath)) {

                        string text = File.ReadAllText(modAboutFilePath);
                        JToken root = JToken.Parse(text);
                        string modName = root.SelectToken("ModName").ToString();

                        string currentModsFolder = modsFolder + "\\" + modName;
                        Directory.CreateDirectory(currentModsFolder);

                        //compile mods
                        if(Directory.Exists(mod + "\\Scripts")) {

                            string compiledScriptsFolder = currentModsFolder + "\\Compiled Scripts";
                            Directory.CreateDirectory(compiledScriptsFolder);
                            Creator.I.SetCompileOutput(compiledScriptsFolder);

                            JToken ScriptsRoot = JToken.Parse(File.ReadAllText(mod + "\\Scripts\\ModLoadOrder.json"));
                            JToken[] scriptsArray = ScriptsRoot.SelectToken("Scripts").Children().ToArray();

                            foreach(JToken singleScript in scriptsArray) {
                                
                                string scriptPath = singleScript.SelectToken("ScriptPath").ToString();

                                string fullScriptPath = mod + "\\" + scriptPath;

                                if (File.Exists(fullScriptPath)) {

                                    JToken[] requiresArray = singleScript.SelectToken("Requires").Children().ToArray();

                                    bool foundAllRequired = Helper.foundAllScriptRequirements(requiresArray, modName, scriptPath);

                                    if (foundAllRequired) {

                                        string allText = File.ReadAllText(fullScriptPath);
                                        CompilationResult result =  Creator.I.compile(allText);
                                        //result.OutputFile

                                        string[] linkParts = scriptPath.Split(new char[] { '\\' });
                                        string[] fileParts = linkParts[linkParts.Length - 1].Split(new char[] { '.' });
                                        string newFileName = compiledScriptsFolder + "\\" + fileParts[0] + ".dll";
                                        Debug.Log(newFileName);
                                        string assemblyFilePath = result.OutputFile;

                                        File.Move(assemblyFilePath, newFileName);

                                        if(File.Exists(assemblyFilePath + ".pdb")) {
                                            File.Delete(assemblyFilePath + ".pdb");
                                        }

                                    } else {
                                        throw new Exception("Not all the requirements have been met to compile " + scriptPath + " in " + modName);
                                    }
                                } else {
                                    throw new Exception("ScriptPathNotFound: Could not find script on path " + scriptPath);
                                }
                            }
                        }

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
    }
}