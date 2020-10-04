using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VertexExtension {

    public static Vector2[] toVector2Array(this Vector3[] v3) {
        return System.Array.ConvertAll<Vector3, Vector2>(v3, V3toV2);
    }  

    public static Vector3[] toVector3Array(this Vector2[] v2) {
        return System.Array.ConvertAll<Vector2, Vector3>(v2, V2toV3);
    }

    public static Vector2 V3toV2(Vector3 v3) {
        return new Vector2(v3.x, v3.y);
    }

    public static Vector3 V2toV3(Vector2 v2) {
        return new Vector3(v2.x, v2.y,0);
    }
}
