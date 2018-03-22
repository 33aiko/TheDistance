using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CheckPointController : MonoBehaviour {

	public GameObject checkpointItem; 
    public bool isInCave = false;

	Animator checkpointAnim; 

    int cnt = 0;
    public bool isCollected = false;

    void Start()
    {
		checkpointAnim = this.GetComponent<Animator> ();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            cnt++;
            if(cnt == 2)
            {
                if (isCollected) return;
                isCollected = true;
                Player p = collision.GetComponent<Player>();
                p.curCheckPoint = transform.position;
                print("Arrived first check point");
                //gameObject.SetActive(false);
				if (checkpointAnim!=null) {
					checkpointAnim.SetTrigger ("isActivated");
				} else {
					GetComponent<SpriteRenderer> ().DOFade (1, 0.5f);
					transform.DOScale (new Vector3 (12, 12, 12), 0.5f);
					transform.DOScale (new Vector3 (11, 11, 11), 0.5f).SetDelay (0.5f);
				}

                //FindObjectOfType<CaveEffectController>().SetShaderPosition("_CheckpointPos", transform.position);
                foreach (CaveEffectController cec in FindObjectsOfType<CaveEffectController>())
                {
                    cec.AddCheckpointLight(transform.position);
                }

				GameObject.Find ("AudioManager").GetComponent<AudioManager> ().Play ("Checkpoint");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            cnt--;
        }
    }
}
