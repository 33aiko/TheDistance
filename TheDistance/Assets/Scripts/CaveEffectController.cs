using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveEffectController : MonoBehaviour {

    public Material caveMaterial;
    Renderer caveRender;

	void Start () {
        caveRender = GetComponent<Renderer>();
        caveMaterial = caveRender.material;

        caveRender.sortingLayerName = "Foreground";
	}
}
