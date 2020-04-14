using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WindowInput {

	private Dictionary<KeyCombination, Action> actions;

	private InputData currentFrameData;


	public WindowInput(){
		//stores what happens when the player hit an input
		
		this.actions = new Dictionary<KeyCombination, Action> ();
	}

	public void activateInputs(InputData inputs){
		//activates all the InputActions that have thier inputs passed in

		this.currentFrameData = inputs;

		List<KeyCombination> combinations = new List<KeyCombination> (this.actions.Keys);

		//loops through all the combinations
		foreach (KeyCombination com in combinations) {

			//checks to see if the key is in the dictionary
			if (com.active(inputs)) {
				this.actions [com].Invoke ();
			}
		}
	}

	public void addInput(KeyCombination key, Action action){

		//checks to see if the key already exits
		if (actions.ContainsKey (key)) {

			//adds an action to the existing actions for the key
			actions [key] += action;

		} else {

			//adds it to the dictionary
			actions.Add (key, action);
		}
	}

	public InputData CurrentFrameData {
		get {
			return currentFrameData;
		}
	}
}
