/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
public class SoundManager
{
	public static void PlaySound(string name, float volume = 0.6f)
	{
		if (Data.isSound == 1)
		{
			Sound objectForType = PoolsSound.instance.GetObjectForType("Sound", onlyPooled: false, isParent: true);
			objectForType.PlaySound(PoolsSound.instance.audioClips[name], volume);
		}
	}
}
