using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DeathBlack : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //this.transform.GetComponent<Image>().DOFade(0, 0);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void FadeInAndOut(float t)
    {
        this.transform.GetComponent<Image>().DOFade(1, t);
        //GameObject.Find("UI/Canvas/durability").GetComponent<BoatDurability>().Initializations();
        this.transform.GetComponent<Image>().DOFade(0, t).SetDelay(t);
    }


    public void FadeIn(float t)
    {
        Debug.Log("fade in");
        //this.gameObject.SetActive(true);
        this.transform.GetComponent<Image>().DOFade(1, t);
    }

    public void FadeOut(float t)
    {
        Debug.Log("fade out");
        this.transform.GetComponent<Image>().DOFade(0, t);
        //this.gameObject.SetActive(false);
    }

}
