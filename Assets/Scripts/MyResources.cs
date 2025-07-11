/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using UnityEngine;

public class MyResources : MonoBehaviour
{
	public Sprite spriteCellFrontSlive;

	public Sprite spriteCellBomb;

	public Sprite spriteNet;

	public Sprite spriteCellTime;

	public Sprite[] spriteCellFront;

	public Sprite[] endsName;

	public static MyResources instance;

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
	}
}
