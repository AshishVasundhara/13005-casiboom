/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
	public HiScore[] hiScores;

	public RectTransform Frame;

	public RectTransform noti;

	public GameObject loadingObject;

	public GameObject renameButton;

	public Image imageShadow;

	public static LeaderBoard instance;

	public Button okRename;

	public InputField inputField;

	public Image imgOkRename;

	private int idPopupPre;

	public Sprite title88;

	public Sprite title1010;

	public Sprite titleBomb;

	public Sprite titleTime;

	public Image title;

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
		if (Data.countView > 5)
		{
			//AdMobController.instance.ShowInterstitial();
			Data.countView = 0;
		}
		else
		{
			Data.countView++;
		}
		imageShadow.enabled = true;
		loadingObject.SetActive(value: true);
		idPopupPre = Data.idPopup;
		Data.idPopup = 3;
	}

	public void Hide()
	{
		Frame.DOScale(1.1f, 0.2f).SetLoops(2, LoopType.Yoyo).OnComplete(delegate
		{
			SoundManager.PlaySound("dialog");
			Data.isPlayEffect = true;
			imageShadow.enabled = false;
			Frame.gameObject.SetActive(value: false);
			loadingObject.SetActive(value: false);
			noti.gameObject.SetActive(value: false);
			Data.isPlayEffect = false;
			Data.idPopup = idPopupPre;
		});
	}

	public void SetData(List<LeaderBoardData.Player> list, int scorePlayer, int playerPos, int playerIDAvatar)
	{
		Data.isPlayEffect = true;
		switch (Data.idTypeGame)
		{
		case 9:
			title.sprite = titleTime;
			break;
		case 10:
			title.sprite = title1010;
			break;
		case 11:
			title.sprite = titleBomb;
			break;
		default:
			title.sprite = title88;
			break;
		}
		title.SetNativeSize();
		Frame.gameObject.SetActive(value: true);
		Frame.DOScale(1.1f, 0.2f).SetLoops(2, LoopType.Yoyo);
		loadingObject.SetActive(value: false);
		noti.gameObject.SetActive(value: false);
		string @string = PlayerPrefs.GetString("Name", Data.namePlayer);
		hiScores[hiScores.Length - 1].gameObject.SetActive(value: true);
		hiScores[hiScores.Length - 1].setStt(playerPos);
		hiScores[hiScores.Length - 1].setName(@string);
		hiScores[hiScores.Length - 1].setScore(scorePlayer);
		hiScores[hiScores.Length - 1].setAvt(playerIDAvatar);
		int num = 0;
		num = ((list.Count <= hiScores.Length - 2) ? list.Count : (hiScores.Length - 1));
		for (int i = 0; i < num; i++)
		{
			LeaderBoardData.Player player = list[i];
			hiScores[i].gameObject.SetActive(value: true);
			hiScores[i].setName(player.player_name);
			hiScores[i].setScore(player.score);
			hiScores[i].setAvt(player.avt);
		}
		Data.isPlayEffect = false;
	}

	public void SetActiveNoti(bool isActive)
	{
		if (isActive)
		{
			noti.gameObject.SetActive(isActive);
			imageShadow.enabled = isActive;
			noti.DOScale(1.1f, 0.2f).SetLoops(2, LoopType.Yoyo);
		}
		else
		{
			noti.DOScale(1.1f, 0.2f).SetLoops(2, LoopType.Yoyo).OnComplete(delegate
			{
				noti.gameObject.SetActive(isActive);
				imageShadow.enabled = isActive;
			});
		}
		if (isActive)
		{
			Data.idPopup = 4;
		}
		else
		{
			Data.idPopup = idPopupPre;
		}
	}

	public void SetActiveLoading(bool isActive)
	{
		loadingObject.SetActive(isActive);
	}

	public void SetActiveRename(bool isActive)
	{
		inputField.text = Data.namePlayer;
		inputField.gameObject.SetActive(isActive);
		okRename.gameObject.SetActive(isActive);
		renameButton.SetActive(!isActive);
		if (isActive)
		{
			Data.idPopup = 5;
		}
		else
		{
			Data.idPopup = 3;
		}
	}

	public void ChangeValueInput()
	{
		if (inputField.text.Length > 2)
		{
			okRename.interactable = true;
			imgOkRename.color = new Color(255f, 255f, 255f, 255f);
		}
		else
		{
			okRename.interactable = false;
			imgOkRename.color = new Color(128f, 128f, 128f, 255f);
		}
	}

	public void OKRename()
	{
		Data.namePlayer = inputField.text;
		PlayerPrefs.SetString("Name", Data.namePlayer);
		PlayerPrefs.Save();
		SetActiveRename(isActive: false);
		DataServer.instance.Request(2);
	}
}
