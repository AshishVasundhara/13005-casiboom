/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using Utilities;

public class GamePlayUI : MonoBehaviour
{
	public TextMesh txtScore;

	public TextMesh txtBest;

	private int score;

	private int bestScore;

	public static GamePlayUI instance;

	public int Score
	{
		get
		{
			return score;
		}
		set
		{
			score = value;
			txtScore.text = score.ToString();
		}
	}

	public int Best
	{
		get
		{
			return bestScore;
		}
		set
		{
			bestScore = value;
			txtBest.text = bestScore.ToString();
		}
	}

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
	}

	public void SettingClick()
	{
		if (!Data.isPauseGame && !Data.isPlayEffect)
		{
			Data.isPlayEffect = true;
			Data.idPopup = 1;
			Data.isPauseGame = true;
			Setting.instance.SetActive(isActive: true);
		}
	}

	public void SetScore(int score)
	{
        DOTween.To(()=>this.Score, (DOSetter<int>)delegate (int x)
        {
            Score = x;
        }, score, 2f);
	}

	public void SetBest(int bestScore)
	{
		DOTween.To(()=>this.Best, (DOSetter<int>)delegate(int x)
		{
			Best = x;
		}, bestScore, 2f);
	}

	public void ResetScore()
	{
		bestScore = Utilities.PlayerPrefs.GetInt(Data.KEY_BEST_SCORE, 0);
		DOTween.To(()=>this.Score, (DOSetter<int>)delegate(int x)
		{
			Score = x;
		}, 0, 2f);
        DOTween.To(() => this.Best, (DOSetter<int>)delegate (int x)
          {
              Best = x;
          }, bestScore, 2f);
	}
}
