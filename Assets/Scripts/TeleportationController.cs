using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationController : MonoBehaviour {
	[Header("Navigation effect object")]
	public GameObject navigator;
	[Space(10)]
	[Header("Detecting range")]
	public float rayLength;

	RaycastHit hit;
	// Teleportation navigator가 가르키는 위치
	Vector3 point;
	
	// Teleportation이 가능한지 상태를 알려주는 boolean 변수
	bool canIMove = true;

	///	<summary>
	/// 지정한 위치에 Teleport가 유효한지 확인하여 Navigator를 위치시키는 함수
	/// </summary>
	public void CheckTeleportRange () {
		if (Physics.Raycast (this.transform.position, this.transform.forward, out hit, rayLength))
		{
			if (hit.collider.tag == "Plane")
			{
				navigator.SetActive(true);
				MoveNavigator (hit.point);
				point = hit.point;
			}
			else
			{
				navigator.SetActive(false);
			}
		}
	}

	public Vector3 GetPoint () {
		return point;
	}

	public bool GetCanIMove () {
		return canIMove;
	}

	public void SetCanIMove (bool value) {
		canIMove = value;
	}

	void MoveNavigator (Vector3 point)
	{
		navigator.transform.position = point;
	}
}