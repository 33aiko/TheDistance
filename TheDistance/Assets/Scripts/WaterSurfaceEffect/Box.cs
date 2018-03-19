using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {

    public float g = 9.8f * 0.03f;
    float mass = 1.0f;
    float buoyancy = 0;
    public float vy = 0;
    public Vector3 move;

    Water water;

    public bool playerOnTop = false;
    public Player p;

    private void Start()
    {
        water = FindObjectOfType<Water>();
    }

    public Vector3 leftButtom; public Vector3 leftUp;
    public Vector3 rightButtom; public Vector3 rightUp;

    void UpdateBound()
    {
        BoxCollider2D bc = GetComponent<BoxCollider2D>();
        float sx = bc.size.x * transform.localScale.x;
        float sy = bc.size.y * transform.localScale.y;
        leftButtom = transform.position + new Vector3(-sx, -sy) / 2.0f;
        rightButtom = transform.position + new Vector3(sx, -sy) / 2.0f;
        leftUp = transform.position + new Vector3(-sx, +sy) / 2.0f;
        rightUp = transform.position+ new Vector3(sx, +sy) / 2.0f;

        Debug.DrawRay(leftUp, Vector3.up * 100, Color.red);
        Debug.DrawRay(rightUp, Vector3.up * 100, Color.red);
        Debug.DrawRay(leftButtom, Vector3.down * 100, Color.red);
        Debug.DrawRay(rightButtom, Vector3.down * 100, Color.red);

    }

    void Update () {
        UpdateBound();
        float Fp = 0;
        if (playerOnTop && p.velocity.y < -200.0f)
        {
            print("adding splash");
            AddSplash();
            Fp = -g * mass * 20;
        }
        if(playerOnTop)
        {
            Fp = -g * mass * 1f;
        }

        float percent = water.Intersect(this);
        float Fg = -g * mass;
        float Fb = 3 * percent * mass * g;
        vy += (Fb + Fg + Fp) * Time.deltaTime;

        if (percent > 0)
            vy *= 0.985f;

        move = Vector3.up * vy * Time.deltaTime;
        if (playerOnTop)
        {
            movePlayer();
            playerOnTop = false;
        }

        transform.Translate(move);
	}

    void movePlayer()
    {
        p.controller.Move(move);
        p.controller.collisions.below = true;
    }

    public void AddSplash()
    {
        water.AddSplash(leftButtom.x, rightButtom.x);
    }
}
