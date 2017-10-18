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


	Image memoryBackground; 
	Text memoryHint; 

	bool memoryShowed = false; 

	private void Start()
	{
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
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			cnt++;
			if (cnt == 2)
			{
				Player p = collision.GetComponent<Player>();
				p.haveKey[keyIdx] = true;
                p.checkWho(keyIdx);				
            }

        }
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if(collision.gameObject.tag == "Player")
		{
			cnt--;
		}
	}

    
	public void setBoth()
    {
        this.GetComponent<SpriteRenderer>().sprite = fragSprite[both[0] + both[1]];
    }

	public void ShowEricMemory()
	{
		GameObject.Find ("AudioManager").GetComponent<AudioManager> ().Play ("MemoryContent");
		memoryBackground.DOFade (1, 0.5f).SetDelay(0.5f);
		memoryHint.DOFade (1, 0.5f).SetDelay(0.5f).OnComplete (() => {
				memoryContent [0].SetActive (true);
			});
		memoryShowed = true; 
	}

	public void ShowNatalieMemory(){
		GameObject.Find ("AudioManager").GetComponent<AudioManager> ().Play ("MemoryContent");
		memoryBackground.DOFade (1, 0.5f).SetDelay(0.5f);
		memoryHint.DOFade (1, 0.5f).SetDelay(0.5f).OnComplete (() => {
			memoryContent [1].SetActive (true);
		});
		memoryShowed = true; 
	}

}
