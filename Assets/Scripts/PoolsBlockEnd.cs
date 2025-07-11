/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using System.Collections.Generic;
using UnityEngine;

public class PoolsBlockEnd : MonoBehaviour
{
	public static PoolsBlockEnd instance;

	public BlockEndGame objectPrefab;

	private List<BlockEndGame> pooledObjects;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		pooledObjects = new List<BlockEndGame>();
	}

	public BlockEndGame GetObjectForType()
	{
		BlockEndGame blockEndGame = UnityEngine.Object.Instantiate(objectPrefab);
		blockEndGame.name = objectPrefab.name;
		blockEndGame.transform.parent = base.transform;
		pooledObjects.Add(blockEndGame);
		return blockEndGame;
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
