using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// Enemy 开枪朝 Player射击
/// </summary>
public class DoneEnemyShooting : MonoBehaviour
{
		public Single maximumDamage = 120f;
		public Single minimumDamage = 45f;
		public AudioClip shotClip;
		public Single flashIntensity = 3f;
		public Single FadeSpeed = 10f;

		private Animator anim;
		private GameObject player;
		private SphereCollider col;
		private DoneHashIDs hash;
		// 射出去的 laser line
		private LineRenderer laserShootLine;
		// 枪口冒出的火花 (fx_laserShot 的 Light Component 是 ShotLine上的 点光源)
		private Light laserShootLight;
		private Boolean shooting;
		// player 受枪击所承受的 伤害
		private Single scaledDamage;
		private DonePlayerHealth playerHealth;

		void Awake ()
		{
				anim = GetComponent<Animator> ();
				player = GameObject.FindGameObjectWithTag (DoneTags.player);
				col = GetComponent<SphereCollider> ();
				hash = GameObject.FindGameObjectWithTag (DoneTags.gameController).GetComponent<DoneHashIDs> ();
				playerHealth = GameObject.FindGameObjectWithTag (DoneTags.player).GetComponent<DonePlayerHealth> ();
				laserShootLine = GetComponentInChildren<LineRenderer> ();
				//laserShotLight = GetComponentInChildren<Light>();
				laserShootLight = laserShootLine.gameObject.light;

				scaledDamage = maximumDamage - minimumDamage;

				// 设置 line 和 light 都是off
				laserShootLine.enabled = false;
				laserShootLight.intensity = 0f;
		}

		// Use this for initialization
		void Start ()
		{

		}

		// Update is called once per frame
		void Update ()
		{
				Single shot = anim.GetFloat (hash.shotFloat);
				if (shot > 0.5f && !shooting) {
						// shoot
						Shoot ();
						laserShootLight.intensity = Mathf.Lerp (0, flashIntensity, FadeSpeed * Time.deltaTime);
				} else {
						// 停止射击后，就把 shooting 设置为 false (要不然下次射不了了)和 line.enable设置为 false，LineRenderer的光线效果就会消失
						shooting = false;
						laserShootLine.enabled = false;
				}
		}

		void Shoot ()
		{
				shooting = true;

				// 计算伤害值
				// 和距离有关(enemy 和 player 之间的距离 与 enemy collider半径的 比值)

				Single fractionalDistance = (col.radius - Vector3.Distance (transform.position, player.transform.position)) / col.radius;

				Single damage = scaledDamage * fractionalDistance + minimumDamage;
				// player 受到伤害
				playerHealth.TakeDamage (damage);

				// shot effect
				ShotEffects ();
		}

		void ShotEffects ()
		{
				// 设置 shotline 的起始点和终止点
				// 设置 shotline 的 起始点为 枪口的位置
				laserShootLine.SetPosition (0, laserShootLine.transform.position);
				laserShootLine.SetPosition (1, player.transform.position + Vector3.up * 1.5f);

				laserShootLine.enabled = true;
				laserShootLight.intensity = flashIntensity;

				AudioSource.PlayClipAtPoint (shotClip, laserShootLine.transform.position);
		}

		void OnAnimatorIK (int layerIndex)
		{
				Single aimWeight = anim.GetFloat (hash.aimWeightFloat);

				anim.SetIKPosition (AvatarIKGoal.RightHand, player.transform.position + Vector3.up * 1.5f);
				anim.SetIKPositionWeight (AvatarIKGoal.RightHand, aimWeight);
		}
}
