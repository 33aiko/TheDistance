﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour {

    private void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(delegate { restartLevel(); });
    }

    void restartLevel()
    {
        print("RUA, Button!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
