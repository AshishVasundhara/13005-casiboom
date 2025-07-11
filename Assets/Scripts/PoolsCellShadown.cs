/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolsCellShadown : MonoBehaviour
{
	public static PoolsCellShadown instance;

	public CellShadown objectPrefab;

	private List<CellShadown> pooledObjects;

	private List<CellShadown> pooledObjectsSuggest;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		pooledObjects = new List<CellShadown>();
		pooledObjectsSuggest = new List<CellShadown>();
	}

	public CellShadown GetObjectForType(string objectType, bool onlyPooled, bool isParent = false, bool isSuggest = false)
	{
		if (objectPrefab.name == objectType)
		{
			if (pooledObjects.Count > 0)
			{
				CellShadown cellShadown = pooledObjects[0];
				pooledObjects.RemoveAt(0);
				cellShadown.transform.parent = null;
				cellShadown.gameObject.SetActive(value: true);
				if (isParent)
				{
					cellShadown.transform.parent = base.transform;
				}
				if (isSuggest)
				{
					pooledObjectsSuggest.Add(cellShadown);
				}
				return cellShadown;
			}
			if (!onlyPooled)
			{
				CellShadown cellShadown2 = UnityEngine.Object.Instantiate(objectPrefab);
				cellShadown2.name = objectPrefab.name;
				if (isParent)
				{
					cellShadown2.transform.parent = base.transform;
				}
				if (isSuggest)
				{
					pooledObjectsSuggest.Add(cellShadown2);
				}
				return cellShadown2;
			}
		}
		return null;
	}

	public void PoolObject(CellShadown obj, float time)
	{
		if (time != 0f)
		{
			DOVirtual.DelayedCall(time, delegate
			{
				PoolObject(obj, isDestroy: true);
			});
		}
		else
		{
			PoolObject(obj, isDestroy: false);
		}
	}

	private void PoolObject(CellShadown obj, bool isDestroy)
	{
		if (objectPrefab.name == obj.name)
		{
			if (isDestroy)
			{
				UnityEngine.Object.Destroy(obj.gameObject);
				return;
			}
			obj.gameObject.SetActive(value: false);
			obj.transform.parent = base.transform;
			pooledObjects.Add(obj);
		}
	}

	public void PoolObject()
	{
		for (int i = 0; i < pooledObjects.Count; i++)
		{
			UnityEngine.Object.Destroy(pooledObjects[i].gameObject);
		}
		pooledObjects.Clear();
		pooledObjectsSuggest.Clear();
	}

	public void PoolObjectChild()
	{
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				UnityEngine.Object.Destroy(transform.gameObject);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		pooledObjects.Clear();
		pooledObjectsSuggest.Clear();
	}

	public void ResetSuggest()
	{
		for (int i = 0; i < pooledObjectsSuggest.Count; i++)
		{
			PoolObject(pooledObjectsSuggest[i], isDestroy: false);
		}
		pooledObjectsSuggest.Clear();
	}
}
