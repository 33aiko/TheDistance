using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharingEffectsController : MonoBehaviour {

	[SerializeField] ParticleSystem defaultEffect;
	[SerializeField] ParticleSystem selectedEffect;
	[SerializeField] ParticleSystem sharedEffect;


	void Start () {
		if (defaultEffect != null) {
			defaultEffect.Stop ();
		}
		if (selectedEffect != null) {
			selectedEffect.Stop ();
		}
		if (sharedEffect != null) {
			sharedEffect.Stop ();
		}
	}

	void Update(){
		if (this.CompareTag ("FloatingPlatform") || this.CompareTag("Box")) {
			if (defaultEffect != null && !defaultEffect.isPlaying) {
				defaultEffect.Play ();
			}
		}
	}
		

	public void PlaySelectedEffect(){
		if (selectedEffect != null && !selectedEffect.isPlaying) {
			selectedEffect.Play ();
		}
	}

	public void StopSelectedEffect(){
		if (selectedEffect != null && selectedEffect.isPlaying) {
			selectedEffect.Stop ();
		}
	}

	public void PlaySharedEffect(){
		if (sharedEffect != null && !sharedEffect.isPlaying) {
			sharedEffect.Play ();
		}
	}
}
