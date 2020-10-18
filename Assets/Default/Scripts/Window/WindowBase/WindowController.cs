using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WindowController : MonoBehaviour {

	private Window data;
	private WindowManager manager;
	//private WindowContents contents;
	private RectTransform rectTransform;
	private Canvas canvas;

	// Use this for initialization
	void Start () {

		this.rectTransform = this.GetComponent<RectTransform> ();

		//closes the window when exiting 
		transform.Find("Header").Find("Exit").GetComponent<Button> ().onClick.AddListener ( () => {

			WindowManager.Instance.destroyWindow(this);
		});	

		//makes sure the contents is the correct size
		this.onResizeWindow ();
	}

	public void setUpWindow(Window window, WindowManager manager, Canvas canvas){

		//holds all the  data
		this.canvas = canvas;
		this.data = window;
		this.manager = manager;

		//spawns the contents
		Transform conentPanel = this.transform.Find ("MiddlePanel").Find("Content");
		this.data.Contents.spawnContents (this, conentPanel, this.canvas);

		//sets the min width
		this.setWidth (window.MinWidth, window.MinHeight);
		this.setFixedSize (window.Contents.FixxedSize);

        //updates the name
        Transform header = this.transform.Find("Header");
        header.Find ("WindowTitle").GetComponent<Text> ().text = window.Name;

		//makes this window the primary window
		manager.makeActive(this);

		//makes it so you cant resize the window
		if (this.data.Contents.FixxedSize) {
			Transform resizeTrans = this.transform.Find ("BottomPanel/ResizePanel");
			resizeTrans.GetComponent<ResizeWindow> ().enabled = false;
			resizeTrans.GetComponent<Image> ().color = new Color(.5f,.0f,.0f);
		}

        if (!this.data.Contents.Movable) {
            header.GetComponent<DragWindow>().enabled = false;
        }
	}

	private void setWidth(int width, int height){
		RectTransform rt = this.GetComponent<RectTransform> ();
		rt.sizeDelta = new Vector2 (width - Screen.width, height - Screen.height);
	}

	private void setFixedSize(bool value){
		//makes it so the window can change size or not

		if (value == true) {
			this.transform.Find ("BottomPanel").gameObject.SetActive (false);
		} else {
			this.transform.Find ("BottomPanel").gameObject.SetActive(true);
		}
	}

	public void onResizeWindow(){
		//updates the contents when the window has changes size
		//fix zooming on y

		Rect r = RectTransformUtility.PixelAdjustRect (this.rectTransform, this.canvas);
		//Debug.Log (r);
		this.data.Contents.changeWindowSize ((int)r.width, (int)r.height);
	}

	public WindowManager Manager {
		get {
			return manager;
		}
	}

	public Window Data {
		get {
			return data;
		}
	}
}