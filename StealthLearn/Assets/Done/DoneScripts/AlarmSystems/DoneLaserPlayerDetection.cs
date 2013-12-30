using UnityEngine;
using System.Collections;

public class DoneLaserPlayerDetection : MonoBehaviour
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
        // 光线是否还存在
        if (renderer.enabled)
        {
            // 被 player 碰到了，则通知 enemy 更新 player最近的位置
            if (other.gameObject == player)
            {
                lastPlayerSighting.position = player.transform.position;
            }
        }
    }
}
