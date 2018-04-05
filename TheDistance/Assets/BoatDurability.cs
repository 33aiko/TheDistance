using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoatDurability : MonoBehaviour {


    public float maxLifeNum = 4;
    //public float speed = 0.05f;
    public Image hp;

    private float targetLifeNum;
    private float curLifeNum;
    private float curLife;

    // starting value for the Lerp
    static float t = 0.0f;


    // Use this for initialization
    void Start () {
        targetLifeNum = maxLifeNum;
        curLifeNum = maxLifeNum;
        curLife = 1;
        hp = this.transform.Find("circle").GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
        //if (targetLife != hp.fillAmount)
        //      {
        //          Debug.Log("targetLife" + targetLife);
        //          Debug.Log("hp.fillAmount"+hp.fillAmount);
        //          hp.fillAmount -= speed;
        //      }
        //      else
        //      {
        //          curLifeNum = (int) (targetLife * maxLifeNum);
        //      }
        Debug.Log("curLife"+curLife);
        Debug.Log("targetLifeNum/maxLifeNum" + targetLifeNum / maxLifeNum);

        hp.fillAmount = Mathf.Lerp(curLife, targetLifeNum/maxLifeNum, t);
        // .. and increate the t interpolater
        t += 0.5f * Time.deltaTime;


        // now check if the interpolator has reached 1.0
        // and swap maximum and minimum so game object moves
        // in the opposite direction.
        if (t > 1.0f)
        { 
            curLife = hp.fillAmount;
        }
        //if (hp.fillAmount == targetLifeNum / maxLifeNum)
        //{
            
        //}

        if(targetLifeNum == 1)
        {
            hp.color = Color.red;

        }

     
    }

    public void LifeDecreaseByOne()
    {
        Debug.Log("LifeDecreaseByOne()");
        targetLifeNum--;
        t = 0.0f;
    }


}
