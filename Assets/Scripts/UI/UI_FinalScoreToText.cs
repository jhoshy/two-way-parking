using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_FinalScoreToText : MonoBehaviour
{
	private Text scoreText;
	private TextMeshProUGUI scoreTextM;

	void OnEnable()
	{
		var score = SharedState.score.ToString();

		scoreText = GetComponent<Text>();
		if (scoreText) scoreText.text = score;

		scoreTextM = GetComponent<TextMeshProUGUI>();
		if (scoreTextM) scoreTextM.text = score;
	}
}