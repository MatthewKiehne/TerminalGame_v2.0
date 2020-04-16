using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock {

    private int updatesPerSecond;
    private float timePassed;
    private bool pause = false;

    public Clock(int updatesPerSecond, float timePassed, bool canStep, bool canSlowDown) {
        this.updatesPerSecond = updatesPerSecond;
        this.timePassed = timePassed;
    }

    public int updateTime(float time) {
        //return the number of updates that have occured over the sum of the time passed and the passed in time

        int updatesPassed = 0;

        if (!this.pause) {

            timePassed = time + timePassed;

            float period = 1f / updatesPerSecond;

            if (timePassed > period) {

                updatesPassed = (int)(timePassed / period);
                timePassed = timePassed % period;
            }
        }

        return updatesPassed;
    }

    public bool Pause {
        get {
            return this.pause;
        } set {
            this.pause = value;
        }
    }
}
