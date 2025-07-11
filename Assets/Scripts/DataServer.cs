/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using System.Collections;
using UnityEngine;
using Utilities;

public class DataServer : MonoBehaviour
{
	public string text;

	public static DataServer instance;

	private void Awake()
	{
		if (!instance)
		{
			instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private IEnumerator SendGETRequest(int idAction)
	{
		int score = Utilities.PlayerPrefs.GetInt(Data.KEY_BEST_SCORE);
		string imei = SystemInfo.deviceUniqueIdentifier;
		int around = 4;
		string url = "";
		WWW www = new WWW(url);
		yield return www;
		if (www.error == null)
		{
			RequestSuccess(www.text, idAction);
		}
		else if (idAction == 2)
		{
			LeaderBoard.instance.SetActiveLoading(isActive: false);
			LeaderBoard.instance.SetActiveNoti(isActive: true);
		}
	}

	private void RequestSuccess(string text, int idAction)
	{
		this.text = text;
		LeaderBoardData leaderBoardData = JsonUtility.FromJson<LeaderBoardData>(text);
		int score = leaderBoardData.score;
		int avt = leaderBoardData.avt;
		if (score > Utilities.PlayerPrefs.GetInt(Data.KEY_BEST_SCORE))
		{
			if (Manager.instance != null)
			{
				Manager.instance.SynchronizedBestScore(score);
			}
			Utilities.PlayerPrefs.SetInt(Data.KEY_BEST_SCORE, score);
			Utilities.PlayerPrefs.Flush();
		}
		int current_position = leaderBoardData.current_position;
		if (idAction != 1 && idAction == 2)
		{
			LeaderBoard.instance.SetData(leaderBoardData.pre_player, score, current_position, avt);
		}
	}

	public void Request(int idAction)
	{
		if (idAction == 2)
		{
			LeaderBoard.instance.Show();
		}
		StartCoroutine(SendGETRequest(idAction));
	}
}
