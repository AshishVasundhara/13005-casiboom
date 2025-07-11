/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using UnityEngine;

public class CellShadown : MonoBehaviour
{
	public SpriteRenderer spr;

	public Transform mTransform;

	public void SetSprite(int idSprite, Color32 color)
	{
		spr.sprite = MyResources.instance.spriteCellFront[idSprite];
		spr.color = color;
	}

	public void SetTranform(float scale, Vector3 position, int delta)
	{
		mTransform.localScale = Vector3.one * scale * Data.scaleBlockTypeGame;
		mTransform.position = position - new Vector3(0f, 0f, 0.1f);
		PoolsCellShadown.instance.PoolObject(this, (float)Mathf.Abs(delta) * 0.1f);
	}

	public void SetTranform(float scale, Vector3 position)
	{
		mTransform.localScale = Vector3.one * scale * Data.scaleBlockTypeGame;
		mTransform.position = position - new Vector3(0f, 0f, 0.1f);
	}

	public void SetTranform(float scale, Vector3 position, float delta)
	{
		mTransform.localScale = Vector3.one * scale * Data.scaleBlockTypeGame;
		mTransform.position = position - new Vector3(0f, 0f, 0.1f);
		PoolsCellShadown.instance.PoolObject(this, delta);
	}
}
