using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D coll)
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if (coll.gameObject.tag == "Obstacle")
        {
            if (!audioManager.GetSound("HitGround").source.isPlaying)
                audioManager.Play("HitGround");
            // play hit groud music
        }
        else if (coll.gameObject.tag == "Water")
        {
            if (!audioManager.GetSound("HitWater").source.isPlaying)
                audioManager.Play("HitWater");
            // play water music
        }
    }
}
