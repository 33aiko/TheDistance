using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharingEffectsController : MonoBehaviour {

	[SerializeField] ParticleSystem[] defaultEffect;
	[SerializeField] ParticleSystem[] selectedEffect;
	[SerializeField] ParticleSystem sharedEffect;
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
				if ( p.isPlaying)
				p.Stop ();
	}

	void Start () {
		StopParticles (selectedEffect);

		if (sharedEffect != null) {
			sharedEffect.Stop ();
		}
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

	}
		
	public void StopAll(){
		StopParticles (defaultEffect);
		StopParticles (selectedEffect);
		sharedEffect.Stop ();
		state = State.Stop;
	}

	public void PlaySelectedEffect(){

		StopParticles (defaultEffect);
		PlayParticles (selectedEffect);

		if ( wind != null )
			wind.windMain = windIntensity;
		state = State.Selected;
	}

	public void StopSelectedEffect(){
		StopParticles (defaultEffect);
		StopParticles (selectedEffect);

		if (sharedEffect != null) {
			sharedEffect.Play ();
		}

		wind.windMain = - windIntensity;
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
