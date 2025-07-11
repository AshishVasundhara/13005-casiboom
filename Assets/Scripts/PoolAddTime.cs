/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class PoolAddTime : MonoBehaviour
{
	public static PoolAddTime instance;

	public TimeAdd objectPrefab;

	public List<TimeAdd> listAddTime;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		listAddTime = new List<TimeAdd>();
	}

	public TimeAdd GetObjectForType()
	{
		TimeAdd timeAdd = UnityEngine.Object.Instantiate(objectPrefab);
		timeAdd.name = objectPrefab.name;
		timeAdd.transform.parent = base.transform;
		listAddTime.Add(timeAdd);
		return timeAdd;
	}

	public void PoolObject(TimeAdd obj, float time)
	{
		if (time != 0f)
		{
			DOVirtual.DelayedCall(time, delegate
			{
				obj.Fly();
			});
			return;
		}
		listAddTime.Remove(obj);
		SoundManager.PlaySound("appear", 1f);
		UnityEngine.Object.Destroy(obj.gameObject);
	}

	public void PoolObject()
	{
		for (int i = 0; i < listAddTime.Count; i++)
		{
			UnityEngine.Object.Destroy(listAddTime[i].gameObject);
		}
		listAddTime.Clear();
	}

	public void Effect(int i, int j, bool isShake)
	{
		int num = 0;
		while (true)
		{
			if (num < listAddTime.Count)
			{
				if (listAddTime[num].CheckVisible(i, j))
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		if (isShake)
		{
			listAddTime[num].Shake();
		}
		else
		{
			listAddTime[num].Idle();
		}
	}
}
