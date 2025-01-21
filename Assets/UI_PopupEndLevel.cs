using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PopupEndLevel : MonoBehaviour
{
	public void ClickedYes()
	{
		var answer = GameplayController.Singleton.lastAnswer == LastAnswer.yes;
		GameplayController.Singleton.lastAnswerIsRight = answer;
		GameplayController.Singleton.CheckForVictoryOrDefeat();
	}
	public void ClickedNo()
	{
		var answer = GameplayController.Singleton.lastAnswer == LastAnswer.no;
		GameplayController.Singleton.lastAnswerIsRight = answer;
		GameplayController.Singleton.CheckForVictoryOrDefeat();
	}
}