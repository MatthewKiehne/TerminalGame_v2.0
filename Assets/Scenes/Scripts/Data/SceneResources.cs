using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneResouces {

    public static Dictionary<Type, Dictionary<string, System.Object>> SceneObjects = new Dictionary<Type, Dictionary<string, System.Object>>();

    public static void loadBundle(string pathToSceneFile) {
        //loads in a assetbundl
    }

    public static void loadResources(string path) {
        //load from resources
        
        UnityEngine.Object obj = Resources.Load(path);

        if (obj != null) {
            Type typeParameterType = obj.GetType();

            if (!SceneObjects.ContainsKey(typeParameterType)) {
                SceneObjects.Add(typeParameterType, new Dictionary<string, object>());
            }

            SceneObjects[typeParameterType].Add(obj.name, obj);

        } else {
            throw new Exception("Load Reources failed to find path: " + path);
        } 
    }

    public static void clearResources() {
        //clears the dictionary
        
    }
}
