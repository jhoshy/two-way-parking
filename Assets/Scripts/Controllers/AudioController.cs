using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
	public static AudioController Singleton;
	public AudioSource musicSource;
	public AudioSource gameplaySource;

	[Header("Game Audio")]
	//Music
	public AudioClip music;
	//Sfx
	public AudioClip victory, click;

	public AudioClip correct, finalCorrect;
	public AudioClip wrong;

	public AudioClip extraStarWin, extraStarLose;

	public List<AudioClip> carCrash;
	public AudioClip carOn;
	public AudioClip carOff;

	public void PlayCarCrash()
	{
		if (carCrash != null && carCrash.Count > 0)
		{		
			Singleton.gameplaySource.PlayOneShot(carCrash[Random.Range(0, carCrash.Count)]);
		}
	}

	public void PlayCarOn()
	{
		if (carOn)
		{
			Singleton.gameplaySource.PlayOneShot(carOn);
		}
	}
	public void PlayCarOff()
	{
		if (carOff)
		{
			Singleton.gameplaySource.PlayOneShot(carOff);
		}
	}

	//Gameplay
	public void ResetGameplayPitch()
	{
		Singleton.gameplaySource.pitch = 1f;
	}

	public void PlayExtraStar(bool rightAnswer)
	{
		if (extraStarWin && extraStarLose)
		{
			Singleton.gameplaySource.PlayOneShot(rightAnswer ? extraStarWin : extraStarLose);
		}		
	}

	public void PlayFinalCorrect()
	{
		if (finalCorrect)
		{
			Singleton.gameplaySource.PlayOneShot(finalCorrect);
		}
	}

	public void PlayCorrect(float pitch)
	{
		if (correct)
		{
			Singleton.gameplaySource.pitch = pitch;
			Singleton.gameplaySource.PlayOneShot(correct);
		}
	}
	public void PlayWrong()
	{
		if (wrong)
		{
			Singleton.gameplaySource.PlayOneShot(wrong);
		}
	}

	//Gameplay
	public void PlayVictory()
	{
		if (victory)
		{
			Singleton.gameplaySource.PlayOneShot(victory);
		}
	}
	public void PlayClick()
	{
		if (click)
		{
			Singleton.gameplaySource.PlayOneShot(click);
		}
	}
	
	public void Mute(bool value)
	{
		Singleton.gameplaySource.mute = value;
		Singleton.musicSource.mute = value;
	}

	#region Initialization
	void Awake()
	{
		if (!Singleton)
		{
			Singleton = this;
		}

		if (Singleton == this)
		{
			DontDestroyOnLoad(this.gameObject);
			//if (!source)
			//{
			//	source = this.gameObject.AddComponent<AudioSource>();
			//}

			//Initializes game music, if it isnt there yet
			if (!musicSource.clip)
			{
				Singleton.musicSource.clip = music;
				Singleton.musicSource.loop = true;
				Singleton.musicSource.Play();
			}
		}
		else
		{
			Destroy(this.gameObject);
		}
	}
	#endregion
}