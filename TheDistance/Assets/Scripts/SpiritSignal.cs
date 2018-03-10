using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpiritSignal : MonoBehaviour {

    [SerializeField]
    SpriteRenderer checkpoint;
    [SerializeField]
    SpriteRenderer glowcircle_1;
    [SerializeField]
    SpriteRenderer glowcircle_2;

    private void OnEnable()
    {
        checkpoint.transform.localScale = Vector3.zero;
        checkpoint.DOFade(0.5f, 0);
        glowcircle_1.transform.localScale = Vector3.zero;
        glowcircle_2.transform.localScale = Vector3.zero;

        {
            Sequence s = DOTween.Sequence();
            checkpoint.transform.DOScale(Vector3.one * 150, 1.2f);
            s.PrependInterval(0.5f);
            s.Append(checkpoint.DOFade(0, 1.0f).SetEase(Ease.OutCubic));
        }

        {
            Sequence s = DOTween.Sequence();
            glowcircle_1.transform.DOScale(Vector3.one * 120, 1.2f);
            glowcircle_1.DOFade(0, 0.8f).SetEase(Ease.InCubic);
        }

        {
            Sequence s = DOTween.Sequence();
            s.PrependInterval(0.5f);
            s.Append(glowcircle_2.transform.DOScale(Vector3.one * 80, 1.2f));
            glowcircle_2.DOFade(0, 1.8f).SetEase(Ease.InCubic);
        }

    }
}
