using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleManager : MonoBehaviour {
	// 종합적으로 pole들을 관리하는 poleManager
	public DBPlayerController dBPlayerController;
	public GameObject poles;
	
	[Space(10)]	
	[Header("Current active pole")]
	[SerializeField]

	GameObject currentActivePole;
	Queue<Transform> pole;

	int collidingCount = 0;
	bool readyToCollide = false;
	
	void Awake () {
		pole = new Queue<Transform>();

		foreach (Transform child in poles.transform)
		{
			pole.Enqueue(child);
		}
	}
	void Start () {
		
	}
	
	void Update () {
		// 이정표 5개 충돌마다, VRSS 등장
		if (collidingCount % 5 == 0 && readyToCollide != true)
		{
			dBPlayerController.userInterfaceManager.OnRendering ();
		}
		
		// 이정표 충돌마다, 다음 이정표 생성.
		if (pole.Count != 0 && readyToCollide != true)
		{
			currentActivePole = pole.Dequeue().gameObject;
			currentActivePole.SetActive(true);
			readyToCollide = true;
		}
		
		// 출력할 이정표가 없다면 종료.
		else if (pole.Count == 0 && readyToCollide != true)
		{
			Debug.Log("종료");
		}
	}

	public void OnReadyToCollide () {
		collidingCount ++;
		Destroy(currentActivePole);
		readyToCollide = false;
	}
}

