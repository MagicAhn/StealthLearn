using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// Enemy AI 包含 patrol chase 和 stop to shot
/// </summary>
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
		private GameObject player;
		private Single patrolTimer;
		private Single chaseTimer;
		// patrol way point 的 index
		private Int32 wayPointIndex;
		private DoneLastPlayerSighting lastPlayerSighting;
		private DoneEnemySight enemySight;

		void Awake ()
		{
				nav = GetComponent<NavMeshAgent> ();
				player = GameObject.FindGameObjectWithTag (DoneTags.player);
				lastPlayerSighting = GetComponent<DoneLastPlayerSighting> ();
				enemySight = GetComponent<DoneEnemySight> ();
		}
		
		// Use this for initialization
		void Start ()
		{

		}

		// Update is called once per frame
		void Update ()
		{
				// 如果 player is alive 并且在 sight中,shot
				if (enemySight.playerInSight) {
						Shooting ();
				} else if (enemySight.personalLastSighting != lastPlayerSighting.resetPosition) {
						// 如果 enemy 知道了 player的位置，并且 player is alive,chase
						Chasing ();
				} else {
						// enemy 不知道 player 的位置，就 patrol
						Patrolling ();
				}
		}

		/// <summary>
		/// Enemy 停下来才能 射击
		/// </summary>
		void Shooting ()
		{
				// Enemy 是通过 NavMeshAgent来移动的，所以将 nav stop
				nav.Stop ();
		}

		/// <summary>
		/// Enemy patrol
		/// </summary>
		void Patrolling ()
		{
				// 设置 enemy Patrol 时的速度
				nav.speed = patrolSpeed;

				// 如果 enemy 到达了 下一个 way point，或者是 没有目标位置了，enemy 就会在那里站一会
				if (nav.destination == lastPlayerSighting.resetPosition || nav.remainingDistance < nav.stoppingDistance) {
						// 开始计时
						patrolTimer += Time.deltaTime;
						// patrol 到一定的时间后，就开始去下一个 way point 巡逻了
						// 首先 需要设置 way point
						if (patrolTimer >= patrolWaitTime) {
								if (wayPointIndex >= patrolWayPoints.Length - 1) {
										wayPointIndex = 0;
								} else {
										wayPointIndex ++;
								}
								patrolTimer = 0f;
						}
				} else {
						patrolTimer = 0f;
				}

				// 设置下一个 patrol way point 为 enemy 的 目标位置（distinction）
				nav.destination = patrolWayPoints [wayPointIndex].position;
		}
		
		void Chasing ()
		{
				// 计算 enemy 到 player 被发现时的位置 的 距离
				Vector3 sightingDeltaPos = enemySight.personalLastSighting - transform.position;

				// 如果 enemy和player被发现时的位置 不是很接近到 4f,则让 enemy 跑到 personalLastSighting
				if (sightingDeltaPos.sqrMagnitude > 4f) {
						nav.destination = enemySight.personalLastSighting;
				}

				// 设置 enemy chase时的 速度
				nav.speed = chaseSpeed;

				if (nav.remainingDistance < nav.stoppingDistance) {
						// 开始计时 
						chaseTimer += Time.deltaTime;
						// chase 到一定的时间后，就放弃 chase 了
						// 重置 last global sight 和 personalLastSighting
						if (chaseTimer >= chaseWaitTime) {
								lastPlayerSighting.position = lastPlayerSighting.resetPosition;
								enemySight.personalLastSighting = lastPlayerSighting.position;
								chaseTimer = 0f;
						}
				} else {
						chaseTimer = 0f;
				}
		}



















}
