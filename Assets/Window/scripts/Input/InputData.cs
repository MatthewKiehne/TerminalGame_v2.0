using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputData {

	//returns all the raycast result
	private List<RaycastResult> raycastResults;

	//holds the mouse position on screen
	private Vector2 mousePosition;

	//holds the scrollWheel data
	private float scrollWheel;

	public InputData(){
		this.raycastResults = new List<RaycastResult> ();
	}

	public List<RaycastResult> RaycastResults {
		get {
			return raycastResults;
		}
		set {
			raycastResults = value;
		}
	}

	public Vector2 MousePosition {
		get {
			return mousePosition;
		}
		set {
			mousePosition = value;
		}
	}

	public float ScrollWheel {
		get {
			return scrollWheel;
		}
		set {
			scrollWheel = value;
		}
	}
}
