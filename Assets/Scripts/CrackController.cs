/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using Spine.Unity;
using UnityEngine;

public class CrackController : MonoBehaviour
{
	private enum mColor
	{
		skyblue,
		green,
		pink,
		red,
		purple,
		orange
	}

	public SkeletonAnimation mSkeletonGraphic;

	public Transform mTransform;

	private string[] mStatus = new string[6]
	{
		"skyblue",
		"green",
		"pink",
		"red",
		"purple",
		"orange"
	};

	public void SetTranform(float scale, Vector3 position, int idSprite)
	{
		mTransform.localScale = Vector3.one * scale * Data.scaleBlockTypeGame;
		mTransform.position = position - new Vector3(0f, 0f, 0.2f);
		mSkeletonGraphic.AnimationState.SetAnimation(0, mStatus[idSprite], loop: true);
	}
}
