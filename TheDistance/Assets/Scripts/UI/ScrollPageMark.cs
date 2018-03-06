using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScrollPageMark : MonoBehaviour
{
    public ScrollPage scrollPage;
    public ToggleGroup toggleGroup;
    public Toggle togglePrefab;

    public List<Toggle> toggleList = new List<Toggle>();
	
    void Awake()
    {
        scrollPage.OnPageChanged = OnScrollPageChanged;
    }

    void Update()
    {
    }


    public void OnScrollPageChanged(int pageCount, int currentPageIndex)
    {
        if(pageCount!=toggleList.Count)
        {
            if(pageCount>toggleList.Count)
            {
                int cc = pageCount - toggleList.Count;
                for(int i=0; i< cc; i++)
                {
                    toggleList.Add(CreateToggle());
                }
            }
            else if(pageCount < toggleList.Count)
            {
                while(toggleList.Count > pageCount)
                {
                    Toggle t = toggleList[toggleList.Count - 1];
                    toggleList.Remove(t);
                    DestroyImmediate(t.gameObject);
                }
            }
        }
        //foreach(Toggle toggle in toggleList)
        //{
        //    toggle.isOn = false;
        //}
        if(currentPageIndex>=0)
        {
            toggleList[currentPageIndex].isOn = true;
        }
    }

    Toggle CreateToggle()
    {
        Toggle t = GameObject.Instantiate<Toggle>(togglePrefab);
        t.gameObject.SetActive(true);
        t.transform.SetParent(toggleGroup.transform);
        t.transform.localScale = Vector3.one;
        t.transform.localPosition = Vector3.zero;
        t.group = toggleGroup;
        t.onValueChanged.AddListener((value) =>
        {   // you are missing this
            onClick(value);       // this is just a basic method call within another method
        }   // and this one
       );
        return t;
    }

    public void onClick(bool b)
    {
        if (b == true)
        {
            int tIndex = 0;
            for (int i = 0; i < toggleList.Count; i++)
            {
                if (toggleList[i].isOn)
                {
                    tIndex = i;
                }
            }
            scrollPage.currentPageIndex = tIndex;
            scrollPage.targethorizontal = scrollPage.pages[scrollPage.currentPageIndex];
        }
    }
}
