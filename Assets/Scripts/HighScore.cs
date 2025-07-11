/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using DG.Tweening.Core;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
	public SkeletonGraphic mSkeletonGraphic;

	public GameObject frame;

	//public GameObject buttonShare;

	public GameObject buttonClose;

	public GameObject txt;

	public Image blackImage;

	public static HighScore instance;

	public Text txtScore;

	private int bestScore;

	public int Best
	{
		get
		{
			return bestScore;
		}
		set
		{
			bestScore = value;
			txtScore.text = bestScore.ToString();
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

	public void SetActive(bool isActive, int bestScore = 0)
	{
		if (isActive)
		{
			frame.SetActive(isActive);
			blackImage.enabled = isActive;
			mSkeletonGraphic.AnimationState.SetAnimation(0, "animation", loop: false);
			buttonClose.transform.DOScale(1f, 2f);
			txt.transform.DOScale(1f, 2f);
			txtScore.transform.DOScale(1f, 2f).OnComplete(delegate
			{
				InvokeRepeating("StepTime", 0f, 0.287f);
				DOTween.To(()=>this.Best, (DOSetter<int>)delegate(int x)
				{
					Best = x;
				}, bestScore, 1f).OnComplete(delegate
				{
					//buttonShare.transform.DOScale(1f, 1f);
					CancelInvoke("StepTime");
				});
			});
			Data.idPopup = 10;
		}
		else
		{
			frame.transform.DOScale(1.1f, 0.2f).SetLoops(2, LoopType.Yoyo).OnComplete(delegate
			{
				//buttonShare.transform.localScale = Vector3.zero;
				buttonClose.transform.localScale = Vector3.zero;
				txt.transform.localScale = Vector3.zero;
				txtScore.transform.localScale = Vector3.zero;
				Best = 0;
				blackImage.enabled = isActive;
				frame.SetActive(isActive);
				Data.idPopup = 2;
				if (Data.isShowInterstitial)
				{
					//AdMobController.instance.ShowInterstitial();
				}
				else
				{
					Data.isShowInterstitial = true;
				}
			});
		}
	}

	

	public void CloseButton()
	{
		SetActive(isActive: false);
	}

	private void StepTime()
	{
		SoundManager.PlaySound("score", 0.8f);
	}
}
