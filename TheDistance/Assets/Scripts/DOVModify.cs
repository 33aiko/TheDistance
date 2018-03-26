using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class DOVModify : MonoBehaviour {
	public PostProcessingProfile profile;



	DepthOfFieldModel.Settings s; 


	void Start () { 
		SetActive (false);
		s = profile.depthOfField.settings; 

	}


	public void SetFocalLength(float focalLength){
		SetActive (true);
		s.focalLength = focalLength;
		profile.depthOfField.settings = s; 
	}

	public void SetActive(bool isActive){
		profile.depthOfField.enabled = isActive; 
	}
}
