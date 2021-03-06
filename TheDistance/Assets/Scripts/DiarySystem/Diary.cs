﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Diary : MonoBehaviour {
    public GameObject StoryItemBtn;
    public GameObject StoryContentItem;
    public Transform content;
    public Transform storyContent;



    // Use this for initialization
    public void Initiate () {
        int diaryIndex = 0;
		foreach (var item in TextSystem.textDictionary) {
			//add story btn
			GameObject btnObj = Instantiate (StoryItemBtn, content);
			btnObj.transform.Find ("Text").GetComponent<Text> ().text = item.Key;
			btnObj.name = diaryIndex.ToString ();

			diaryIndex++;

			btnObj.SetActive (true);
			btnObj.SetActive (false);


			//construct texts with line break
			string total_s = "";
			for (int i = 0; i < item.Value.Count - 1; i++) {
				total_s += item.Value [i] + '\n';
			}
			total_s += item.Value [item.Value.Count - 1];
			total_s = total_s.Replace(";","\n");

			//add story content
			GameObject contentObj = Instantiate (StoryContentItem, storyContent);
			contentObj.transform.Find ("Text").GetComponent<Text> ().text = total_s;
			contentObj.name = item.Key;

			//add btn listener
			Button btn = btnObj.GetComponent<Button> ();
			btn.onClick.AddListener (delegate () {
				this.OnClick (contentObj);
			});
		}

            //hide all content
            foreach (Transform child in storyContent.transform)
            {
                child.gameObject.SetActive(false);
            }
    }

    private void OnEnable()
    {
        print("diary enabled!");
    }

    private void Update()
    {
        if(Input.GetButtonDown("Submit"))
        {
            print("tryign to close diary");
            gameObject.SetActive(false);
        }
    }

    public void OnClick(GameObject contentObj)
    {
        foreach (Transform child in storyContent.transform)
        {
            if (child.gameObject == contentObj)
            {
                contentObj.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }

	public void ShowContents(){
	}
 
}
