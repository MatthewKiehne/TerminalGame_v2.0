using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class InitGraphController : MonoBehaviour {

    // Start is called before the first frame update
    void Start () {

        SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
        sr.drawMode = SpriteDrawMode.Tiled;
        int width = Random.Range(4, 7);
        int height = Random.Range(4, 7);
        sr.size = new Vector2(width, height );

        this.transform.position = new Vector3(width / 2f, height / 2f, 0);
        
    }
}
