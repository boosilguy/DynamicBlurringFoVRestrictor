using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

[RequireComponent(typeof(ParticipantsManager))]
public class DBPlayerController : MonoBehaviour {
	
	// Locomotion type
	public enum InteractionType {
		ControllerBased,
		TeleportationBased,
		MotionBased,
		RoomscaleBased
	}

	[Header("Locomotion Type")]
	public InteractionType interactionType;
	
	[Space(10)]
	[Header("Controller Setup")]
	public SteamVR_Input_Sources handType;
	public SteamVR_Action_Vector2 touchPadPos;
	public SteamVR_Action_Boolean touchPadButton;
	public SteamVR_Action_Boolean gunTriggerButton;
	
	[Space(10)]
	public UserInterfaceManager userInterfaceManager;
	public NavigationManager navigationManager;
	public FileWriterManager fileWriterManager;
	public CharacterController playerController;
	public GameObject teleportationController;
	public GameObject headMountedDisplay;
	public GameObject playerCommander;
	public GameObject waitingImg;

    [Space(10)]
    public Transform displayPos;

    [Space(10)]
	public float maximumSpeed = 1.5f;
	
	Vector3 playerDirection;
	WalkingInPlace walkingInPlace;
	List<string> generalDataList;
	
	// MotionBased parameters
	Vector3 movement = Vector3.zero;
	Vector3 dir = Vector3.zero;
	float height;
	float sBottomPeak = 0;		// 현재 Bottom peak
	float sTopPeak = 0;			// 현재 Top peak
	float sDif = 0;				// 현재 인식된 Walking in place의 s 차이
	float iDif = 0;				// 현재 Step interval
	float firstBottomPeakTime = 0;
	float SecondBottomPeakTime = 0;
	float motionBasedVelocity = 0;
	int stepCount = 0;
	bool isItCalibrated = false;

	// Controller trackpad
	float touchPadToWorldX;
	float touchPadToWorldZ;

	// System variable
	float wholeTime = 0;
	[HideInInspector]
	public float navTime = 0;
	Vector3 participantsPos = Vector3.zero;
	Vector3 participantsHeadRot = Vector3.zero;
	int currentNavigator = 1;
	Vector3 currentNavigatorPos = Vector3.zero;
	[SerializeField]
	float slopeForceRayLength;
	[SerializeField]
	float slopeForce;
	
	/*
	20.5.14
	- Teleportation Logic은 모두 주석 처리 및 모션캡처 툴 주석 처리. 향후 이 부분은 삭제될 예정.
	- 두 개의 Navigation map 구성 (Roomscale과 그렇지 않는 경우)
	*/

	///	<summary>
	/// 실험을 종료하는 함수
	/// </summary>
	public void SetExpEnd ()
	{
		userInterfaceManager.ApplyImg(userInterfaceManager.expEndImg);
		userInterfaceManager.OnRendering ();
	}

	void Awake () {
		generalDataList = new List<string>();
		
		GameObject normalNavMap = GameObject.Find("NavigatorManager (Normal)");
		GameObject roomNavMap = GameObject.Find("NavigatorManager (Roomscale)");

		waitingImg.SetActive(false);

		if (interactionType != InteractionType.RoomscaleBased)
        {
			navigationManager = normalNavMap.GetComponent<NavigationManager>();
			roomNavMap.SetActive(false);
        }

		// RoomscaleBased 경우의 초기화 로직
        else
        {
			navigationManager = roomNavMap.GetComponent<NavigationManager>();
			normalNavMap.SetActive(false);
        }

        // TeleportationBased 경우의 초기화 로직
        if (interactionType == InteractionType.TeleportationBased)
		{
			teleportationController.GetComponent<LineRenderer>().enabled = true;
		}

		// 그 외 경우의 초기화 로직
		else
		{
			// 그 중, MotionBased 경우의 초기화 로직
			if (interactionType == InteractionType.MotionBased)
			{
				walkingInPlace = new WalkingInPlace(3, 5);
				waitingImg.SetActive(true);
			}
			
			teleportationController.GetComponent<LineRenderer>().enabled = false;
		}
	}

	void Start () {
		
	}

	void Update () {
		if (!navigationManager.GetNavigationStatus())
		{
			if (userInterfaceManager.GetBaseLineReportStatus())
			{
				wholeTime += Time.deltaTime;
				participantsPos = headMountedDisplay.transform.position;
				participantsHeadRot = headMountedDisplay.transform.rotation.eulerAngles; // we need to modify this
				currentNavigatorPos = navigationManager.CurrentDestPos();

				fileWriterManager.WriteGeneral (CollectGeneralData(participantsPos, participantsHeadRot, currentNavigatorPos));
				generalDataList.Clear();
			}

			// Reset HMD pose
			if (Input.GetKeyDown("r"))
				RecenterHMD();

			// Controller based interaction 사용시
			if (interactionType == InteractionType.ControllerBased) {
				if (GetTouchPadDown() && !userInterfaceManager.reportingImg.enabled) {
					navTime += Time.deltaTime;

					touchPadToWorldX = GetTouchPadPos().x;
					touchPadToWorldZ = GetTouchPadPos().y;
					
					UpdatePlayerController (touchPadToWorldX, touchPadToWorldZ);
				}

				else if (GetTouchPadDownOnce() && userInterfaceManager.reportingImg.enabled) {
					touchPadToWorldX = GetTouchPadPos().x;
					UpdateReportController (touchPadToWorldX);
				}

				else if (GetGunTriggerDownOnce() && userInterfaceManager.reportingImg.enabled) {
					userInterfaceManager.ReportVRSS ();
					userInterfaceManager.OffRendering ();
				}

				else if (!userInterfaceManager.reportingImg.enabled)
				{
					navTime += Time.deltaTime;
					UpdatePlayerController (0, 0);
				}
			}

			// Teleportation based interaction 사용시
			else if (interactionType == InteractionType.TeleportationBased) {
				TeleportationController tpControllerScript = teleportationController.GetComponent<TeleportationController>();
				tpControllerScript.CheckTeleportRange();
				
				if (GetGunTriggerDownOnce() && !userInterfaceManager.reportingImg.enabled) {
					if (tpControllerScript.GetCanIMove())
					{
						navTime += Time.deltaTime;

						playerDirection = tpControllerScript.GetPoint();
						tpControllerScript.SetCanIMove(false);
					}
						
				}
				else if (GetTouchPadDownOnce() && userInterfaceManager.reportingImg.enabled) {
					touchPadToWorldX = GetTouchPadPos().x;
					UpdateReportController (touchPadToWorldX);
				}

				else if (GetGunTriggerDownOnce() && userInterfaceManager.reportingImg.enabled) {
					userInterfaceManager.ReportVRSS ();
					userInterfaceManager.OffRendering ();
				}

				else if (!userInterfaceManager.reportingImg.enabled)
				{
					navTime += Time.deltaTime;
				}

				if (!tpControllerScript.GetCanIMove())
					UpdatePlayerController(playerDirection);
			}

			// Motion based interaction 사용시
			else if (interactionType == InteractionType.MotionBased) {
				if (Input.GetKeyDown("c"))
				{
					isItCalibrated = MotionBasedCalibration();
					waitingImg.SetActive(false);
					Debug.Log("Done!");
				}
					
				
				if (isItCalibrated == true)
				{
					walkingInPlace.eyeLevelList.Add(headMountedDisplay.transform.localPosition.y);
					
					if (walkingInPlace.eyeLevelList.Count == walkingInPlace.listSize)
					{
						float avgEyeLevel = walkingInPlace.GetAverageEyeLevel();
						walkingInPlace.ReleaseListComponent();
						walkingInPlace.queue.Enqueue(avgEyeLevel);
					}

					if (walkingInPlace.queue.Count == walkingInPlace.queueSize)
					{
						UpdateMotionBasedStatus(walkingInPlace.queue);
						walkingInPlace.queue.Dequeue();
					}
					if (!userInterfaceManager.reportingImg.enabled)
						navTime += Time.deltaTime;

					UpdatePlayerController();

					if (GetTouchPadDownOnce() && userInterfaceManager.reportingImg.enabled) {
						touchPadToWorldX = GetTouchPadPos().x;
						UpdateReportController (touchPadToWorldX);
					}

					else if (GetGunTriggerDownOnce() && userInterfaceManager.reportingImg.enabled) {
						userInterfaceManager.ReportVRSS ();
						userInterfaceManager.OffRendering ();
					}
				}
			}

			// Roomsacle based interaction 사용시
			else if (interactionType == InteractionType.RoomscaleBased) {
				float commanderXPos = playerController.transform.position.x;
				float commanderZPos = playerController.transform.position.z;
				playerCommander.transform.position = new Vector3 (commanderXPos, 0, commanderZPos);

				if (!userInterfaceManager.reportingImg.enabled)
					navTime += Time.deltaTime;

				if (GetTouchPadDownOnce() && userInterfaceManager.reportingImg.enabled) {
					touchPadToWorldX = GetTouchPadPos().x;
					UpdateReportController (touchPadToWorldX);
				}

				else if (GetGunTriggerDownOnce() && userInterfaceManager.reportingImg.enabled) {
					userInterfaceManager.ReportVRSS ();
					userInterfaceManager.OffRendering ();
				}
			}
		}
		else
		{
			SetExpEnd();
		}
		
	}

    ///	<summary>
    /// 플레이어 이동을 최종 결정하여, 반영하는 함수 (Controller-based 전용).
    /// </summary>
    /// <param name="x">트랙 패드 x 변수</param>
    /// <param name="z">트랙 패드 y 변수</param>
    void UpdatePlayerController (float x, float z)
	{
		playerDirection = new Vector3 (x, 0, z);
		// Local dir로 전환
		playerDirection = playerController.transform.TransformDirection(playerDirection);
		playerDirection = playerDirection * maximumSpeed;

		if (OnSlope())
			playerDirection += Vector3.down * playerController.height / 2 * slopeForce;
		
		playerController.Move(playerDirection * Time.deltaTime);
	}

	///	<summary>
	/// 플레이어 이동을 최종 결정하여, 반영하는 함수 (Teleportation-based 전용).
	/// </summary>
	/// <param name="point">Teleport할 Vector3 위치</param>
	void UpdatePlayerController (Vector3 point)
	{
		Vector3 offset = point - playerController.transform.position;
		Vector2 offsetCalculate = new Vector2 (point.x, point.z) - new Vector2 (playerController.transform.position.x, playerController.transform.position.z);
		
		if (offsetCalculate.magnitude > 1f)
		{
			offset = offset.normalized * maximumSpeed;
			playerController.Move(offset * Time.deltaTime);
		}
		else
		{
			TeleportationController tpControllerScript = teleportationController.GetComponent<TeleportationController>();
			tpControllerScript.SetCanIMove(true);
		}
	}

	///	<summary>
	/// 플레이어 이동을 최종 결정하여, 반영하는 함수 (Motion-based 전용).
	/// </summary>
	void UpdatePlayerController ()
	{
		dir = playerController.transform.TransformDirection(Vector3.forward);
		movement = dir * Time.deltaTime * motionBasedVelocity;

		if (movement != Vector3.zero)
		{
			if (OnSlope())
				movement += Vector3.down * playerController.height / 2 * slopeForce * Time.deltaTime;

			playerController.Move(movement);	
		}
		
		motionBasedVelocity -= 0.05f;

		if (motionBasedVelocity <= 0)
			motionBasedVelocity = 0;
	}

	///	<summary>
	/// ParticipantsManager 기반으로 Calibration 진행하는 함수 (Motion-based 전용).
	/// </summary>
	bool MotionBasedCalibration () {
		
		height = this.GetComponent<ParticipantsManager>().height / 100;
		
		return true;
	}

	///	<summary>
	/// Walking In Place 감지 메소드 (Motion-based 전용).
	/// </summary>
	/// <param name="q">WIP의 상태를 저장하는 Queue (프레임 별 Head y pos)</param>
	void UpdateMotionBasedStatus (Queue<float> q) {
		
		float smallest = walkingInPlace.GetBottomPeak(q);
		float largest = walkingInPlace.GetTopPeak(q);
		
		if (smallest != 0)
		{
			if (this.GetComponent<ParticipantsManager>().playType == ParticipantsManager.PlayType.Debug)
				Debug.Log ("WIP Recognition!");
			
			sBottomPeak = smallest;
			sDif = (float)((sTopPeak - sBottomPeak));

			firstBottomPeakTime = Time.time;
			if (SecondBottomPeakTime != 0)
			{
				float tmp = firstBottomPeakTime;
				iDif = firstBottomPeakTime - SecondBottomPeakTime;
				SecondBottomPeakTime = tmp;
			}
			else
			{
				SecondBottomPeakTime = firstBottomPeakTime;
			}

			

			if (sDif >= walkingInPlace.sMin && sDif <= walkingInPlace.sMax)
			{
				if (iDif >= walkingInPlace.iMin && iDif <= walkingInPlace.iMax)
				{
					stepCount++;
					motionBasedVelocity = Mathf.Lerp(walkingInPlace.velocityMin, walkingInPlace.velocityMax, sDif / (walkingInPlace.sMax - walkingInPlace.sMin));
				}
				else
				{
					if (this.GetComponent<ParticipantsManager>().playType == ParticipantsManager.PlayType.Debug)
						Debug.Log ("고개 움직임이 너무 작거나 큼!");
				}
			}
			else
			{
				if (this.GetComponent<ParticipantsManager>().playType == ParticipantsManager.PlayType.Debug)
					Debug.Log ("스텝 간격이 너무 좁거나 큼!");
			}
		}
		else
		{
			if (largest != 0)
			{
				if (this.GetComponent<ParticipantsManager>().playType == ParticipantsManager.PlayType.Debug)
					Debug.Log ("This central datum is Top peak!");
				
				sTopPeak = largest;
			}
			else
			{
				if (this.GetComponent<ParticipantsManager>().playType == ParticipantsManager.PlayType.Debug)
					Debug.Log ("Non-WIP Recognition!");
			}
		}
	}

	///	<summary>
	/// 가상 멀미 스코어를 리포트하는 함수.
	/// </summary>
	/// <param name="touchPadX">터치패드 X 위치</param>
	void UpdateReportController (float touchPadX)
	{
		if (touchPadX > 0)
			userInterfaceManager.ScaleUpScore ();
		else
			userInterfaceManager.ScaleDownScore ();
	}

	///	<summary>
	/// General Data 수집 함수.
	/// </summary>
	/// <param name="partPos">Participant 위치</param>
	/// <param name="partHeadRot">Participant 회전</param>
	/// <param name="currentNavPos">현재 목적지 위치</param>
	List<string> CollectGeneralData (Vector3 partPos, Vector3 partHeadRot, Vector3 currentNavPos)
	{
		Vector2 part2DPos = new Vector2(partPos.x, partPos.z);
		Vector2 nav2DPos = new Vector2(currentNavPos.x, currentNavPos.z);
		float remainDistance = (partPos - currentNavPos).magnitude;
		int navIdx = currentNavigator + navigationManager.GetNavigatorNum();
		
		List<string> dataList = new List<string>();

		dataList.Add(wholeTime.ToString());
		dataList.Add(navTime.ToString());
		dataList.Add(partPos.x.ToString());
		dataList.Add(partPos.y.ToString());
		dataList.Add(partPos.z.ToString());
		dataList.Add((partHeadRot.x - 180).ToString());
		dataList.Add((partHeadRot.y - 180).ToString());
		dataList.Add((partHeadRot.z - 180).ToString());
		dataList.Add(navIdx.ToString());
		dataList.Add((part2DPos - nav2DPos).magnitude.ToString());
		
		return dataList;
	}

	Vector2 GetTouchPadPos () {
		return touchPadPos.GetAxis (handType);
	}

	bool GetTouchPadDown () {
		return touchPadButton.GetState (handType);
	}

	bool GetTouchPadDownOnce () {
		return touchPadButton.GetLastStateDown (handType);
	}

	bool GetGunTriggerDownOnce () {
		return gunTriggerButton.GetLastStateDown (handType);
	}

	void RecenterHMD () {
		UnityEngine.XR.InputTracking.Recenter();
	}

	bool OnSlope () {
		RaycastHit hit;

		if (Physics.Raycast(playerController.transform.position, Vector3.down, out hit, playerController.height / 2 * slopeForceRayLength))
			if (hit.normal != Vector3.up)
				return true;
		
		return false;
	}
}
