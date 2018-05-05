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

	GameObject mainMenu; 

	void Start () {
		mainCam = Camera.main;
		LobbyManager.SetActive (false);
		instructions = GetComponentsInChildren<Text> ();
		foreach (Text t in instructions) {
			t.DOFade (0, 0);
		}
		//GetComponent<AudioSource> ().Play ();
	}
	

	void Update () {
		if (Scene1.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("done")) {
			//Debug.Log ("animation done");
            if(LobbyManager)
		    	LobbyManager.SetActive (true);
		}
	}

	public void StartNewGame(){
		mainCam.transform.DOMoveZ (0, 2);
		Scene2.SetActive (true);

		foreach (Text t in instructions) {
			t.DOFade (1, 0.5f).SetDelay (2);
		}
		instructions [1].text = "Wait for your friend to join the game...";

	}


	public void MatchGame(){
		instructions [1].text = "Press Enter when you are ready to start!";
	}

	public void WaitForPartner(){
		instructions[1].text = "Wait for your friend to start...";
		instructions [0].DOFade (0, 0.5f);
	}


	public void JoinNewGame(){
		mainCam.transform.DOMoveZ (0, 2).OnComplete (() => {
			LobbyManager.GetComponentInChildren<Prototype.NetworkLobby.LobbyMainMenu>().OnClickBeClient();
		});
		GameObject.Find("DirectPlaySubPanel/StartPage").gameObject.SetActive(false);
		Scene2.SetActive (true);
	}

	public void TurnToHelpPage(){
		Scene1.GetComponent<Animator> ().SetTrigger ("HelpPage");
	}

	public void BackToStartPate(){
		Scene1.GetComponent<Animator> ().SetTrigger ("BackToStart");
	}


		
}
