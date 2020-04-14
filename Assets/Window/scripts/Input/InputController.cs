using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour {
	private WindowManager windowManager;

	// Use this for initialization
	void Start () {

		this.windowManager = GameObject.Find ("WindowManager").GetComponent<WindowManager>();


	}

	// Update is called once per frame
	void Update () {

		//gets all the input data from the screen
		InputData data = new InputData ();

		data.ScrollWheel = Input.GetAxis ("Mouse ScrollWheel");

		//determine if it is over a window
		PointerEventData pointerData = new PointerEventData (EventSystem.current);
		pointerData.position = Input.mousePosition;

		List<RaycastResult> raycastResults = new List<RaycastResult> ();
		EventSystem.current.RaycastAll (pointerData, raycastResults);

		data.RaycastResults = raycastResults;
		data.MousePosition = Input.mousePosition;

		//gives the data to the active window
		windowManager.giveActiveWindowInputs (data);
	}
}
