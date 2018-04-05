using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Flower : MonoBehaviour {
    Camera mainCamera;
    //int cnt = 0;
    //int zoomFlag = 0;
    public FlowerBox fb;
    Text txt;
    bool hasTriggered = false;

    public int Idx;

	Animator flowerAnim; 

    // Use this for initialization
    void Start () {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
		txt = GameObject.Find("FlowerBox"+Idx.ToString()+"/Canvas/Text").GetComponent<Text>();
		txt.DOFade (0, 0);
		flowerAnim = GetComponentInChildren<Animator> ();

    }
	


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasTriggered) return;
        if (collision.gameObject.tag == "Boat")
        {
            mainCamera.GetComponent<CamFollow>().FocusObject(fb.gameObject);

			if (flowerAnim != null) {
				flowerAnim.SetTrigger ("getFlower");
			}
           // txt.DOFade(1, 1);
			FadeCharByChar(txt);
            hasTriggered = true;
        }
		GetComponent<AudioSource> ().Play ();

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Boat")
        {
            RowBoat boat = collision.GetComponent<RowBoat>();
            Debug.Log("out flower");
        }
    }

	public float fadeCharTime = 0.1f;
	bool fadeFinished = false;

	void FadeCharByChar(Text orig)
	{
		fadeFinished = false;
		string orig_str = orig.text;
		orig.text = "";
		int l = orig_str.Length;
		string shown = "";

		Sequence seq = DOTween.Sequence();
		for (int i = 0; i < l; i++)
		{
			int vic = 30;
			char tmp = orig_str[i];
			seq.Append(
				DOTween.To(() => vic, x =>
					{
						vic = x;
						orig.text = shown + "<color=#ffffff" + vic.ToString("X2") + ">" + tmp + "</color>";
					}, 255, fadeCharTime).
				OnComplete(() =>
					{
						shown += (tmp);
					}).SetEase(Ease.Linear)
			);
		}
		seq.OnComplete(() =>
			{
				print("poem fade in char by char finished");
			});
	}


}
