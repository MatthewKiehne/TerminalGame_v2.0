using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]

public class LogicGraphCameraController : MonoBehaviour {
    // Start is called before the first frame update


    private LogicChip graph;
    private Vector3 offset;

    private readonly float speed = 5f;
    private Camera cam;

    private int orthoMin = 3;
    private int orthoMax = 50;

    private int horizontal = 0;
    private int vertical = 0;
    private float cameraZoom = 0;

    private InputActions input;
    private RawImage rawImage;

    public void setUp(Vector3 offset, LogicChip graph, InputActions input, RawImage image) {
        //sets up the camera controller

        this.graph = graph;
        this.offset = offset;
        this.cam = this.GetComponent<Camera>();
        this.input = input;
        this.rawImage = image;

        //up
        input.addInput(new KeyCombination(KeyCode.W, KeyStatus.Down), () => {
            this.vertical += 1;
        });
        input.addInput(new KeyCombination(KeyCode.W, KeyStatus.Held), () => {
            this.vertical += 1;
        });

        //down
        input.addInput(new KeyCombination(KeyCode.S, KeyStatus.Down), () => {
            this.vertical -= 1;
        });
        input.addInput(new KeyCombination(KeyCode.S, KeyStatus.Held), () => {
            this.vertical -= 1;
        });

        //left
        input.addInput(new KeyCombination(KeyCode.D, KeyStatus.Down), () => {
            this.horizontal += 1;
        });
        input.addInput(new KeyCombination(KeyCode.D, KeyStatus.Held), () => {
            this.horizontal += 1;
        });

        //right
        input.addInput(new KeyCombination(KeyCode.A, KeyStatus.Down), () => {
            this.horizontal -= 1;
        });
        input.addInput(new KeyCombination(KeyCode.A, KeyStatus.Held), () => {
            this.horizontal -= 1;
        });

        input.addInput(new KeyCombination(KeyCode.C, KeyStatus.Rest), () => {
            this.cameraZoom = input.CurrentFrameData.ScrollWheel;
        });
    }

    // Update is called once per frame
    void Update() {

        Vector3 updatedPosition = this.transform.position;

        //gets the movement
        if (this.vertical == 1) {
            updatedPosition += (this.transform.up * this.speed * cam.orthographicSize * Time.deltaTime);
        } else if (this.vertical == -1) {
            updatedPosition += (-this.transform.up * this.speed * cam.orthographicSize * Time.deltaTime);
        }

        if (this.horizontal == 1) {
            updatedPosition += (this.transform.right * this.speed * cam.orthographicSize * Time.deltaTime);
        } else if (this.horizontal == -1) {
            updatedPosition += (-this.transform.right * this.speed * cam.orthographicSize * Time.deltaTime);
        }

        //corrects the out of bounds
        if (updatedPosition.x < offset.x) {
            updatedPosition.x = offset.x;
        } else if (updatedPosition.x > offset.x + graph.LightGraph.Width) {
            updatedPosition.x = offset.x + graph.LightGraph.Width;
        }

        if (updatedPosition.y < offset.y) {
            updatedPosition.y = offset.y;
        } else if (updatedPosition.y > offset.y + graph.LightGraph.Height) {
            updatedPosition.y = offset.y + graph.LightGraph.Height;
        }

        //updates the position
        this.transform.position = updatedPosition;


        float updatedOrtho = cam.orthographicSize + (this.cameraZoom * 10f);
        if (updatedOrtho < this.orthoMin) {
            updatedOrtho = this.orthoMin;
        } else if (updatedOrtho > this.orthoMax) {
            updatedOrtho = this.orthoMax;
        }
        cam.orthographicSize = updatedOrtho;

        this.cameraZoom = 0;
        this.vertical = 0;
        this.horizontal = 0;
    }
}
