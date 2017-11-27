using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SharingEffectsController : MonoBehaviour {

	[SerializeField] ParticleSystem[] defaultEffect;
	[SerializeField] ParticleSystem[] selectedEffect;
	[SerializeField] ParticleSystem[] sharedEffect;
	[SerializeField] WindZone wind;
	[SerializeField][Range(0,1000f)] float windIntensity = 200f;
	[SerializeField] bool isTest = false;

	public enum State
	{
		Default,
		Selected,
		Shared,
		Stop,
	}
	public State state;

	public void PlayParticles( ParticleSystem[] ps )
	{
		if (ps != null)
			foreach (var p in ps) {
				if (!p.isPlaying)
					p.Play ();
			}
	}

	public void StopParticles( ParticleSystem[] ps )
	{
		if (ps != null)
			foreach (var p in ps) {
				if (p.isPlaying && p != null)
					p.Stop ();
			}
	}

	void Awake () {

		if (selectedEffect != null) {
			StopParticles (selectedEffect);
		}
		if (sharedEffect != null) {
			StopParticles (sharedEffect);
		}
//		if (sharedEffect != null) {
//			sharedEffect.Stop ();
//		}
		if (wind != null)
			wind.windMain = 0;
		
		state = State.Default;
	}

	void Update(){
		if (state == State.Default) {
			if (this.CompareTag ("FloatingPlatform") || this.CompareTag ("Box")) {
				PlayParticles( defaultEffect);
			}
		}

		if (isTest) {
			if (Input.GetKeyDown (KeyCode.I) && Input.GetKey (KeyCode.LeftControl)) {
				PlaySelectedEffect ();
			}

			if (Input.GetKeyUp (KeyCode.I) && Input.GetKey (KeyCode.LeftControl)) {
				StopSelectedEffect ();
			}
		}
	}
		
	public void StopAll(){
		Debug.Log ("Stop All " + name);
		StopParticles (defaultEffect);
		StopParticles (selectedEffect);
		StopParticles (sharedEffect);
		state = State.Stop;
	}

	public void PlaySelectedEffect(){

		Debug.Log ("Play Selected " + name);
		StopParticles (defaultEffect);
		PlayParticles (selectedEffect);

		if ( wind != null )
			wind.windMain = windIntensity;
		state = State.Selected;
	}

	public void StopSelectedEffect(){
		Debug.Log ("Stop Selected " + name);

		StopParticles (defaultEffect);
		StopParticles (selectedEffect);
		PlayParticles (sharedEffect);

		wind.windMain = - windIntensity;
		DOTween.To (() => wind.windMain, (x) => wind.windMain = x, 0, 1f).SetDelay (1f);
		state = State.Shared;

	}

//	public void PlaySharedEffect(){
//		if (sharedEffect != null && !sharedEffect.isPlaying) {
//			sharedEffect.Play ();
//		}
//
//		if (defaultEffect != null) {
//			defaultEffect.Stop ();
//		}
//		if (selectedEffect != null) {
//			foreach (var e in selectedEffect) {
//				e.Stop ();
//			}
//		}
//		wind.windMain = - windIntensity;
}
