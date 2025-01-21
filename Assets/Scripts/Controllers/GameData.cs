using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using NaughtyAttributes;

public class GameData : ScriptableObject
{
	public void OnEnable()
	{
		Application.runInBackground = false;

		SharedState.maxProgress = maxProgress;

		SharedState.levels = new Level[totalLevels];
		for (int i = 0; i < SharedState.levels.Length; i++)
		{
			SharedState.levels[i] = new Level();
		}
	}

	[Header("Settings")]
	public string applicationID = "nomedaempresa.nomedojogo";
	// Progress should be a minimun of 8
	public int maxProgress = 8;
	public int totalLevels = 3;
}