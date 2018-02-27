using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollPageBtn : MonoBehaviour {

    public ScrollPage scrollPage;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    }

    public void RightBtnClick()
    {
        if (scrollPage.currentPageIndex < scrollPage.pages.Count - 1)
        {
            scrollPage.currentPageIndex++;
            scrollPage.targethorizontal = scrollPage.pages[scrollPage.currentPageIndex];
        }
    }
    public void LeftBtnClick()
    {
        if (scrollPage.currentPageIndex > 0)
        {
            scrollPage.currentPageIndex--;
            scrollPage.targethorizontal = scrollPage.pages[scrollPage.currentPageIndex];
        }
    }
}
