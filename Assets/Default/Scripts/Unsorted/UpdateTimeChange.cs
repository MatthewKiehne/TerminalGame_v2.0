using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]

public class UpdateTimeChange : MonoBehaviour {

    [SerializeField]
    private UpdateToggle toggle;

    public float MaxTime;

    private Image image;

    public void Start() {
        this.image = this.GetComponent<Image>();

        this.GetComponent<Button>().onClick.AddListener(() => {
            this.toggle.setMaxTime(this.MaxTime);
            this.toggle.setColor(this.image.color);
        });
    }



}