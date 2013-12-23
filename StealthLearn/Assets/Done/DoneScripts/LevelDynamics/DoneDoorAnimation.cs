using UnityEngine;
using System.Collections;
using System;

public class DoneDoorAnimation : MonoBehaviour
{
		// Door 是否需要 key才能打开
		public Boolean requireKey;
		// 打开/关闭 Door时的声音
		public AudioClip doorSwishClip;
		// Player 没有打开 这扇 Door 时的声音
		public AudioClip accessDeniedClip;

		// Door 的 Animator
		private Animator anim;
		private DoneHashIDs hash;
		private GameObject player;
		private DonePlayerInventory playerInventory;
		// 用来表示 与Door的 Collider 相撞的 Collider的数量
		private Int32 count;

		void Awake ()
		{
				anim = gameObject.GetComponent<Animator> ();
				player = GameObject.FindGameObjectWithTag (DoneTags.player);
				hash = GameObject.FindGameObjectWithTag (DoneTags.gameController).GetComponent<DoneHashIDs> ();
				playerInventory = player.GetComponent<DonePlayerInventory> ();
		}

		// Use this for initialization
		void Start ()
		{

		}

		void OnTriggerEnter (Collider other)
		{
				// 如果 检测到的 Collider 是 player
				if (other.gameObject == player) {
						// 需要 key才能打开 Door
						if (requireKey) {

								if (playerInventory.hasKey) {
										count++;
								} else {
										audio.clip = accessDeniedClip;
										if (!audio.isPlaying) {
												audio.Play ();
										}	
								}
						} else {
								count++;
						}
				} else if (other.gameObject.tag.Equals (DoneTags.enemy)) {
						if (other is CapsuleCollider) {
								count++;
						}
				}
		}

		void OnTriggerExit (Collider other)
		{
				// 当从 Collider 离开的是 player 或者 enemy的时候，并且这个 Collider是 CapsuleCollider，就将 count -1，
				// 让 Door 关闭
				if (other.gameObject == player || other.gameObject.tag == DoneTags.enemy || other is CapsuleCollider) {
						count = Mathf.Max (0, count - 1);
				}
		}

		// Update is called once per frame
		void Update ()
		{
				// 控制 Door的animator 播放
				anim.SetBool (hash.openBool, count > 0);
				if (anim.IsInTransition (0) && !audio.isPlaying) {
						audio.clip = doorSwishClip;
						audio.Play ();
				}
		}
}
