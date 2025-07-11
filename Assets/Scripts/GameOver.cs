/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
//using Gley.GameServices;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
	public Text txtScore;

	public Text txtHighScore;

	public static GameOver instance;

	private int score;

	private int bestScore;

	public GameObject frame;

	public Sprite title88;

	public Sprite title1010;

	public Sprite titleBomb;

	public Sprite titleTime;

	public Image title;

	public Button rankButton;

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
			txtHighScore.text = bestScore.ToString();
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

#if GLEY_GAMESERVICES_ANDROID || GLEY_GAMESERVICES_IOS
		rankButton.gameObject.SetActive(true);
#else
		rankButton.gameObject.SetActive(false);
#endif
	}

	public void SetScore(int score)
	{
		InvokeRepeating("StepTime", 0f, 0.287f);
		DOTween.To(()=>this.Score, (DOSetter<int>)delegate(int x)
		{
			Score = x;
		}, score, 2f).OnComplete(delegate
		{
			CancelInvoke("StepTime");
		});
	}

	private void StepTime()
	{
		SoundManager.PlaySound("score", 0.8f);
	}

	public void SetBest(int bestScore)
	{
        DOTween.To(() => this.Best, (DOSetter<int>)delegate (int x)
          {
              Best = x;
          }, bestScore, 2f);
	}

	public void ResetScore()
	{
        DOTween.To(() => this.Score, (DOSetter<int>)delegate (int x)
          {
              Score = x;
          }, 0, 2f);
        DOTween.To(() => this.Best, (DOSetter<int>)delegate (int x)
          {
              Best = x;
          }, 0, 2f);
	}

	public void SetActive(bool isActive, bool isHigh)
	{

		Debug.Log("TYPE MODE :" + Data.idTypeGame + "BEST SCORE :" + Utilities.PlayerPrefs.GetInt(Data.KEY_BEST_SCORE, bestScore));

		if (isActive)
		{
			switch (Data.idTypeGame)
			{
			case 9:
				title.sprite = titleTime;
#if GLEY_GAMESERVICES_ANDROID || GLEY_GAMESERVICES_IOS
					API.SubmitScore(Utilities.PlayerPrefs.GetInt(Data.KEY_BEST_SCORE, bestScore), LeaderboardNames.TimeMode, ScoreSubmitted);
#endif
				break;
			case 10:
				title.sprite = title1010;
#if GLEY_GAMESERVICES_ANDROID || GLEY_GAMESERVICES_IOS
					API.SubmitScore(Utilities.PlayerPrefs.GetInt(Data.KEY_BEST_SCORE, bestScore), LeaderboardNames.Classic10x10Mode, ScoreSubmitted);
#endif
					break;
			case 11:
				title.sprite = titleBomb;
#if GLEY_GAMESERVICES_ANDROID || GLEY_GAMESERVICES_IOS
					API.SubmitScore(Utilities.PlayerPrefs.GetInt(Data.KEY_BEST_SCORE, bestScore), LeaderboardNames.BombMode, ScoreSubmitted);
#endif
					break;
			default:
				title.sprite = title88;
#if GLEY_GAMESERVICES_ANDROID || GLEY_GAMESERVICES_IOS
					API.SubmitScore(Utilities.PlayerPrefs.GetInt(Data.KEY_BEST_SCORE, bestScore), LeaderboardNames.Classic8x8Mode, ScoreSubmitted);
#endif
					break;
			}
			title.SetNativeSize();
			frame.SetActive(isActive);
			frame.transform.DOScale(1.1f, 0.2f).SetLoops(2, LoopType.Yoyo);
			if (!isHigh)
			{
				if (Data.isShowInterstitial)
				{
					//AdMobController.instance.ShowInterstitial();
				}
				else
				{
					Data.isShowInterstitial = true;
				}
			}
		}
		else
		{
			frame.transform.DOScale(1.1f, 0.2f).SetLoops(2, LoopType.Yoyo).OnComplete(delegate
			{
				frame.SetActive(isActive);
			});
		}
		if (isHigh)
		{
			DOVirtual.DelayedCall(2.5f, delegate
			{
				SoundManager.PlaySound("newbest");
				HighScore.instance.SetActive(isActive: true, bestScore);
			});
		}
	}

#if GLEY_GAMESERVICES_ANDROID || GLEY_GAMESERVICES_IOS
	//Automatically called when a score was submitted 
	private void ScoreSubmitted(bool success, GameServicesError error)
	{
		if (success)
		{
			//score successfully submitted
		}
		else
		{
			//an error occurred
			Debug.LogError("Score failed to submit: " + error);
		}
		Debug.Log("Submit score result: " + success + " message:" + error);
	}
#endif

	public void RankButton()
	{
#if GLEY_GAMESERVICES_ANDROID || GLEY_GAMESERVICES_IOS
		API.ShowLeaderboadsUI();
#endif
	}

	public void HomeButton()
	{
		DOTween.KillAll();
		if (!Data.isGameOver)
		{
			Manager.instance.SaveGameData();
		}
		Manager.instance.blackOut.gameObject.SetActive(value: true);
		Manager.instance.blackOut.DOColor(Color.black, 0.5f).OnComplete(delegate
		{
			SceneManager.LoadScene("Menu");
		});
	}

	public void RateButton()
	{
		//ShareNativeManager.Instance.OpenRateGame();
	}

	public void ReSetButton()
	{
		Manager.instance.RePlayButton();
		Data.isPauseGame = false;
		Data.isPlayEffect = false;
		Data.idPopup = 0;
		SetActive(isActive: false, isHigh: false);
	}

	
}
