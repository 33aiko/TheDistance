using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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


	Image memoryBackground; 
	Text memoryHint; 

	bool memoryShowed = false;

    Text t;

    bool collected = false;
    bool playerNearby = false;


	private void Start()
	{
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
		memoryBackground.DOFade (0, 0);
		memoryHint.DOFade (0, 0);
		if (memoryContent.Length == 2) {
			memoryContent [0].SetActive (false);
			memoryContent [1].SetActive (false);
		}
	}

	private void Update(){
		if(Input.GetKeyUp(KeyCode.R)){
			if (memoryShowed) {
				memoryContent [0].SetActive (false);
				memoryContent [1].SetActive (false);
				memoryBackground.DOFade (0, 1);
				memoryHint.DOFade (0, 1);
			}
		}
        updateText();
	}

    private void updateText()
    {
        if (!playerNearby) t.text = "";
        else if (collected == false)
        {
            t.text = "Press E to examine";
        }
        else if (both[0] + both[1] != 2)
        {
            t.text = "Wait for your partner to trigger \n another half of memory";
        }
        else
        {
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
			GameObject.Find ("AudioManager").GetComponent<AudioManager> ().PlayMusicTrack("musicTrack01");
		}
		memoryBackground.DOFade (1, 0.5f).SetDelay(0.5f);
		memoryHint.DOFade (1, 0.5f).SetDelay(0.5f).OnComplete (() => {
				memoryContent [0].SetActive (true);
			});
		memoryShowed = true; 
	}

	public void ShowNatalieMemory(){
		GameObject.Find ("AudioManager").GetComponent<AudioManager> ().Play ("MemoryContent");
		if (keyIdx == 0) {
			GameObject.Find ("AudioManager").GetComponent<AudioManager> ().PlayMusicTrack ("musicTrack01");
		}
		memoryBackground.DOFade (1, 0.5f).SetDelay(0.5f);
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
