/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class TipController : MonoBehaviour
{
	public GameObject frame;

	public Image icon;

	public Image txt;

	public Sprite bombIcon;

	public Sprite timeIcon;

	public Sprite bombTxt;

	public Sprite timeTxt;

	public static TipController instance;

	public Image blackImage;

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
		if (Data.idTypeGame == 9)
		{
			icon.sprite = timeIcon;
			txt.sprite = timeTxt;
		}
		else if (Data.idTypeGame == 11)
		{
			icon.sprite = bombIcon;
			txt.sprite = bombTxt;
		}
		icon.SetNativeSize();
		txt.SetNativeSize();
	}

	public void SetActive(bool isActive)
	{
		if (isActive)
		{
			blackImage.enabled = isActive;
			frame.SetActive(isActive);
			frame.transform.DOScale(1.1f, 0.2f).SetLoops(2, LoopType.Yoyo);
			Data.idPopup = 11;
			if (Data.idTypeGame == 9)
			{
				Data.isTipTime = true;
				Utilities.PlayerPrefs.SetBool("BlockGudianblock_tip_time", Data.isTipTime);
			}
			else if (Data.idTypeGame == 11)
			{
				Data.isTipBoomb = true;
				Utilities.PlayerPrefs.SetBool("BlockGudianblock_tip_boomb", Data.isTipBoomb);
			}
			Utilities.PlayerPrefs.Flush();
		}
		else
		{
			frame.transform.DOScale(1.1f, 0.2f).SetLoops(2, LoopType.Yoyo).OnComplete(delegate
			{
				blackImage.enabled = isActive;
				frame.SetActive(isActive);
				Data.idPopup = 0;
			});
		}
		if (Data.idTypeGame == 9)
		{
			TimeCount.instance.PauseTime(isActive);
		}
	}
}
