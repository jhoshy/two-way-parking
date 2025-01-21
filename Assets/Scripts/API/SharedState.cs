using UnityEngine;
using SimpleJSON;
using LoLSDK;

// SharedState class handles all the "game save" info as static values
public static class SharedState
{
	public static JSONNode startGameData;
	public static JSONNode languageDefs;

	// Saved level infos
	public static Level[] levels;

	public static int score = 0;	
	public static int progress = 0;

	// This is defined inside Loader.Awake() through the "GameConfig" ScriptableObject.
	public static int maxProgress;
}

public class Level
{
	// Level being played at the moment
	public static int currentLevelId = 0;
	public static Level CurrentLevel
	{
		get { return SharedState.levels[currentLevelId]; }
	}

	// Max reached level from the player
	public static int reachedLevel = 0;
	
	// Info for each specific level instance
	public int score;
	public int starAmount;
}