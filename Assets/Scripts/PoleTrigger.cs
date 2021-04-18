using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleTrigger : MonoBehaviour {
	// 종합적으로 pole들을 관리하는 poleManager
	// pole 프리팹의 Component
	GameObject poleManager;

	void Start () {
		poleManager = GameObject.Find("PoleManager");
	}

	void OnTriggerEnter (Collider col) {
		if (col.tag == "Player")
		{
			poleManager.GetComponent<PoleManager>().OnReadyToCollide();
		}
	}
}
