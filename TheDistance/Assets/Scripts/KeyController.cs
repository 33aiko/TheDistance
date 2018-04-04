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


    public GameObject collectEffect;
    public RectTransform diaryUIRect;

    public bool isInCave = false;
    Material caveMaterial;

	Image memoryBackground; 
	Text memoryTitle; 
	public Text memoryHint; 

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
        cnt = 0;

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
            foreach(CaveEffectController cec in FindObjectsOfType<CaveEffectController>())
            {
                cec.AddFragmentLight(transform.position);
            }
        }
        if(collectEffect.gameObject)
            collectEffect.gameObject.SetActive(false);
    }

    private void Update(){
		if(Input.GetButtonDown("Submit")){
			if (memoryShowed && fadeFinished) {
				Camera.main.GetComponent<DOVModify> ().SetActive (false);
				memoryContent [0].SetActive (false);
				memoryContent [1].SetActive (false);
				memoryBackground.DOFade (0, 1);
				memoryTitle.DOFade (0, 1);
				memoryHint.DOFade (0, 1);
				diaryBG.DOFade (0, 1);
				divideLine [0].DOFade (0, 1);
				divideLine [1].DOFade (0, 1);
                CollectEffect();
                diaryUIRect.gameObject.SetActive(true);
                memoryShowed = false;
			}
		}
        updateText();
        //if(collectEffect)
        //print(effectScreenPosition());
        if (Input.GetKeyDown(KeyCode.U))
        {
            CollectEffect();
        }
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
		if (collision.gameObject.tag == "Player" && collision.gameObject.name == "Player")
		{
			cnt++;
            //print(name + " with cnt " + cnt);
			if (cnt == 2)
			{
                //print("player enter fragment area! " + transform.parent.name);
				Player p = collision.GetComponent<Player>();
                playerNearby = true;
                p.curFragment = this;
                //print("player's fragment: " + p.curFragment);
                inputUI.gameObject.SetActive(true);
                updateText();
            }
        }
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player" && collision.gameObject.name == "Player")
		{
			cnt--;
            if(cnt != 2)
            {
                //print("player leaves fragment area!");
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
		Debug.Log (both [0] + both [1]);
        this.GetComponent<SpriteRenderer>().sprite = fragSprite[both[0] + both[1]];
    }

    public bool isWordByWord = false;

    public float fadeTime = 0.3f;
    void FadeWordByWord(Text orig)
    {
        string orig_str = orig.text;
        orig.text = "";
        var arr = orig_str.Split(' ');
        int l = arr.Length;
        string shown = "";

        Sequence seq = DOTween.Sequence();
        for(int i = 0; i < l; i++)
        {
            int vic = 30;
            string tmp = arr[i];
            seq.Append(
                DOTween.To(() => vic, x =>
                {
                    vic = x;
                    orig.text = shown + "<color=#ffffff" + vic.ToString("X2") + ">" + tmp + "</color>";
                }, 255, fadeTime).
                OnComplete(() =>
                {
                    shown += (tmp + " ");
                }).SetEase(Ease.Linear)
            );
        }
    }

    public float fadeCharTime = 0.1f;
    bool fadeFinished = false;
    void FadeCharByChar(Text orig)
    {
        fadeFinished = false;
        string orig_str = orig.text;
        orig.text = "";
        int l = orig_str.Length;
        string shown = "";

        Sequence seq = DOTween.Sequence();
        for (int i = 0; i < l; i++)
        {
            int vic = 30;
            char tmp = orig_str[i];
            seq.Append(
                DOTween.To(() => vic, x =>
                {
                    vic = x;
                    orig.text = shown + "<color=#ffffff" + vic.ToString("X2") + ">" + tmp + "</color>";
                }, 255, fadeCharTime).
                OnComplete(() =>
                {
                    shown += (tmp);
                }).SetEase(Ease.Linear)
            );
        }
        seq.OnComplete(() =>
        {
            print("diary fade in char by char finished");
            memoryHint.DOFade(1, 0.5f).OnComplete(() => { fadeFinished = true; });
        });
    }


    public void ShowEricMemory()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("MemoryContent");
        if (keyIdx == 0)
        {
            GameObject.Find("AudioManager").GetComponent<AudioManager>().PlayMusicTrack("musicTrack01");
            if (diaryBtn != null)
            {
                diaryBtn.SetActive(true);
            }
        }
        else if (keyIdx == 1)
        {
            GameObject.Find("AudioManager").GetComponent<AudioManager>().StopMusicTrack("musicTrack01");
            GameObject.Find("AudioManager").GetComponent<AudioManager>().PlayMusicTrack("musicTrack02");
        }
        else if (keyIdx == 2)
        {
            GameObject.Find("AudioManager").GetComponent<AudioManager>().StopMusicTrack("musicTrack02");
            GameObject.Find("AudioManager").GetComponent<AudioManager>().PlayMusicTrack("musicTrack03");
        }
        Camera.main.GetComponent<DOVModify>().SetActive(true);
        Camera.main.GetComponent<DOVModify>().SetFocalLength(100);
        memoryHint.DOFade(0, 0);
        memoryBackground.DOFade(0.4f, 0.5f).SetDelay(0.5f);
        memoryTitle.text = "Fragment " + (keyIdx + 1).ToString();
        memoryTitle.DOFade(1, 0.5f).SetDelay(0.5f);
        diaryUIRect.gameObject.SetActive(false);
        if (divideLine.Length == 2)
        {
            divideLine[0].DOFade(1, 0.5f).SetDelay(0.5f);
            divideLine[1].DOFade(1, 0.5f).SetDelay(0.5f);
        }
        diaryBG.DOFade(1, 0.5f).SetDelay(0.5f).OnComplete(() =>
        {
            memoryContent[0].SetActive(true);
            if(isWordByWord)
                FadeWordByWord(memoryContent[0].GetComponentInChildren<Text>());
            else
                FadeCharByChar(memoryContent[0].GetComponentInChildren<Text>());
        });
        memoryShowed = true;
    }

    public void ShowNatalieMemory()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("MemoryContent");
        if (keyIdx == 0)
        {
            GameObject.Find("AudioManager").GetComponent<AudioManager>().PlayMusicTrack("musicTrack01");
            if (diaryBtn != null)
            {
                diaryBtn.SetActive(true);
            }
        }
        else if (keyIdx == 1)
        {
            GameObject.Find("AudioManager").GetComponent<AudioManager>().StopMusicTrack("musicTrack01");
            GameObject.Find("AudioManager").GetComponent<AudioManager>().PlayMusicTrack("musicTrack02");
        }
        else if (keyIdx == 2)
        {
            GameObject.Find("AudioManager").GetComponent<AudioManager>().StopMusicTrack("musicTrack02");
            GameObject.Find("AudioManager").GetComponent<AudioManager>().PlayMusicTrack("musicTrack03");
        }

        Camera.main.GetComponent<DOVModify>().SetActive(true);
        Camera.main.GetComponent<DOVModify>().SetFocalLength(100);
        memoryHint.DOFade(0, 0);
        memoryBackground.DOFade(0.4f, 0.5f).SetDelay(0.5f);
        memoryTitle.text = memoryTitle.text + " " + (keyIdx + 1).ToString();
        memoryTitle.DOFade(1, 0.5f).SetDelay(0.5f);
        diaryUIRect.gameObject.SetActive(false);
        if (divideLine.Length == 2)
        {
            divideLine[0].DOFade(1, 0.5f).SetDelay(0.5f);
            divideLine[1].DOFade(1, 0.5f).SetDelay(0.5f);
        }
        diaryBG.DOFade(1, 0.5f).SetDelay(0.5f).SetDelay(0.5f).OnComplete(() =>
        {
            memoryContent[1].SetActive(true);
            if(isWordByWord)
                FadeWordByWord(memoryContent[1].GetComponentInChildren<Text>());
            else
                FadeCharByChar(memoryContent[1].GetComponentInChildren<Text>());
        });
        memoryShowed = true;
    }

    public void PlayEffect()
    {
        Debug.Log("trigger");
        if (!triggerEffect.GetComponent<ParticleSystem>().isPlaying)
        {
            triggerEffect.GetComponent<ParticleSystem>().Play();
        }
        triggerEffect.transform.DOScale(new Vector3(0.3f, 1.4f, 1), 0);
        triggerEffect.transform.DOScale(new Vector3(0.5f, 1.5f, 1), 0.5f);
        triggerEffect.transform.DOScale(new Vector3(0.4f, 1.4f, 1), 0.5f).SetDelay(0.5f);
    }

    public void CollectEffect()
    {
        if (!diaryUIRect) return;
        collectEffect.gameObject.SetActive(true);
        Rect r = RectTransformToScreenSpace(diaryUIRect);
        Vector3 end_screen = new Vector3(r.x + r.width / 2, Screen.height - r.y - r.height / 2, 200);
        Vector3 start = collectEffect.transform.position;
        Vector3 start_screen = Camera.main.WorldToScreenPoint(start);
        DOTween.To(() => start_screen, x =>
          {
              start_screen = x;
              collectEffect.transform.position = Camera.main.ScreenToWorldPoint(x);

          }, end_screen, 1.0f);
        diaryUIRect.DOScale(0.3f, 0.3f).SetLoops(2, LoopType.Yoyo).SetDelay(1.0f);
    }

    public Vector3 effectScreenPosition()
    {
        return Camera.main.WorldToScreenPoint(collectEffect.transform.position);
    }

    public static Rect RectTransformToScreenSpace(RectTransform transform)
    {
        Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
        Rect rect = new Rect(transform.position.x, Screen.height - transform.position.y, size.x, size.y);
        rect.x -= (transform.pivot.x * size.x);
        rect.y -= ((1.0f - transform.pivot.y) * size.y);
        return rect;
    }
}
