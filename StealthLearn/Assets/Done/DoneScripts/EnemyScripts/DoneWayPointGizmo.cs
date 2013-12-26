using UnityEngine;
using System.Collections;

/// <summary>
/// 让我们 把小旗插到 地球的每个角落。哈哈哈哈
/// </summary>
public class DoneWayPointGizmo : MonoBehaviour
{
		void OnDrawGizmos ()
		{
				Gizmos.DrawIcon (transform.position, "wayPoint.png", true);
		}
}
