using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputCoordinator : MonoBehaviour
{

    public static InputCoordinator Instance;

    private void Awake() {
        InputCoordinator.Instance = this;
    }

    private void Update() {

        //gets all the input data from the screen
        InputData data = new InputData();

        data.ScrollWheel = Input.GetAxis("Mouse ScrollWheel");

        //determine if it is over a window
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        data.RaycastResults = raycastResults;
        data.MousePosition = Input.mousePosition;

        WindowManager.Instance.giveActiveWindowInputs(data);
    }
}