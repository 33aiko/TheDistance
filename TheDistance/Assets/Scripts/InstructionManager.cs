using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System; 

public class InstructionManager : MonoBehaviour {

	public string instructionForKeyboard; 
	public string instructionForController; 
	public int idx = -1;

	bool isController = false; 


	void Start(){

		isController = GameObject.Find ("InputDeviceManager").GetComponent<InputDeviceManager> ().isController; 

		if (isController) {
			instructionForController = instructionForController.Replace(";","\n");
			if (idx != -1) {
				GetComponent<InstructionAreaTrigger> ().npcTalks [idx] = instructionForController; 
			} else {
				GetComponent<Text> ().text = instructionForController;
			}

		} else {
			instructionForKeyboard = instructionForKeyboard.Replace(";","\n");
			if (idx != -1) {
				GetComponent<InstructionAreaTrigger> ().npcTalks [idx] = instructionForKeyboard; 
			} else {
				GetComponent<Text> ().text  = instructionForKeyboard;
			}


		}


	}





}
