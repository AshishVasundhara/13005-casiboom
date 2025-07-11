/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class Block : MonoBehaviour
{
	public List<BPoint> matrix;

	public int id;

	public Transform[] trans;

	public float posY;

	private SpriteRenderer[] sprsBlockFront;

	private BPoint bPointFirstCurent;

	private BPoint bPointFirst;

	public BPoint[] matrixLocal;

	private BPoint[] matrixWorld;

	private bool isVisible;

	public int idPosInBot;

	public int idSprite;

	private Transform mTranform;

	private bool isEffectMove;

	private void Awake()
	{
		sprsBlockFront = new SpriteRenderer[trans.Length];
		for (int i = 0; i < trans.Length; i++)
		{
			sprsBlockFront[i] = trans[i].GetComponent<SpriteRenderer>();
			trans[i].localPosition = trans[i].localPosition * (Data.DISTANCE_BLOCK / 0.85f);
		}
		mTranform = base.transform;
		Vector3 position = mTranform.position;
		posY = position.y;
		bPointFirstCurent = new BPoint();
		bPointFirst = new BPoint();
		matrixLocal = new BPoint[trans.Length];
		SetMatrix();
	}

	private void Start()
	{
		isEffectMove = false;
		matrixWorld = new BPoint[trans.Length];
		ScaleChild(0.9f);
	}

	private void SetMatrix()
	{
		Vector3 localScale = mTranform.localScale;
		float x = localScale.x;
		Vector3 position = trans[0].position;
		for (int i = 0; i < trans.Length; i++)
		{
			Vector3 position2 = trans[i].position;
			float num = position2.x - position.x;
			float num2 = position2.y - position.y;
			int y = Mathf.RoundToInt(num / (Data.DISTANCE_BLOCK * x));
			int x2 = Mathf.RoundToInt(num2 / (Data.DISTANCE_BLOCK * x));
			matrixLocal.SetValue(new BPoint(x2, y, 1), i);
		}
	}

	public void SetTransform(Vector3 pos, Vector3 scale)
	{
		if (isEffectMove)
		{
			mTranform.position = pos;
			mTranform.localScale = scale;
		}
		else
		{
			mTranform.DOMove(pos, 0.1f).OnComplete(delegate
			{
				isEffectMove = true;
			});
			mTranform.DOScale(scale, 0.1f);
		}
	}

	public void SetTransform(float posX, Vector3 scale)
	{
		mTranform.localPosition = new Vector3(posX, 0f, -2f);
		mTranform.localScale = scale;
	}

	private void SetMove()
	{
		bPointFirst = Data.PosToMatrix(trans[0].position);
		if (bPointFirst.x == bPointFirstCurent.x && bPointFirst.y == bPointFirstCurent.y)
		{
			return;
		}
		Cells.instance.ResetAbilitySpriteCells(matrixWorld);
		PoolsCellShadown.instance.ResetSuggest();
		bPointFirstCurent = bPointFirst;
		for (int i = 0; i < trans.Length; i++)
		{
			BPoint bPoint = LocalToWorld(i, bPointFirst.x, bPointFirst.y);
			if (bPoint.x < 0)
			{
				bPoint.x = 0;
			}
			if (bPoint.x >= Data.MATRIX_N)
			{
				bPoint.x = Data.MATRIX_N - 1;
			}
			if (bPoint.y < 0)
			{
				bPoint.y = 0;
			}
			if (bPoint.y >= Data.MATRIX_N)
			{
				bPoint.y = Data.MATRIX_N - 1;
			}
			matrixWorld.SetValue(bPoint, i);
		}
		if (CheckAbilitiInMatrixBlock(bPointFirst.x, bPointFirst.y))
		{
			Cells.instance.SetSuggests(0.7f, matrixWorld, idSprite);
			Cells.instance.CheckAbilitySpriteCells(matrixWorld, idSprite);
		}
	}

	private BPoint LocalToWorld(int countLocalMatrix, int iFirstWorldMatrix, int jFirstWorldMatrix)
	{
		return new BPoint(iFirstWorldMatrix + matrixLocal[countLocalMatrix].x, jFirstWorldMatrix + matrixLocal[countLocalMatrix].y, 1);
	}

	public BPoint[] GetMatrixWorld(int iFirstWorldMatrix, int jFirstWorldMatrix)
	{
		BPoint[] array = new BPoint[trans.Length];
		for (int i = 0; i < trans.Length; i++)
		{
			BPoint value = LocalToWorld(i, iFirstWorldMatrix, jFirstWorldMatrix);
			array.SetValue(value, i);
		}
		return array;
	}

	public void Down(Vector2 touchDown)
	{
		isEffectMove = false;
		if (!Data.isPauseGame)
		{
			mTranform.DOScale(1.1f, 0.1f);
			Transform target = mTranform;
			float x = touchDown.x;
			float y = touchDown.y + 2.6f;
			Vector3 position = mTranform.position;
			target.DOMove(new Vector3(x, y, position.z), 0.1f).OnComplete(delegate
			{
				isEffectMove = true;
			});
			SetMove();
			if (Data.countHelp < 4)
			{
				Manager.instance.SetGuilHand(is_active: false);
			}
		}
		else
		{
			SetTransform(new Vector3(Data.POS_BLOCK_BOT[idPosInBot], posY, -2f), Vector3.one * Data.SCALE_BOT);
			PoolsCellShadown.instance.PoolObjectChild();
		}
	}

	public void Drag(Vector2 touchInput)
	{
		if (!Data.isPauseGame)
		{
			if (isEffectMove)
			{
				Transform transform = mTranform;
				float x = touchInput.x;
				float y = touchInput.y + 2.6f;
				Vector3 position = mTranform.position;
				transform.position = new Vector3(x, y, position.z);
				SetMove();
			}
		}
		else
		{
			SetTransform(new Vector3(Data.POS_BLOCK_BOT[idPosInBot], posY, -2f), Vector3.one * Data.SCALE_BOT);
			PoolsCellShadown.instance.PoolObjectChild();
		}
	}

	public void Up()
	{
		if (!Data.isPauseGame)
		{
			PoolCrack.instance.PoolObject(isDestroy: true);
			if (CheckAbilitiInMatrixBlock(bPointFirst.x, bPointFirst.y))
			{
				SoundManager.PlaySound("move", 0.8f);
				PoolsCellShadown.instance.ResetSuggest();
				Cells.instance.ResetUnique();
				if (Data.countHelp >= 4)
				{
					Manager.instance.SetScore(trans.Length);
				}
				Cells.instance.SetActiveCells(isActive: true, idSprite, matrixWorld);
				if (Cells.instance.CheckFullCells(matrixWorld, idSprite) > 0)
				{
					Cells.instance.EatCells(matrixWorld, idSprite);
				}
				else if (!Data.isGameOver)
				{
					Manager.instance.CreateBomb();
					Manager.instance.CreateRocket();
					Manager.instance.CreateAddTime();
				}
				Manager.instance.CheckBomb();
				if (Data.countHelp < 4)
				{
					Data.countHelp++;
					DOVirtual.DelayedCall(1f, delegate
					{
						Manager.instance.CheckAndCreateBlocks(this, 3);
					});
					if (Data.countHelp == 4)
					{
						Manager.instance.SetGuilHand(is_active: false);
						Utilities.PlayerPrefs.SetInt("BlockGudianblock_gudian_help", Data.countHelp);
						Utilities.PlayerPrefs.Flush();
					}
				}
				else
				{
					Manager.instance.CheckAndCreateBlocks(this, 3);
				}
				DOTween.Kill(mTranform);
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				isEffectMove = false;
				if (Data.countHelp < 3)
				{
					Manager.instance.SetGuilHand(is_active: true);
				}
				SoundManager.PlaySound("move_bad", 0.8f);
				SetTransform(new Vector3(Data.POS_BLOCK_BOT[idPosInBot], posY, -2f), Vector3.one * Data.SCALE_BOT);
				SetColors(isVisible);
			}
		}
		else
		{
			isEffectMove = false;
			SetTransform(new Vector3(Data.POS_BLOCK_BOT[idPosInBot], posY, -2f), Vector3.one * Data.SCALE_BOT);
			PoolsCellShadown.instance.PoolObjectChild();
		}
	}

	public bool CheckAbilitiInMatrixBlock(int iFirstWorldMatrixi, int jFirstWorldMatrix)
	{
		if (Data.countHelp < 3)
		{
			if (Data.countHelp == 0)
			{
				if (iFirstWorldMatrixi == 5 && jFirstWorldMatrix == 3)
				{
					return true;
				}
				return false;
			}
			if (Data.countHelp == 1)
			{
				if (iFirstWorldMatrixi == 6 && jFirstWorldMatrix == 5)
				{
					return true;
				}
				return false;
			}
			if (Data.countHelp == 2)
			{
				if (iFirstWorldMatrixi == 5 && jFirstWorldMatrix == 4)
				{
					return true;
				}
				return false;
			}
			return true;
		}
		for (int i = 0; i < trans.Length; i++)
		{
			if (!CheckAbilitiInMatrixChild(i, iFirstWorldMatrixi, jFirstWorldMatrix))
			{
				return false;
			}
		}
		return true;
	}

	private bool CheckAbilitiInMatrixChild(int countLocalMatrix, int iFirstWorldMatrix, int jFirstWorldMatrix)
	{
		BPoint bPoint = LocalToWorld(countLocalMatrix, iFirstWorldMatrix, jFirstWorldMatrix);
		if (bPoint.x < 0 || bPoint.x >= Data.MATRIX_N || bPoint.y < 0 || bPoint.y >= Data.MATRIX_N || Cells.instance.IsActiveCell(bPoint.x, bPoint.y))
		{
			return false;
		}
		return true;
	}

	public void SetColors(bool isVisible)
	{
		this.isVisible = isVisible;
		Color32 color = default(Color32);
		color = (isVisible ? new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue) : new Color32(120, 120, 120, byte.MaxValue));
		for (int i = 0; i < trans.Length; i++)
		{
			sprsBlockFront[i].color = color;
		}
	}

	public void SetSprites()
	{
		for (int i = 0; i < trans.Length; i++)
		{
			sprsBlockFront[i].sprite = MyResources.instance.spriteCellFront[idSprite];
		}
	}

	public GameData.BlockData GetBlockData()
	{
		return new GameData.BlockData(id, idPosInBot, idSprite);
	}

	public void ScaleChild(float scale)
	{
		for (int i = 0; i < trans.Length; i++)
		{
			trans[i].localScale = Vector3.one * scale * Data.scaleBlockTypeGame;
		}
	}
}
