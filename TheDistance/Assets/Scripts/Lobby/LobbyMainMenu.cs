using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Prototype.NetworkLobby
{
    //Main menu, mainly only a bunch of callback called by the UI (setup throught the Inspector)
    public class LobbyMainMenu : MonoBehaviour 
    {
        public LobbyManager lobbyManager;

        public RectTransform lobbyServerList;
        public RectTransform lobbyPanel;

        public InputField ipInput;
        public InputField matchNameInput;

        public void OnEnable()
        {
<<<<<<< HEAD
            //switchToPage("InitPage");
=======
>>>>>>> parent of a0a2eee... add new lobby
            lobbyManager.topPanel.ToggleVisibility(true);

            ipInput.onEndEdit.RemoveAllListeners();
            ipInput.onEndEdit.AddListener(onEndEditIP);

            matchNameInput.onEndEdit.RemoveAllListeners();
            matchNameInput.onEndEdit.AddListener(onEndEditGameName);
<<<<<<< HEAD


            
           // regInitPageButtons();
            //regStartPageButtons();
            //regClientJoinPageButtons();
        }

        private void regInitPageButtons()
        {
            Button startBttn = this.transform.Find("InitPage/Btn_start").GetComponent<Button>();
            startBttn.onClick.AddListener(OnClickStart);
        }

        private void regStartPageButtons()
        {
            Transform startPage = this.transform.Find("StartPage");
            Button inviteBttn = startPage.Find("InviteBttn").GetComponent<Button>();
            inviteBttn.onClick.AddListener(OnClickInvite);

            Button joinBttn = startPage.Find("JoinBttn").GetComponent<Button>();
            joinBttn.onClick.AddListener(OnClickBeClient);
        }

        private void regClientJoinPageButtons()
        {
            Transform clientJoinPage = this.transform.Find("ClientJoinPage");
            Button joinButton = clientJoinPage.Find("JoinButton").GetComponent<Button>();
            joinButton.onClick.AddListener(OnClickJoin);
        }



        public void OnClickStart()
        {
            switchToPage("StartPage");
        }

        public void OnClickInvite()
        {
            lobbyManager.StartHost();
            switchToPage("StartPage");
=======
>>>>>>> parent of a0a2eee... add new lobby
        }

        public void OnClickHost()
        {
<<<<<<< HEAD
            //switchToPage("LobbyPage");
=======
>>>>>>> parent of a0a2eee... add new lobby
            lobbyManager.StartHost();
        }

        public void OnClickJoin()
        {
            lobbyManager.ChangeTo(lobbyPanel);

            lobbyManager.networkAddress = ipInput.text;
            lobbyManager.StartClient();

            lobbyManager.backDelegate = lobbyManager.StopClientClbk;
            lobbyManager.DisplayIsConnecting();

            lobbyManager.SetServerInfo("Connecting...", lobbyManager.networkAddress);
        }

        public void OnClickDedicated()
        {
            lobbyManager.ChangeTo(null);
            lobbyManager.StartServer();

            lobbyManager.backDelegate = lobbyManager.StopServerClbk;

            lobbyManager.SetServerInfo("Dedicated Server", lobbyManager.networkAddress);
        }

        public void OnClickCreateMatchmakingGame()
        {
            lobbyManager.StartMatchMaker();
            lobbyManager.matchMaker.CreateMatch(
                matchNameInput.text,
                (uint)lobbyManager.maxPlayers,
                true,
				"", "", "", 0, 0,
				lobbyManager.OnMatchCreate);

            lobbyManager.backDelegate = lobbyManager.StopHost;
            lobbyManager._isMatchmaking = true;
            lobbyManager.DisplayIsConnecting();

            lobbyManager.SetServerInfo("Matchmaker Host", lobbyManager.matchHost);
        }

        public void OnClickOpenServerList()
        {
            lobbyManager.StartMatchMaker();
            lobbyManager.backDelegate = lobbyManager.SimpleBackClbk;
            lobbyManager.ChangeTo(lobbyServerList);
        }

        void onEndEditIP(string text)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnClickJoin();
            }
        }

        void onEndEditGameName(string text)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnClickCreateMatchmakingGame();
            }
        }

    }
}
