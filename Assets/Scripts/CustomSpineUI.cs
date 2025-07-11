/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using Spine.Unity;
using UnityEngine;

public class CustomSpineUI : MonoBehaviour
{
	public SkeletonGraphic skeletonAnimation;

	[Range(0.0166666675f, 0.125f)]
	public float timeInterval = 0.0416666679f;

	private float deltaTime;

	private void Start()
	{
		if (skeletonAnimation == null)
		{
			skeletonAnimation = GetComponent<SkeletonGraphic>();
		}
		skeletonAnimation.Initialize(overwrite: false);
		ManualUpdate();
	}

	private void Update()
	{
		deltaTime += Time.deltaTime;
		if (deltaTime >= timeInterval)
		{
			ManualUpdate();
		}
	}

	private void ManualUpdate()
	{
		skeletonAnimation.Update(deltaTime);
		skeletonAnimation.LateUpdate();
		deltaTime -= timeInterval;
	}
}
