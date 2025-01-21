using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UI_LevelButton : MonoBehaviour
{
	public int myLevel = 0;

	public Sprite levelLocked, levelOpen;
	public List<GameObject> stars;

	private TextMeshProUGUI text;
	public TextMeshProUGUI Text
	{
		get
		{
			if (!text)
				text = GetComponentInChildren<TextMeshProUGUI>(true);
			return text;
		}
		set => text = value;
	}

#if UNITY_EDITOR
	private void OnValidate()
	{
		SetNumberText();
	}
#endif
	
	void OnEnable()
	{
		var button = this.gameObject.GetComponent<Button>();

		// adding click event to the button, it will load the scene "Level1" <- using "myLevel" as the number
		button.onClick.AddListener(()=> { SceneLoader.Singleton.LoadGameLevel(myLevel); });

		button.interactable = true;

		if (Level.reachedLevel < myLevel - 1)
		{
			button.interactable = false;
			this.transform.GetComponentInChildren<TextMeshProUGUI>().gameObject.SetActive(false);
			this.gameObject.GetComponent<Image>().sprite = levelLocked;
		}
		// level open
		if (Level.reachedLevel == myLevel - 1)
		{
			this.gameObject.GetComponent<Image>().sprite = levelOpen;
			this.transform.DOScale(this.transform.localScale * 1.14f, 0.8f).SetLoops(-1, LoopType.Yoyo);
		}

		// hide all stars
		for (int i = 0; i < stars.Count; i++)
		{
			stars[i].SetActive(false);
		}

		// shows level stars
		if (SharedState.levels != null)
		{
			var length = SharedState.levels[myLevel - 1].starAmount;
			for (int i = 0; i < stars.Count; i++)
			{
				stars[i].SetActive(i < length);
			}
		}

		SetNumberText();
	}

	private void SetNumberText()
	{
		if (!Text.text.Equals(myLevel.ToString()))
		{
			Text.text = myLevel.ToString();
		}
	}
}