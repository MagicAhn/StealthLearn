using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// 存放 控制 Animator中 Parameters 名称的 hash tags
/// </summary>
public class DoneHashIDs : MonoBehaviour
{
    // DonePlayerAnimator
    public Int32 speedFloat;
    public Int32 sneakingBool;
    public Int32 deadBool;
    public Int32 shoutingBool;
    public Int32 locomotionState;

    void Awake()
    {
        speedFloat = Animator.StringToHash("Speed");
        sneakingBool = Animator.StringToHash("Sneaking");
        deadBool = Animator.StringToHash("Dead");
        shoutingBool = Animator.StringToHash("Shouting");
        locomotionState = Animator.StringToHash("Base Layer.Locomotion");
    }
}
