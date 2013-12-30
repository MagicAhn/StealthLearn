using UnityEngine;
using System.Collections;
using System;

public class DoneAlarmLight : MonoBehaviour
{
		// 警报光照 渐变强度
		public Single fadeSpeed = 2f;
		public Single highIntensity = 2f;
		public Single lowIntensity = 0.5f;
		// 控制被光瞄准到的 gameobject 与周边光照强度 差距范围，来控制 gameobject上的 光照强度
		public Single changeMargin = 0.2f;
		public Boolean alarmOn;

		// 控制 alarmlight 要把强度变成多少
		private Single targetIntensity;

		void Awake ()
		{	
				// 当场景刚开始的时候, 我们希望 alarmlight的强度 就为 0
				light.intensity = 0f;
				// 当 alarmlight开始的时候,以最大的光照强度 照到目标
				targetIntensity = highIntensity;
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (alarmOn) {
						// 警报开启，alarmlight intensity 渐变为 highIntensity
						light.intensity = Mathf.Lerp (light.intensity, targetIntensity, fadeSpeed * Time.deltaTime);
						ChangeTargetIntensity ();
				} else {
						// 警报解除,alarmlight intensity 渐变为 0f
						light.intensity = Mathf.Lerp (light.intensity, 0f, fadeSpeed * Time.deltaTime);
				}
		}

		/// <summary>
		/// alarmlight 当前强度与 要达到的强度之间的差距 来控制 alarmlight要达到的强度
		/// </summary>
		void ChangeTargetIntensity ()
		{
				if (Mathf.Abs (targetIntensity - light.intensity) < changeMargin) {
						if (targetIntensity == highIntensity) {
								targetIntensity = lowIntensity;
						} else {
								targetIntensity = highIntensity;
						}
				}
		}
}
