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

	public bool active(InputData data){
		//same as above

		/*
		List<KeyCode> codes = data.Down;

		if (this.status == KeyStatus.Held) {
			codes = data.Held;
		} else if (this.status == KeyStatus.Up) {
			codes = data.Up;
		} else if(this.status == KeyStatus.Rest){
			codes = data.Rest;
		}

		return this.active (codes, this.status);
		*/

		bool result = true;
		//get the status
		int counter = 0;

		while (result && counter < this.combination.Count) {

			//checks key down
			if (this.status == KeyStatus.Down) {
				result = Input.GetKeyDown (this.combination [counter]);

			//checks the key held
			} else if (this.status == KeyStatus.Held) {
				result = Input.GetKey (this.combination [counter]);

			//checks the key up
			} else if (this.status == KeyStatus.Up) {
				result = Input.GetKeyUp (this.combination [counter]);

			//chekcs the key is at rest
			} else {
				result = (!Input.GetKey (this.combination [counter]) && 
					!Input.GetKeyDown (this.combination [counter]) && 
					!Input.GetKeyUp (this.combination [counter]));
			}

			counter++;
		}
		return result;
	}

	/*
	public bool active(List<KeyCode> keys, KeyStatus status){
		//retuns true if the combination is a subset of the keys

		bool result = true;
		int comIndex = 0;

		//makes sure it is the same status
		if (this.status != status) {
			result = false;
		}

		//loops through all the combination keycodes
		while (result && comIndex < combination.Count) {

			//the keys does not holds one of the combination keys
			if (!keys.Contains (this.combination [comIndex])) {
				result = false;
			}
			comIndex++;
		}

		return result;
	}

	*/

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
