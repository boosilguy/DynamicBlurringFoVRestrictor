using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationManager : MonoBehaviour {
	
	public DBPlayerController dBPC;
	public GameObject playerController;
	public GameObject playerCommander;

	public Transform cam;
	Vector3 initLookRot;

	void Awake () {
		initLookRot = playerController.transform.eulerAngles;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (dBPC.interactionType == DBPlayerController.InteractionType.ControllerBased || 
			dBPC.interactionType == DBPlayerController.InteractionType.TeleportationBased ||
			dBPC.interactionType == DBPlayerController.InteractionType.MotionBased)
		{
			SyncRotation ();
		}
	}

	void SyncRotation () {
		playerCommander.transform.position = playerController.transform.position;
		playerController.transform.eulerAngles = new Vector3 (0, initLookRot.y + cam.transform.rotation.eulerAngles.y, 0);
		
	}
}