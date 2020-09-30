using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour {

    public static WindowManager Instance;

    [SerializeField]
    private Canvas useCanvas;

	private List<WindowController> windows = new List<WindowController> ();
    private bool allowSpawnWindows = true;

    public void Awake() {
        WindowManager.Instance = this;
    }

    public bool windowAlreadyExists(WindowContent contents) {
        //checks to see if the window already exists

        bool result = false;
        int counter = 0;
        while (!result && counter < this.windows.Count) {

            if (this.windows[counter].Data.Contents.sameContent(contents)) {
                result = true;
            }

            counter++;
        }

        return result;
    }

    public GameObject spawnWindow(Canvas canvas, Window window){
        //creates a window on passed in canvas
        //returns the window
        //makes the Window GameObject

        GameObject win = null;

        if (this.windowAlreadyExists(window.Contents)) {
            //pops up the pervious window

            //Debug.Log("WindowManager -> spawnWindow(): make existing window active");

            WindowController controller = this.getControllerByData(window.Contents);
            this.makeActive(controller);

        } else {
            //creates a new window

            //Debug.Log("WindowManager -> spawnWindow(): create new window");

            GameObject windowPrefab = (GameObject)SceneResouces.SceneObjects[typeof(GameObject)]["Window"];

            win = Instantiate(windowPrefab);
            win.transform.SetParent(canvas.transform, false);
            win.transform.name = window.Name;

            //adds the list of all windows
            WindowController wc = win.GetComponent<WindowController>();
            this.windows.Add(wc);

            //sets up the window
            wc.setUpWindow(window, this, canvas);

            //makes the keycode 'ESC' close the window and make the next window active
            window.Contents.Inputs.addInput(new KeyCombination(KeyCode.Escape, KeyStatus.Up), () => {

                this.removeWindow(wc);
            });
        }

		return win;
	}

    public GameObject spawnWindow(Window window) {
        //creates a window using the "main canvas"
        //returns the window

        return this.spawnWindow(this.useCanvas, window);
    }

    public WindowController getControllerByData(WindowContent content) {
        //gets the controller based by the content


        WindowController result = null;
        int counter = 0;

        while(result == null && counter < this.windows.Count) {

            if (this.windows[counter].Data.Contents.sameContent(content)) {
                result = this.windows[counter];
            }

            counter++;
        }

        return result;
    }

	public void giveActiveWindowInputs(InputData data){
		
		//gives the active window the keyboard iputs

		WindowController active = this.getActiveWindow ();

		if (active != null) {
			active.Data.Contents.Inputs.activateInputs (data);
		}
	}

	public WindowController getActiveWindow(){
		//returns the current active window

		WindowController result = null;

		if (this.windows.Count != 0) {
			result = this.windows [0];
		}

		return result;
	}

    public bool mouseOverWindow(InputData data) {
        //checks to see if the click was over a window

        //tries to find the first Window Controller
        WindowController firstWindow = null;
        int hitIndex = 0;
        while (hitIndex < data.RaycastResults.Count && firstWindow == null) {

            WindowController con = data.RaycastResults[hitIndex].gameObject.GetComponent<WindowController>();

            if (con != null) {

                firstWindow = con;
            }

            hitIndex++;
        }

        if (firstWindow != null) {
            //makes the first window active
            this.makeActive(firstWindow);
        }

        return firstWindow != null;

    }

	public void removeWindow(WindowController controller){

		//removes the window from the list
		windows.Remove (controller);

		//destroys the window when clicked
		GameObject.Destroy(controller.gameObject);

		//makes the next window active
		if (this.windows.Count > 0) {
			this.makeActive (this.windows [0]);
		}
	}

    public void removeAllWindows() {
        //removes all the windows

        int counter = 0;

        while(this.windows.Count > 0 || counter > 100) {
            this.windows[0].destroyWindow();
            counter++;
        }
    }

    public void setActivityofCurrentWindows(bool value) {
        //shows or hides all the windows

        foreach(WindowController content in this.windows) {
            content.gameObject.SetActive(value);
        }
    }

	public void makeActive(WindowController controller){

		bool found = windows.Remove (controller);

		if (found) {

			windows.Insert (0, controller);
			controller.transform.SetAsLastSibling ();

		} else {

			throw new UnityException ("the window contolle was not found when tryting to set to active window");
		}
	}

	public static Canvas GetMainCanvas(){
		Canvas result = GameObject.Find ("MainCanvas").GetComponent<Canvas>();

		return  result;
	}

    public bool AllowSpawnWindows {
        get {
            return this.allowSpawnWindows;
        }
        set {
            this.allowSpawnWindows = value;
        }
    } 
}
