using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LogicComponent : InteractiveComponent {

    

    //sets the state of the component based on the logic check and then clears the receivers
    public abstract void setState();

    public LogicComponent(Vector2Int position, Vector2Int size, int rotation, bool flipped,  int numSenders, int numReceivers) : 
        base(position,size,rotation,flipped, numSenders, numReceivers){
    }

    public override List<Tuple> getValues() {

        List<Tuple> result = new List<Tuple>();

        for (int i = 0; i < this.receivers.Count; i++) {
            result.Add(new Tuple("rec:" + i, this.getReceiverAt(i).getActive() + ""));
        }

        return result;
    }

    public override void setValues(List<Tuple> values) {
        //sets the receivers to the correct value

        string receiverString = "rec:";
        foreach (Tuple tup in values) {
            if (tup.Name.StartsWith(receiverString)) {

                int index = int.Parse(tup.Name.Substring(receiverString.Length));
                bool state = bool.Parse(tup.Value);

                //Debug.Log(index + " " + state);
                this.getReceiverAt(index).setActive(state);
            }
        }
    }


    public void SendSignal() {
        //sends the state to the sender to send off

        int currentState = state;

        for (int i = 0; i < senders.Count; i++) {
            Sender s = this.getSenderAt(i);

            if(((currentState >> i) & 1) == 1) {
                s.setTargetsActive();
            }

            //moves to the next bit
            currentState = state >> 1;
        }
    }

    public void clearReceivers() {
        //clears the state of all the receivers

        for(int i = 0; i < this.receiverCount(); i++) {
            this.getReceiverAt(i).setActive(false);
        }
    }
}
