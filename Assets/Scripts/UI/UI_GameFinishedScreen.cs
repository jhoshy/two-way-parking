using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UI_GameFinishedScreen : MonoBehaviour
{
	public static UI_GameFinishedScreen Singleton;

	[Header("Tween Settings")]
	public float delay = 0.4f;
	public float tweenDuration = 0.8f;
	public Ease easeType = Ease.OutElastic;

	[Header("Object References")]
	public GameObject trophy;
	public GameObject gameFinishedScreen;

	public static bool IsScreenActive
	{
		get
		{
			return Singleton.gameFinishedScreen.activeSelf;
		}
	}

	private void Awake()
	{
		Singleton = this;
	}
	
	/// <summary>
	/// Invokes the game over screen
	/// </summary>
	public void TryShowGameFinishedScreen()
	{
		int level = Level.reachedLevel;	
		if (level >= SharedState.levels.Length)
		{
			Show();
		}
	}

	/// <summary>
	/// Invokes the game finished screen (when game is finally over)
	/// </summary>
	public void Show()
	{
		this.gameFinishedScreen.SetActive(true);
		trophy.transform.localScale = Vector3.one * 0.1f;
		trophy.SetActive(false);

		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			yield return new WaitForSeconds(delay);
			trophy.SetActive(true);
			trophy.transform.DOScale(1, tweenDuration).SetEase(easeType);

		}
	}
}