/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using UnityEngine;

public class Sound : MonoBehaviour
{
	public AudioSource audioSource;

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public void PlaySound(AudioClip audioClip, float volume = 1f, bool isLoop = false)
	{
		audioSource.clip = audioClip;
		audioSource.volume = volume;
		audioSource.Play();
		if (!isLoop)
		{
			PoolsSound.instance.PoolObject(this, audioClip.length);
		}
		else
		{
			audioSource.loop = true;
		}
	}
}
