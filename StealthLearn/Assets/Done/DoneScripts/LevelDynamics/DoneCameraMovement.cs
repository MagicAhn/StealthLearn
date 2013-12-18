using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// Camera 跟随 Player移动的逻辑
/// </summary>
public class DoneCameraMovement : MonoBehaviour
{
    // 思考 Camera 跟随 Player移动的 过程中，我们需要定义哪些变量？？？
    // 移动的 平化程度
    // Camera 是相对于 Player而运动的，所以要得到 两者之间 位置上的 差的 向量,这就还需要 知道 Player的位置
    // Camera 需要旋转

    public Single smooth = 1.5f;

    private Transform player;
    // camera 的相对位置
    private Vector3 relCameraPos;
    private Single relCameraPosMag;
    // camera 试图出现在这个位置
    private Vector3 newPos;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag(DoneTags.player).transform;
        relCameraPos = transform.position - player.position;
        relCameraPosMag = relCameraPos.magnitude - 0.5f;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 从 5个点 判断 在其位置能否 跟踪到 Player，只要有一个点可以 跟踪到 Player,那么，这一帧 camera 的位置就是在这个点
    /// </summary>
    void FixedUpdate()
    {
        // 两个边界为之 相当于 以 Player.position 化的一个圆弧的两个顶点
        // 边界位置 （camera的标准位置）
        Vector3 standardPos = player.position + relCameraPos;
        // 边界位置 （Player 正上方的位置）
        Vector3 abovePos = player.position + Vector3.up * relCameraPosMag;

        Vector3[] checkPoints = new Vector3[5];
        checkPoints[0] = standardPos;
        checkPoints[1] = Vector3.Lerp(standardPos, abovePos, 0.25f);
        checkPoints[2] = Vector3.Lerp(standardPos, abovePos, 0.5f);
        checkPoints[3] = Vector3.Lerp(standardPos, abovePos, 0.75f);
        checkPoints[4] = abovePos;

        for (int i = 0; i < checkPoints.Length; i++)
        {
            if (ViewingPosCheck(checkPoints[i]))
            {
                break;
            }
        }

        transform.position = Vector3.Lerp(standardPos, newPos, smooth * Time.deltaTime);

        // 确保 camera 始终是 跟踪 Player的
        SmoothLookAt();
    }

    /// <summary>
    ///  判断 在这个点 Camera 是否还能照到 Player
    /// </summary>
    /// <param name="checkPos"></param>
    /// <returns></returns>
    Boolean ViewingPosCheck(Vector3 checkPos)
    {
        // 从 一个点 向 另一点 发射 ray，如果碰到东西（从 camera 向 player 发射ray）
        RaycastHit raycastHit;
        if (Physics.Raycast(checkPos, player.position - checkPos, out raycastHit, relCameraPosMag))
        {
            // 被击中的 rigidbody或collider的 transform 不是 player（Player的transform）
            if (raycastHit.transform != player)
            {
                // 表示这个点 不对，不能跟踪到 Player
                return false;
            }
            else
            {
                // 如果 击中 Player
                newPos = checkPos;
                return true;
            }
        }
        else
        {
            // 如果没有 击中 Player
            newPos = checkPos;
            return true;
        }
    }

    /// <summary>
    /// Camera 平滑的移动
    /// </summary>
    void SmoothLookAt()
    {
        // Player 相对 Camera的 位置（视为从 camera这个点出发）
        Vector3 relPlayerPos = player.position - transform.position;
        Quaternion lookAtRotation = Quaternion.LookRotation(relPlayerPos, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookAtRotation, Time.deltaTime * smooth);
        // 因为 不需要 不间断的rotate，所以不必 Rigidbody.MoveRotation
    }
}
