/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

public class Bomb : MonoBehaviour
{
	public TextMesh txtCount;

	private bool isShake;

	private Vector3 localPos;

	public SkeletonAnimation skeletonAnimation;

	public int Count
	{
		get;
		set;
	}

	public int IBomb
	{
		get;
		set;
	}

	public int JBomb
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
		IBomb = i;
		JBomb = j;
		IsEat = false;
		localPos = base.transform.localPosition;
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
		if (IBomb == i && JBomb == j)
		{
			result = true;
		}
		return result;
	}

	public void SetState(int state)
	{
		if (state == 2)
		{
			txtCount.gameObject.SetActive(value: false);
			GameObject objectForType = Pools.instance.GetObjectForType("Explosive");
			objectForType.transform.position = base.transform.position - new Vector3(0f, 0f, 0.1f);
			Pools.instance.PoolObject(objectForType, 0.7f);
			PoolsBomb.instance.PoolObject();
		}
	}

	public void Shake()
	{
		isShake = true;
		base.transform.DOLocalMoveY(localPos.y + UnityEngine.Random.Range(0.05f, 0.08f), UnityEngine.Random.Range(0.5f, 0.8f)).SetEase(Ease.OutBounce).SetLoops(-1, LoopType.Yoyo);
	}

	public void Idle()
	{
		if (isShake)
		{
			base.transform.DOKill();
			base.transform.localPosition = localPos;
			isShake = false;
		}
	}
}
