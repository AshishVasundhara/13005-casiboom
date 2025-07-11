/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using UnityEngine;

public class Rocket : MonoBehaviour
{
	private bool isShake;

	public int IRocket
	{
		get;
		set;
	}

	public int JRocket
	{
		get;
		set;
	}

	public void SetTranform(Vector3 position, int i, int j)
	{
		base.transform.position = position;
		base.transform.DOScale(Data.scaleBlockTypeGame, 0.5f).SetEase(Ease.OutBack);
		IRocket = i;
		JRocket = j;
	}

	public bool CheckVisible(int i, int j)
	{
		bool result = false;
		if (IRocket == i && JRocket == j)
		{
			result = true;
		}
		return result;
	}

	public void SetInvisible(int delta)
	{
		PoolsRocket.instance.PoolObject(this, (float)Mathf.Abs(delta) * 0.1f);
	}

	public void Shake()
	{
		isShake = true;
		base.transform.DOLocalRotate(new Vector3(0f, 0f, 360f), 2f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Yoyo);
	}

	public void Idle()
	{
		if (isShake)
		{
			base.transform.DOKill();
			base.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			isShake = false;
		}
	}
}
