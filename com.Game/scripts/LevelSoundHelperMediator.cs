using UnityEngine;
using System.Collections;

public class LevelSoundHelperMediator : MonoSingleton<LevelSoundHelperMediator> 
{	
	public AudioClip ShootSound;
	
	public AudioClip BG1Sound;
	
	public AudioClip BG2Sound;

	public void Playsound(int soundID, bool loop)
	{
		switch(soundID)
		{
			case 0:
				SoundManager.Get.PlayClip( ShootSound, loop );
			break;
			case 1:
				SoundManager.Get.PlayClip( BG1Sound, loop );
			break;
			case 2:
				SoundManager.Get.PlayClip( BG2Sound, loop );
			break;
		}
	}

	void Start()
	{
		Playsound(1, true);
	}
	
}
