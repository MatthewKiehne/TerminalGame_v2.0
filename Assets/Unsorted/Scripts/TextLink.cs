using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;


[RequireComponent(typeof(TMPro.TMP_Text))]

public class TextLink : MonoBehaviour, IPointerDownHandler{

    private Dictionary<string, string> siteTable = new Dictionary<string, string>();

    [SerializeField]
    private Camera camera;
    private TMP_Text text;

    private void Start() {
        this.text = this.GetComponent<TMP_Text>();
        this.addLink("link1", "https://www.electronics-tutorials.ws/boolean/bool_7.html");
    }


    public void OnPointerDown(PointerEventData eventData) {
        int link = TMP_TextUtilities.FindIntersectingLink(this.text, eventData.position, eventData.pressEventCamera);
        
        if(link != -1) {

            string website = this.siteTable[this.text.textInfo.linkInfo[link].GetLinkID()];
            Application.OpenURL(website);
        }
    }

    public void addLink(string id, string link) {
        this.siteTable.Add(id, link);
    }

    public void setCamera(Camera cam) {
        this.camera = cam;
    }
}