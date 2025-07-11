/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using UnityEngine;

public class BlockEndGame : MonoBehaviour
{
	public SpriteRenderer spriteRenderer;

	public void SetTranform(Transform transform)
	{
		base.transform.position = transform.position + new Vector3(0f, 0f, 0.1f);
		base.transform.localScale = Vector3.one * Data.scaleBlockTypeGame;
		base.transform.parent = transform;
	}

	public void SetSprite(Sprite sprite)
	{
		spriteRenderer.sprite = sprite;
	}
}
