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

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag(DoneTags.player);
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        hash = GameObject.FindGameObjectWithTag(DoneTags.gameController).GetComponent<DoneHashIDs>();
        enemySight = GetComponent<DoneEnemySight>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
