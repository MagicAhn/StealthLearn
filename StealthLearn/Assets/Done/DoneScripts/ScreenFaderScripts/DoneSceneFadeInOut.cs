using UnityEngine;
using System.Collections;
using System;

public class DoneSceneFadeInOut : MonoBehaviour
{

		// 屏幕 渐变到黑色 和 从黑色渐变回来 的 速度
		public Single fadeSpeed = 1.5f;
		// 场景是否还在 fading
		private Boolean startScene = true;

		void Awake ()
		{
				// 设置 texture的 位置及大小
				guiTexture.pixelInset = new Rect (0f, 0f, Screen.width, Screen.height);
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (startScene) {
						StartScene ();
				}
			
		}

		/// <summary>
		/// Starts the scene.
		/// </summary>
		void StartScene ()
		{
		}

		/// <summary>
		/// Ends the scene.
		/// </summary>
		void EndScene ()
		{
		}

		/// <summary>
		/// Fades to clear.
		/// </summary>
		void FadeToClear ()
		{
		}

		/// <summary>
		/// Fades to black.
		/// </summary>
		void FadeToBlack ()
		{
		}
}
