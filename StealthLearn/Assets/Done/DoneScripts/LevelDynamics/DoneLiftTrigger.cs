using System;
using UnityEngine;
using System.Collections;

public class DoneLiftTrigger : MonoBehaviour
{
    // player 进入 lift后，关门的时间
    public Single timeToDoorClose = 2f;
    public Single timeToLiftStart = 3f;
    public Single timeToEndLevel = 6f;
    public Single liftSpeed = 3f;

    private GameObject player;
    private Animator playerAnim;
    private DoneHashIDs hash;
    private DoneCameraMovement camMovemnet;
    private DoneSceneFadeInOut sceneFadeInOut;
    private DoneLiftDoorsTracking liftDoorsTracking;
    private Boolean playerInLift;
    private Single timer;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag(DoneTags.player);
        playerAnim = player.GetComponent<Animator>();
        hash = GameObject.FindGameObjectWithTag(DoneTags.gameController).GetComponent<DoneHashIDs>();
        camMovemnet = Camera.main.GetComponent<DoneCameraMovement>();
        sceneFadeInOut = GameObject.FindGameObjectWithTag(DoneTags.fader).GetComponent<DoneSceneFadeInOut>();
        liftDoorsTracking = GetComponent<DoneLiftDoorsTracking>();

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInLift = false;
            timer = 0;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInLift = true;
        }
    }



    // Update is called once per frame
    private void Update()
    {
        if (playerInLift)
        {
            LiftActivation();
        }
        if (timer < timeToDoorClose)
        {
            liftDoorsTracking.DoorFollowing();
        }
        else
        {
            liftDoorsTracking.CloseDoors();
        }
}

    private void LiftActivation()
    {
        timer += Time.deltaTime;

        // lift 动起来
        if (timer >= timeToLiftStart)
        {
            playerAnim.SetFloat(hash.speedFloat, 0f);

            camMovemnet.enabled = false;
            // 让 player 随着 lift 一起运动
            player.transform.parent = transform;

            transform.Translate(Vector3.up * liftSpeed * Time.deltaTime);

            // 播放 audio
            if (!audio.isPlaying)
            {
                audio.Play();
            }

            // 是时间该 淡出场景了
            if (timer >= timeToEndLevel)
            {
                sceneFadeInOut.EndScene();
            }
        }
    }
}
