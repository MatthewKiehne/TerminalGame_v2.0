using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragWindow : MonoBehaviour, IDragHandler, IPointerDownHandler {

	private RectTransform window;
	private Vector3 offset;

	private WindowController controller;

	// Use this for initialization
	void Start () {

		this.window = this.transform.parent.GetComponent<RectTransform> ();
		this.controller = this.window.GetComponent<WindowController> ();
	}

	public void OnPointerDown (PointerEventData eventData) {
		this.offset = window.position - Input.mousePosition;
		this.controller.Manager.makeActive (controller);
	}		

	public void OnDrag (PointerEventData eventData) {

		//makes sure it is within the window
		// to do

		Vector3 vec = Input.mousePosition;
		this.window.position = vec + offset;
	}
}
