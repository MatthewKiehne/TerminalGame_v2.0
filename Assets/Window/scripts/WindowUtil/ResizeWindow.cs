using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResizeWindow : MonoBehaviour, IDragHandler, IPointerDownHandler {

	private RectTransform window;
	private Vector3 downMousePos;
	private Vector3 downWinPos;
	private Rect downRect;

	private WindowController winController;

	// Use this for initialization
	void Start () {

		this.window = this.transform.parent.parent.GetComponent<RectTransform> ();
		this.winController = this.window.GetComponent<WindowController> ();
	}
		
	public void OnPointerDown (PointerEventData eventData) {
		
		this.downMousePos = Input.mousePosition;
		this.downWinPos = this.window.position;
		this.downRect = new Rect (this.window.rect);
		winController.Manager.makeActive (this.winController);
	}

	public void OnDrag (PointerEventData eventData) {

		Vector3 dif = (Input.mousePosition - this.downMousePos);


		Vector2 newDelta = window.sizeDelta;
		Vector2 newPosition = window.position;

		bool changedSize = false;

		//checks to see if it is within the legal bounds
		if (this.downRect.width + dif.x > this.winController.Data.MinWidth ) {

			newDelta.x =  this.downRect.width + dif.x - Screen.width;

			newPosition.x = downWinPos.x + (dif.x / 2);

			changedSize = true;
		}

		if( this.downRect.height - dif.y > this.winController.Data.MinHeight ){

			newDelta.y = this.downRect.height - dif.y - Screen.height;
			newPosition.y = downWinPos.y + (dif.y / 2);

			changedSize = true;
		}

		//updates the positions and delta
		this.window.position = newPosition;
		this.window.sizeDelta = newDelta;

		if (changedSize) {
			//tells the window controller that the window has changed size
			this.winController.onResizeWindow ();
		}
	}
}