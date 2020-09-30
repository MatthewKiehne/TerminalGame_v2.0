using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCombination {

	//holds all the keys in the combination
	private List<KeyCode> combination = new List<KeyCode>();
	private KeyStatus status;

	public KeyCombination(KeyCode key, KeyStatus status){
		this.combination.Add (key);
		this.status = status;
	}

	public KeyCombination(List<KeyCode> keys, KeyStatus status){
		this.combination = keys;
		this.status = status;
	}

	public KeyCombination(KeyCode[] keys, KeyStatus status){
		foreach (KeyCode key in this.combination) {
			this.combination.Add (key);
		}
		this.status = status;
	}

    public bool active(InputData data) {
        //same as above

        bool result = true;
        //get the status
        int counter = 0;

        while (result && counter < this.combination.Count) {

            switch (this.status) {
                case KeyStatus.Down:
                    result = Input.GetKeyDown(this.combination[counter]);
                    break;
                case KeyStatus.Held:
                    result = Input.GetKey(this.combination[counter]);
                    break;
                case KeyStatus.Up:
                    result = Input.GetKeyUp(this.combination[counter]);
                    break;
                case KeyStatus.Rest:
                    result = (!Input.GetKey(this.combination[counter]) &&
                    !Input.GetKeyDown(this.combination[counter]) &&
                    !Input.GetKeyUp(this.combination[counter]));
                    break;

            }

            counter++;
        }
        return result;
    }

	public bool sameCombination(KeyCombination comb){
		//retuns true if all the same keys are in both combinations
		bool result = true;
		List<KeyCode> combList = comb.getKeys ();

		if (combList.Count == this.getKeys ().Count) {

			//loop through all this class' keys
			int counter = 0; 

			while (result && counter < this.getKeys ().Count) {

				if(!combList.Contains(this.getKeys()[counter])){
					result = false;
				}

				counter++;
			}

		} else {
			result = false;
		}

		return result;
	}

	public List<KeyCode> getKeys(){
		return this.combination;
	}
}
