using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReporterManager : MonoBehaviour {
	public bool timeBased;
	public int sessionPerReport;

	[HideInInspector]
	public bool reportVRSS;
	DBPlayerController playerController;
	UserInterfaceManager userInterfaceManager;
	float remain = 60f;
	
	void Awake () {
		playerController = GameObject.FindObjectOfType<DBPlayerController>().GetComponent<DBPlayerController>();
		userInterfaceManager = GameObject.FindObjectOfType<UserInterfaceManager>().GetComponent<UserInterfaceManager>();
	}
		
	// Update is called once per frame
	void Update () {
		// 분 당 리포팅
		if (userInterfaceManager.GetBaseLineReportStatus() && timeBased == true)
		{
			if (playerController.navTime > remain)
			{
				userInterfaceManager.OnRendering();
				remain += 60f;
			}
			
		}

		// 구간 별 리포팅
		else if (userInterfaceManager.GetBaseLineReportStatus() && timeBased == false)
		{
			int navNum = playerController.navigationManager.GetNavigatorNum();
			int reportCount = userInterfaceManager.GetReportCount();
			if (navNum % sessionPerReport == 0 && navNum != 0 && navNum/sessionPerReport == reportCount)
				userInterfaceManager.OnRendering();
		}
	}
}
