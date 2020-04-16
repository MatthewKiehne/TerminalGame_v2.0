using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : GraphInteger<TExtension>{

    


    private Clock clock;

    private List<TExtension> extensions = new List<TExtension>();

    private string name;

    public Terminal(string name) {
        this.name = name;
    }

    public int updateTime(float timePassed) {
        //updates the extensions based on the number of periods the clock has passed 

        int numUpdates = 0;

        if(this.clock != null) {

            numUpdates = this.clock.updateTime(timePassed);
            Debug.Log(numUpdates);
            for(int i = 0; i < numUpdates; i++) {
                this.updateExtensions();
            }
        }
        return numUpdates;
    }

    public void update() {
        this.updateExtensions();
    }

    private void updateExtensions() {
        //updates the terminal and all of the TExtensions in it

        //Debug.Log("Terminal Update Extensions " + this.Name);

        //sets the state of each extension then clears the extensions
        for (int i = 0; i < this.extensions.Count; i++) {
            this.extensions[i].setState();
            this.extensions[i].clearReceivers();

            //Debug.Log("extension Update: " + this.extensions[i].Name);
        }

        //sends the signal to the extensions
        for(int i = 0; i < this.extensions.Count; i++) {
            this.extensions[i].sendSignal();
        }
    }

    public TExtension getExtentionAt(int index) {
        return this.extensions[index];
    }

    public int getExtentionLength() {
        return this.extensions.Count;
    }

    
    public void addExtension(TExtension extension) {
        this.extensions.Add(extension);
    }

    #region GraphInteger
    public bool addComponent(TExtension component) {
        throw new System.NotImplementedException();
    }

    public bool removeComponent(TExtension component) {
        throw new System.NotImplementedException();
    }

    public bool canPlace(TExtension component) {
        throw new System.NotImplementedException();
    }

    public TExtension getComponentAt(int x, int y) {
        throw new System.NotImplementedException();
    }

    public List<TExtension> getAllGraphComponents() {
        throw new System.NotImplementedException();
    }
    #endregion

    public Clock Clock {
        get {
            return this.clock;
        }
        set {
            this.clock = value;
        }
    }

    public string Name {
        get {
            return this.name;
        }
    }
}
