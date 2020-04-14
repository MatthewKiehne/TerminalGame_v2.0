using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BundleBuilder : Editor { 

    [MenuItem("Assets/ Build AssetBundles")]
    static void BuildAllAssetBundles() {
        BuildPipeline.BuildAssetBundles(@"C:\Users\New_User\Desktop\BundleLocation", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);
    }
}
