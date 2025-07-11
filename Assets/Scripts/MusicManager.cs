/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using UnityEngine;

public class MusicManager : MonoBehaviour
{
	public static MusicManager instance;

	public AudioSource audioSourceBG;

	public AudioSource audioSourceSave;

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
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public void Play(AudioSource audioSource, float volume = 1f)
	{
		if (Data.isMusic == 1)
		{
			audioSource.Play();
			audioSource.volume = volume;
		}
	}

	public void Pause(AudioSource audioSource)
	{
		audioSource.Pause();
	}

	public void UnPause(AudioSource audioSource)
	{
		audioSource.UnPause();
	}

	public void Stop(AudioSource audioSource)
	{
		audioSource.Stop();
	}

	public bool IsPlaying(AudioSource audioSource)
	{
		return audioSource.isPlaying;
	}
}
