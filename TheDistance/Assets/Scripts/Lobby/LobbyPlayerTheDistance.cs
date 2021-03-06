﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace Prototype.NetworkLobby
{
    //Player entry in the lobby. Handle selecting color/setting name & getting ready for the game
    //Any LobbyHook can then grab it and pass those value to the game player prefab (see the Pong Example in the Samples Scenes)
    public class LobbyPlayerTheDistance : NetworkLobbyPlayer
    {
        //public Button readyButton;
        public GameObject playerAtLobby;
        public GameObject friendAtLobby;
        public GameObject ericAtLobby;
        public GameObject natalieAtLobby;
        public GameObject spiritAtLobby;
        public GameObject platform_temp;
        public GameObject lobbyPage;
		public TitleScreenManager titleScreenManager;

        public string EricAnimator = "Animations/Player_1";
        public string NatalieAnimator = "Animations/Player_2_Override";
        private string EricSpiritAnimator = "Animations/Spirit_1";
        private string NatalieSpiritAnimator = "Animations/Spirit_2";

        private string EricIdle = "Sprites/Characters/eric_idle";
        private string NatalieIdle = "Sprites/Characters/natalie_jump";

        private string EricReady = "Sprites/Characters/eric_push";
        private string NatalieReady = "Sprites/Characters/natalie_push";
        
        static Color JoinColor = new Color(255.0f / 255.0f, 0.0f, 101.0f / 255.0f, 1.0f);
        static Color NotReadyColor = new Color(34.0f / 255.0f, 44 / 255.0f, 55.0f / 255.0f, 1.0f);
        static Color ReadyColor = new Color(0.0f, 204.0f / 255.0f, 204.0f / 255.0f, 1.0f);
        static Color TransparentColor = new Color(0, 0, 0, 0);



        private bool someoneReady = false;

        static int playerNum = 0;


        void Update()
        {
            if (readyToBegin)
                return;

            //if (SceneManager.GetActiveScene().name != "loading_temp" && SceneManager.GetActiveScene().name != "StartScreen")
            //{
            //    SendReadyToBeginMessage();
            //}


			if (Input.GetButtonDown("Submit") && (friendAtLobby.activeSelf || spiritAtLobby.activeSelf))
            {
                print("sending ready begin message");
                SendReadyToBeginMessage();
				if (isLocalPlayer) {
					titleScreenManager.WaitForPartner ();
				}
            }

            //print("updating!!!");

            if (!someoneReady && playerAtLobby && friendAtLobby)
            {
                //Debug.Log("checking move");
                if (isServer)
                {
                    //print("sending to client");
                    PlayerSimple pPS = playerAtLobby.GetComponent<PlayerSimple>();
                    RpcMove(pPS.input.x, pPS.playerJumping, pPS.transform.position);
                }
                else
                {
                    //if (playerAtLobby.GetComponent<PlayerSimple>().input != Vector2.zero)
                    PlayerSimple pPS = playerAtLobby.GetComponent<PlayerSimple>();
                    CmdMove(pPS.input.x, pPS.playerJumping, pPS.transform.position);
                    //print("update cmd move");
					//CmdMove(playerAtLobby.transform.position,playerAtLobby.GetComponent<PlayerSimple>().playerJumping);
                }
					
            }

            


        }
        
        public override void OnClientEnterLobby()
        {

            playerNum++;

            lobbyPage = GameObject.Find("LobbyManagerTheDistance/LobbyPanel/LobbyPage");
            //playerAtLobby = GameObject.Find("LobbyManager_customized/LobbyPanel/LobbyPage/platform_temp/player");
            //spiritAtLobby = GameObject.Find("LobbyManager_customized/LobbyPanel/LobbyPage/platform_temp/Spirit");
            platform_temp = GameObject.Find("platform_temp");
			titleScreenManager = GameObject.Find("TitleScreenManager").GetComponent<TitleScreenManager>();
		

            ericAtLobby = platform_temp.transform.Find("eric").gameObject;
            natalieAtLobby = platform_temp.transform.Find("natalie").gameObject;
            playerAtLobby = isServer ? ericAtLobby : natalieAtLobby;
            friendAtLobby = !isServer ? ericAtLobby : natalieAtLobby;
            //playerAtLobby = platform_temp.transform.Find("player").gameObject;
            friendAtLobby.SetActive(false);
            spiritAtLobby = platform_temp.transform.Find("Spirit").gameObject;

            friendAtLobby.GetComponent<PlayerSimple>().ControlledByOthers(); // cannot control friend move on this side



            //this.gameObject.SetActive(false); // make it invisible

            base.OnClientEnterLobby();

            if (LobbyManager.s_Singleton != null) LobbyManager.s_Singleton.OnPlayersNumberModified(1);

            if (!spiritAtLobby.GetComponent<Animator>())
                spiritAtLobby.AddComponent<Animator>();
            spiritAtLobby.GetComponent<Animator>().runtimeAnimatorController = Instantiate(Resources.Load(isServer ? EricSpiritAnimator: NatalieSpiritAnimator)) as RuntimeAnimatorController;

            //playerAtLobby.AddComponent<Animator>();
            //playerAtLobby.GetComponent<Animator>().runtimeAnimatorController = Instantiate(Resources.Load(isServer ? EricAnimator : NatalieAnimator)) as RuntimeAnimatorController;


            if (playerAtLobby)
            {
                Debug.Log("have playerAtLobby");
            }
            else
            {
                Debug.Log("playerAtLobby not exist");
            }
            //playerAtLobby.SetActive(true);
            //playerAtLobby.GetComponent<Animator>().runtimeAnimatorController = Instantiate(Resources.Load(isServer ? EricAnimator : NatalieAnimator)) as RuntimeAnimatorController;

            if(playerNum == 2)
            {
                friendAtLobby.GetComponent<PlayerSimple>().controllerEnteredLobby = true;
                friendAtLobby.SetActive(true);
				titleScreenManager.MatchGame ();
            }
                

            if (isLocalPlayer)
            {
                Debug.Log("OnClientEnterLobby, isLocalPlayer");

                
                //playerAtLobby.GetComponent<Image>().sprite = Instantiate(Resources.Load(isServer ? EricIdle : NatalieIdle)) as Sprite;
                SetupLocalPlayer();
            }
            else
            {
                Debug.Log("OnClientEnterLobby, is NOT LocalPlayer");

                if (isServer)
                {
                    Debug.Log("OnClientEnterLobby, is NOT LocalPlayer - isServer");
                }
                else
                {
                    Debug.Log("OnClientEnterLobby, is NOT LocalPlayer - is not Server");
                    
                }
                spiritAtLobby.SetActive(false);

                //spiritAtLobby.AddComponent<Animator>();
                //spiritAtLobby.GetComponent<Animator>().runtimeAnimatorController = Instantiate(Resources.Load(isServer ? EricSpiritAnimator : NatalieSpiritAnimator)) as RuntimeAnimatorController;
                
                SetupOtherPlayer();
            }

        }

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();

            //if we return from a game, color of text can still be the one for "Ready"
            //readyButton.transform.GetChild(0).GetComponent<Text>().color = Color.white;

            SetupLocalPlayer();
        }

        //void ChangeReadyButtonColor(Color c)
        //{
        //    ColorBlock b = readyButton.colors;
        //    b.normalColor = c;
        //    b.pressedColor = c;
        //    b.highlightedColor = c;
        //    b.disabledColor = c;
        //    readyButton.colors = b;
        //}

        void SetupOtherPlayer()
        {
           
            //ChangeReadyButtonColor(NotReadyColor);

            //readyButton.transform.GetChild(0).GetComponent<Text>().text = "...";
            //readyButton.interactable = false;

            OnClientReady(false);
        }

        void SetupLocalPlayer()
        {
          
            //ChangeReadyButtonColor(JoinColor);

            //readyButton.transform.GetChild(0).GetComponent<Text>().text = "JOIN";
            //readyButton.interactable = true;

            //readyButton.onClick.RemoveAllListeners();
            //readyButton.onClick.AddListener(OnReadyClicked);

            //when OnClientEnterLobby is called, the loval PlayerController is not yet created, so we need to redo that here to disable
            //the add button if we reach maxLocalPlayer. We pass 0, as it was already counted on OnClientEnterLobby
            if (LobbyManager.s_Singleton != null) LobbyManager.s_Singleton.OnPlayersNumberModified(0);
        }


        public override void OnClientReady(bool readyState)
        {
            if (readyState)
            {
                //Debug.Log("ready");
                if (!isLocalPlayer)
                {
                    Debug.Log("bie ren ready");
                //    lobbyPage.transform.Find("YourFriendReady").gameObject.SetActive(true);
                    spiritAtLobby.transform.position = friendAtLobby.transform.position;
                    spiritAtLobby.SetActive(true);
                    friendAtLobby.SetActive(false);
					spiritAtLobby.GetComponentInChildren<Text> ().DOFade (1, 0.5f);

                }
                else
                {
                    Debug.Log("wo ready");
                //    lobbyPage.transform.Find("YouAreReady").gameObject.SetActive(true); 
                    playerAtLobby.GetComponent<PlayerSimple>().Change2ReadyState();
					playerAtLobby.GetComponentInChildren<Text> ().DOFade (1, 0.5f);

                }
            }
            else
            {
                Debug.Log("mei you ready");
                if (!isLocalPlayer)
                {
                    
                }
                else
                {
                    
                }
            }
        }

       

        public void OnReadyClicked()
        {
            someoneReady = true;
            SendReadyToBeginMessage();
        }

        

        //Cleanup thing when get destroy (which happen when client kick or disconnect)
        public void OnDestroy()
        {
            
            if (LobbyManager.s_Singleton != null) LobbyManager.s_Singleton.OnPlayersNumberModified(-1);

           
        }

        [ClientRpc]
        public void RpcUpdateCountdown(int countdown)
        {
            LobbyManager.s_Singleton.countdownPanel.UIText.text = "Match Starting in " + countdown;
            LobbyManager.s_Singleton.countdownPanel.gameObject.SetActive(countdown != 0);
        }


        //sent by server, run on all clients
        [ClientRpc]
        public void RpcMove(float input, bool isJumping, Vector3 targetPos)
        {

            if (isJumping)
                print("Rpc jump");
            if (!isServer && friendAtLobby)
            {
                //print("input: " + input.x);
                friendAtLobby.GetComponent<PlayerSimple>().UpdatePosAndAnimator(input, isJumping, targetPos);
            }
        }

        //sent by client, run on server
        [Command]
        public void CmdMove(float input, bool isJumping, Vector3 targetPos)
        {
            if (isJumping)
                print("Cmd jump");
			if (friendAtLobby)
            {
                //print("input: " + input.x);
                friendAtLobby.GetComponent<PlayerSimple>().UpdatePosAndAnimator(input, isJumping, targetPos);
			}
        }
    }
}
