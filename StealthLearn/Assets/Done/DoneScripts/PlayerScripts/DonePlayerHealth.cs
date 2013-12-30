using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// player dead 后，需要执行下列操作
/// 1,player 不能继续 move
/// 2,触发 player dead时的 animation
/// 3,enemy 知道的 player的位置需要重置
/// 4,场景淡入淡出,重新开始场景（dead后 多长时间 淡入淡出 需要控制）
/// </summary>
public class DonePlayerHealth : MonoBehaviour
{
    public Single health = 100f;
    public Single resetAfterDeathTime = 5f;
    public AudioClip deathClip;

    private Animator anim;
    // player dead后 ,就不允许 player 继续 move了
    private DonePlayerMovement playerMovement;
    private DoneHashIDs hash;
    // player dead后 ,计时器 启动
    private Single timer;
    private DoneSceneFadeInOut sceneFadeInOut;
    private DoneLastPlayerSighting lastPlayerSighting;
    private Boolean playerDead;

    void Awake()
    {
        playerMovement = GetComponent<DonePlayerMovement>();
        hash = GameObject.FindGameObjectWithTag(DoneTags.gameController).GetComponent<DoneHashIDs>();
        anim = GetComponent<Animator>();
        sceneFadeInOut = GameObject.FindGameObjectWithTag(DoneTags.fader).GetComponent<DoneSceneFadeInOut>();
        lastPlayerSighting = GameObject.FindGameObjectWithTag(DoneTags.gameController).GetComponent<DoneLastPlayerSighting>();

    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0f)
        {
            if (!playerDead)
            {
                PlayerDying();
            }
            else
            {
                PlayerDead();
                LevelReset();
            }
        }
    }
    /// <summary>
    /// player 受到伤害
    /// </summary>
    public void TakeDamage(Single amount)
    {
        health -= amount;
    }

    /// <summary>
    /// player 痛苦的倒地,发出了一阵惨痛的呻吟
    /// </summary>
    void PlayerDying()
    {
        playerDead = true;
        anim.SetBool(hash.deadBool, playerDead);
        AudioSource.PlayClipAtPoint(deathClip, transform.position);
    }

    /// <summary>
    /// player 挂了,enemy 都可以走了,该解除警报了
    /// </summary>
    void PlayerDead()
    {
        // 如果 player 正处在 dying state（player 已经挂了）,重置 player的 dead参数，要不然 player会诈尸
        if (anim.GetCurrentAnimatorStateInfo(0).nameHash == hash.dyingState)
        {
            anim.SetBool(hash.deadBool, false);
        }
        // player 都要挂了，还哪来的 移动速度？自然设为 0
        anim.SetFloat(hash.speedFloat, 0f);
        playerMovement.enabled = false;
        // 重置 enemy 看到 player的位置, 即可 解除 警报
        lastPlayerSighting.position = lastPlayerSighting.resetPosition;
        // 消除 player 脚步声
        audio.Stop();
    }

    /// <summary>
    /// 重新加载场景
    /// </summary>
    void LevelReset()
    {
        timer += Time.deltaTime;
        if (timer >= resetAfterDeathTime)
        {
            sceneFadeInOut.EndScene();
        }
    }

























}
