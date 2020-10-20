using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour {

    private LineRenderer line;

    private void getLines() {
        //makes sure the lines are found before doing any operations

        if (this.line == null) {
            this.line = this.transform.GetChild(0).GetComponent<LineRenderer>();
        }
    }

    public void setPoints(Vector3[] points) {
        this.getLines();

        this.line.positionCount = points.Length;

        this.line.SetPositions(points);
    }

    public void updateSignal(bool state) {
        //udpates the signals for the wire

        this.getLines();

        if (state == true) {
            this.line.startColor = new Color(1, 0, 0, .5f);
            this.line.endColor = new Color(1, 0, 0, .5f);
        } else {
            this.line.startColor = new Color(.5f,.5f,.5f,.5f);
            this.line.endColor = new Color(.5f, .5f, .5f, .5f);
        }
    }
}
