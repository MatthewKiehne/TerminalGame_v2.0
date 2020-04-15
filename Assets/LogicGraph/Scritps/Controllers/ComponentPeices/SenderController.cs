using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenderController : MonoBehaviour {

    private List<LineController> lineControllers = new List<LineController>();

    private GameObject RayGOPrefab;
    private Sender sender;

    public void setUp(Sender sender) {
        //sets up the controller

        this.sender = sender;
    }

    public void updateSignal(bool state) {
        //updates the visuals of the lines
        //Debug.Log("SenderController -> UpdateSignal(): line controllers:" + this.lineControllers.Count);
        for(int i = 0; i < this.lineControllers.Count; i++) {
            this.lineControllers[i].updateSignal(state);
        }
    }

    public void makeNewRays() {
        //destroys the old rays and makes new ones
        //Debug.Log("SenderController -> makeNewRays(): startCount:" + this.lineControllers.Count);
        this.destroyRays();
        this.createRays();
    }

    private void destroyRays() {
        //destroys all the rays
        foreach(LineController lc in this.lineControllers) {
            GameObject.Destroy(lc.gameObject);
        }
        this.lineControllers.Clear();
        this.lineControllers = new List<LineController>();
        //Debug.Log("SenderController -> destroyRays(): count:" + this.lineControllers.Count);
    }

    private void createRays() {
        //makes all the rays and then adds them to the list

        Transform child = this.transform.Find("Starting Point");

        if(child == null) {
            child = new GameObject("Starting Point").transform;

            child.SetParent(this.transform);
            child.localPosition = new Vector3(0, .55f, 0);
            child.rotation = this.transform.rotation;
        }

        List<List<Vector2>> allLines = this.getPoints(child.position, child.up);

        GameObject rayPrefab = (GameObject)SceneResouces.SceneObjects[typeof(GameObject)]["Ray"];

        foreach (List<Vector2> lines in allLines) {

            GameObject rayGO = Instantiate(rayPrefab);
            rayGO.transform.SetParent(this.transform);
            rayGO.transform.position = Vector3.zero;
            LineController lc = rayGO.GetComponent<LineController>();
            lc.setPoints(VertexExtension.toVector3Array(lines.ToArray()));
            this.lineControllers.Add(lc);
        }

        //Debug.Log("SenderController -> createRays(): count" + this.lineControllers.Count);
    }

    private List<List<Vector2>> getPoints(Vector2 startingPosition, Vector2 startingDirection) {
        return this.getPoints(startingPosition, startingDirection, 0);
    }

    private List<List<Vector2>> getPoints(Vector2 startingPosition, Vector2 startingDirection, int bounceNum) {
        //return a list of lines
        //the list inside the list are the positions for the the given line

        //Debug.Log("SenderController -> getPoints()");

        Vector2 currentPoint = startingPosition;
        Vector2 currentDirection = startingDirection;

        bool continueRayCasting = true;

        int bounceCounter = bounceNum;
        int maxBounceCounter = LogicGraph.maxBounces;

        List<List<Vector2>> allLines = new List<List<Vector2>>();
        List<Vector2> currentLines = new List<Vector2>();
        currentLines.Add(startingPosition);

        while(continueRayCasting && bounceCounter < maxBounceCounter) {

            RaycastHit2D currentHit = this.shootRay(currentPoint, currentDirection);

            if(currentHit.collider != null) {

                currentLines.Add(currentHit.point);

                //Debug.Log(currentHit.collider.name + " B:" + bounceCounter + " Dir:" + currentDirection + " Point:" + currentHit.point);

                MirrorController mc = currentHit.collider.GetComponent<MirrorController>();
                PassingMirrorController pmc = currentHit.collider.GetComponent<PassingMirrorController>();

                if (mc != null) {

                    Vector2 newDirection = Vector2.Reflect(currentDirection, currentHit.normal);
                    newDirection = new Vector2(Mathf.Round(newDirection.x), Mathf.Round(newDirection.y));
                    Vector2 newPoint = currentHit.point + (newDirection * .005f);

                    currentDirection = newDirection;
                    currentPoint = newPoint;

                } else if(pmc != null){


                    Vector2 inverseDirection = -1 * currentDirection;
                    Vector2 checkPoint = currentHit.point + (currentDirection * .25f);

                    RaycastHit2D checkHit = this.shootRay(checkPoint, inverseDirection);
                    Vector2 calculatedPoint = checkHit.point + (currentDirection * .19f);

                    List<List<Vector2>> pointsThroughMirror = this.getPoints(calculatedPoint, currentDirection, bounceCounter + 1);
                    allLines = this.combineLists(allLines, pointsThroughMirror);

                    Vector2 newDirection = Vector2.Reflect(currentDirection, currentHit.normal);
                    Vector2 newPoint = currentHit.point + (newDirection * .005f);

                    currentDirection = newDirection;
                    currentPoint = newPoint;

                }
                else {

                    continueRayCasting = false;
                }
            } 

            bounceCounter++;
        }

        if(currentLines.Count != 1) {
            allLines.Add(currentLines);
        }

        return allLines;
    }

    private List<List<Vector2>> combineLists(List<List<Vector2>> listOne, List<List<Vector2>> listTwo) {
        //combines two arrays of lists

        List<List<Vector2>> result = new List<List<Vector2>>();

        foreach(List<Vector2> list in listOne) {
            result.Add(list);
        }

        foreach (List<Vector2> list in listTwo) {
            result.Add(list);
        }

        return result;
    }

    private RaycastHit2D shootRay(Vector2 startingPoint, Vector2 direction) {
        //returns a hitData if the ray hits something it can connect to
        //checks if it hits a collider within 500 units

        return Physics2D.Raycast(startingPoint, direction, LogicGraph.MaxRayDistance);
    }
}
