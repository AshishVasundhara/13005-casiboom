/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class PoolsBomb : MonoBehaviour
{
	public static PoolsBomb instance;

	public Bomb objectPrefab;

	public List<Bomb> listBomb;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		listBomb = new List<Bomb>();
	}

	public Bomb GetObjectForType()
	{
		Bomb bomb = UnityEngine.Object.Instantiate(objectPrefab);
		bomb.name = objectPrefab.name;
		bomb.transform.parent = base.transform;
		listBomb.Add(bomb);
		return bomb;
	}

	public void PoolObject(Bomb obj, float time)
	{
		if (time != 0f)
		{
			DOVirtual.DelayedCall(time, delegate
			{
				listBomb.Remove(obj);
				SoundManager.PlaySound("boomEat", 1f);
				UnityEngine.Object.Destroy(obj.gameObject);
			});
			return;
		}
		listBomb.Remove(obj);
		SoundManager.PlaySound("boomEat", 1f);
		UnityEngine.Object.Destroy(obj.gameObject);
	}

	public void PoolObject()
	{
		for (int i = 0; i < listBomb.Count; i++)
		{
			UnityEngine.Object.Destroy(listBomb[i].gameObject);
		}
		listBomb.Clear();
	}

	public void Effect(int i, int j, bool isShake)
	{
		int num = 0;
		while (true)
		{
			if (num < listBomb.Count)
			{
				if (listBomb[num].CheckVisible(i, j))
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
			listBomb[num].Shake();
		}
		else
		{
			listBomb[num].Idle();
		}
	}
}
