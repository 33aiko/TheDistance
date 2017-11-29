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
			foreach (var p in ps)
				if (!p.isPlaying)
					p.Play ();
	}

	public void StopParticles( ParticleSystem[] ps )
	{
		if (ps != null)
			foreach (var p in ps)
				if ( p != null && p.isPlaying )
				p.Stop ();
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
			if (transform.parent.CompareTag ("FloatingPlatform") || transform.parent.CompareTag ("Box")|| transform.parent.CompareTag("MovingPlatformSharable")) {
				PlayParticles( defaultEffect);
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


}
