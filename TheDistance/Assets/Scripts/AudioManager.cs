using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;

	//I think these can go.
	public AudioMixerGroup mixerGroup;
	public AudioMixerGroup atmoMixerGroup;
	public AudioMixerGroup envMixerGroup;
	public AudioMixerGroup musicMixerGroup;
	//END

	public Sound[] sounds;
	public Sound[] atmo;
	public EnvSound[] env;
	public Sound[] music;


	bool musicScene1played = false; 

	private void Start()
	{
		if (SceneManager.GetActiveScene ().name == "StartScreen") {
			PlayMusicTrack ("startscreenBG");
		}

	}

	//On game load
	void Awake()
	{

		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = mixerGroup;
		}

		foreach (Sound a in atmo) {
			a.source = gameObject.AddComponent<AudioSource>();
			a.source.clip = a.clip;
			a.source.loop = a.loop;

			a.source.outputAudioMixerGroup = atmoMixerGroup;
		}

		foreach (EnvSound e in env) {
			e.source = gameObject.AddComponent<AudioSource>();
			e.source.clip = e.clip;
			e.source.loop = e.loop;

			e.source.outputAudioMixerGroup = envMixerGroup;
		}

		foreach (Sound m in music) {
			m.source = gameObject.AddComponent<AudioSource> ();
			m.source.clip = m.clip;
			m.source.loop = m.loop;

			m.source.outputAudioMixerGroup = musicMixerGroup;

		}

		//musicIndex = 0;
	}

	void Update(){
		if (SceneManager.GetActiveScene ().name == "LX_scene1" && !musicScene1played) {
			PlayAtmo ("atmosphere01");
		}
	}


	//
	public Sound GetSound(string sound){
		Sound s = Array.Find (sounds, item => item.name == sound);
		if (s != null)
			return s;
		else
			return null;
	}

	//generic sound accessor method
	//sound triggers are handled elsewhere
	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found in Sound (bad warning)!");
		}
		else if (!s.source.isPlaying) {
			s.source.volume = s.volume * (1f + UnityEngine.Random.Range (-s.volumeVariance / 2f, s.volumeVariance / 2f));
			s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range (-s.pitchVariance / 2f, s.pitchVariance / 2f));

			//s.source.volume = 1f;
			//s.source.pitch = 1f;

			s.source.Play ();
		}
		EnvSound e = Array.Find(env, item => item.name == sound);
		if (e == null){
			Debug.LogWarning("Sound: " + sound + " not found in ENV (bad warning)!");
		}
		else if (!s.source.isPlaying) {
			e.source.Play ();
		}

	}

	public void StopAndPlay(string sound){
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null) {
			Debug.LogWarning ("Sound: " + name + " not found in Sound (bad warning)!");
		} else {
			if (s.source.isPlaying)
				s.source.Stop ();
			s.source.Play ();
		}
	}

	//Stop sound.
	public void Stop(string sound)
	{
		Sound s = Array.Find (sounds, item => item.name == sound);
		if (s == null) {
			Debug.LogWarning ("Sound: " + name + " not found!");
		} else {
			s.source.Stop ();
		}

		EnvSound e = Array.Find(env, item => item.name == sound);
		if (e == null) {
			Debug.LogWarning ("Sound: " + sound + " not found in ENV (bad warning)!");
		} else {
			e.source.Play ();
		}
	}



	//Atmos

	//atmo sound accessor method
	public void PlayAtmo( string atmoName ){
		Sound a = Array.Find(atmo, item => item.name == atmoName);
		if (a == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		if (!a.source.isPlaying) {
			//a.source.volume = a.volume * (1f + UnityEngine.Random.Range (-a.volumeVariance / 2f, a.volumeVariance / 2f));;
			a.source.pitch = a.pitch * (1f + UnityEngine.Random.Range (-a.pitchVariance / 2f, a.pitchVariance / 2f));
			a.source.volume = 0f;
			a.source.Play ();
			
			//Code to Fade in on play
			while(a.source.volume <= 1f){
				a.source.volume += Time.deltaTime / 1f;
			}
			
			
		}
	}

	public void StopAtmo( string atmoName ){
		Sound a = Array.Find(atmo, item => item.name == atmoName);
		if (a == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		
		while(a.source.volume > 0){
			a.source.volume -= Time.deltaTime / 1f;
		
		}
		a.source.Stop ();
	}

// 	//Using for Env Sounds
// 	public void Update(){
// 		foreach(EnvSound e in env){
// 			e.source.volume = ( 1 / ( (e.dto * e.dto) + 1));
// 		}
//
// //		Sound e = Array.Find (env, item => item.name == env);
// //		if (e != null)
// //		{
// //			e.source.volume = ( 1 / ( (distanceToObject * distanceToObject) + 1));
// //		}
// //		else
// //		{
// //
// //		}
// 	}

	//Music

	//music
	public void PlayMusicTrack( string trackName ){
		print ("play next music track");

		Sound m = Array.Find(music, item => item.name == trackName);
		if (m == null)
		{
			Debug.LogWarning("Sound: " + trackName + " not found!");
			return;
		}
		if (!m.source.isPlaying) {

			m.source.volume = m.volume * (1f + UnityEngine.Random.Range (-m.volumeVariance / 2f, m.volumeVariance / 2f));;
			m.source.pitch = m.pitch * (1f + UnityEngine.Random.Range (-m.pitchVariance / 2f, m.pitchVariance / 2f));

			m.source.Play ();

			//TODO: duck any tracks playing now
		}

	}

	public void StopMusicTrack( string trackName ){


		Sound m = Array.Find(music, item => item.name == trackName);
		if (m == null)
		{
			Debug.LogWarning("Sound: " + trackName+ " not found!");
			return;
		}
		m.source.Stop ();
	}

	public void StopAllMusic(){
		foreach (var m in music) {
			if (m.source.isPlaying) {
				m.source.Stop ();
			}
		}
	}

}


