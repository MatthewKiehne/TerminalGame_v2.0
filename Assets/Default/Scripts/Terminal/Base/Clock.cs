using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock {

    private float maxTime;
    private float timePassed;

    public Clock(float maxTime) {
        this.maxTime = maxTime;
        this.timePassed = 0;
    }

    public int updateTime(float time) {
        //return the number of updates that have occured over the sum of the time passed and the passed in time

        int updatesPassed = 0;

        timePassed = time + timePassed;

        if (timePassed >= this.maxTime) {

            updatesPassed = (int)(timePassed / maxTime);
            timePassed = timePassed % maxTime;
        }

        return updatesPassed;
    }

    public float MaxTime {
        get {
            return this.maxTime;
        }
    }
}
