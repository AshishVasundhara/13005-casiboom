/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using UnityEngine;

public class TimeAdd : MonoBehaviour
{
	public TextMesh txtCount;

	private bool isShake;

	public int Count
	{
		get;
		set;
	}

	public int IAddTime
	{
		get;
		set;
	}

	public int JAddTime
	{
		get;
		set;
	}

	public bool IsEat
	{
		get;
		set;
	}

	public void SetTranform(Vector3 position, int i, int j)
	{
		txtCount.gameObject.SetActive(value: true);
		base.transform.position = position;
		IAddTime = i;
		JAddTime = j;
		IsEat = false;
	}

	public void SetCount(int count)
	{
		Count = count;
		txtCount.text = count.ToString();
	}

	public void SetTxtCount()
	{
		txtCount.text = Count.ToString();
	}

	public bool CheckVisible(int i, int j)
	{
		bool result = false;
		if (!base.gameObject.activeInHierarchy)
		{
			result = false;
		}
		if (IAddTime == i && JAddTime == j)
		{
			result = true;
		}
		return result;
	}

	public void Fly()
	{
		Vector3 finish = new Vector3(-2.1f, -3f, -5f);
		Transform transform = base.transform;
		Vector3 position = base.transform.position;
		transform.DOMoveY(position.y + 0.5f, 0.5f).SetEase(Ease.OutBounce).OnComplete(delegate
		{
			base.transform.DOMove(finish, Vector3.Distance(base.transform.position, finish) * 0.1f).SetDelay(0.2f).OnComplete(delegate
			{
				TimeCount.instance.AddTime(Count);
				PoolAddTime.instance.PoolObject(this, 0f);
			});
		});
	}

	public void Shake()
	{
		isShake = true;
		base.transform.DOLocalRotate(new Vector3(0f, 0f, 30f), 0.5f).SetLoops(-1, LoopType.Yoyo);
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
