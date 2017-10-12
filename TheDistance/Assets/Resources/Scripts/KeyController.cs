using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyController : MonoBehaviour {

	public int keyIdx;

	int cnt = 0;

	Image ima;

	private void Start()
	{
		ima = GameObject.Find("HaveFragment" + keyIdx).GetComponent<Image>();
		if (ima == null)
			print("Nothign found! something wrong");
		ima.enabled = false;
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
				if (p.checkBothKey(keyIdx)) {
					Debug.Log("both get");
				}
				else
				{
					Debug.Log("need another player get key");
				}
				ima.enabled = true;
				print("Player got the key!");
				gameObject.SetActive(false);
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
}
