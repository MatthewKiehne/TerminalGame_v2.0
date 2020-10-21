using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveComponentController : GraphComponentController {

    protected List<SenderController> senderControllers = new List<SenderController>();

    protected void formBasicShape(InteractiveComponent comp) {

        Transform middle = this.transform.Find("MiddleBody");
        Transform reciever = this.transform.Find("Reciever");
        Transform sender = this.transform.Find("Sender");

        GameObject.Destroy(sender.gameObject);
        GameObject.Destroy(reciever.gameObject);

        //makes the middle
        //GameObject currentMiddle = Instantiate(middle.gameObject);
        middle.transform.SetParent(this.transform);
        middle.transform.localScale = new Vector3(comp.Size.x - .2f, comp.Size.y - .4f);
        middle.transform.localPosition = Vector3.zero;
        middle.transform.rotation = this.transform.rotation;

        reciever.localPosition = new Vector3(((comp.Size.x - 1) / -2f), ((comp.Size.y - 1) / -2f) - .35f, 0);

        //makes the receivers
        for (int i = 0; i < comp.receiverCount(); i++) {
            GameObject currentReciever = Instantiate(reciever.gameObject);
            currentReciever.transform.SetParent(this.transform);
            currentReciever.transform.localPosition = new Vector3(((comp.Size.x - 1) / -2f) + i, ((comp.Size.y - 1) / -2f) - .35f, 0);
            currentReciever.transform.rotation = this.transform.rotation;

            BoxCollider2D bc = currentReciever.GetComponent<BoxCollider2D>();
            //bc.enabled = true;
        }

        //makes the senders
        for (int i = 0; i < comp.senderCount(); i++) {
            GameObject currentSender = Instantiate(sender.gameObject);
            currentSender.transform.SetParent(this.transform);
            currentSender.transform.localPosition = new Vector3(((comp.Size.x - 1) / -2f) + i, ((comp.Size.y - 1) / 2f) + .35f, 0);
            currentSender.transform.rotation = this.transform.rotation;


            SenderController cs = currentSender.AddComponent<SenderController>();
            BoxCollider2D bc = currentSender.GetComponent<BoxCollider2D>();

            cs.setUp(comp.getSenderAt(i));
            this.senderControllers.Add(cs);
        }
    }

    public void displaySprite(string name) {

        Transform middle = this.transform.Find("MiddleBody");

        Texture2D gateTexture = (Texture2D)SceneResouces.SceneObjects["Default"][typeof(Texture2D)][name];
        SpriteRenderer gateSpriteRenderer = middle.transform.Find("LogicComponentSprite").GetComponent<SpriteRenderer>();
        Sprite gateSprite = Sprite.Create(gateTexture, new Rect(0, 0, gateTexture.width, gateTexture.height), Vector2.one * .5f, gateTexture.width);
        gateSpriteRenderer.sprite = gateSprite;

        middle.gameObject.SetActive(true);
    }

    public override void tickUpdate() {

        InteractiveComponent comp = (InteractiveComponent)this.graphComponent;

        int state = -1;

        if (comp.GetType().IsSubclassOf(typeof(LinkComponent))) {
            state = ((LinkComponent)comp).getExtensionConnection().Value;
        } else if(comp.GetType().IsSubclassOf(typeof(LogicComponent))) {
            state = ((LogicComponent)comp).getState();
        } else {
            throw new Exception(comp.GetType() + " was not found");
        }

        

        for (int i = 0; i < this.senderControllers.Count; i++) {

            bool isActive = Convert.ToBoolean((state >> (this.senderControllers.Count - 1 - i)) & 1);

            this.senderControllers[i].updateSignal(isActive);
        }
    }

    public override void createNewRays() {
        //makes all new rays for the component
        foreach (SenderController sc in this.senderControllers) {
            sc.makeNewRays();
        }
        tickUpdate();
    }

    public List<SenderController> SenderControllers {
        get {
            return this.senderControllers;
        }
    }
}
