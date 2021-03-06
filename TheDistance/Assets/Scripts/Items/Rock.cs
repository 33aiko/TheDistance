﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D coll)
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if(coll.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            if (!audioManager.GetSound("HitGround").source.isPlaying)
                audioManager.Play("HitGround");
            // play hit groud music
        }
        else if (coll.gameObject.tag == "Water")
        {
			if (!audioManager.GetSound ("HitWater").source.isPlaying) {
				audioManager.Play ("HitWater");
				Debug.Log (this.gameObject.name);
			}
            // play water music
        }
    }
}
