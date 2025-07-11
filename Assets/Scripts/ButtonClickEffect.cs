/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using UnityEngine;

public class ButtonClickEffect : MonoBehaviour
{
	private bool isClick;

	public bool oneClick;

	public bool isDeniAds;

	public void ClickEffect()
	{
		if (!isClick)
		{
			isClick = true;
			SoundManager.PlaySound("btn");
			base.transform.DOScale(1.1f, 0.2f).SetLoops(2, LoopType.Yoyo).OnComplete(delegate
			{
				if (!oneClick)
				{
					isClick = false;
				}
			});
			Data.isAllowShowVideo = !isDeniAds;
		}
	}
}
