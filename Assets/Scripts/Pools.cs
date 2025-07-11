/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Pools : MonoBehaviour
{
	public static Pools instance;

	public GameObject[] objectPrefabs;

	public List<GameObject>[] pooledObjects;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		pooledObjects = new List<GameObject>[objectPrefabs.Length];
		int num = 0;
		GameObject[] array = objectPrefabs;
		foreach (GameObject gameObject in array)
		{
			pooledObjects[num] = new List<GameObject>();
			num++;
		}
	}

	public GameObject GetObjectForType(string objectType)
	{
		for (int i = 0; i < objectPrefabs.Length; i++)
		{
			GameObject gameObject = objectPrefabs[i];
			if (gameObject.name == objectType)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(objectPrefabs[i]);
				gameObject2.name = objectPrefabs[i].name;
				gameObject2.transform.parent = base.transform;
				return gameObject2;
			}
		}
		return null;
	}

	public void PoolObject(GameObject obj, float time)
	{
		if (time != 0f)
		{
			DOVirtual.DelayedCall(time, delegate
			{
				PoolObject(obj);
			});
		}
		else
		{
			PoolObject(obj);
		}
	}

	private void PoolObject(GameObject obj)
	{
		for (int i = 0; i < objectPrefabs.Length; i++)
		{
			if (objectPrefabs[i].name == obj.name)
			{
				UnityEngine.Object.Destroy(obj);
			}
		}
	}
}
