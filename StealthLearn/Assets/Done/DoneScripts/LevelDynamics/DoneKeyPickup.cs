using UnityEngine;
using System.Collections;

public class DoneKeyPickup : MonoBehaviour
{
		public AudioClip keyGrab;

		private DonePlayerInventory playerInventory;
		private GameObject player;
		
		void Awake ()
		{
				player = GameObject.FindGameObjectWithTag (DoneTags.player);
				playerInventory = player.GetComponent<DonePlayerInventory> ();
		}

		void OnTriggerEnter (Collider other)
		{
				if (other.gameObject == player) {
						AudioSource.PlayClipAtPoint (keyGrab, transform.position);
						playerInventory.hasKey = true;
						Destroy (gameObject);
				}
		}
}
