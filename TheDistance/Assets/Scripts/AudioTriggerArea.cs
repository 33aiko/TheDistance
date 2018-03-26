using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTriggerArea : MonoBehaviour {

	public string audioName;
	public AudioManager audioManager;

	int cnt = 0;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player") {
			print ("music!");
			cnt++;
			if (cnt == 2) {
				audioManager.Play (audioName);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision){
		if (collision.gameObject.tag == "Player") {
			cnt--;
			if (cnt != 2) {
				audioManager.Stop (audioName);
			}
		}
	}
}
