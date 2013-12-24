using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// 控制 Enemy的跑动和转弯
/// </summary>
public class DoneAnimatorSetup : MonoBehaviour
{
    public Single speedDampTime = 0.1f;
    public Single angularSpeedDampTime = 0.7f;
    public Single angularResponseTime = 0.6f;

    private Animator anim;
    private DoneHashIDs hash;

    public DoneAnimatorSetup(Animator animtor, DoneHashIDs hahIDs)
    {
        anim = animtor;
        hash = hahIDs;
    }

    public void Setup(Single speed, Single angle)
    {
        // 角速度
        Single angularSpeed = angle / angularResponseTime;

        anim.SetFloat(hash.speedFloat, speed, speedDampTime, Time.deltaTime);
        anim.SetFloat(hash.angularSpeedFloat, angularSpeed, angularSpeedDampTime, Time.deltaTime);
    }
}
