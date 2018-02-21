using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class EnvSound : Sound{

	public GameObject objLoc;
	private float dto;

	public void Update(){
		//calc dto
		//source.volume = ( 1 / ( (dto * dto) + 1));
		
	}

}
