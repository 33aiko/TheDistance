using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Controller2D))]
public class BoxController : MonoBehaviour {

    public bool shareToFloatingBox = false;
    public bool haveGravity = true;
    float gravity;
    Vector3 velocity;

    bool musicPlayed = false;

    public Controller2D controller;
    string UIPath = "Sprites/UI/controls/input hint UI";
    string ps4UIName = "inputUI_square";
    string keyboardUIName = "Sprites/UI/controls/inputUI_keyQ";

    bool currentIsKeyboard = true;

    Image inputUI;

    void Start () {
        controller = GetComponent<Controller2D>();
        gravity = 300.0f / 0.3f / 0.3f;
        velocity.y = 0;
        haveGravity = true;
        inputUI = GetComponentInChildren<Image>();
		if(inputUI != null&&this.CompareTag("BoxCannotShare"))
            inputUI.sprite = Resources.Load<Sprite>(keyboardUIName);
    }

    void Update () {
        if (controller.collisions.above || controller.collisions.below)
        {
            //print("box on the ground");
            velocity.y = 0;
        }
        if(haveGravity)
            velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        GameObject pGO = GameObject.Find("Player");
        if(pGO != null)
        {
			if (this.CompareTag ("BoxCannotShare")) {
				inputUI.gameObject.SetActive (true);
				Player p = pGO.GetComponent<Player> ();
				bool isKeyboard = (p.currentInputDevice == Player.InputDeviceType.KEYBOARD);
				if (isKeyboard != currentIsKeyboard) {
					if (isKeyboard) {
						if (inputUI != null)
							inputUI.sprite = Resources.Load<Sprite> (keyboardUIName);
					} else {
						Sprite[] sprites;
						sprites = Resources.LoadAll<Sprite> (UIPath);
						if (inputUI != null)
							inputUI.sprite = sprites.Where (tmp => tmp.name == ps4UIName).First ();
					}

					currentIsKeyboard = isKeyboard;
				}
			} else {
				inputUI.gameObject.SetActive (false);
			}
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (musicPlayed) return;
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if(coll.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            if(!audioManager.GetSound("HitGround").source.isPlaying)
                audioManager.Play("HitGround");
            musicPlayed = true;
            // play hit groud music
        }
        else if(coll.gameObject.tag == "Water")
        {
            if(!audioManager.GetSound("HitWater").source.isPlaying)
                audioManager.Play("HitWater");
            musicPlayed = true;
            // play water music
        }
    }

}
