using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WindowContent
{

    protected InputActions inputs;
    protected bool fixxedSize = false;
    protected bool movable = true;

    private List<WindowContent> childWindows = new List<WindowContent>();

    /// <summary>
    /// Spawns a window that is attached to the currenet window <br></br>
    /// When the current window is Destroyed, the children windows will be as well
    /// </summary>

    public WindowContent() {
        this.inputs = new InputActions();

        //destorys all the children when window is destroyed
    }

    /// <summary>
    /// Spawns a window that is the child of this window <br></br>
    /// Child windows will be destroyed when parent window is destroyed
    /// </summary>
    /// <param name="window"></param>
    protected void spawnChildWindow(Window window) {

        GameObject result = WindowManager.Instance.spawnWindow(window);

        if (result != null) {

            childWindows.Add(window.Contents);
        }
    }

    /// <summary>
    /// Destroys the content and the child windows
    /// </summary>
    public void onDestroy() {

        this.destroyContent();

        int initCount = this.childWindows.Count;
        int counter = 0;
        while (this.childWindows.Count > 0 && counter < initCount) {
            WindowController controller = WindowManager.Instance.getControllerByData(this.childWindows[0]);
            WindowManager.Instance.destroyWindow(controller);
            counter++;
        }
    }

    protected abstract void destroyContent();

    public abstract void spawnContents(WindowController windowController, Transform contentPanel, Canvas canvas);

    public abstract void changeWindowSize(int width, int height);

    public abstract bool sameContent(WindowContent content);

    public abstract void receiveBroadcast(string message);

    protected Vector3 RectToWorld(Vector3 mousePos, Camera cam, RectTransform rectTransform, Canvas can) {
        //converts the rect transform position into world space for the camera

        Vector3 pos = rectTransform.position;
        Rect rect = RectTransformUtility.PixelAdjustRect(rectTransform, can);
        pos.x = pos.x - (rect.width / 2);
        pos.y = pos.y - (rect.height / 2);

        Vector3 diff = mousePos - pos;

        float xPer = diff.x / rect.width;
        float yPer = diff.y / rect.height;

        Vector3 result = cam.ViewportToWorldPoint(new Vector3(xPer, yPer, cam.nearClipPlane));
        result.z = 0;

        return result;
    }

    public InputActions Inputs {
        get {
            return inputs;
        }
        set {
            inputs = value;
        }
    }

    public bool FixxedSize {
        get {
            return this.fixxedSize;
        }
    }

    public bool Movable {
        get {
            return this.movable;
        }
    }
}
