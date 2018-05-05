using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Networking;

public class BoatDurability : NetworkBehaviour {

    
    public float maxLifeNum = 4;
    //public float speed = 0.05f;
    public Image hp;

    [SyncVar(hook = "On_target_life_num")]
    public float targetLifeNum;

    private float curLifeNum;
    private float curLife;

    // starting value for the Lerp
    static float t = 0.0f;

    private bool isAnimating;

    // Use this for initialization
    void Start () {
       Initializations();
    }

    public void Initializations()
    {
        targetLifeNum = maxLifeNum;
        curLifeNum = maxLifeNum;
        curLife = 1;
        hp = this.transform.Find("circle").GetComponent<Image>();
        hp.fillAmount = 1;
      
        this.transform.Find("circle").GetComponent<Image>().color = Color.white;
        
    }

    // Update is called once per frame
    void Update() {

        //Debug.Log("curLife" + curLife);
        //Debug.Log("targetLifeNum/maxLifeNum" + targetLifeNum / maxLifeNum);

        hp.fillAmount = Mathf.Lerp(curLife, targetLifeNum / maxLifeNum, t);
        // .. and increate the t interpolater
        t += 1.0f * Time.deltaTime;

        if (t > 1.0f)
        {
            if (targetLifeNum == 1)
            {
                hp.color = Color.red;
            }

            transform.DOScale(new Vector3(1, 1, 1), 0.5f);
            curLife = hp.fillAmount;

            if(curLife == 0)
            {
                GameObject.Find("boat").GetComponent<RowBoat>().BoatDeath();
            }
        }

       
        //if (isAnimating)
        //{
            
        //}
     
    }

    public void LifeDecreaseByOne()
    {
        //Debug.Log("LifeDecreaseByOne()");
        targetLifeNum--;
        t = 0.0f;
        //isAnimating = true;
        transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.5f);
        
    }

    void On_target_life_num(float newValue)
    {
        Debug.Log("On_target_life_num");
        t = 0.0f;
        transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.5f);
        //变量被访问之后会执行此方法，newValue为变量被赋的值，类型要与SyncVar的变量类型一致。
    }

}
