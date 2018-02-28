using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CaveEffectController : MonoBehaviour {

    public Material caveMaterial;
    Renderer caveRender;
    Tween spiritTween;
    public Vector4[] checkpointPosList = {
        new Vector4(0,0,0,-1),
        new Vector4(0,0,0,-1),
        new Vector4(0,0,0,-1)
    };
    int idx_cp = 0;

    public Vector4[] fragmentPosList =  {
        new Vector4(0,0,0,-1),
        new Vector4(0,0,0,-1),
        new Vector4(0,0,0,-1)
    };
    int idx_fg = 0;



    void Start() {
        caveRender = GetComponent<Renderer>();
        caveMaterial = caveRender.material;
        caveRender.sortingLayerName = "Foreground";
        caveMaterial.SetVectorArray("_CheckpointPos", checkpointPosList);
    }

    public void SetShaderPosition(string n, Vector3 pos)
    {
        caveMaterial.SetVector(n, pos);
    }

    public void AddCheckpointLight(Vector3 pos)
    {
        if (idx_cp < checkpointPosList.Length - 1) 
            checkpointPosList[idx_cp++] = pos;
        caveMaterial.SetVectorArray("_CheckpointPos", checkpointPosList);
    }

    public void AddFragmentLight(Vector3 pos)
    {
        if (idx_fg < checkpointPosList.Length - 1)
            fragmentPosList[idx_fg++] = pos;
        caveMaterial.SetVectorArray("_FragmentPos", fragmentPosList);
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
