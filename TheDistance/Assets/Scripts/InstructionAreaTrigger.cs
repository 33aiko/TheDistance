using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InstructionAreaTrigger : MonoBehaviour {

    int cnt;

    public Image ui_borderUp;
    public Image ui_borderDown;
    public Image ui_background;
    public Text ui_text;

    public float expand_time = 0.5f;
    public float fade_time = 0.5f;

    public bool isUsed = false;

    Sequence seq;

    private void Start()
    {
        cnt = 0;

        ui_borderUp.transform.localPosition = Vector3.zero;
        ui_borderDown.transform.localPosition = Vector3.zero;
        ui_borderUp.DOFade(0, 0);
        ui_borderDown.DOFade(0, 0);
        ui_background.DOFade(0, 0);
        ui_background.transform.DOScaleY(0.1f, 0);
        ui_text.canvasRenderer.SetAlpha(0);

        seq = DOTween.Sequence();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            cnt++;
            if(cnt == 2)
            {
                Player p = collision.GetComponent<Player>();
                p.cur_instruction = this;
                ShowUI();
            }
        }
    }

    public void ShowUI()
    {
        if (isUsed) return;
        ui_borderUp.DOFade(1, fade_time);
        ui_borderDown.DOFade(1, fade_time);
        ui_borderUp.transform.DOLocalMoveY(50, expand_time);
        ui_borderDown.transform.DOLocalMoveY(-50, expand_time);

        ui_background.DOFade(1, fade_time);
        ui_background.transform.DOScaleY(1, fade_time);
        float vic = 0;
        DOTween.To(() => vic, x => { ui_text.canvasRenderer.SetAlpha(vic); vic = x; }, 1, fade_time);
    }

    public void HideUI()
    {
        if (isUsed) return;
        ui_background.DOFade(0, fade_time);
        ui_background.transform.DOScaleY(0.1f, fade_time);
        float vic = 0;
        DOTween.To(() => vic, x => { ui_text.canvasRenderer.SetAlpha(vic); vic = x; }, 0, fade_time);
        ui_borderUp.transform.DOLocalMoveY(0, expand_time);
        ui_borderDown.transform.DOLocalMoveY(0, expand_time);
        ui_borderUp.DOFade(0, fade_time);
        ui_borderDown.DOFade(0, fade_time);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            cnt--;
            if(cnt != 0)
            {
                Player p = collision.GetComponent<Player>();
                p.cur_instruction = null;
                HideUI();
            }
        }
    }

}
