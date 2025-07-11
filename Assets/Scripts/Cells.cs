/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Cells : MonoBehaviour
{
	private Cell[,] cells;

	private Transform[,] transformsCells;

	public Cell cellPrefab;

	public static Cells instance;

	private List<GameObject> listUnique;

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
		listUnique = new List<GameObject>();
	}

	private void Start()
	{
		cells = new Cell[Data.MATRIX_N, Data.MATRIX_N];
		DOVirtual.DelayedCall(0.7f, delegate
		{
			cells = CreateCells(cellPrefab);
			SetTransformsCells();
		});
	}

	private Cell[,] CreateCells(Cell cellPrefab)
	{
		Cell[,] array = new Cell[Data.MATRIX_N, Data.MATRIX_N];
		for (int i = 0; i < Data.MATRIX_N; i++)
		{
			for (int j = 0; j < Data.MATRIX_N; j++)
			{
				Cell cell = UnityEngine.Object.Instantiate(cellPrefab);
				cell.Clear();
				cell.transform.parent = base.transform;
				cell.transform.localPosition = Data.MatrixToPos(i, j);
				cell.transform.localScale = Vector3.one * Data.scaleBlockTypeGame;
				array[i, j] = cell;
				int index = (Data.MATRIX_N - i - 1 + j) / (2 * Data.MATRIX_N - 1) + (Data.MATRIX_N - i - 1 + j) % (2 * Data.MATRIX_N - 1);
				cell.Effect(Color.white, index);
				cell.gameObject.SetActive(value: true);
			}
		}
		return array;
	}

	public void EffectIn()
	{
		for (int i = 0; i < Data.MATRIX_N; i++)
		{
			for (int j = 0; j < Data.MATRIX_N; j++)
			{
				int index = (Data.MATRIX_N - i - 1 + j) / (2 * Data.MATRIX_N - 1) + (Data.MATRIX_N - i - 1 + j) % (2 * Data.MATRIX_N - 1);
				cells[i, j].Effect(Color.white, index);
			}
		}
	}

	private void SetTransformsCells()
	{
		transformsCells = new Transform[Data.MATRIX_N, Data.MATRIX_N];
		for (int i = 0; i < Data.MATRIX_N; i++)
		{
			for (int j = 0; j < Data.MATRIX_N; j++)
			{
				transformsCells[i, j] = cells[i, j].transform;
			}
		}
	}

	public bool IsActiveCell(int i, int j)
	{
		if (cells[i, j].Status == 0)
		{
			return false;
		}
		return true;
	}

	public Vector2 GetAveragePosCells(BPoint[] matrix)
	{
		Vector2 zero = Vector2.zero;
		for (int i = 0; i < matrix.Length; i++)
		{
			float x = zero.x;
			Vector3 position = transformsCells[matrix[i].x, matrix[i].y].position;
			zero.x = x + position.x;
			float y = zero.y;
			Vector3 position2 = transformsCells[matrix[i].x, matrix[i].y].position;
			zero.y = y + position2.y;
		}
		zero.x /= matrix.Length;
		zero.y /= matrix.Length;
		return zero;
	}

	public int CheckFullCells(BPoint[] matrix, int idTypeBlock)
	{
		int num = 0;
		bool flag = true;
		bool flag2 = true;
		HashSet<int> hashSet = new HashSet<int>();
		HashSet<int> hashSet2 = new HashSet<int>();
		for (int i = 0; i < matrix.Length; i++)
		{
			hashSet.Add(matrix[i].x);
			hashSet2.Add(matrix[i].y);
		}
		foreach (int item in hashSet)
		{
			flag = true;
			for (int j = 0; j < Data.MATRIX_N; j++)
			{
				if (!IsActiveCell(item, j))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				num++;
				for (int k = 0; k < Data.MATRIX_N; k++)
				{
					cells[item, k].IsAbilityEat = true;
					if (cells[item, k].Status == 4)
					{
						Data.isHaveRocketFly = true;
					}
				}
			}
		}
		foreach (int item2 in hashSet2)
		{
			flag2 = true;
			for (int l = 0; l < Data.MATRIX_N; l++)
			{
				if (!IsActiveCell(l, item2))
				{
					flag2 = false;
					break;
				}
			}
			if (flag2)
			{
				num++;
				for (int m = 0; m < Data.MATRIX_N; m++)
				{
					cells[m, item2].IsAbilityEat = true;
					if (cells[m, item2].Status == 4)
					{
						Data.isHaveRocketFly = true;
					}
				}
			}
		}
		if (num > 0)
		{
			SoundManager.PlaySound("erase");
			if (Data.countHelp >= 4)
			{
				Manager.instance.SetScore(GetScore(num));
			}
			SetLabelScore(matrix, num, idTypeBlock);
		}
		return num;
	}

	public void CheckAbilitySpriteCells(BPoint[] matrix, int idSprite)
	{
		HashSet<int> hashSet = new HashSet<int>();
		HashSet<int> hashSet2 = new HashSet<int>();
		for (int i = 0; i < matrix.Length; i++)
		{
			cells[matrix[i].x, matrix[i].y].IsAbilityFull = true;
			hashSet.Add(matrix[i].x);
			hashSet2.Add(matrix[i].y);
		}
		foreach (int item in hashSet)
		{
			bool flag = true;
			for (int j = 0; j < Data.MATRIX_N; j++)
			{
				if (!cells[item, j].IsAbilityFull)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				for (int k = 0; k < Data.MATRIX_N; k++)
				{
					cells[item, k].ChangeSprite(idSprite, item, k);
				}
			}
		}
		foreach (int item2 in hashSet2)
		{
			bool flag2 = true;
			for (int l = 0; l < Data.MATRIX_N; l++)
			{
				if (!cells[l, item2].IsAbilityFull)
				{
					flag2 = false;
					break;
				}
			}
			if (flag2)
			{
				for (int m = 0; m < Data.MATRIX_N; m++)
				{
					cells[m, item2].ChangeSprite(idSprite, m, item2);
				}
			}
		}
	}

	public void ResetAbilitySpriteCells(BPoint[] matrix)
	{
		if (matrix[0] != null)
		{
			HashSet<int> hashSet = new HashSet<int>();
			HashSet<int> hashSet2 = new HashSet<int>();
			for (int i = 0; i < matrix.Length; i++)
			{
				cells[matrix[i].x, matrix[i].y].IsAbilityFull = false;
				hashSet.Add(matrix[i].x);
				hashSet2.Add(matrix[i].y);
			}
			foreach (int item in hashSet)
			{
				for (int j = 0; j < Data.MATRIX_N; j++)
				{
					cells[item, j].ResetSprite(item, j);
				}
			}
			foreach (int item2 in hashSet2)
			{
				for (int k = 0; k < Data.MATRIX_N; k++)
				{
					cells[k, item2].ResetSprite(k, item2);
				}
			}
		}
	}

	public void SetSuggests(float scale, BPoint[] matrix, int idSprite)
	{
		if (!Data.isPauseGame)
		{
			for (int i = 0; i < matrix.Length; i++)
			{
				CellShadown objectForType = PoolsCellShadown.instance.GetObjectForType("CellShadown", onlyPooled: false, isParent: true, isSuggest: true);
				objectForType.SetSprite(idSprite, new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 111));
				objectForType.SetTranform(scale, transformsCells[matrix[i].x, matrix[i].y].position);
			}
		}
	}

	public void SetUnique(BPoint[] matrix)
	{
		for (int i = 0; i < matrix.Length; i++)
		{
			GameObject objectForType = Pools.instance.GetObjectForType("Suggest");
			objectForType.transform.position = transformsCells[matrix[i].x, matrix[i].y].position - new Vector3(0f, 0f, 0.05f);
			objectForType.transform.parent = base.transform;
			objectForType.transform.localScale = Vector3.one * Data.scaleBlockTypeGame;
			listUnique.Add(objectForType);
		}
	}

	public void ResetUnique()
	{
		for (int i = 0; i < listUnique.Count; i++)
		{
			Pools.instance.PoolObject(listUnique[i], 0f);
		}
		listUnique.Clear();
	}

	private int GetScore(int row)
	{
		switch (row)
		{
		case 1:
			return 72;
		case 2:
			SoundManager.PlaySound("sound_good");
			return 216;
		case 3:
			SoundManager.PlaySound("sound_great");
			return 432;
		case 4:
			SoundManager.PlaySound("sound_amazing");
			return 864;
		default:
			SoundManager.PlaySound("sound_exilent");
			return 1440;
		}
	}

	private void SetLabelScore(BPoint[] matrix, int row, int idTypeBlock)
	{
		int num = 0;
		Vector3 a = Vector3.zero;
		for (int i = 0; i < matrix.Length; i++)
		{
			if (cells[matrix[i].x, matrix[i].y].IsAbilityEat)
			{
				a += transformsCells[matrix[i].x, matrix[i].y].position;
				num++;
			}
		}
		if (num > 0)
		{
			a /= num;
			Manager.instance.PlaySkeletonGraphic(isPlay: true);
			ScoreAndEffect.instance.SetActive(row, idTypeBlock, new Vector3(a.x, a.y, -3f), GetScore(row));
		}
	}

	public void SetActiveCell(bool isActive, int idSprite, int status, int i, int j)
	{
		cells[i, j].SetActive(isActive, idSprite, status);
		if (Data.idTypeGame == 11 && !isActive)
		{
			for (int k = 0; k < PoolsBomb.instance.listBomb.Count; k++)
			{
				if (PoolsBomb.instance.listBomb[k].CheckVisible(i, j))
				{
					PoolsBomb.instance.listBomb[k].IsEat = true;
				}
			}
		}
		if (Data.idTypeGame != 9 || isActive)
		{
			return;
		}
		for (int l = 0; l < PoolAddTime.instance.listAddTime.Count; l++)
		{
			if (PoolAddTime.instance.listAddTime[l].CheckVisible(i, j))
			{
				PoolAddTime.instance.listAddTime[l].IsEat = true;
			}
		}
	}

	public void SetActiveEffect(int idSprite, int i, int j, int delta)
	{
		CellShadown objectForType = PoolsCellShadown.instance.GetObjectForType("CellShadown", onlyPooled: false, isParent: true);
		objectForType.SetSprite(idSprite, new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
		objectForType.SetTranform(1f, transformsCells[i, j].transform.position, delta);
		float time = (float)Mathf.Abs(delta) * 0.1f;
		cells[i, j].SetSpriteMask(isActive: false, time);
		for (int k = 0; k < PoolsRocket.instance.pooledObjects.Count; k++)
		{
			if (PoolsRocket.instance.pooledObjects[k].CheckVisible(i, j))
			{
				PoolsRocket.instance.pooledObjects[k].SetInvisible(delta);
			}
		}
		if (Data.idTypeGame == 11)
		{
			for (int l = 0; l < PoolsBomb.instance.listBomb.Count; l++)
			{
				if (PoolsBomb.instance.listBomb[l].CheckVisible(i, j))
				{
					PoolsBomb.instance.PoolObject(PoolsBomb.instance.listBomb[l], time);
				}
			}
		}
		if (Data.idTypeGame != 9)
		{
			return;
		}
		for (int m = 0; m < PoolAddTime.instance.listAddTime.Count; m++)
		{
			if (PoolAddTime.instance.listAddTime[m].CheckVisible(i, j))
			{
				PoolAddTime.instance.PoolObject(PoolAddTime.instance.listAddTime[m], time);
			}
		}
	}

	public void SetActiveEffect(int idSprite, int i, int j, float delta)
	{
		CellShadown objectForType = PoolsCellShadown.instance.GetObjectForType("CellShadown", onlyPooled: false, isParent: true);
		objectForType.SetSprite(idSprite, new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
		objectForType.SetTranform(1f, transformsCells[i, j].transform.position, delta);
		cells[i, j].SetSpriteMask(isActive: false, delta);
	}

	public void SetActiveCells(bool isActive, int idJewel, BPoint[] matrix)
	{
		for (int i = 0; i < matrix.Length; i++)
		{
			SetActiveCell(isActive, idJewel, matrix[i].status, matrix[i].x, matrix[i].y);
		}
	}

	private void EatCell(int i, int j, int delta, int idSprite, bool isReset = false)
	{
		SetActiveCell(isActive: false, 0, 0, i, j);
		cells[i, j].IsAbilityFull = false;
		cells[i, j].IsAbilityEat = false;
		cells[i, j].EatEffect(idSprite, delta, i, j, isReset);
		SetActiveEffect(idSprite, i, j, delta);
	}

	private void EatCell(int i, int j, float delta = 2f)
	{
		if (cells[i, j].Status != 0)
		{
			SetActiveEffect(cells[i, j].IdSprite, i, j, delta);
		}
		SetActiveCell(isActive: false, 0, 0, i, j);
		cells[i, j].IsAbilityFull = false;
		cells[i, j].IsAbilityEat = false;
	}

	public void EatCells(BPoint[] matrix, int idSprite)
	{
		PoolCrack.instance.PoolObject(isDestroy: true);
		for (int i = 0; i < matrix.Length; i++)
		{
			for (int j = 0; j < Data.MATRIX_N; j++)
			{
				if (cells[matrix[i].x, j].IsAbilityEat)
				{
					EatCell(matrix[i].x, j, matrix[i].y - j, idSprite);
				}
			}
			for (int k = 0; k < Data.MATRIX_N; k++)
			{
				if (cells[k, matrix[i].y].IsAbilityEat)
				{
					EatCell(k, matrix[i].y, matrix[i].x - k, idSprite);
				}
			}
		}
		SoundManager.PlaySound("erase");
	}

	public bool EatCells()
	{
		bool result = false;
		for (int i = 0; i < Data.MATRIX_N; i++)
		{
			for (int j = 0; j < Data.MATRIX_N; j++)
			{
				if (IsActiveCell(i, j))
				{
					EatCell(i, j, 0, cells[i, j].IdSprite, isReset: true);
					result = true;
				}
			}
		}
		SoundManager.PlaySound("erase");
		return result;
	}

	public void EatCells(int i, int j, int type)
	{
		switch (type)
		{
		case 0:
			for (int l = j; l < Data.MATRIX_N; l++)
			{
				Vector3 vector = Data.MatrixToPos(i, l);
				float delta = 1f / Vector2.Distance(new Vector2(vector.x, 7f), new Vector2(vector.x, vector.y));
				EatCell(i, l, delta);
			}
			break;
		case 1:
			for (int num = i; num >= 0; num--)
			{
				EatCell(num, j);
			}
			break;
		case 2:
			for (int num2 = j; num2 >= 0; num2--)
			{
				EatCell(i, num2);
			}
			break;
		default:
			for (int k = i; k < Data.MATRIX_N; k++)
			{
				EatCell(k, j);
			}
			break;
		}
	}

	public void EatCells(int i, int j)
	{
		if (cells[i, j].Status != 0)
		{
			EatCell(i, j, 0, cells[i, j].IdSprite);
		}
	}

	public void EatCellEffect(int i, int j)
	{
		for (int k = 0; k < PoolsRocket.instance.pooledObjects.Count; k++)
		{
			if (PoolsRocket.instance.pooledObjects[k].CheckVisible(i, j))
			{
				PoolsRocket.instance.pooledObjects[k].SetInvisible(0);
			}
		}
	}

	public List<GameData.CellData> GetCellsData()
	{
		List<GameData.CellData> list = new List<GameData.CellData>();
		for (int i = 0; i < Data.MATRIX_N; i++)
		{
			for (int j = 0; j < Data.MATRIX_N; j++)
			{
				if (!IsActiveCell(i, j))
				{
					continue;
				}
				if (Data.idTypeGame == 11)
				{
					int count = 0;
					for (int k = 0; k < PoolsBomb.instance.listBomb.Count; k++)
					{
						if (PoolsBomb.instance.listBomb[k].CheckVisible(i, j))
						{
							count = PoolsBomb.instance.listBomb[k].Count;
						}
					}
					list.Add(new GameData.CellData(i, j, cells[i, j].IdSprite, cells[i, j].Status, count, 0));
				}
				else if (Data.idTypeGame == 9)
				{
					int addTime = 0;
					for (int l = 0; l < PoolAddTime.instance.listAddTime.Count; l++)
					{
						if (PoolAddTime.instance.listAddTime[l].CheckVisible(i, j))
						{
							addTime = PoolAddTime.instance.listAddTime[l].Count;
						}
					}
					list.Add(new GameData.CellData(i, j, cells[i, j].IdSprite, cells[i, j].Status, 0, addTime));
				}
				else
				{
					list.Add(new GameData.CellData(i, j, cells[i, j].IdSprite, cells[i, j].Status, 0, 0));
				}
			}
		}
		return list;
	}

	public void GameOverEffect()
	{
		for (int i = 0; i < Data.MATRIX_N; i++)
		{
			for (int j = 0; j < Data.MATRIX_N; j++)
			{
				if (IsActiveCell(i, j))
				{
					cells[i, j].Clear((float)i * 0.1f);
				}
			}
		}
	}

	public void ResetCells()
	{
		for (int i = 0; i < Data.MATRIX_N; i++)
		{
			for (int j = 0; j < Data.MATRIX_N; j++)
			{
				SetActiveCell(isActive: false, 0, 0, i, j);
				cells[i, j].Reset();
				cells[i, j].Clear();
			}
		}
		PoolsBomb.instance.PoolObject();
		PoolAddTime.instance.PoolObject();
	}

	public void EatCellsSaveMe(int xRandom, int yRandom)
	{
		int countCell = 0;
		int i;
		for (i = xRandom; i < xRandom + 5; i++)
		{
			int j;
			for (j = yRandom; j < yRandom + 5; j++)
			{
				if (IsActiveCell(i, j))
				{
					countCell++;
					transformsCells[i, j].DOShakeRotation(1.5f, new Vector3(0f, 0f, 10f)).SetDelay(UnityEngine.Random.Range(0.1f, 0.3f)).OnComplete(delegate
					{
						countCell--;
						transformsCells[i, j].DOKill();
						transformsCells[i, j].eulerAngles = new Vector3(0f, 0f, 0f);
						if (countCell <= 0)
						{
							SoundManager.PlaySound("erase");
							for (int k = xRandom; k < xRandom + 5; k++)
							{
								for (int l = yRandom; l < yRandom + 5; l++)
								{
									EatCell(k, l, 0, cells[k, l].IdSprite);
								}
							}
							Data.isGameOver = false;
							Manager.instance.CheckAbilitiContinue();
						}
					});
				}
			}
		}
	}

	public float GetZIndex(int i, int j)
	{
		Vector3 localPosition = transformsCells[i, j].localPosition;
		return localPosition.z;
	}

	public bool SetActiveBomb()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		do
		{
			num = UnityEngine.Random.Range(0, Data.MATRIX_N);
			num2 = UnityEngine.Random.Range(0, Data.MATRIX_N);
			num3++;
		}
		while ((cells[num, num2].IsBomb() || !IsActiveCell(num, num2) || cells[num, num2].IsRocket() || cells[num, num2].IsAddTime()) && num3 < Data.MATRIX_N * Data.MATRIX_N);
		if (num3 != Data.MATRIX_N * Data.MATRIX_N)
		{
			cells[num, num2].Status = 3;
			cells[num, num2].ChangeColor(new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 200));
			Bomb objectForType = PoolsBomb.instance.GetObjectForType();
			objectForType.SetTranform(transformsCells[num, num2].position - new Vector3(0f, 0f, 0.3f), num, num2);
			objectForType.SetCount(10);
			objectForType.SetState(0);
			return true;
		}
		return false;
	}

	public void SetActiveRocket(int max)
	{
		for (int i = 0; i < max; i++)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			do
			{
				num = UnityEngine.Random.Range(0, Data.MATRIX_N);
				num2 = UnityEngine.Random.Range(0, Data.MATRIX_N);
				num3++;
			}
			while ((cells[num, num2].IsBomb() || !IsActiveCell(num, num2) || cells[num, num2].IsRocket() || cells[num, num2].IsAddTime()) && num3 < Data.MATRIX_N * Data.MATRIX_N);
			if (num3 != Data.MATRIX_N * Data.MATRIX_N)
			{
				CreateRocket(num, num2);
			}
		}
	}

	public void SetActiveAddTime(int max)
	{
		for (int i = 0; i < max; i++)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			do
			{
				num = UnityEngine.Random.Range(0, Data.MATRIX_N);
				num2 = UnityEngine.Random.Range(0, Data.MATRIX_N);
				num3++;
			}
			while ((cells[num, num2].IsBomb() || !IsActiveCell(num, num2) || cells[num, num2].IsRocket() || cells[num, num2].IsAddTime()) && num3 < Data.MATRIX_N * Data.MATRIX_N);
			if (num3 != Data.MATRIX_N * Data.MATRIX_N)
			{
				int time = UnityEngine.Random.Range(5, 11);
				CreateAddTime(num, num2, time);
			}
		}
	}

	public bool CheckExplose()
	{
		bool result = false;
		for (int i = 0; i < PoolsBomb.instance.listBomb.Count; i++)
		{
			if (!PoolsBomb.instance.listBomb[i].IsEat)
			{
				PoolsBomb.instance.listBomb[i].Count--;
				PoolsBomb.instance.listBomb[i].SetTxtCount();
				if (PoolsBomb.instance.listBomb[i].Count <= 0 && cells[PoolsBomb.instance.listBomb[i].IBomb, PoolsBomb.instance.listBomb[i].JBomb].Status != 0)
				{
					PoolsBomb.instance.listBomb[i].SetState(2);
					result = true;
					break;
				}
			}
		}
		return result;
	}

	public void CheckAddTime()
	{
		for (int i = 0; i < PoolAddTime.instance.listAddTime.Count; i++)
		{
			if (!PoolAddTime.instance.listAddTime[i].IsEat)
			{
				PoolAddTime.instance.listAddTime[i].Count--;
				PoolAddTime.instance.listAddTime[i].SetTxtCount();
				if (PoolAddTime.instance.listAddTime[i].Count <= 0)
				{
					int iAddTime = PoolAddTime.instance.listAddTime[i].IAddTime;
					int jAddTime = PoolAddTime.instance.listAddTime[i].JAddTime;
					cells[iAddTime, jAddTime].Status = 1;
					cells[iAddTime, jAddTime].ChangeColor(new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
					PoolAddTime.instance.PoolObject(PoolAddTime.instance.listAddTime[i], 0f);
				}
			}
		}
	}

	public void CreateBomb(int i, int j, int count)
	{
		Bomb objectForType = PoolsBomb.instance.GetObjectForType();
		objectForType.SetTranform(transformsCells[i, j].position - new Vector3(0f, 0f, 0.1f), i, j);
		objectForType.SetCount(count);
		if (count <= 5)
		{
			objectForType.SetState(1);
		}
	}

	public void CreateRocket(int i, int j)
	{
		cells[i, j].Status = 4;
		cells[i, j].ChangeColor(new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 200));
		Rocket rocket = PoolsRocket.instance.GetRocket();
		rocket.SetTranform(transformsCells[i, j].position - new Vector3(0f, 0f, 0.3f), i, j);
	}

	public void CreateAddTime(int i, int j, int time)
	{
		cells[i, j].Status = 5;
		cells[i, j].ChangeColor(new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 200));
		TimeAdd objectForType = PoolAddTime.instance.GetObjectForType();
		objectForType.SetCount(time);
		objectForType.SetTranform(transformsCells[i, j].position - new Vector3(0f, 0f, 0.3f), i, j);
	}

	public void ResetEffect()
	{
		int num = Data.MATRIX_N - 1;
		for (int num2 = num; num2 >= 0; num2--)
		{
			for (int num3 = num; num3 >= 0; num3--)
			{
				int index = (num - num3 + num2) / (2 * num + 1) + (num - num3 + num2) % (2 * num + 1);
				cells[num2, num3].Effect(Color.clear, index);
			}
		}
	}

	public void SetSpriteMask(bool isActive)
	{
		if (isActive)
		{
			for (int i = 0; i < Data.MATRIX_N; i++)
			{
				for (int j = 0; j < Data.MATRIX_N; j++)
				{
					if (cells[i, j].Status != 0)
					{
						cells[i, j].SetSpriteMask(isActive: true);
					}
				}
			}
			return;
		}
		for (int k = 0; k < Data.MATRIX_N; k++)
		{
			for (int l = 0; l < Data.MATRIX_N; l++)
			{
				cells[k, l].SetSpriteMask(isActive: false);
			}
		}
	}
}
