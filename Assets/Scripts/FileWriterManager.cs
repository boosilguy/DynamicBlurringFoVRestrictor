using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class FileWriterManager : MonoBehaviour {
	
	// 저장 경로는 바탕화면 (Desktop)
	// 바탕화면에 Exp 폴더 생성 후, Participant's identity에 맞게 분류.
	[Header("Directory (root : Desktop")]
	public string path = "/Exp/";
	ParticipantsManager participantsManager;
	DBPlayerController dBPlayerController;
	Stream generalDataStream;
	Stream vrssDataStream;
	StreamWriter generalWriter;
	StreamWriter vrssWriter;
	string vrss;
	string general;
	bool initVrss = false;
	bool initGeneral = false;

	// 옳게 저장 경로 및 데이터 파일이 저장되었는지 확인.
	bool dirStatus = false;
	
	void Awake () {
		participantsManager = (ParticipantsManager) GameObject.FindObjectOfType(typeof(ParticipantsManager));
		dBPlayerController = (DBPlayerController) GameObject.FindObjectOfType(typeof(DBPlayerController));
	}
	void Start () {
		dirStatus = MakeDataDir ();
	}

	void OnApplicationQuit () {
		if (generalDataStream.CanRead)
			generalDataStream.Close();
		if (vrssDataStream.CanRead)
			vrssDataStream.Close();
	}
	
	public bool MakeDataDir () {
		try {
			string fileName = participantsManager.GetExpDate() + " " + dBPlayerController.interactionType.ToString();
			string format = ".csv";
			path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + path + participantsManager.GetParticipantsInfo();
			Directory.CreateDirectory (path);

			string nowTime = DateTime.Now.ToString("HHmmss");

			// Dynamic blurring이 적용된지 확인
			if (dBPlayerController.headMountedDisplay.GetComponent<DynamicBlurring>().enabled == true)
			{
				general = path + "/DB " + fileName + " (" + nowTime + ")" + format;
				vrss = path + "/DB " + fileName + " VRSS (" + nowTime + ")" + format;
			}
			else
			{
				general = path + "/NB " + fileName + " (" + nowTime + ")" + format;
				vrss = path + "/NB " + fileName + " VRSS (" + nowTime + ")" + format;
			}
			
			// 파일 쓰기를 위한 스트림 형성
			generalDataStream = File.Create(general);
			vrssDataStream = File.Create(vrss);
			generalWriter = new StreamWriter(generalDataStream);
			vrssWriter = new StreamWriter(vrssDataStream);

			return true;
		} catch (System.Exception e) {
			Debug.LogAssertion ("경고! 피험자 데이터 없이 진행됩니다 - " + e);
			return false;
		}
	}

	public bool GetDirStatus () {
		return dirStatus;
	}
	
	public bool WriteVRSS (int score) {
		if (dirStatus)
		{
			try {
				// 컬럼 정보 삽입
				if (initVrss != true)
				{
					string columnInfo = "reportTime,sicknessSymptomLevel";
					vrssWriter.WriteLine(columnInfo);
					vrssWriter.Flush();
					initVrss = true;
				}
				string inputLine = Time.time.ToString() + "," + score.ToString();
				
				vrssWriter.WriteLine(inputLine);
				vrssWriter.Flush();
			} catch (System.Exception e) {
				Debug.LogAssertion ("경고! 데이터 디렉토리는 확인되었으나, 접근하거나 기록할 수 없습니다. - " + e);
				return false;
			}
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool WriteGeneral (List<String> list) {
		if (generalDataStream.CanWrite)
		{
			try {
				// 컬럼 정보 삽입
				if (initGeneral != true)
				{
					string columnInfo = 
					"time,navTime,posX,posY,posZ,headX,headY,headZ,currentDest,remainDistance";
					generalWriter.WriteLine(columnInfo);
					generalWriter.Flush();
					initGeneral = true;
				}
				generalWriter = new StreamWriter(generalDataStream);
				string inputLine = "";

				foreach (String data in list)
				{
					inputLine += data + ",";
				}
				
				generalWriter.WriteLine(inputLine);
				generalWriter.Flush();
			} catch (System.Exception e) {
				Debug.LogAssertion ("경고! 데이터 디렉토리는 확인되었으나, 접근하거나 기록할 수 없습니다. - " + e);
				return false;
			}
			return true;
		}

		return false;
	}

}
