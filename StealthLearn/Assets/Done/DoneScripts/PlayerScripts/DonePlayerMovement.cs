using System;
using UnityEngine;
using System.Collections;

public class DonePlayerMovement : MonoBehaviour
{
    // Player shout 时播放的 音效
    public AudioClip ShoutingClip;

    public Single speedDampTime = 0.1f;
    // Player 转弯的 平滑度
    public Single turnSmoothing = 15f;

    private Animator anim;
    private DoneHashIDs hash;

    void Awake()
    {
        anim = GetComponent<Animator>();
        // 通过 获得 名为 GameController 的 GameObject上所附加的 DoneHashIDs 脚本
        hash = GameObject.FindGameObjectWithTag(DoneTags.gameController).GetComponent<DoneHashIDs>();
        // 设置 Shouting Layer 的 Weight(权重) 为 1
        anim.SetLayerWeight(1, 1f);
    }

    // Use this for initialization
    void Start()
    {

    }

    void FixedUpdate()
    {
        Single horizontal = Input.GetAxis("Horizontal");
        Single vertical = Input.GetAxis("Vertical");
        Boolean sneak = Input.GetButton("Sneak");

        MovementManagement(horizontal, vertical, sneak);
    }

    // Update is called once per frame
    void Update()
    {
        Boolean shout = Input.GetButtonDown("Attract");
        anim.SetBool(hash.shoutingBool, shout);

        AudioManagement(shout);
    }

    // 通过让 FixedUpdate和Update 调用 MovementManagement 控制每一帧 Player的移动
    void MovementManagement(Single horizontal, Single vertical, Boolean sneaking)
    {
        // 通过 sneaking 判断 Sneak 是否播放
        anim.SetBool(hash.sneakingBool, sneaking);
        Debug.Log(222);

        // 通过 传入的 horizontal和vertical 判断 Player 是否 移动
        if (horizontal != 0.0f || vertical != 0.0f)
        {

            Debug.Log(111);
            // 设置 Player旋转
            Rotating(horizontal, vertical);
            // 设置 speed 为5.5f（Animator中设置的边界值）
            anim.SetFloat(hash.speedFloat, 5.5f, speedDampTime, Time.deltaTime);
        }
        else
        {
            anim.SetFloat(hash.speedFloat, 0);
        }
    }

    // 通过被 MovementManagement 控制而实现 Player 旋转
    void Rotating(Single horizontal, Single verticle)
    {
        // 得到 水平面上的本地forward（Player 是 垂直于 水平面的）
        Vector3 targetDirection = new Vector3(horizontal, 0f, verticle);
        // 根据 水平面上的本地forward 和 世界坐标系的 upward 创建一个 rotation
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        // rigidbody.rotation 类似于 transform.rotation,但是只会在 Physics Step 结尾时才会 被应用的 transform上
        // 如果不间断地想 移动 rigidbody或者是 kinematic rigidbody（刚体运动） ,要使用 MovePosition和MoveRotation
        // Quaternion.Lerp(from,to,t) 从 from 到to 不断插值 t秒，然后将值 标准化
        Quaternion newRotation = Quaternion.Lerp(rigidbody.rotation, targetRotation, Time.deltaTime * turnSmoothing);

        rigidbody.MoveRotation(newRotation);
    }

    // 控制 脚步声和shout 播放,脚步声只有在 locomotion state 才会播放
    void AudioManagement(Boolean shout)
    {
        //// 注意 错误: anim.GetCurrentAnimatorStateInfo(0).nameHash.Equals(hash.locomotionState)
        //// 如果 Player正处在 locomotion state，则有脚步声
        //if (anim.GetCurrentAnimatorStateInfo(0).nameHash == hash.locomotionState)
        //{
        //    // 不能一直 让clip开始播放呀
        //    if (!audio.isPlaying)
        //    {
        //        audio.Play();
        //    }
        //}
        //else
        //{
        //    audio.Stop();
        //}

        //if (shout)
        //{
        //    AudioSource.PlayClipAtPoint(ShoutingClip, transform.position);
        //}

        // If the player is currently in the run state...
        if (anim.GetCurrentAnimatorStateInfo(0).nameHash == hash.locomotionState)
        {
            // ... and if the footsteps are not playing...
            if (!audio.isPlaying)
                // ... play them.
                audio.Play();
        }
        else
            // Otherwise stop the footsteps.
            audio.Stop();

        // If the shout input has been pressed...
        if (shout)
            // ... play the shouting clip where we are.
            AudioSource.PlayClipAtPoint(ShoutingClip, transform.position);
    }
}
