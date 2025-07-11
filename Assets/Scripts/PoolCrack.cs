/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using System.Collections.Generic;
using UnityEngine;

public class PoolCrack : MonoBehaviour
{
	public static PoolCrack instance;

	public CrackController objectPrefab;

	public List<CrackController> pooledObjects;

	public List<CrackController> currentObjects;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		pooledObjects = new List<CrackController>();
		currentObjects = new List<CrackController>();
	}

	public CrackController GetObjectForType(bool onlyPooled = false, bool isParent = true)
	{
		if (pooledObjects.Count > 0)
		{
			CrackController crackController = pooledObjects[0];
			pooledObjects.RemoveAt(0);
			crackController.transform.parent = null;
			crackController.gameObject.SetActive(value: true);
			if (isParent)
			{
				crackController.transform.parent = base.transform;
			}
			currentObjects.Add(crackController);
			return crackController;
		}
		if (!onlyPooled)
		{
			CrackController crackController2 = UnityEngine.Object.Instantiate(objectPrefab);
			crackController2.name = objectPrefab.name;
			if (isParent)
			{
				crackController2.transform.parent = base.transform;
			}
			currentObjects.Add(crackController2);
			return crackController2;
		}
		return null;
	}

	public void PoolObject(bool isDestroy)
	{
		int count = currentObjects.Count;
		if (count <= 0)
		{
			return;
		}
		if (!isDestroy)
		{
			for (int i = 0; i < count; i++)
			{
				CrackController crackController = currentObjects[i];
				crackController.gameObject.SetActive(value: false);
				pooledObjects.Add(crackController);
			}
		}
		else
		{
			for (int j = 0; j < count; j++)
			{
				UnityEngine.Object.Destroy(currentObjects[j].gameObject);
			}
		}
		currentObjects.Clear();
	}

	public void PoolObject()
	{
		for (int i = 0; i < pooledObjects.Count; i++)
		{
			UnityEngine.Object.Destroy(pooledObjects[i].gameObject);
		}
		pooledObjects.Clear();
	}
}
