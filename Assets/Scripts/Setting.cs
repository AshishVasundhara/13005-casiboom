
/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;

public class Setting : MonoBehaviour
{
	public Image sprSoundOff;

	public Image sprMusicOff;

	public Image blackImage;

	public GameObject frame;

	public static Setting instance;

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

	private void Start()
	{
		sprSoundOff.gameObject.SetActive((Data.isSound <= 0) ? true : false);
		sprMusicOff.gameObject.SetActive((Data.isMusic <= 0) ? true : false);
	}

	public void SoundButton()
	{
		if (Data.isSound == 0)
		{
			Data.isSound = 1;
		}
		else
		{
			Data.isSound = 0;
		}
		sprSoundOff.gameObject.SetActive((Data.isSound <= 0) ? true : false);
		Utilities.PlayerPrefs.SetInt("BlockGudianblock3d_key_sound", Data.isSound);
		Utilities.PlayerPrefs.Flush();
	}

	public void MusicButton()
	{
		if (Data.isMusic == 0)
		{
			Data.isMusic = 1;
			if (!MusicManager.instance.IsPlaying(MusicManager.instance.audioSourceBG))
			{
				MusicManager.instance.Play(MusicManager.instance.audioSourceBG);
			}
		}
		else
		{
			Data.isMusic = 0;
			if (MusicManager.instance.IsPlaying(MusicManager.instance.audioSourceBG))
			{
				MusicManager.instance.Pause(MusicManager.instance.audioSourceBG);
			}
		}
		sprMusicOff.gameObject.SetActive((Data.isMusic <= 0) ? true : false);
		Utilities.PlayerPrefs.SetInt("BlockGudianblock3d_key_music", Data.isMusic);
		Utilities.PlayerPrefs.Flush();
	}

	public void SetActive(bool isActive)
	{
		if (isActive)
		{
			blackImage.enabled = isActive;
			frame.SetActive(isActive);
			frame.transform.DOScale(1.1f, 0.2f).SetLoops(2, LoopType.Yoyo);
		}
		else
		{
			frame.transform.DOScale(1.1f, 0.2f).SetLoops(2, LoopType.Yoyo).OnComplete(delegate
			{
				blackImage.enabled = isActive;
				frame.SetActive(isActive);
			});
		}
		if (Data.idTypeGame == 9)
		{
			TimeCount.instance.PauseTime(isActive);
		}
	}

	public void ResumeButton()
	{
		Data.isPauseGame = false;
		Data.isPlayEffect = false;
		Data.idPopup = 0;
		SetActive(isActive: false);
	}

	public void RankButton()
	{
		DataServer.instance.Request(2);
	}

	public void HomeButton()
	{
		DOTween.KillAll();
		if (!Data.isGameOver)
		{
			Manager.instance.SaveGameData();
		}
		SetActive(isActive: false);
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
		Manager.instance.ResetButton();
		Data.isPauseGame = false;
		Data.isPlayEffect = false;
		Data.idPopup = 0;
		SetActive(isActive: false);
	}

	public void ExitButton()
	{
		Application.Quit();
	}

	public void Policy()
	{
		//ShareNativeManager.Instance.PolicyGame();
	}
}
