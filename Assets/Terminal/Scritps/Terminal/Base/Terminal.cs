using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : GraphInteger<TExtension>{

    private Clock clock;

    private List<HeldExtension> heldExtensions = new List<HeldExtension>();

    private string name;

    public Terminal(string name) {
        this.name = name;
    }
    public Terminal(TerminalData data) {
        this.name = data.Name;
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
        for (int i = 0; i < this.heldExtensions.Count; i++) {
            this.heldExtensions[i].Extension.setState();
            this.heldExtensions[i].Extension.clearReceivers();

            //Debug.Log("extension Update: " + this.extensions[i].Name);
        }

        //sends the signal to the extensions
        for(int i = 0; i < this.heldExtensions.Count; i++) {
            this.heldExtensions[i].Extension.sendSignal();
        }
    }

    public TExtension getExtentionAt(int index) {
        return this.heldExtensions[index].Extension;
    }

    public int getExtentionLength() {
        return this.heldExtensions.Count;
    }

    public List<TExtension> placedExtensions() {
        //gets all the placed TExtensions
        List<TExtension> result = new List<TExtension>();

        for(int i = 0; i < this.heldExtensions.Count; i++) {
            if (this.heldExtensions[i].Placed) {
                result.Add(this.heldExtensions[i].Extension);
            }
        }
        return result;
    }

    #region GraphInteger
    public bool addComponent(TExtension component) {
        this.heldExtensions.Add(new HeldExtension(component));
        return true;
    }

    public bool removeComponent(TExtension component) {
        throw new System.NotImplementedException();
    }

    public bool canPlace(TExtension component) {
        //checks if the component passed in overlaps a component that is already placed

        bool result = true;
        List<TExtension> placed = this.placedExtensions();
        int counter = 0;

        while(result && counter < placed.Count) {

            if (placed[counter].getDimentions().Overlaps(component.getDimentions())){
                result = false;
            }
            counter++;
        }

        return result;
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

    private class HeldExtension {

        private TExtension extension;
        private bool placed;

        public HeldExtension(TExtension extension) {
            this.extension = extension;
            this.placed = false;
        }

        public TExtension Extension {
            get {
                return this.extension;
            }
        }

        public bool Placed {
            get {
                return this.placed;
            }
            set {
                this.placed = value;
            }
        }

    }
}
