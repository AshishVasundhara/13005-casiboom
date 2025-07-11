/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using UnityEngine;

public class RocketFly : MonoBehaviour
{
	private int PCurrent;

	private int PPre;

	private Transform mTransform;

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

	public int ZRocket
	{
		get;
		set;
	}

	private void Awake()
	{
		mTransform = base.transform;
	}

	public void SetTranform(Vector3 position, int i, int j, int z)
	{
		mTransform.position = position;
		mTransform.localScale = Vector3.one * Data.scaleBlockTypeGame;
		IRocket = i;
		JRocket = j;
		ZRocket = z;
		mTransform.Rotate(new Vector3(0f, 0f, z * 90));
	}

	public void Fly(float time)
	{
		switch (ZRocket)
		{
		case 0:
			mTransform.DOMoveY(7f, time).OnUpdate(delegate
			{
				ClearCells();
			}).OnComplete(delegate
			{
				DOTween.Kill(mTransform);
				PoolsRocket.instance.PoolObject(this);
			});
			break;
		case 1:
			mTransform.DOMoveX(-5f, time).OnUpdate(delegate
			{
				ClearCells();
			}).OnComplete(delegate
			{
				DOTween.Kill(mTransform);
				PoolsRocket.instance.PoolObject(this);
			});
			break;
		case 2:
			mTransform.DOMoveY(-7f, time).OnUpdate(delegate
			{
				ClearCells();
			}).OnComplete(delegate
			{
				DOTween.Kill(mTransform);
				PoolsRocket.instance.PoolObject(this);
			});
			break;
		default:
			mTransform.DOMoveX(5f, time).OnUpdate(delegate
			{
				ClearCells();
			}).OnComplete(delegate
			{
				DOTween.Kill(mTransform);
				PoolsRocket.instance.PoolObject(this);
			});
			break;
		}
	}

	private void ClearCells()
	{
		if (ZRocket == 0 || ZRocket == 2)
		{
			Vector3 position = mTransform.position;
			PCurrent = Mathf.RoundToInt((position.y - Data.B_BOT) / Data.DISTANCE_BLOCK);
			if (PCurrent != PPre && PCurrent >= 0 && PCurrent < Data.MATRIX_N)
			{
				Cells.instance.EatCells(PCurrent, JRocket);
				PPre = PCurrent;
			}
		}
		else
		{
			Vector3 position2 = mTransform.position;
			PCurrent = Mathf.RoundToInt((position2.x - Data.B_LEFT) / Data.DISTANCE_BLOCK);
			if (PCurrent != PPre && PCurrent >= 0 && PCurrent < Data.MATRIX_N)
			{
				Cells.instance.EatCells(IRocket, PCurrent);
				PPre = PCurrent;
			}
		}
	}
}
