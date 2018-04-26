using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonSFX : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler {

	AudioManager audioManager; 

	void Start(){
		audioManager = GameObject.Find ("AudioManager").GetComponent<AudioManager> (); 
	}

	public void OnPointerEnter (PointerEventData eventdata){
		if (audioManager != null) {
//			audioManager.Play ("UIMouseHover");
		}
	}

	public void OnPointerDown(PointerEventData eventdata){
		if (audioManager != null) {
            if (this.name == "DiaryBtn")
            {
                audioManager.Play("DiaryOpen");
            }
            else if (this.name == "MixerGroup")
            {

            }
            else
            {
                print("clicking: " + name);
                audioManager.Play("UIMouseClick");
            }
		}
	}

}
