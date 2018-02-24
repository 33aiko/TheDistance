using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Networking;
using DG.Tweening;

public class KeyController : MonoBehaviour {

	public int keyIdx;

	int cnt = 0;

	Image ima;

    public Sprite[] fragSprite; //array to store sprite
	public GameObject[] memoryContent ; 
    public int[] both;
	public GameObject triggerEffect; 
	public GameObject diaryBtn; 
	public Image[] divideLine; 
	public Image diaryBG;

    public bool isInCave = false;
    Material caveMaterial;

	Image memoryBackground; 
	Text memoryTitle; 
	Text memoryHint; 

	bool memoryShowed = false;

    Text t;
	Image inputUI;

    bool collected = false;
    bool playerNearby = false;

    string UIPath = "Sprites/UI/controls/input hint UI";
    string ps4UIName = "inputUI_tri";
    string keyboardUIName = "inputUI_keyE";
    bool currentIsKeyboard = true;

	float blurFocalLength = 100, normalFocalLength = 50; 

    private void Start()
	{
        loadSprite(UIPath, keyboardUIName);
        inputUI.gameObject.SetActive(false);

        t = GetComponentInChildren<Text>();
        t.text = "";
        both = new int[2];
        both[0] = 0;
        both[1] = 0;
		ima = GameObject.Find("HaveFragment" + keyIdx).GetComponent<Image>();
		if (ima == null)
			print("Nothign found! something wrong");
		ima.enabled = true;
		ima.sprite = Resources.Load<Sprite>("Sprites/Items/UI_fragment_uncollected") ;

		memoryBackground = GameObject.Find ("MemoryBackground").GetComponent<Image> ();
		memoryHint = GameObject.Find ("MemoryHint").GetComponent<Text> ();
		memoryTitle = GameObject.Find ("MemoryTitle").GetComponent<Text> ();
		memoryTitle.DOFade (0, 0);
		if (divideLine.Length == 2) {
			divideLine [0].DOFade (0, 0);
			divideLine [1].DOFade (0, 0);
		}
		diaryBG.DOFade (0, 0);
		memoryBackground.DOFade (0, 0);
		memoryHint.DOFade (0, 0);
		if (memoryContent.Length == 2) {
			memoryContent [0].SetActive (false);
			memoryContent [1].SetActive (false);
		}

        if (isInCave)
        {
            caveMaterial = FindObjectOfType<CaveEffectController>().caveMaterial;
            caveMaterial.SetVector("_FragmentPos", transform.position);
        }
    }

	private void Update(){
		if(Input.GetButtonDown("Submit")){
			if (memoryShowed) {
				Camera.main.GetComponent<DOVModify> ().SetActive (false);
				memoryContent [0].SetActive (false);
				memoryContent [1].SetActive (false);
				memoryBackground.DOFade (0, 1);
				memoryTitle.DOFade (0, 1);
				memoryHint.DOFade (0, 1);
				diaryBG.DOFade (0, 1);
				divideLine [0].DOFade (0, 1);
				divideLine [1].DOFade (0, 1);
			}
		}
        updateText();
	}

    private void loadSprite(string path, string UIname)
    {
        inputUI = GetComponentInChildren<Image>();
        Sprite[] sprites;
        sprites = Resources.LoadAll<Sprite>(path);
        inputUI.sprite = sprites.Where(tmp => tmp.name == UIname).First();
    }

    public void setImage(bool isKeyboard)
    {
        if (isKeyboard == currentIsKeyboard) return;
        if (isKeyboard)
        {
            loadSprite(UIPath, keyboardUIName);
        }
        else
        {
            loadSprite(UIPath, ps4UIName);
        }

        currentIsKeyboard = isKeyboard;
    }

    private void updateText()
    {
        if (!playerNearby) t.text = "";
        else if (collected == false)
        {
           // t.text = "Press E to examine";
			inputUI.gameObject.SetActive(true);

        }
        else if (both[0] + both[1] != 2)
        {
			inputUI.gameObject.SetActive(false);
            t.text = "Wait for your partner...";
        }
        else
        {
			inputUI.gameObject.SetActive(false);
            t.text = "Review content in the diary";
        }
    }

    public void collectThis(Player p)
    {
        collected = true;
        p.haveKey[keyIdx] = true;
        p.checkWho(keyIdx);

//        if (both[0]==0||both[1]==0) return;
//       
//        GameObject showDiary = GameObject.Find("UI/Canvas/Diary/StoryBtnList/Scroll View/Viewport/Content/"+keyIdx.ToString());
//        showDiary.SetActive(true);

		updateText();
    }


    private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			cnt++;
			if (cnt == 2)
			{
				Player p = collision.GetComponent<Player>();
                playerNearby = true;
                p.curFragment = this;
                inputUI.gameObject.SetActive(true);
                updateText();
            }
        }
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if(collision.gameObject.tag == "Player")
		{
			cnt--;
            if(cnt != 2)
            {
				Player p = collision.GetComponent<Player>();
                playerNearby = false;
                t.text = "";
				inputUI.gameObject.SetActive (false);
                p.curFragment = null;
            }
		}
	}

    
	public void setBoth()
    {
        this.GetComponent<SpriteRenderer>().sprite = fragSprite[both[0] + both[1]];
    }

	public void ShowEricMemory()
	{
		GameObject.Find ("AudioManager").GetComponent<AudioManager> ().Play ("MemoryContent");
		if (keyIdx == 0) {
			GameObject.Find ("AudioManager").GetComponent<AudioManager> ().PlayMusicTrack ("musicTrack01");
			if (diaryBtn != null) {
				diaryBtn.SetActive (true);
			}
		} else if (keyIdx == 1) {
			GameObject.Find ("AudioManager").GetComponent<AudioManager> ().StopMusicTrack ("musicTrack01");
			GameObject.Find ("AudioManager").GetComponent<AudioManager> ().PlayMusicTrack ("musicTrack02");
		} else if (keyIdx == 2) {
			GameObject.Find ("AudioManager").GetComponent<AudioManager> ().StopMusicTrack ("musicTrack02");
			GameObject.Find ("AudioManager").GetComponent<AudioManager> ().PlayMusicTrack ("musicTrack03");
		}
		Camera.main.GetComponent<DOVModify> ().SetActive (true);
		Camera.main.GetComponent<DOVModify> ().SetFocalLength (100);
		memoryBackground.DOFade (0.4f, 0.5f).SetDelay(0.5f);
		memoryTitle.text = "Fragment " + (keyIdx + 1).ToString ();
		memoryTitle.DOFade (1, 0.5f).SetDelay (0.5f);
		diaryBG.DOFade (1, 0.5f).SetDelay (0.5f);
		if (divideLine.Length == 2) {
			divideLine [0].DOFade (1, 0.5f).SetDelay (0.5f);
			divideLine [1].DOFade (1, 0.5f).SetDelay (0.5f);
		}
		memoryHint.DOFade (1, 0.5f).SetDelay(0.5f).OnComplete (() => {
				memoryContent [0].SetActive (true);
			});
		memoryShowed = true; 
	}

	public void ShowNatalieMemory(){
		GameObject.Find ("AudioManager").GetComponent<AudioManager> ().Play ("MemoryContent");
		if (keyIdx == 0) {
			GameObject.Find ("AudioManager").GetComponent<AudioManager> ().PlayMusicTrack ("musicTrack01");
			if (diaryBtn != null) {
				diaryBtn.SetActive (true);
			}
		}else if (keyIdx == 1) {
			GameObject.Find ("AudioManager").GetComponent<AudioManager> ().StopMusicTrack ("musicTrack01");
			GameObject.Find ("AudioManager").GetComponent<AudioManager> ().PlayMusicTrack ("musicTrack02");
		} else if (keyIdx == 2) {
			GameObject.Find ("AudioManager").GetComponent<AudioManager> ().StopMusicTrack ("musicTrack02");
			GameObject.Find ("AudioManager").GetComponent<AudioManager> ().PlayMusicTrack ("musicTrack03");
		}

		Camera.main.GetComponent<DOVModify> ().SetActive (true);
		Camera.main.GetComponent<DOVModify> ().SetFocalLength (100);
		memoryBackground.DOFade (0.4f, 0.5f).SetDelay(0.5f);
		memoryTitle.text = memoryTitle.text + " " + (keyIdx + 1).ToString ();
		memoryTitle.DOFade (1, 0.5f).SetDelay (0.5f);
		diaryBG.DOFade (1, 0.5f).SetDelay (0.5f);
		if (divideLine.Length == 2) {
			divideLine [0].DOFade (1, 0.5f).SetDelay (0.5f);
			divideLine [1].DOFade (1, 0.5f).SetDelay (0.5f);
		}
		memoryHint.DOFade (1, 0.5f).SetDelay(0.5f).OnComplete (() => {
			memoryContent [1].SetActive (true);
		});
		memoryShowed = true; 
	}

	public void PlayEffect(){
		Debug.Log ("trigger");
		if (!triggerEffect.GetComponent<ParticleSystem> ().isPlaying) {
			triggerEffect.GetComponent<ParticleSystem> ().Play ();
		}
		triggerEffect.transform.DOScale (new Vector3 (0.3f, 1.4f, 1), 0);
		triggerEffect.transform.DOScale (new Vector3 (0.5f, 1.5f, 1), 0.5f);
		triggerEffect.transform.DOScale (new Vector3 (0.4f, 1.4f, 1), 0.5f).SetDelay(0.5f);
	}
}
