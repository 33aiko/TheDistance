using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlatformEffectController : MonoBehaviour {

	public void Start() {
		PlayShowUpEffect();
	}

	public void PlayShowUpEffect() {
		transform.DOScale(0,1f).From();
	}
}
