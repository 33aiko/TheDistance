using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class EnterSceneInstruction : MonoBehaviour {

	public Image ui_borderUp;
	public Image ui_borderDown;
	public Image ui_background;
	public Text ui_text;

	public float fade_time = 0.5f;
	public float expand_time = 0.5f;

	float upY;
	float downY;

	bool isActivated = false; 

	GameObject durabilityUI;

	void Start(){
		upY = ui_borderUp.transform.localPosition.y;
		downY = ui_borderDown.transform.localPosition.y;
	}

	public void Initialize(){
		Camera.main.GetComponent<DOVModify>().SetActive(true);
		Camera.main.GetComponent<DOVModify>().SetFocalLength(100);

		durabilityUI = GameObject.Find ("UI/Canvas/durability");
		durabilityUI.GetComponent<BoatDurability> ().HideUI ();

		ui_borderUp.transform.localPosition = Vector3.zero;
		ui_borderDown.transform.localPosition = Vector3.zero;
		ui_borderUp.DOFade(0, 0);
		ui_borderDown.DOFade(0, 0);
		ui_background.DOFade(0, 0);
		ui_background.transform.DOScaleY(0f, 0);
		ui_text.DOFade (0, 0);

		Invoke ("ShowUI", 1f);
	}

	void ShowUI(){
		isActivated = true; 
		ui_borderUp.DOFade(1, fade_time);
		ui_borderDown.DOFade(1, fade_time);
		ui_borderUp.transform.DOLocalMoveY(upY, expand_time);
		ui_borderDown.transform.DOLocalMoveY(downY, expand_time);

		ui_background.DOFade(1, fade_time);
		ui_background.transform.DOScaleY(1, fade_time);

		ui_text.DOFade(1, fade_time).SetDelay (expand_time);
	}

	void Update () {
		if (Input.GetButtonDown("Row") && isActivated) {
			ui_background.DOFade(0, fade_time);
			ui_background.transform.DOScaleY(0f, fade_time);
			ui_text.DOFade (0, fade_time);
			ui_borderUp.transform.DOLocalMoveY(0, expand_time);
			ui_borderDown.transform.DOLocalMoveY(0, expand_time);
			ui_borderUp.DOFade(0, fade_time);
			ui_borderDown.DOFade (0, fade_time);

			Camera.main.GetComponent<DOVModify>().SetActive(false);

			durabilityUI.GetComponent<BoatDurability> ().ShowUI ();

		}
	}
}
