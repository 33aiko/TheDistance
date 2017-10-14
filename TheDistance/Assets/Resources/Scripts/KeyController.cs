using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyController : MonoBehaviour {

	public int keyIdx;

	int cnt = 0;

	Image ima;

    public Sprite[] fragSprite; //array to store sprite

    public int[] both;

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
}
