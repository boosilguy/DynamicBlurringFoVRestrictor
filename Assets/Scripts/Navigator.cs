using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigator : MonoBehaviour {
	bool isPlayerArrived = false;
	void OnTriggerStay(Collider c) {
		if (c.tag == "Player")
			isPlayerArrived = true;
	}

	public bool GetPlayerArrived () {
		return isPlayerArrived;
	}
}
