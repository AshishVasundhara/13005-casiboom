/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using UnityEngine;

public class TimeCount : MonoBehaviour
{
	public static TimeCount instance;

	public GameObject frame;

	public TextMesh txtTime;

	public TextMesh addTime;

	public SpriteRenderer sprRender;

	public float countTimeLeft;

	private float maxTime = 120f;

	private int countPlaySound;

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

	public void SetActive(bool isActive, float countTimeLeft = 120f)
	{
		frame.SetActive(isActive);
		txtTime.text = countTimeLeft.ToString();
		CancelInvoke();
		if (isActive)
		{
			sprRender.size = new Vector2(2.52f, 0.32f);
			InvokeRepeating("StepTime", 2f, 1f);
			this.countTimeLeft = countTimeLeft;
			sprRender.size = new Vector2(countTimeLeft / maxTime * 2.52f, 0.32f);
		}
	}

	private void StepTime()
	{
		if (countTimeLeft > 0f)
		{
			countTimeLeft -= 1f;
			txtTime.text = countTimeLeft.ToString();
			sprRender.size = new Vector2(countTimeLeft / maxTime * 2.52f, 0.32f);
			if (countTimeLeft > 10f)
			{
				SoundManager.PlaySound("ReliveCountdown", 0.3f);
			}
			else
			{
				if (countTimeLeft == 10f)
				{
					countPlaySound = 4;
				}
				else
				{
					countPlaySound++;
				}
				if (countPlaySound == 4)
				{
					SoundManager.PlaySound("Countdown", 0.3f);
					countPlaySound = 0;
				}
			}
			Cells.instance.CheckAddTime();
		}
		else
		{
			CancelInvoke("StepTime");
			Manager.instance.SetTxtEnd(isActive: true, isNoMove: false);
			Data.isPauseGame = true;
			DOVirtual.DelayedCall(1f, delegate
			{
				Manager.instance.GameOverFunction();
			});
		}
	}

	public void PauseTime(bool isPause)
	{
		if (isPause)
		{
			CancelInvoke("StepTime");
		}
		else
		{
			InvokeRepeating("StepTime", 2f, 1f);
		}
	}

	public void AddTime(int addTime)
	{
		TextMesh pooledObject = UnityEngine.Object.Instantiate(this.addTime);
		pooledObject.transform.parent = frame.transform;
		pooledObject.transform.localPosition = new Vector3(1.35f, 0.05f, -0.2f);
		pooledObject.gameObject.SetActive(value: true);
		pooledObject.text = addTime.ToString();
		pooledObject.transform.DOLocalMoveY(0.52f, 0.5f).SetEase(Ease.OutBounce).OnComplete(delegate
		{
			if (!Data.isGameOver)
			{
				countTimeLeft += addTime;
				if (countTimeLeft > maxTime)
				{
					countTimeLeft = maxTime;
				}
				txtTime.text = countTimeLeft.ToString();
				sprRender.size = new Vector2(countTimeLeft / maxTime * 2.52f, 0.32f);
			}
			UnityEngine.Object.Destroy(pooledObject.gameObject);
		});
	}
}
