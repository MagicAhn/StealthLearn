using UnityEngine;
using System.Collections;
using System;

public class DoneEnemySight : MonoBehaviour
{
		// Enemy 看前方的视角
		public Single fieldOfViewAngle = 110f;
		// player 是否在 Enemy 的视野内
		public Boolean playerInSight;
		// 当前Enemy 上次看到 player的地点
		public Vector3 personalLastSighting;

		private NavMeshAgent nav;
		private SphereCollider col;
		private GameObject player;
		private Animator anim;
		private Animator playerAnim;
		private DoneHashIDs hash;
		private DoneLastPlayerSighting lastPlayerSighting;

		// 记录 player 在上一帧被 enemy发现时的 位置（这是一个 global position）
		private Vector3 previousPosition;

		void Awake ()
		{
				nav = GetComponent<NavMeshAgent> ();
				player = GameObject.FindGameObjectWithTag (DoneTags.player);
				col = GetComponent<SphereCollider> ();
				anim = GetComponent<Animator> ();
				playerAnim = player.GetComponent<Animator> ();
				hash = GameObject.FindGameObjectWithTag (DoneTags.gameController).GetComponent<DoneHashIDs> ();
				lastPlayerSighting =
            GameObject.FindGameObjectWithTag (DoneTags.gameController).GetComponent<DoneLastPlayerSighting> ();
				personalLastSighting = lastPlayerSighting.resetPosition;
				previousPosition = lastPlayerSighting.resetPosition;
		}

		// Use this for initialization
		void Start ()
		{
				// 
		}

		// Update is called once per frame
		void Update ()
		{
				// 如果 player 被 所有enemy所确定的global position 发生了变化( 不等同于 previousPosition)
				if (lastPlayerSighting.position != previousPosition) {
						// 先记录 personalLastSighting
						personalLastSighting = lastPlayerSighting.position;
				}
				// 把 global position -- lastPlayerSighting.position 记录到 previousPosition
				previousPosition = lastPlayerSighting.position;

				// 判断 player 是否还 alive
		}

		// 如果 player 在 enemy的 周围一定的范围中
		void OnTriggerStay (Collider other)
		{
				if (other.gameObject == player) {
						// 默认 playerInSight 为 false
						playerInSight = false;

						// 计算 player 是否在 enemy的视野范围内
						Vector3 direction = other.transform.position - transform.position;
						Single angle = Vector3.Angle (transform.forward, direction);
						// player 在 enemy 的视野范围内
						if (angle < fieldOfViewAngle * 0.5f) {
								// 因为 player 和 enemy 之间可能隔了一道墙，但是 player还是在 enemy 身边的这个范围内。。。这就需要从 Enemy 发射 ray 来判断了 
								// 是否能看到 player了
								RaycastHit raycastHit;
								if (Physics.Raycast (transform.position + transform.up, direction, out raycastHit, col.radius)) {
										if (raycastHit.collider.gameObject == player) {
												// player 在 enemy的 视野范围内
												playerInSight = true;
												// 并设置 player 当前的位置 为 上一次被看到的位置（所有 enemy 都知道这个位置，而不是 只被一个 
												// enemy 知道的位置）
												lastPlayerSighting.position = player.transform.position;
										}
								}
								// 如果 player正处在 locomotion状态 或者 这在吸引 注意力
								Int32 playerLayerZeroStateHash = playerAnim.GetCurrentAnimatorStateInfo (0).nameHash;
								Int32 playerLayerOneStateHash = playerAnim.GetCurrentAnimatorStateInfo (1).nameHash;
								if (playerLayerZeroStateHash == hash.locomotionState || playerLayerOneStateHash == hash.shoutState) {
										// 并且 player在 听觉范围内，则 记录player当前位置
										if (CalculatePathLength (player.transform.position) <= col.radius) {
												personalLastSighting = player.transform.position;
										}
								}
						}
				}
		}

		void OnTriggerExit (Collider other)
		{
				if (other.gameObject == player) {
						playerInSight = false;
				}
		}

		/// <summary>
		/// 计算 GameObject 当前位置 到 targetPosition的 路程长度
		/// 通过 计算得到的 Path上 总共有多少个 corner， 相邻corner之间的 距离总和 即为得到的 path的长度
		/// </summary>
		/// <returns>The path length.</returns>
		/// <param name="targetPosition">Target position.</param>
		Single CalculatePathLength (Vector3 targetPosition)
		{
				NavMeshPath path = new NavMeshPath ();
				if (nav.enabled) {
						nav.CalculatePath (targetPosition, path);
				}

				Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];
				allWayPoints [0] = transform.position;
				allWayPoints [allWayPoints.Length - 1] = player.transform.position;
				for (int i = 0; i < path.corners.Length; i++) {
						allWayPoints [i + 1] = path.corners [i];
				}
				Single pathLength = 0;
				for (int i = 0; i < allWayPoints.Length - 1; i++) {
						pathLength += Vector3.Distance (allWayPoints [i], allWayPoints [i + 1]);
				}
				return pathLength;
		}
}

























