using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class EnvSound : Sound{

	public GameObject objLoc;
	private float dto;

	public void Update(){
		dto = Vector3.Distance(objLoc.transform.position, GameObject.Find("Player").transform.position);
		//this algorithm probably needs to be adjusted
		//LogWarning(dto);
		Debug.Log(source.name + ": dto is " + dto);
		if (dto < 10) {
			source.volume = 1;
			Debug.Log ("object within range, at full volume");
		} else {
			source.volume = ( 10 / ( (dto * dto) + 10));
		}
	}

}
