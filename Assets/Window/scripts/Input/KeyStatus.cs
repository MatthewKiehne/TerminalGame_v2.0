using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyStatus{
	//rest is when the key is not pressed, down, or up
	//down is the frame the key is pressed
	//held is all the frames the key is pressed minus the first frame
	//up is the frame the key is released after is is Held
	Rest, Down, Held, Up
}
