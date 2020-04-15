using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelector : MonoBehaviour {

    [SerializeField]
    private Color defaultColor;
    [SerializeField]
    private Color selectedColor;
    [SerializeField]
    private Color hoverColor;
    [SerializeField]
    private Color pressedColor;

    List<Button> buttons = new List<Button>();

    private void Start() {

        /*
        //gets all the buttons
        for(int i = 0; i < this.transform.childCount; i++) {

            int index = i;

            Button b = this.transform.GetChild(index).GetComponent<Button>();

            if(b != null) {
                this.buttons.Add(b);
                b.onClick.AddListener( () => {

                    Debug.Log(b.gameObject.transform.name);

                    //clears
                    //this.clear();

                    //make buttons the selected one
                    ColorBlock selectedBlock = ColorBlock.defaultColorBlock;
                    selectedBlock.selectedColor = this.selectedColor;
                    b.colors = selectedBlock;
                });
            }
        } 
        */
    }

    public void clear() {

        for(int i = 0; i < this.buttons.Count; i++) {

            Button b = this.buttons[i];
            b.colors = ColorBlock.defaultColorBlock;
        }
    }
}