using UnityEngine;
using System.Collections;

public class DoneCCTVPlayerDetection : MonoBehaviour
{
    private GameObject player;
    private DoneLastPlayerSighting lastPlayerSighting;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag(DoneTags.player);
        lastPlayerSighting =
            GameObject.FindGameObjectWithTag(DoneTags.gameController).GetComponent<DoneLastPlayerSighting>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            Vector3 relPlayerPost = player.transform.position - transform.position;

            RaycastHit raycastHit;
            if (Physics.Raycast(transform.position, relPlayerPost, out raycastHit))
            {
                if (raycastHit.collider.gameObject == player)
                {
                    lastPlayerSighting.position = player.transform.position;
                }
            }
        }
    }
}
