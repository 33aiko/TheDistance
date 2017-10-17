using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;

	public AudioMixerGroup mixerGroup;

	public Sound[] sounds;

    //	TODO: implement
    //	public float masterVolume;
    //	public float effectVolume;
    //	public float musicVolume;
    //	public float ambientVolume;

    private void Start()
    {
        print("Playing music!");
        Play("MusicTrack01WithAtmo");
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
	}

	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
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

}
