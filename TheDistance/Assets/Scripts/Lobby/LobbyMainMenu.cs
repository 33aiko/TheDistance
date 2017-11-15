using UnityEngine;
using UnityEngine.UI;

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

        //public void OnEnable()
        //{
        //    lobbyManager.topPanel.ToggleVisibility(true);

        //    ipInput.onEndEdit.RemoveAllListeners();
        //    ipInput.onEndEdit.AddListener(onEndEditIP);

        //    matchNameInput.onEndEdit.RemoveAllListeners();
        //    matchNameInput.onEndEdit.AddListener(onEndEditGameName);
        //}


        private void switchToPage(string pageName)
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                GameObject tempChild = this.transform.GetChild(i).gameObject;
                tempChild.SetActive(tempChild.name == pageName);
            }
        }   



        void Start()
        {
            //switchToPage("InitPage");
            lobbyManager.topPanel.ToggleVisibility(true);

            ipInput.onEndEdit.RemoveAllListeners();
            ipInput.onEndEdit.AddListener(onEndEditIP);

            matchNameInput.onEndEdit.RemoveAllListeners();
            matchNameInput.onEndEdit.AddListener(onEndEditGameName);


            
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
        }

        public void OnClickHost()
        {
            //switchToPage("LobbyPage");
            lobbyManager.StartHost();
        }


        public void OnClickBeClient()
        {
            switchToPage("ClientJoinPage");
        }


        public void OnClickJoin()
        {
            //lobbyManager.ChangeTo(lobbyPanel);
            switchToPage("ClientJoinPage");

            //InputField inputField = this.transform.Find("ClientJoinPage/InputFieldNew").GetComponent<InputField>();
            //lobbyManager.networkAddress = inputField.text; //ipInput.text;


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
