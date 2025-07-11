
/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
//using Gley.MobileAds;

public class SaveMe : MonoBehaviour
{
	public static SaveMe instance;

	public Image imageShadow;

	public RectTransform Frame;

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

	public void Show()
	{
		if (Data.idTypeGame == 9)
		{
			TimeCount.instance.PauseTime(isPause: true);
		}
		MusicManager.instance.Pause(MusicManager.instance.audioSourceBG);
		MusicManager.instance.Play(MusicManager.instance.audioSourceSave);
		imageShadow.enabled = true;
		Frame.gameObject.SetActive(value: true);
		Frame.transform.DOScale(1.1f, 0.2f).SetLoops(2, LoopType.Yoyo);
		Data.idPopup = 7;
	}

	public void Hide()
	{
		if (Data.idTypeGame == 9)
		{
			TimeCount.instance.PauseTime(isPause: false);
		}
		MusicManager.instance.Pause(MusicManager.instance.audioSourceSave);
		MusicManager.instance.Play(MusicManager.instance.audioSourceBG);
		Frame.transform.DOScale(1.1f, 0.2f).SetLoops(2, LoopType.Yoyo).OnComplete(delegate
		{
			imageShadow.enabled = false;
			Frame.gameObject.SetActive(value: false);
		});
		Data.idPopup = 0;
	}

	public void ButtonOK()
	{
		Hide();
		//API.ShowRewardedVideo(CompleteMethod);

	}

	private void CompleteMethod(bool completed)
	{
		//Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
		if (completed == true)
		{
			DOTween.PlayAll();
			Manager.instance.SaveMeEat();
		}
		else
		{
			
#if UNITY_EDITOR
			DOTween.PlayAll();
			Manager.instance.SaveMeEat();

#else
			DOTween.PlayAll();
			Manager.instance.SetTxtEnd(isActive: true, isNoMove: true);
			Manager.instance.GameOverFunction();
			Data.isShowInterstitial = false;
#endif
		}
	}

			public void ButtonNoThank()
	{
		Hide();
		Manager.instance.SetTxtEnd(isActive: true, isNoMove: true);
		Manager.instance.GameOverFunction();
	}
}
