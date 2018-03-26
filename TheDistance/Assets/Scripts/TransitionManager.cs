using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour {


	public Color maskColor = Color.black; 
	public float transitionTime = 1; 

	Image maskImage; 


	void Start () {
		maskImage = GetComponent<Image>();
		maskImage.DOFade (0, 0);
	}


	public void FadeInMask(){
		maskImage.color = maskColor;
		maskImage.DOFade (1, transitionTime);
	}

	public void FadeOutMask(){
		maskImage.DOFade (0, transitionTime);
	}

	public void BlackTransition(){
		maskImage.color = Color.black;
		maskImage.DOFade (1, transitionTime);
		maskImage.DOFade (0, transitionTime).SetDelay (0.5f);
	}


}
