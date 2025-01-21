using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressController : MonoBehaviour
{
	public static ProgressController Singleton;
	public GameData gameData;

	private void Awake()
	{
		Singleton = this;
	}

	private void Start()
	{
		if(Level.currentLevelId < SharedState.levels.Length)
			Level.CurrentLevel.score = 0;
	}

	#region GameManagerFunctions 
	/// <summary>
	/// Finishes the level going to the level select screen, enabling the next level.
	/// This is called on the "Continue" button UI event, inside every level.
	/// </summary>
	public void FinishLevel()
	{
		Level.currentLevelId++;
		if (Level.currentLevelId > Level.reachedLevel)
		{
			Level.reachedLevel++;
		}

		AddProgress();

		SceneLoader.Singleton.LoadScene("LevelSelect");
	}
	
	/// <summary>
	/// Finishes the game and sends the message to the LOL platform.
	/// </summary>
	public void FinishGame()
	{
		if (LoLSDK.LOLSDK.Instance.IsInitialized)
		{
			LoLSDK.LOLSDK.Instance.CompleteGame();
		}
	}
	/// <summary>
	/// Submit progress informing the LOL platform that the student advanced.
	/// </summary>
	public static void SubmitProgress()
	{
		if (LoLSDK.LOLSDK.Instance.IsInitialized)
		{
			LoLSDK.LOLSDK.Instance.SubmitProgress(SharedState.score, SharedState.progress, SharedState.maxProgress);
		}
		Debug.Log("[Current Progress] " + SharedState.progress + "/" + SharedState.maxProgress);
	}
	/// <summary>
	/// Adds 1 progress to the game SharedState.
	/// </summary>
	/// <param name="submit">If true, will also submit the values to the LOL platform after adding a progress.</param>
	public static void AddProgress(bool submit = true)
	{
		int value = SharedState.progress + 1;
		if (value < SharedState.maxProgress)
			SharedState.progress = value;
		else
			SharedState.progress = SharedState.maxProgress;

		if (submit)
		{
			SubmitProgress();
		}
	}
	/// <summary>
	/// Adds X score to the game SharedState and to current level score.
	/// </summary>
	/// <param name="submitProgress">If true, will also submit the values to the LOL platform after adding a progress.</param>
	public static void AddProgressiveScore(int amount, bool submitProgress = false)
	{
		SharedState.score += amount;
		Level.CurrentLevel.score += amount;

		if (submitProgress)
		{
			SubmitProgress();
		}

		Debug.Log("Total Score: " + SharedState.score + " | Level Score: " + Level.CurrentLevel.score);
	}
	#endregion
}

public static class Utils
{	
	public static string ColorToHex(Color32 color)
	{
		string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		return hex;
	}
	public static Color HexToColor(string hex)
	{
		byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
		return new Color32(r, g, b, 255);
	}
}