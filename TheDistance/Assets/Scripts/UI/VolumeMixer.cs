using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeMixer : MonoBehaviour {

	public AudioMixerGroup mixerGroup;

    public void SetMixerGroupVolume(float f)
    {
        if (isMute) isMute = false;
        mixerGroup.audioMixer.SetFloat("masterVolume", f);
    }

    public void SetAtomVolume(float f)
    {
        mixerGroup.audioMixer.SetFloat("atmoVolume", f);
    }

    public void SetMusicVolume(float f)
    {
        mixerGroup.audioMixer.SetFloat("musicVolume", f);
    }

    public void SetSFXVolume(float f)
    {
        mixerGroup.audioMixer.SetFloat("SFXVolume", f);
    }

    bool isMute = false;
    float previousVolume = 0;
    public void ToggleMute()
    {
        isMute = !isMute;
        if(isMute)
        {
            mixerGroup.audioMixer.GetFloat("masterVolume", out previousVolume);
            mixerGroup.audioMixer.SetFloat("masterVolume", -80);
        }
        else
        {
            mixerGroup.audioMixer.SetFloat("masterVolume", previousVolume);
        }
    }
}
