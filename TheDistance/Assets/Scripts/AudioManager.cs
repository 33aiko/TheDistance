using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;

	public AudioMixerGroup mixerGroup;
	public AudioMixerGroup musicMixerGroup;
	public AudioMixerGroup atmoMixerGroup;

	public Sound[] sounds;
	public Sound[] atmo;
	public Sound[] music;
	//int musicIndex;

	private void Start()
	{
		//        print("Playing music!");
		//        Play("MusicTrack01WithAtmo");

		print ("playing atmo to start");
		PlayAtmo ( "atmosphere01");
	}

	void Awake()
	{
		//		masterVolume = 1f;
		//		effectVolume = 1f;
		//		musicVolume = 1f;
		//		ambientVolume = 1f;

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

		foreach (Sound m in music) {
			m.source = gameObject.AddComponent<AudioSource> ();
			m.source.clip = m.clip;
			m.source.loop = m.loop;

			m.source.outputAudioMixerGroup = musicMixerGroup;

		}

		//musicIndex = 0;
	}

	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		if (!s.source.isPlaying) {
			s.source.volume = s.volume * (1f + UnityEngine.Random.Range (-s.volumeVariance / 2f, s.volumeVariance / 2f));
			s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range (-s.pitchVariance / 2f, s.pitchVariance / 2f));

			//s.source.volume = 1f;
			//s.source.pitch = 1f;

			s.source.Play ();
		}
	}

	//TEMP Method
	public void PlayAtmo( string atmoName ){
		Sound a = Array.Find(atmo, item => item.name == atmoName);
		if (a == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		if (!a.source.isPlaying) {
			a.source.volume = a.volume * (1f + UnityEngine.Random.Range (-a.volumeVariance / 2f, a.volumeVariance / 2f));;
			a.source.pitch = a.pitch * (1f + UnityEngine.Random.Range (-a.pitchVariance / 2f, a.pitchVariance / 2f));

			a.source.Play ();
		}
	}

	public void StopAtmo( string atmoName ){
		Sound a = Array.Find(atmo, item => item.name == atmoName);
		if (a == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		a.source.Stop ();
	}

	public Sound GetSound(string sound){
		Sound s = Array.Find (sounds, item => item.name == sound);
		if (s != null)
			return s;
		else
			return null;
	}

	public void Stop(string sound)
	{
		Sound s = Array.Find (sounds, item => item.name == sound);
		if (s == null) {
			Debug.LogWarning ("Sound: " + name + " not found!");
			return;
		}

		s.source.Stop();
	}

	//music
	public void PlayMusicTrack( string trackName ){
		print ("play next music track");

		Sound m = Array.Find(music, item => item.name == trackName);
		if (m == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
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
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		m.source.Stop ();
	}


}
