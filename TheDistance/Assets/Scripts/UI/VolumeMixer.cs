using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using DG.Tweening;
using UnityEngine.EventSystems;

public class VolumeMixer : MonoBehaviour {

	public AudioMixerGroup mixerGroup;

    public GameObject slider;
    public Sprite SFXIcon;
    public Sprite SFXIcon_Mute;
    public Image m_button;

    bool sliderHidden = true;
    float showTime = 5.0f;

    private void Start()
    {
        HideSlider();
    }
    public void ToggleSlider()
    {
        if (sliderHidden)
            ShowSlider();
        else
            HideSlider();
    }
    
    public void HideSlider()
    {
        slider.SetActive(false);
        sliderHidden = true;
    }

    public void ShowSlider()
    {
        slider.SetActive(true);
        sliderHidden = false;
        nextHideTime = Time.time + showTime;
    }

    public void SetMixerGroupVolume(float f)
    {
        if (isMute) isMute = false;
        float prev;
        mixerGroup.audioMixer.GetFloat("masterVolume", out prev);
        mixerGroup.audioMixer.SetFloat("masterVolume", f);
        nextHideTime = Time.time + showTime;
		if(f == -16)
        {
            m_button.sprite = SFXIcon_Mute;
        }
        else
        {
            m_button.sprite = SFXIcon;
        }
    }

    float nextHideTime = -1;
    private void Update()
    {
        float x_percent = Input.mousePosition.x / Screen.width;
        float y_percent  = Input.mousePosition.y / Screen.height;
        print(x_percent + ", " + y_percent);
        if (!sliderHidden)
        {
            if(Time.time >= nextHideTime)
            {
                HideSlider();
            }
			if (Input.GetMouseButtonDown (0) ) {

                if (x_percent <= 0.92f || y_percent <= 0.88f)
                {
                    HideSlider();
                }
                /*
				if (EventSystem.current.currentSelectedGameObject == null) {
					HideSlider ();
				} else if (EventSystem.current.currentSelectedGameObject.name != "MixerGroup") {
					HideSlider ();
				}
                 */ 
			}
        }

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
            mixerGroup.audioMixer.SetFloat("masterVolume", -16);
        }
        else
        {
            mixerGroup.audioMixer.SetFloat("masterVolume", previousVolume);
        }
    }
}
