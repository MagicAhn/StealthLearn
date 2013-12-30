using UnityEngine;
using System.Collections;

/// <summary>
/// 控制 Laser 开关
/// </summary>
public class DoneLaserSwitchDeactivation : MonoBehaviour
{
    public GameObject laser;
    // 控制 我在 关掉laser的地方的那块墙上 涂了个鸦（把它的 material 给换了）
    public Material unlockedMat;

    private GameObject player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag(DoneTags.player);
    }
    
    void OnTriggerStay(Collider other)
    {
        // player 在其附近
        if (other.gameObject == player)
        {
            // 并且按下了 “Switch”
            if (Input.GetButton("Switch"))
            {
                // 将 laser Deactive
                laser.SetActive(false);
                // 涂个鸦
                Renderer screen = transform.Find("prop_switchUnit_screen_001").renderer;
                screen.material = unlockedMat;

                // 播放一段音效
                audio.Play();
            }
        }
    }
    
    
}
