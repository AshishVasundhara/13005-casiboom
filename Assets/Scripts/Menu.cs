
/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
//using Gley.GameServices;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
	public RectTransform top;

	public RectTransform title;

	public RectTransform bottom;

	public RectTransform button88;

	public RectTransform buttonBomb;

	public RectTransform buttonRotate;

	public RectTransform button1010;

	public RectTransform buttonRank;

	public RectTransform buttonRate;

	public RectTransform buttonSetting;

	public RectTransform buttonRemoveAds;
	public RectTransform buttonRestore;

	public Image blackIn;

	private bool isEndEffect;

	private void Awake()
	{
		Data.InitStatic();
	}

	private void Start()
	{
		//Utilities.PlayerPrefs.DeleteAll();
		//Login to Game Servicies
#if GLEY_GAMESERVICES_ANDROID
		if (!API.IsLoggedIn())
        {
            API.LogIn(LoginResult);
        }
#endif
		//		Debug.Log("DATA COUNT HELP :" + Data.countHelp);
		if (Data.isMusic == 1 && !MusicManager.instance.IsPlaying(MusicManager.instance.audioSourceBG))
		{
			MusicManager.instance.Play(MusicManager.instance.audioSourceBG);
		}
		isEndEffect = false;
		EffectIn();
		//AdMobController.instance.HideBanner();
		Application.targetFrameRate = 60;
	}

#if GLEY_GAMESERVICES_ANDROID 
	//Automatically called when Login is complete 
	private void LoginResult(bool success)
	{
		if (success == true)
		{
			//Login was successful
		}
		else
		{
			//Login failed
		}
		Debug.Log("Login success: " + success);
	}
#endif

	public void GotoGame(int typeBlock)
	{
		if (isEndEffect)
		{
			isEndEffect = false;
			Data.idTypeGame = typeBlock;
			Data.Init();
			DOVirtual.DelayedCall(0.4f, delegate
			{
				EffectOut();
			});
		}
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			switch (Data.idPopup)
			{
			case 6:
				break;
			case 1:
				Setting.instance.ResumeButton();
				break;
			case 3:
				LeaderBoard.instance.Hide();
				break;
			case 4:
				LeaderBoard.instance.SetActiveNoti(isActive: false);
				break;
			case 5:
				LeaderBoard.instance.SetActiveRename(isActive: false);
				break;
			default:
				Application.Quit();
				break;
			}
		}
	}

	public void EffectIn()
	{
		//character.DOAnchorPosY(0f, 0.8f).SetEase(Ease.OutBack);
		top.DOAnchorPosY(-160f, 0.5f).SetDelay(0.2f);
		bottom.DOAnchorPosY(0f, 0.3f).SetDelay(0.2f);
		title.DOScale(1f, 0.8f).SetDelay(0.3f).SetEase(Ease.OutBack);
		buttonBomb.gameObject.SetActive(Data.countHelp > 0);
		buttonRotate.gameObject.SetActive(Data.countHelp > 0);
		button1010.gameObject.SetActive(Data.countHelp > 0);
		if (Data.countHelp <= 0)
		{
			button88.anchoredPosition = new Vector2(0f, 807f);
		}
		else
		{
			buttonRotate.DOScale(1f, 0.8f).SetDelay(0.7f).SetEase(Ease.OutBack);
			buttonRotate.DOAnchorPosX(155f, 0.6f).SetDelay(0.9f);
			button1010.DOScale(1f, 0.8f).SetDelay(0.5f).SetEase(Ease.OutBack);
			buttonBomb.DOScale(1f, 0.8f).SetDelay(0.7f).SetEase(Ease.OutBack);
			buttonBomb.DOAnchorPosX(-155f, 0.6f).SetDelay(0.9f);
		}
		button88.DOScale(1f, 0.6f).SetDelay(0.5f).SetEase(Ease.OutBack);
#if GLEY_GAMESERVICES_ANDROID 
		buttonRank.DOScale(1f, 0.3f).SetDelay(0.8f).SetEase(Ease.OutBack);
#endif
		buttonRate.DOScale(1f, 0.3f).SetDelay(1f).SetEase(Ease.OutBack);

		//buttonShare.DOScale(1f, 0.3f).SetDelay(1.2f).SetEase(Ease.OutBack);
		buttonSetting.DOScale(1f, 0.3f).SetDelay(1.4f).SetEase(Ease.OutBack);
		buttonRemoveAds.DOScale(1f, 0.3f).SetDelay(1.4f).SetEase(Ease.OutBack)
			.OnComplete(delegate
			{
				isEndEffect = true;
			});

		buttonRestore.DOScale(1f, 0.3f).SetDelay(1.4f).SetEase(Ease.OutBack)
			.OnComplete(delegate
			{
				isEndEffect = true;
			});
	}

	public void EffectOut()
	{
#if GLEY_GAMESERVICES_ANDROID
		buttonRank.DOScale(0f, 0.6f).SetEase(Ease.InBack);
#endif
		buttonRate.DOScale(0f, 0.6f).SetEase(Ease.InBack);
		//buttonShare.DOScale(0f, 0.6f).SetEase(Ease.InBack);
		buttonSetting.DOScale(0f, 0.6f).SetEase(Ease.InBack);
		buttonRemoveAds.DOScale(0f, 0.6f).SetEase(Ease.InBack);
		buttonRestore.DOScale(0f, 0.6f).SetEase(Ease.InBack);
		if (Data.countHelp > 0)
		{
			buttonRotate.DOScale(0f, 0.4f).SetEase(Ease.InBack);
			buttonRotate.DOAnchorPosX(355f, 0.4f);
			button1010.DOScale(0f, 0.6f).SetEase(Ease.InBack);
			buttonBomb.DOScale(0f, 0.4f).SetEase(Ease.InBack);
			buttonBomb.DOAnchorPosX(-355f, 0.4f);
		}

		button88.DOScale(0f, 0.8f).SetDelay(0.3f).SetEase(Ease.InBack);
		//character.DOScale(2f, 1f).SetDelay(0.4f);
		//character.DOAnchorPos(new Vector2(700f, -315f), 1f).SetDelay(0.4f);
		title.DOScale(0f, 1f).SetDelay(0.4f).SetEase(Ease.InBack);
		DOVirtual.DelayedCall(1.4f, delegate
		{
			blackIn.gameObject.SetActive(value: true);
			blackIn.DOColor(Color.black, 0.5f).OnComplete(delegate
			{
				SceneManager.LoadScene("Game");
			});
		});
	}

	public void SettingClick()
	{
		Data.idPopup = 1;
		Setting.instance.SetActive(isActive: true);
	}

	public void RateClick()
	{
        //ShareNativeManager.Instance.OpenRateGame();
    }

    public void LeaderBoardClick()
	{
#if GLEY_GAMESERVICES_ANDROID

		//Login to Game Servicies
		if (!API.IsLoggedIn())
		{
			API.LogIn(LoginResult);
        }
        else
        {
			API.ShowLeaderboadsUI();
		}

#endif
	}

	public void ShareClick()
	{
        //ShareNativeManager.Instance.ShareNative(false);
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			return;
		}
        //if (Data.isAllowShowVideo)
        //{
        //	if (UnityEngine.Random.Range(0, 1000) % 100 < 50)
        //	{
        //		AdMobController.instance.ShowInterstitial();
        //	}
        //}
        else
        {
            Data.isAllowShowVideo = true;
        }
    }
}
