using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine.UI;
using UnityEngine.Events;

public class UI_TutorialHand : MonoBehaviour
{
	[System.Serializable]
	public struct ButtonPlacement
	{
		public Vector3 position;
		public Quaternion rotation;
	}

	public List<ButtonPlacement> positions = new List<ButtonPlacement>();
	public float tweenDuration = 0.5f;
	public float tweenDistance = 1f;

	public int currentPosition = 0;
	Tween tweenT, tweenC;

	public Transform hand;

	private int lastPosition;

	public UnityEvent OnStart;

	private void Start()
	{
		hand.DOLocalMove(hand.transform.up * tweenDistance, tweenDuration).SetLoops(-1, LoopType.Yoyo);

		var color = hand.GetComponent<Image>().color;
		color.a = 1f;
		hand.GetComponent<Image>().color = color;
		hand.GetComponent<Image>().DOFade(0.5f, tweenDuration).SetLoops(-1, LoopType.Yoyo);

		UpdatePositions();

		OnStart.Invoke();
	}

	private void OnEnable()
	{
		hand.DORestart();
	}

	[Button]
	public void AddPositionInEditor()
	{
		positions.Add(new ButtonPlacement { position = this.transform.position, rotation = this.transform.rotation });
	}

	void Update()
	{
		if (lastPosition != currentPosition)
		{
			lastPosition = currentPosition;
			hand.DORestart();
		}

		///* Specific gameplay tutorial functionality */
		//if (GameplayController.Singleton.gameIsReady && !GameplayController.Singleton.playerTableCard)
		//{
		//	// First card
		//	if (currentPosition == 0 && GameplayController.Singleton.botDeck.Count == 2)
		//	{
		//		Show();
		//	}
		//	// Second card
		//	if (currentPosition == 1 && GameplayController.Singleton.botDeck.Count == 1)
		//	{
		//		Show();
		//	}
		//}
	}


	[Button]
	public void NextPosition()
	{
		currentPosition = Mathf.Min(currentPosition + 1, positions.Count - 1);
		OnEnable();
		UpdatePositions();
	}
	[Button]
	public void LastPosition()
	{
		currentPosition = Mathf.Max(currentPosition - 1, 0);
		OnEnable();
		UpdatePositions();
	}

	public void SetPosition(int position)
	{
		currentPosition = Mathf.Clamp(position, 0, positions.Count - 1);

		UpdatePositions();
	}

	[Button]
	public void Show()
	{
		hand.gameObject.SetActive(true);
	}

	[Button]
	public void Hide()
	{
		hand.gameObject.SetActive(false);
	}

	private void UpdatePositions()
	{
		if (positions != null && positions.Count > 0)
		{
			this.transform.position = positions[currentPosition].position;
			this.transform.rotation = positions[currentPosition].rotation;
		}
	}
}