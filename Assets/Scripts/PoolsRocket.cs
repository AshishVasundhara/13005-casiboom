/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class PoolsRocket : MonoBehaviour
{
	public static PoolsRocket instance;

	public Rocket objectPrefab;

	public List<Rocket> pooledObjects;

	public RocketFly rocketFlyPrefabs;

	private List<RocketFly> rocketFlyObjects;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		pooledObjects = new List<Rocket>();
		rocketFlyObjects = new List<RocketFly>();
	}

	public Rocket GetRocket()
	{
		Rocket rocket = UnityEngine.Object.Instantiate(objectPrefab);
		rocket.name = objectPrefab.name;
		rocket.transform.parent = base.transform;
		pooledObjects.Add(rocket);
		return rocket;
	}

	public RocketFly GetRocketFly()
	{
		RocketFly rocketFly = UnityEngine.Object.Instantiate(rocketFlyPrefabs);
		rocketFly.name = objectPrefab.name;
		rocketFly.transform.parent = base.transform;
		rocketFlyObjects.Add(rocketFly);
		return rocketFly;
	}

	public void PoolObject(Rocket obj, float time)
	{
		if (time != 0f)
		{
			DOVirtual.DelayedCall(time, delegate
			{
				DestroyRocket(obj);
			});
		}
		else
		{
			DestroyRocket(obj);
		}
	}

	public void PoolObject()
	{
		for (int i = 0; i < pooledObjects.Count; i++)
		{
			DOTween.Kill(pooledObjects[i].transform);
			UnityEngine.Object.Destroy(pooledObjects[i].gameObject);
		}
		pooledObjects.Clear();
	}

	private void DestroyRocket(Rocket obj)
	{
		if (obj != null)
		{
			SoundManager.PlaySound("RocketsFlight");
			for (int i = 0; i < 4; i++)
			{
				RocketFly rocketFly = GetRocketFly();
				rocketFly.SetTranform(obj.transform.position - new Vector3(0f, 0f, 4f), obj.IRocket, obj.JRocket, i);
				rocketFly.Fly(1f);
			}
			pooledObjects.Remove(obj);
			DOTween.Kill(obj.transform);
			UnityEngine.Object.Destroy(obj.gameObject);
		}
	}

	public void PoolObject(RocketFly obj)
	{
		rocketFlyObjects.Remove(obj);
		UnityEngine.Object.Destroy(obj.gameObject);
		if (rocketFlyObjects.Count == 0)
		{
			Data.isHaveRocketFly = false;
			Manager.instance.CheckAbilitiContinue();
		}
	}

	public void Effect(int i, int j, bool isShake)
	{
		int num = 0;
		while (true)
		{
			if (num < pooledObjects.Count)
			{
				if (pooledObjects[num].CheckVisible(i, j))
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
			pooledObjects[num].Shake();
		}
		else
		{
			pooledObjects[num].Idle();
		}
	}
}
