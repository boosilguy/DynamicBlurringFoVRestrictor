using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticipantsManager : MonoBehaviour{
	public enum PlayType {
		Practice,
		Experiment,
		Debug
	}
	
	[Header("Participant's identity")]
	public string nameTag;
	[Tooltip("cm 단위로 기입할 것")]
	public float height;
	[Space(10)]

	[Header("Participant's play type")]
	[Tooltip("연습모드, 실험모드, 디버깅모드")]
	public PlayType playType;
	int year;
	int month;
	int date;

	void Awake () {
		year = System.DateTime.Now.Year;
		month = System.DateTime.Now.Month;
		date = System.DateTime.Now.Day;
	}

	public string GetExpDate () {
		return year.ToString() + "-" + month.ToString() + "-" + date.ToString();
	}

	public string GetParticipantsInfo () {
		return nameTag + " (" + GetPlayType() + ")";
	}

	public string GetPlayType () {
		if (playType == PlayType.Practice) {
			return "PRACTICE";
		}
		else if (playType == PlayType.Experiment) {
			return "EXPERIMENT";
		}
		else
		{
			return "DEBUG";
		}
	}
}
