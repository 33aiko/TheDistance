using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CaveEffectController : MonoBehaviour {

    public Material caveMaterial;
    Renderer caveRender;
    Tween spiritTween;

    void Start() {
        caveRender = GetComponent<Renderer>();
        caveMaterial = caveRender.material;
        caveRender.sortingLayerName = "Foreground";
    }

    public void SetShaderPosition(string n, Vector3 pos)
    {
        caveMaterial.SetVector(n, pos);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.name == "Spirit")
        {
            Transform pe = coll.transform.Find("spiritIdlePE");
            spiritTween = pe.DOScale(100, 1.0f);
            caveMaterial.SetFloat("_SpiritLightRadius", 300);
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if(coll.name == "Spirit")
        {
            Transform pe = coll.transform.Find("spiritIdlePE");
            spiritTween.Kill();
            spiritTween = pe.DOScale(30, 1.0f);
            caveMaterial.SetFloat("_SpiritLightRadius", 200);
        }
    }

}
