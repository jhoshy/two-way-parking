using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UI_PopupsManager : MonoBehaviour
{
	public static UI_PopupsManager Singleton;

	[Header("Object References")]
	// Screens
	[SerializeField]
	private GameObject victoryScreen;
	[SerializeField]
	private GameObject failScreen;

	public GameObject bonusStarReference;

	public GameObject popupEndLevelInfo;
	// Objects
	[SerializeField]
	private List<GameObject> stars;

	[Header("Victory Stars Settings")]
	public float delayBetweenStars = 0.4f;
	public float tweenDuration = 0.8f;
	public Ease easeType = Ease.OutElastic;

	public List<GameObject> thingsToHideIfPlayerIsStuck;

	private void Awake()
	{
		Singleton = this;
	}

	private void OnEnable()
	{
		if (SceneLoader.isStuckOnThisLevel && thingsToHideIfPlayerIsStuck != null && thingsToHideIfPlayerIsStuck.Count > 0)
		{
			foreach (var item in thingsToHideIfPlayerIsStuck)
			{
				item.SetActive(false);
			}
		}
	}

	/// <summary>
	/// Invokes the victory screen with given star amount (from 1 to 3)
	/// </summary>
	public void ShowLevelVictoryScreen(int starAmount, int score)
	{
		// Functionality
		if (SharedState.levels != null)
		{
			SharedState.levels[Level.currentLevelId].starAmount = Mathf.Max(starAmount, SharedState.levels[Level.currentLevelId].starAmount);
			SharedState.levels[Level.currentLevelId].score = Mathf.Max(score, SharedState.levels[Level.currentLevelId].score);
		}

		// UI visuals
		this.victoryScreen.SetActive(true);
		foreach (var item in stars)
		{
			item.transform.localScale = Vector3.one * 0.1f;
			item.SetActive(false);
		}
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			int i = 1;
			foreach (var item in stars)
			{
				if (i > starAmount)
					yield break;

				item.SetActive(true);
				item.transform.DOScale(1, tweenDuration).SetEase(easeType);
				i++;
				yield return new WaitForSeconds(delayBetweenStars);
			}
		}
	}

	/// <summary>
	/// Invokes the fail screen so the player can restart the level.
	/// </summary>
	public void ShowLevelFailScreen()
	{
		this.failScreen.SetActive(true);
	}

	/// <summary>
	/// Invokes the game over screen
	/// </summary>
	public void TryShowGameFinishedScreen()
	{
		int level = Level.reachedLevel;
		Debug.Log("Cur Level: " + level + " / " + SharedState.levels.Length);
		if (level >= SharedState.levels.Length)
		{
			UI_GameFinishedScreen.Singleton.Show();
		}
	}
}
