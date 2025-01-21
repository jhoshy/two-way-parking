using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UI_DidYouKnowPopupManager : MonoBehaviour
{
	public GameData gameData;
	public List<GameObject> didYouKnowPopups;

	void Start()
	{
		ShowDidYouKnowPopup();
	}

	void ShowDidYouKnowPopup()
	{
		int level = Level.currentLevelId;
		if (level > 0)
		{			
			if (level <= gameData.totalLevels && didYouKnowPopups.Count >= level)
			{
				if (didYouKnowPopups[level - 1] != null)
				{
					didYouKnowPopups[level - 1].SetActive(true);
				}
			}			
		}
	}
}
