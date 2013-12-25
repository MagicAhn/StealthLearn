using System;
using UnityEngine;
using System.Collections;

public class DoneEnemyAnimation : MonoBehaviour
{
    // 不被 Mecanim所控制的 rotation的程度
    public Single deadZone = 5f;

    private Animator anim;
    private NavMeshAgent nav;
    private GameObject player;
    private DoneHashIDs hash;
    private DoneEnemySight enemySight;

    private DoneAnimatorSetup animSetup;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag(DoneTags.player);
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        hash = GameObject.FindGameObjectWithTag(DoneTags.gameController).GetComponent<DoneHashIDs>();
        enemySight = GetComponent<DoneEnemySight>();

        // 不让 NavMeshAgent 旋转，确保 enemy的旋转是由 Mecanim控制的
        nav.updateRotation = false;

        animSetup = new DoneAnimatorSetup(anim, hash);

        // 设置 shooting 和 gun层的 weight为1
        anim.SetLayerWeight(1, 1f);
        anim.SetLayerWeight(2, 1f);

        // 把 deadzone的angle 从角度 转换为 弧度
        deadZone *= Mathf.Deg2Rad;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        NavAnimSetup();
    }
    void OnAnimatorMove()
    {
        // 通过 Animator 在帧变换时 的 位置 偏差 来计算 NavMeshAgent的 Velocity 
        nav.velocity = anim.deltaPosition/Time.deltaTime;
        
        // 设置 enemy的 旋转 等同于 Animator的 旋转 
        transform.rotation = anim.rootRotation;
    }
    
    
    /// <summary>
    /// 计算角度 
    /// </summary>
    /// <param name="fromVector"></param>
    /// <param name="toVector"></param>
    /// <param name="upVector"></param>
    /// <returns></returns>
    Single FindAngle(Vector3 fromVector, Vector3 toVector, Vector3 upVector)
    {
        if (toVector == Vector3.zero)
        { return 0f; }

        Single angle = Vector3.Angle(fromVector, toVector);

        Vector3 normal = Vector3.Cross(fromVector, toVector);

        angle *= Mathf.Sign(Vector3.Dot(normal, upVector));

        angle *= Mathf.Deg2Rad;

        return angle;
    }

    void NavAnimSetup()
    {
        Single speed;
        Single angle;

        // player 在 enemy 视野范围内
        if (enemySight.playerInSight)
        {
            speed = 0;
            angle = FindAngle(transform.forward, player.transform.position - transform.forward, transform.up);
        }
        else
        {
            speed = Vector3.Project(nav.desiredVelocity, transform.forward).magnitude;
            angle = FindAngle(transform.forward, nav.desiredVelocity, transform.up);

            if (Mathf.Abs(angle) < deadZone)
            {
                transform.LookAt(transform.position + nav.desiredVelocity);
                angle = 0;
            }
        }

        animSetup.Setup(speed, angle);
    }
}
