using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;


namespace Prototype.NetworkLobby
{
    
	public class SceneManagerTheDistance
    {
        public static string nextSceneName;
        public static string[] sceneList =
        {
            "LX_scene1",
            "LX_scene2",
			"Boat",
        };
        public static int nextSceneID = 0;
    }

    public class LoadSceneController : MonoBehaviour
    {

        //public NetwrokManager lobbyManager;
        public Slider loadingSlider;

        public Text loadingText;

        private float loadingSpeed = 1;

        private float targetValue;

        private AsyncOperation operation;

        //void Awake()
        //{
        //    if (SceneManager.GetActiveScene().name == "loading_temp")
        //    {
        //        //启动协程  
        //        GameObject lobbyManagerGO = GameObject.Find("LobbyManagerTheDistance");
        //        if (lobbyManagerGO)
        //        {
        //            Debug.Log("#####find lobbyManagerGO");
        //            LobbyManager lm = lobbyManagerGO.GetComponent<LobbyManager>();
        //            if (lm )//&& Globe.nextSceneName != null)
        //            {

        //                lm.playScene = "LX_scene1";// Globe.nextSceneName;
        //                lm.onlineScene = "LX_scene1";// Globe.nextSceneName;

        //                Debug.Log("#####loading changing playscene name to: " + lm.playScene);
        //            }
        //        }
        //    }
        //}
        //// Use this for initialization  
        //void Start()
        //{

        //    Debug.Log("#####start###");
        //    loadingSlider.value = 0.0f;

        //    if (SceneManager.GetActiveScene().name == "loading_temp")
        //    {
        //        //启动协程  


        //        GameObject lobbyManagerGO = GameObject.Find("LobbyManagerTheDistance");
        //        if (lobbyManagerGO)
        //        {
        //            Debug.Log("#####find lobbyManagerGO");
        //            LobbyManager lm = lobbyManagerGO.GetComponent<LobbyManager>();
        //            if (lm && Globe.nextSceneName != null)
        //            {

        //                lm.playScene = Globe.nextSceneName;
        //                lm.onlineScene = Globe.nextSceneName;

        //                Debug.Log("#####loading changing playscene name to: " + lm.playScene);

        //                foreach (var gameObj in FindObjectsOfType(typeof(GameObject)) as GameObject[])
        //                {
        //                    if (gameObj.name == "Player(Clone)")
        //                    {
        //                        Debug.Log("#####find player clone");
        //                        Player player = gameObj.GetComponent<Player>();

        //                        Debug.Log("#####get player");
        //                        if (player && player.isClient && player.isLocalPlayer) // do this on server side only 
        //                            lm.ServerChangeScene(lm.playScene);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        void Start()
        {
            SceneManagerTheDistance.nextSceneID++;

            loadingSlider.value = 0.0f;

            if (SceneManager.GetActiveScene().name == "Loading")
            {
                //启动协程    
                StartCoroutine(AsyncLoading());
            }
        }



        IEnumerator AsyncLoading()
        {
            operation = SceneManager.LoadSceneAsync(SceneManagerTheDistance.sceneList[SceneManagerTheDistance.nextSceneID-1]);
            

            //阻止当加载完成自动切换    
            operation.allowSceneActivation = false;

            yield return operation;
        }
       

        // Update is called once per frame  
        void Update()
        {
            targetValue = operation.progress;

            if (operation.progress >= 0.9f)
            {
                //operation.progress的值最大为0.9  
                targetValue = 1.0f;
            }

            if (targetValue != loadingSlider.value)
            {
                //插值运算  
                loadingSlider.value = Mathf.Lerp(loadingSlider.value, targetValue, Time.deltaTime * loadingSpeed);
                if (Mathf.Abs(loadingSlider.value - targetValue) < 0.01f)
                {
                    loadingSlider.value = targetValue;
                }
            }

            loadingText.text = ((int)(loadingSlider.value * 100)).ToString() + "%";

            if ((int)(loadingSlider.value * 100) == 100)
            {

                //允许异步加载完毕后自动切换场景  
                operation.allowSceneActivation = true;
            }
        }
    }
}