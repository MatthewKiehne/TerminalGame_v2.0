using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]

public class UpdateToggle : MonoBehaviour {

    [SerializeField]
    private Button updateButton;

    private Button runButton;

    private float updateMaxTime = 2.5f;
    private float timePassed = 0f;

    private bool updateTime = false;

    private Image timeDisplay;

    private void Start() {

        this.runButton = this.GetComponent<Button>();
        this.timeDisplay = this.transform.Find("TimeDisplay").GetComponent<Image>();

        this.runButton.onClick.AddListener(() => {
            this.updateTime = !updateTime;
        });
    }

    private void Update() {


        if (this.updateTime) {
            this.timePassed += Time.deltaTime;
            this.timeDisplay.fillAmount = this.timePassed / this.updateMaxTime;

            if (this.timePassed > this.updateMaxTime) {

                this.updateButton.onClick.Invoke();

                this.timePassed = this.timePassed % this.updateMaxTime;
            }
        }
    }

    public void setColor(Color color) {
        this.timeDisplay.color = color;
    }
    public void setMaxTime(float time) {
        this.updateMaxTime = time;
        this.timeDisplay.fillAmount = this.timePassed / this.updateMaxTime;
    }

}