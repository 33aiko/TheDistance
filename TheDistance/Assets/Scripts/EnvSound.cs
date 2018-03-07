using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class EnvSound : Sound{

	public GameObject objLoc;
	private float dto;

	public void Update(){
		dto = Vector3.Distance(objLoc.transform.position, GameObject.Find("Player").transform.position);
		//source.volume = ( 1 / ( (dto * dto) + 1));

		if (dto < 1000) {
			source.volume = 1;
		} else {
			source.volume = ( 1000 / ( (dto * dto) + 1000));
		}
	}

}
