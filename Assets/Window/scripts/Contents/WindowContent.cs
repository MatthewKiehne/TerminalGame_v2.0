using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WindowContent {

    protected InputActions inputs;
    protected bool fixxedSize = false;
    protected bool movable = true;

    public WindowContent() {
        this.inputs = new InputActions();
    }

    public abstract void spawnContents(WindowController windowController, Transform contentPanel, Canvas canvas);

    public abstract void changeWindowSize(int width, int height);

    public abstract void onDestroy();

    public abstract bool sameContent(WindowContent content);

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
