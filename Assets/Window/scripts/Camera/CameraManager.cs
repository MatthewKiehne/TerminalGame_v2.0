using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    //spawns a new camera
    //makes  new material
    //add it to window

    private Vector3 offset = new Vector3(-10000, 0);

    //holds all the camera;
    public Camera[] cams = new Camera[100];

    public Camera makeNewCamera(bool leaveAudioListener) {
        //makes a new camera and then adds it to the array

        
        int space = this.getOpenSpace();
        if(space == -1) {
            throw new System.Exception("camra space: -1. There is not a space for a new camera");
        }

        GameObject cameraPrefab = (GameObject)SceneResouces.SceneObjects[typeof(GameObject)]["Camera"];

        GameObject c = Instantiate(cameraPrefab);
        c.transform.position = new Vector3(offset.x * (space + 1), 0, -10);
        c.transform.name = "Camera:" + space;
        c.transform.SetParent(this.transform);

        Camera cam = c.GetComponent<Camera>();
        cams[space] = cam;

        if (!leaveAudioListener) {
            GameObject.Destroy(cam.GetComponent<AudioListener>());
        }

        return cam;
    }

    public Camera makeNewCamera() {
        //makes a new camera in open "slot"

        return this.makeNewCamera(false);
    }

    private int getOpenSpace() {
        //get the nearest open camera space

        bool found = false;
        int index = 0;
        int result = -1;

        while (!found && index < cams.Length) {

            if (cams[index] == null) {
                found = true;
                result = index;

            } else {

                index++;
            }
        }

        return result;
    }

    public static Camera GetMainCamera() {
        return Camera.main;
    }

    public bool removeCamera(Camera cam) {
        //removes the camera

        bool found = false;
        int counter = 0;

        //loops through the until found or not in array
        while (!found && counter < this.cams.Length) {

            //checks to see if it is a match
            if (this.cams[counter] == cam) {

                //releases the render texture if it exitsted
                if (this.cams[counter].targetTexture != null) {
                    this.cams[counter].targetTexture.Release();
                }

                //destroys the gameobject and sets the array slot to null
                GameObject.Destroy(this.cams[counter].gameObject);
                this.cams[counter] = null;
                found = true;
            }
            counter++;
        }

        return found;
    }
}
