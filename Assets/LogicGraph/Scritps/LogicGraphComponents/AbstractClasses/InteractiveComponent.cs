using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveComponent : GraphComponent {

    protected List<Sender> senders = new List<Sender>();
    protected List<Receiver> receivers = new List<Receiver>();
    protected int state;

    public InteractiveComponent(Vector2Int position, Vector2Int size, int rotation, bool flipped, int numSenders, int numRecievers, ComponentCategory type) : 
        base(position, size, rotation, flipped, type) {

        Vector2Int rotatedSize = size;
        if ((rotation % 2) == 1) {
            rotatedSize = new Vector2Int(size.y, size.x);
        }

        //make the body
        Rect bodyRect = new Rect(position + new Vector2(.2f, .2f), new Vector2(rotatedSize.x - .4f, rotatedSize.y - .4f));
        ComponentBody body = new ComponentBody(bodyRect);
        componentPieces.Add(body);

        //the size of the sensor
        Vector2 sensorSize = new Vector2(.6f, .1f);

        //incroment info
        Vector2 incromentor = new Vector2(1, 0);

        Vector2 toRecieverCorner = new Vector2(.2f, .1f);
        Vector2 recieverStartPos = Vector2.zero;

        Vector2 toSenderCorner = new Vector2(.2f, -.2f);
        Vector2 senderStartPos = new Vector2(0, rotatedSize.y);

        if (flipped) {
            //flipped

            if (rotation == 0) {

                incromentor = new Vector2(-1, 0);

                senderStartPos = new Vector2(rotatedSize.x, rotatedSize.y);
                toSenderCorner = new Vector2(-.8f, -.2f);

                recieverStartPos = new Vector2(rotatedSize.x, 0);
                toRecieverCorner = new Vector2(-.8f, .1f);

            } else if (rotation == 1) {

                incromentor = new Vector2(0, 1);
                sensorSize = new Vector2(.1f, .6f);

                senderStartPos = new Vector2(rotatedSize.x, 0);
                toSenderCorner = new Vector2(-.2f, .2f);

                recieverStartPos = Vector2.zero;
                toRecieverCorner = new Vector2(.1f, .2f);

            } else if (rotation == 2) {

                senderStartPos = Vector2.zero;
                toSenderCorner = new Vector2(.2f, .1f);

                recieverStartPos = new Vector2(0, rotatedSize.y);
                toRecieverCorner = new Vector2(.2f, -.2f);

            } else if (rotation == 3) {

                incromentor = new Vector2(0, -1f);
                sensorSize = new Vector2(.1f, .6f);

                senderStartPos = new Vector2(0, rotatedSize.y);
                toSenderCorner = new Vector2(.1f, -.8f);

                recieverStartPos = new Vector2(rotatedSize.x, rotatedSize.y);
                toRecieverCorner = new Vector2(-.2f, -.8f);
            }

        } else {
            //not flipped

            if (rotation == 0) {
                //this is the default and does nothing

            } else if (rotation == 1) {

                incromentor = new Vector2(0, -1f);
                sensorSize = new Vector2(.1f, .6f);

                senderStartPos = new Vector2(rotatedSize.x, rotatedSize.y);
                toSenderCorner = new Vector2(-.2f, -.8f);

                recieverStartPos = new Vector2(0, rotatedSize.y);
                toRecieverCorner = new Vector2(.1f, -.8f);

            } else if (rotation == 2) {

                incromentor = new Vector2(-1, 0);

                senderStartPos = new Vector2(rotatedSize.x, 0);
                toSenderCorner = new Vector2(-.8f, .1f);

                recieverStartPos = new Vector2(rotatedSize.x, rotatedSize.y);
                toRecieverCorner = new Vector2(-.8f, -.2f);

            } else if (rotation == 3) {

                incromentor = new Vector2(0, 1);
                sensorSize = new Vector2(.1f, .6f);

                senderStartPos = Vector2.zero;
                toSenderCorner = new Vector2(.1f, .2f);

                recieverStartPos = new Vector2(rotatedSize.x, 0);
                toRecieverCorner = new Vector2(-.2f, .2f);

            }
        }

        //make the recievers
        for (int i = 0; i < numRecievers; i++) {
            //Rect reciever = new Rect(position + new Vector2(.2f + i, .1f), sensorSize);
            Vector2 botLeft = position + recieverStartPos + toRecieverCorner + (incromentor * i);
            //Debug.Log("Reciever: " + botLeft);
            Rect recieverRect = new Rect(botLeft, sensorSize);

            Receiver receiver = null;

            if (this.GetType().IsSubclassOf(typeof(BridgeComponent))) {

                int state = 1;
                state = state << i;

                receiver = new BReceiver(recieverRect,(SendBridge)this,state);

            } else if(this.GetType().IsSubclassOf(typeof(LogicComponent))) {
                receiver = new LReceiver(recieverRect);
            } else {
                throw new System.Exception(this.GetType() + " is not supported when creating the receivers");
            }

             
            this.componentPieces.Add(receiver);
            this.receivers.Add(receiver);
        }

        //make the senders
        for (int i = 0; i < numSenders; i++) {
            Vector2 botLeft = position + senderStartPos + toSenderCorner + (incromentor * i);
            //Debug.Log("Sendr: " + botLeft);
            Rect senderRect = new Rect(botLeft, sensorSize);

            Sender sender = new Sender(senderRect);
            this.componentPieces.Add(sender);
            this.senders.Add(sender);
        }
    }

    public void clearRecievers() {
        //clears all the receivers

        for(int i = 0; i < this.receivers.Count; i++) {
            this.receivers[i].setActive(false);
        }
    }

    public Sender getSenderAt(int index) {
        return this.senders[index];
    }

    public Receiver getReceiverAt(int index) {
        return this.receivers[index];
    }

    public int senderCount() {
        return this.senders.Count;
    }

    public int receiverCount() {
        return this.receivers.Count;
    }

    public int getState() {
        return this.state;
    }
}
