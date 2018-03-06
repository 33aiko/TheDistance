using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour {

	[SerializeField] GameObject Scene1,Scene2; 
	[SerializeField] GameObject LobbyManager; 
	[SerializeField] GameObject player; 

	Text[] instructions; 
	Camera mainCam; 

	void Start () {
		mainCam = Camera.main;
		LobbyManager.SetActive (false);
		instructions = GetComponentsInChildren<Text> ();
		foreach (Text t in instructions) {
			t.DOFade (0, 0);
		}
		GetComponent<AudioSource> ().Play ();
	}
	

	void Update () {
		if (Scene1.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("done")) {
			//Debug.Log ("animation done");
			LobbyManager.SetActive (true);
		}
	}

	public void StartNewGame(){
		mainCam.transform.DOMoveZ (0, 2);
		Scene2.SetActive (true);
		foreach (Text t in instructions) {
			t.DOFade (1, 0.5f).SetDelay (2);
		}
	}
		
}
