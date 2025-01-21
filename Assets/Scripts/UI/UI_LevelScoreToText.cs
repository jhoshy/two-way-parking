using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_LevelScoreToText : MonoBehaviour
{
	private Text scoreText;
	private TextMeshProUGUI scoreTextM;

	void OnEnable()
    {
		if (SharedState.levels == null)
			return;

		var score = SharedState.levels[Level.currentLevelId].score.ToString();
		Debug.Log("LevelScore: "  + score);

		scoreText = GetComponent<Text>();
		if (scoreText) scoreText.text = score;

		scoreTextM = GetComponent<TextMeshProUGUI>();
		if (scoreTextM) scoreTextM.text = score;
	}
}