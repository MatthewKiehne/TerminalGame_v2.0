using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class SceneResouces {

    public static Dictionary<string,            //Name of the File
        Dictionary<Type,                        //Type of the object you want
        Dictionary<string,                      //Name of the object 
            System.Object>>> SceneObjects = new Dictionary<string, Dictionary<Type, Dictionary<string, object>>>();

    /// <summary>
    /// Loads the object at the path in SceneObjects Dictionary
    /// </summary>
    public static void loadResources(string path) {
        //load an object from resources and stores it in the dictionary
        
        UnityEngine.Object obj = Resources.Load(path);

        if (obj != null) {
            Type typeParameterType = obj.GetType();

            string[] parts = path.Split(new char[]{ '/' });

            if (!SceneObjects.ContainsKey(parts[0])) {
                SceneObjects.Add(parts[0], new Dictionary<Type, Dictionary<string, object>>());
            }

            if (!SceneObjects[parts[0]].ContainsKey(typeParameterType)) {
                SceneObjects[parts[0]].Add(typeParameterType, new Dictionary<string, object>());
            }

            SceneObjects[parts[0]][typeParameterType].Add(obj.name, obj);

        } else {
            throw new Exception("Load Reources failed to find path: " + path);
        } 
    }

    public static void clearResources() {
        //clears the dictionary 
    }
}
