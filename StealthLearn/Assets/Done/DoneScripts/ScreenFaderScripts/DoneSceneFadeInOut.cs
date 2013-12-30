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
				// 游戏开始， 将场景颜色慢慢地 淡去
				FadeToClear ();
				// 当场景 慢慢淡去的差不多的时候
				if (guiTexture.color.a < 0.5f) {
						// 把 guiTexture的颜色设置为 Color.clear,并且让GuiTexture 不可用
						guiTexture.color = Color.clear;
						guiTexture.enabled = false;
						// 场景已经加载完毕，所以 把 startScene 设置为 false，没有必要再加载了
						startScene = false;
				}
		}

		/// <summary>
		/// Ends the scene.
		/// </summary>
		public void EndScene ()
		{
				// 游戏结束，重新加载场景
				// GuiTexture可用了
				guiTexture.enabled = true;
				// 场景可用后，即可以让其慢慢 变黑
				FadeToBlack ();
				// 黑到差不多的时候，重新加载场景
				if (guiTexture.color.a > 0.95f) {
						Application.LoadLevel (0);
				}
		}

		/// <summary>
		/// Fades to clear.
		/// </summary>
		void FadeToClear ()
		{
				guiTexture.color = Color.Lerp (guiTexture.color, Color.clear, fadeSpeed * Time.deltaTime);
		}

		/// <summary>
		/// Fades to black.
		/// </summary>
		void FadeToBlack ()
		{
				guiTexture.color = Color.Lerp (guiTexture.color, Color.black, fadeSpeed * Time.deltaTime);
		}
}
