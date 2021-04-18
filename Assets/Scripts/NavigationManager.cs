using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationManager : MonoBehaviour {
	[Header("List of navigators")]
	public List<GameObject> navigatorList;
	GameObject currentDestination;
	int navigatorCount = 0;
	bool isItOff = false;

	void Awake () {
		InitNavigatorStatus ();
	}

	void Start () {

	}

	void Update () {
		if (navigatorList.Count > navigatorCount)
		{
			RenderNavigator ();
			CheckArrivedStatus (currentDestination);
		}
		else
		{
			isItOff = true;
		}
	}

	public bool GetNavigationStatus () {
		return isItOff;
	}

	public int GetNavigatorNum () {
		return navigatorCount;
	}

	public Vector3 CurrentDestPos () {
		return currentDestination.transform.position;
	}

	///	<summary>
	/// Navigation 상태 초기화
	/// </summary>
	void InitNavigatorStatus () {
		navigatorList = new List<GameObject>();

		foreach (Transform navigator in transform)
		{
			navigatorList.Add(navigator.gameObject);
			navigator.gameObject.SetActive(false);
		}

	}

	///	<summary>
	/// Navigator 렌더링
	/// </summary>
	void RenderNavigator () {
		currentDestination = navigatorList[navigatorCount];
		if (currentDestination.activeInHierarchy == false)
			currentDestination.SetActive(true);
	}

	///	<summary>
    /// 현재 Navigation 목적지의 상태 확인
    /// </summary>
    /// <param name="x">현재 Navigation 목적지</param>
	void CheckArrivedStatus (GameObject currentDestination) {
		bool arrivedStatus = currentDestination.GetComponentInChildren<Navigator>().GetPlayerArrived();

		//
		// Data 기록 관련 작성 영역
		//

		// 현재 목적지에 도달했다면, 현재 Navigator를 Off하고 도달한 Navigator count를 늘린다.
		if (arrivedStatus == true)
		{
			currentDestination.SetActive(false);
			navigatorCount ++;
		}
	}
}
