using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Controller2D))]
public class BoxController : MonoBehaviour {

    float gravity;
    public bool haveGravity = true;
    Vector3 velocity;


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
        if(inputUI != null)
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
            Player p = pGO.GetComponent<Player>();
            bool isKeyboard = (p.currentInputDevice == Player.InputDeviceType.KEYBOARD);
            if (isKeyboard != currentIsKeyboard)
            {
                print("rua!");
                if (isKeyboard)
                {
                    if (inputUI != null)
                        inputUI.sprite = Resources.Load<Sprite>(keyboardUIName);
                }
                else
                {
                    Sprite[] sprites;
                    sprites = Resources.LoadAll<Sprite>(UIPath);
                    if (inputUI != null)
                        inputUI.sprite = sprites.Where(tmp => tmp.name == ps4UIName).First();
                }

                currentIsKeyboard = isKeyboard;
            }
        }
    }

}
