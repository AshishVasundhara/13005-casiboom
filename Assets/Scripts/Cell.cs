/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using DG.Tweening;
using UnityEngine;

public class Cell : MonoBehaviour
{
	public SpriteRenderer sprFront;

	private int status;

	public SpriteMask spriteMask;

	private bool isAbilityFull;

	public int Status
	{
		get
		{
			return status;
		}
		set
		{
			status = value;
			if (!base.gameObject.activeSelf)
			{
				status = 0;
			}
		}
	}

	public bool IsAbilityEat
	{
		get;
		set;
	}

	public int IdSprite
	{
		get;
		set;
	}

	public bool IsAbilityFull
	{
		get
		{
			return isAbilityFull;
		}
		set
		{
			isAbilityFull = value;
			if (Status != 0)
			{
				isAbilityFull = true;
			}
		}
	}

	public void EatEffect(int idSprite, int delta, int i, int j, bool isReset = false)
	{
		DOVirtual.DelayedCall((float)Mathf.Abs(delta) * 0.1f, delegate
		{
			GameObject objectForType = Pools.instance.GetObjectForType(idSprite.ToString());
			Transform transform = objectForType.transform;
			Vector3 position = base.transform.position;
			float x = position.x;
			Vector3 position2 = base.transform.position;
			transform.position = new Vector3(x, position2.y, -4f);
			Pools.instance.PoolObject(objectForType, 1.5f);
		});
	}

	public void Clear(float delta)
	{
		BlockEndGame objectForType = PoolsBlockEnd.instance.GetObjectForType();
		objectForType.SetTranform(base.transform);
		if (Data.idTypeGame == 11)
		{
			objectForType.SetSprite(MyResources.instance.spriteCellBomb);
		}
		if (Data.idTypeGame == 9)
		{
			objectForType.SetSprite(MyResources.instance.spriteCellTime);
		}
		DOVirtual.DelayedCall(delta, delegate
		{
			float duration = UnityEngine.Random.Range(1f, 6f);
			sprFront.DOColor(Color.clear, duration);
		});
	}

	public void Reset()
	{
		sprFront.color = Color.clear;
		IsAbilityFull = false;
	}

	public void SetActive(bool isActive, int idSprite, int status)
	{
		IdSprite = idSprite;
		if (status != 0)
		{
			sprFront.sprite = MyResources.instance.spriteCellFront[idSprite];
		}
		else
		{
			sprFront.color = Color.white;
			sprFront.sprite = MyResources.instance.spriteNet;
		}
		this.status = status;
		if (status != 0)
		{
			base.gameObject.SetActive(isActive);
		}
		IsAbilityFull = isActive;
	}

	public bool IsStar()
	{
		if (status == 2)
		{
			return true;
		}
		return false;
	}

	public bool IsBomb()
	{
		if (status == 3)
		{
			return true;
		}
		return false;
	}

	public bool IsRocket()
	{
		if (status == 4)
		{
			return true;
		}
		return false;
	}

	public bool IsAddTime()
	{
		if (status == 5)
		{
			return true;
		}
		return false;
	}

	public void ChangeSprite(int idSprite, int i, int j)
	{
		sprFront.sprite = MyResources.instance.spriteNet;
		CrackController objectForType = PoolCrack.instance.GetObjectForType();
		objectForType.SetTranform(1f, base.transform.position, idSprite);
		if (status == 3)
		{
			PoolsBomb.instance.Effect(i, j, isShake: true);
		}
		else if (status == 4)
		{
			PoolsRocket.instance.Effect(i, j, isShake: true);
		}
		else if (status == 5)
		{
			PoolAddTime.instance.Effect(i, j, isShake: true);
		}
	}

	public void ResetSprite(int i, int j)
	{
		if (status != 0)
		{
			sprFront.sprite = MyResources.instance.spriteCellFront[IdSprite];
			PoolCrack.instance.PoolObject(isDestroy: false);
		}
		else
		{
			sprFront.sprite = MyResources.instance.spriteNet;
		}
		if (status == 3)
		{
			PoolsBomb.instance.Effect(i, j, isShake: false);
		}
		else if (status == 4)
		{
			PoolsRocket.instance.Effect(i, j, isShake: false);
		}
		else if (status == 5)
		{
			PoolAddTime.instance.Effect(i, j, isShake: false);
		}
	}

	public void Effect(Color color, int index)
	{
		DOVirtual.DelayedCall((float)index * 0.05f + 0.5f, delegate
		{
			sprFront.DOColor(color, 0.1f);
		});
	}

	public void Clear()
	{
		sprFront.color = Color.clear;
	}

	public void ChangeColor(Color color)
	{
		sprFront.color = color;
	}

	public void SetSpriteMask(bool isActive, float time = 0f)
	{
		if (time == 0f)
		{
			spriteMask.enabled = isActive;
		}
		else
		{
			DOVirtual.DelayedCall(time, delegate
			{
				spriteMask.enabled = isActive;
			});
		}
	}
}
