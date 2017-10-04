using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Networking;


[RequireComponent(typeof(NetworkManager))]
[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
public class StartPage : MonoBehaviour {

    public NetworkManager manager;
    void Awake()
    {
        manager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }


    // Use this for initialization
    void Start () {
        Button newGame = transform.Find("ButtonPanel/NewGame").gameObject.GetComponent<Button>();
        newGame.onClick.AddListener(LoadServerUI);
        Button joinGame = transform.Find("ButtonPanel/JoinGame").gameObject.GetComponent<Button>();
        joinGame.onClick.AddListener(LoadClientUI);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void LoadServerUI()
    {
        Debug.Log("server");

        if (!manager)
            Debug.Log("no manager");

       

        transform.Find("ButtonPanel").gameObject.SetActive(false);
        transform.Find("ServerPage").gameObject.SetActive(true);


        if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
        {
            manager.StartServer();
        }



        Text serverText = transform.Find("ServerPage/ServerText").gameObject.GetComponent<Text>();
        serverText.text = "Your IP: " + manager.networkAddress;
        serverText.text += "\nWaiting...";
    }

    void LoadClientUI()
    {
        Debug.Log("client");
        transform.Find("ButtonPanel").gameObject.SetActive(false);
        transform.Find("ClientPage").gameObject.SetActive(true);
    }

}
