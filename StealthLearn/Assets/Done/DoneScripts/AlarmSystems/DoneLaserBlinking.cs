using UnityEngine;
using System.Collections;
using System;

public class DoneLaserBlinking : MonoBehaviour
{
		public Single onTime;
		public Single offTime;

		private Single timer;
	
		// Update is called once per frame
		void Update ()
		{
				timer += Time.deltaTime;

				if (renderer.enabled && timer > onTime) {
						SwitchBeam ();
				}
				if (!renderer.enabled && timer > offTime) {
						SwitchBeam ();
				}
		}

		void SwitchBeam ()
		{
				// 重置 timer
				timer = 0f;

				renderer.enabled = !renderer.enabled;
				light.enabled = !light.enabled;
		}
}
