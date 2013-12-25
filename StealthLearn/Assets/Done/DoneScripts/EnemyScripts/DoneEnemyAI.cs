using System;
using UnityEngine;
using System.Collections;

public class DoneEnemyAI : MonoBehaviour
{
    // 当 enemy patrol时 NavMeshAgent的速度
    public Single patrolSpeed = 2f;
    // 当 enemy chase时 NavMeshAgent的速度
    public Single chaseSpeed = 5f;

    // 当 enemy patrol到一个 patrol way point时，停下来等一会
    public Single patrolWaitTime = 1f;
    // 当 enemy 看到 player后，对 player 展开 chase，脑子停顿了一会的 反应时间（last sight 不为 初始值的时候） 
    public Single chaseWaitTime = 5f;

    // enemy patrol 时，肯定需要 patrol line，patrol line 肯定有多个 way point，所以要记录 它们的 坐标
    public Transform[] patrolWayPoints;

    private NavMeshAgent nav;
    private 

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
