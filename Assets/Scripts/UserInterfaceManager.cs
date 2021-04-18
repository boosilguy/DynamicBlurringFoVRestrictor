using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UserInterfaceManager : MonoBehaviour {
	[Header("Image Renderer")]
	public Image reportingImg;

	[HideInInspector]
	public Sprite expEndImg;
	FileWriterManager fileWriterManager;
	List<Sprite> VRSicknessScoreImg;
	int currentScore = 0;
	int reportCount = 0;
	bool baseLineReport = false;

	void Awake () {
		VRSicknessScoreImg = new List<Sprite>();
		fileWriterManager = (FileWriterManager) GameObject.FindObjectOfType(typeof(FileWriterManager));
		
		InitImg (21);
	}

	void Update () {
		ApplyImg (VRSicknessScoreImg[currentScore]);
	}

	public void ScaleUpScore () {
		currentScore++;
		if (currentScore > 20)
			currentScore = 20;
	}

	public void ScaleDownScore () {
		currentScore--;
		if (currentScore < 0)
			currentScore = 0;
	}

	public void ReportVRSS () {
		if (fileWriterManager.WriteVRSS(currentScore))
		{
			reportCount++;
			Debug.Log ("VR sickness score 정상 기록");
		}
			
		else
			Debug.Log ("VR sickness score 기록 실패");

		if (baseLineReport == false)
			baseLineReport = true;
	}

	public void OnRendering () {
		reportingImg.enabled = true;
	}

	public void OffRendering () {
		reportingImg.enabled = false;
	}

	public void ApplyImg (Sprite sprite) {
		reportingImg.sprite = sprite;
	}

	public bool GetBaseLineReportStatus () {
		return baseLineReport;
	}

	public int GetReportCount () {
		return reportCount;
	}
	
	///	<summary>
	/// Resources 폴더의 모든 VR sickness score report img들을 호출하여 초기화 및 종료 화면 초기화
	/// </summary>
	void InitImg (int scaleSize) {
		// 실험 종료 이미지
		expEndImg = Resources.Load<Sprite>("Others/End");

		// VR sickness score reporting 이미지
		for(int idx = 0; idx < scaleSize; idx ++) {
			VRSicknessScoreImg.Add(Resources.Load<Sprite>("VR sickness score/" + idx.ToString()));
		}
	}
}