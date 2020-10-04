using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public static class Save {

    public static DirectoryInfo makeDirectory(string path, string directoryName) {
        //makes a directory with the given name

        string combined = path;
        if (!combined.EndsWith("/")) {
            combined += "/";
        }
        combined += directoryName;

        return Save.makeDirectory(combined);
    }

    public static DirectoryInfo makeDirectory(string path) {
        //makes a directory from the path

        return System.IO.Directory.CreateDirectory(path);
    }

    public static void saveJson<T>(T obj, string path, string name) {
        //saves a json file with the contents passed in

        string combined = path;
        if (!combined.EndsWith("/")) {
            combined += "/";
        }
        combined += name;

        Save.saveJson<T>(obj, combined);
    }

    public static void saveJson<T>(T obj, string path) {
        //saves an object to a certain path

        string json = JsonUtility.ToJson(obj);

        FileStream fs = new FileStream(path, FileMode.Create);
        StreamWriter writer = new StreamWriter(fs);
        writer.Write(json);

        writer.Close();
        fs.Close();
    }

    public static T loadJson<T>(string path) {
        //loads an object from a path

        FileStream open = new FileStream(path, FileMode.Open);
        StreamReader reader = new StreamReader(open);

        string all = reader.ReadToEnd();

        reader.Close();
        open.Close();

        return JsonUtility.FromJson<T>(all);
    }
}