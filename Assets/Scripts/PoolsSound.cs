/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class PoolsSound : MonoBehaviour
{
	public static PoolsSound instance;

	public Sound objectPrefab;

	private List<Sound> pooledObjects;

	public Dictionary<string, AudioClip> audioClips;

	private void Awake()
	{
		if (!instance)
		{
			instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		Object.DontDestroyOnLoad(base.gameObject);
		audioClips = new Dictionary<string, AudioClip>();
		LoadSound();
	}

	private void Start()
	{
		pooledObjects = new List<Sound>();
	}

	public Sound GetObjectForType(string objectType, bool onlyPooled, bool isParent = false)
	{
		if (pooledObjects.Count > 0)
		{
			Sound sound = pooledObjects[0];
			pooledObjects.RemoveAt(0);
			sound.gameObject.SetActive(value: true);
			if (isParent)
			{
				sound.transform.parent = base.transform;
			}
			return sound;
		}
		if (!onlyPooled)
		{
			Sound sound2 = UnityEngine.Object.Instantiate(objectPrefab);
			sound2.name = objectPrefab.name;
			if (isParent)
			{
				sound2.transform.parent = base.transform;
			}
			return sound2;
		}
		return null;
	}

	public void PoolObject(Sound obj, float time)
	{
		if (time != 0f)
		{
			DOVirtual.DelayedCall(time, delegate
			{
				obj.gameObject.SetActive(value: false);
				obj.transform.parent = base.transform;
				pooledObjects.Add(obj);
			});
			return;
		}
		obj.gameObject.SetActive(value: false);
		obj.transform.parent = base.transform;
		pooledObjects.Add(obj);
	}

	private void LoadSound()
	{
		AudioClip[] array = Resources.LoadAll<AudioClip>("Sounds");
		for (int i = 0; i < array.Length; i++)
		{
			audioClips.Add(array[i].name, array[i]);
		}
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
