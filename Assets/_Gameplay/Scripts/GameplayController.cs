using System.Collections.Generic;
using TMPro;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;

public enum LastAnswer
{
	yes,
	no,
}

public class GameplayController : MonoBehaviour
{
	public static GameplayController Singleton;

	[Range(1, 6)]
	public int numberOfRows = 1;

	public LastAnswer lastAnswer;

	[HideInInspector]
	public bool levelIsOver = false;

	[HideInInspector]
	public List<ParkingSpot> spotList;
	[HideInInspector]
	public int spotCounter;
	[HideInInspector]
	public bool isTryingToPark;

	[BoxGroup("On Level Start")]
	public bool showLevelInfo;

	[BoxGroup("References")]
	public GameObject popupLevelInfo;
	[BoxGroup("References")]
	public GameObject popupEndLevelInfo;

	[SerializeField]
	private GameObject pressSpacebarText;
	[SerializeField]
	private List<GameObject> tableModuleList;

	[HideInInspector]
	public List<Car> allCarsList = new List<Car>();

	private void Awake()
	{
		Singleton = this;

		if (!SceneLoader.isStuckOnThisLevel)
		{
			popupLevelInfo?.SetActive(showLevelInfo);
		}

		WarnForSpacebar(false);


		// Replaces the last line title for "total" title
		for (int i = tableModuleList.Count - 1; i >= 0; i--)
		{
			var item = tableModuleList[i];
			if (item.gameObject.activeSelf)
			{
				var sdkText = item.GetComponentInChildren<LoadSDKText>();
				sdkText.replaceUnderscoreWithCurrentLevel = false;
				sdkText.SetKey("titleTotal");
				break;
			}
		}
	}
	private void OnValidate()
	{
		for (int i = 0; i < tableModuleList.Count; i++)
		{
			var item = tableModuleList[i];
			item.SetActive(i < numberOfRows);
		}
	}

	public bool spacebarPressed;
	void Update()
	{
		//Assim que o level acabar e mostrar a tela de vitoria, interrompe o gameplay
		if (levelIsOver)
			return;

		if (spotCounter <= 0)
		{
			levelIsOver = true;
			//CheckForVictoryOrDefeat();
			ShowLevelEndInfoPopup();
		}

		CheckForPlayerInputs();

#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.F12))
		{
			UI_PopupsManager.Singleton.ShowLevelVictoryScreen(3, Level.CurrentLevel.score);
		}
#endif
	}
	public Car selectedCar;
	void CheckForPlayerInputs()
	{
		if (selectedCar)
		{
			selectedCar.UpdateInputs(Input.GetAxis("Horizontal"), Input.GetAxisRaw("Vertical"));
		}
	}

	Coroutine conuntDownCoroutine;
	public void WarnForSpacebar(bool value)
	{
		pressSpacebarText.SetActive(value);

		if (value)
		{
			GameplayController.Singleton.isTryingToPark = true;
			conuntDownCoroutine = StartCoroutine(Countdown());
		}
		else if (conuntDownCoroutine != null)
		{
			GameplayController.Singleton.isTryingToPark = false;
			StopCoroutine(conuntDownCoroutine);
		}

		IEnumerator Countdown()
		{
			var uiText = pressSpacebarText.GetComponent<TextMeshProUGUI>();
			var counter = 3;
			while (counter > 0)
			{
				uiText.transform.DOKill();
				uiText.transform.localScale = Vector3.one * 3;
				uiText.transform.DOScale(Vector3.one, 1f);
				uiText.text = counter.ToString();
				yield return new WaitForSeconds(1f);
				counter--;
			}
			GameplayController.Singleton.spacebarPressed = true;
		}
	}

	[HideInInspector]
	public bool lastAnswerIsRight;
	public void CheckForVictoryOrDefeat()
	{
		//Level finalizado
		levelIsOver = true;

		StartCoroutine(Routine());

		IEnumerator Routine()
		{

			//NO CASO DESSE JOGO É SE TODAS AS RESPOSTAS ESTAO CERTAS. SE ELE ESTACIONAR ERRADO, VOU DAR "TRY AGAIN"
			bool victory = AreAllAnswersRight();
			//foreach (var item in spotList)
			//{
			//	if (item.IsRightAnswer())
			//	{
			//		continue;
			//	}
			//	else
			//	{
			//		victory = false;
			//		break;
			//	}
			//}

			var levelScore = 100 + (spotList.Count * 12);
			ProgressController.AddProgressiveScore(levelScore);

			//SE TIVER STUCK< PERDE UMA ESTRELA.
			int starAmount = 3;
			if (SceneLoader.isStuckOnThisLevel)
				starAmount--;
			if (!lastAnswerIsRight)
				starAmount--;


			//****************//
			var starReference = UI_PopupsManager.Singleton.bonusStarReference;
			starReference.gameObject.SetActive(true);
			AudioController.Singleton.PlayExtraStar(lastAnswerIsRight);

			// Ex.: Condição de VITORIA
			if (lastAnswerIsRight)
			{
				starReference.transform.DOPunchScale(starReference.transform.localScale * 0.35f, 0.8f).SetEase(Ease.Linear);
				starReference.transform.DOMoveY(starReference.transform.position.y + 300f, 0.75f).SetEase(Ease.InCirc);
				starReference.GetComponent<Image>().DOFade(0f, 0.75f - 0.2f).SetDelay(0.2f).SetEase(Ease.InOutQuad);
			}
			else
			{
				//starReferece.transform.DOPunchScale(starReferece.transform.localScale * 0.35f, 0.8f).SetEase(Ease.Linear);
				starReference.transform.DOMoveY(starReference.transform.position.y - 250f, 1.35f).SetEase(Ease.InCirc);
				starReference.GetComponent<Image>().color = Color.black;
				starReference.GetComponent<Image>().DOFade(0f, 2f).SetEase(Ease.InOutQuad);
			}

			//Level finalizado
			levelIsOver = true;
			yield return new WaitForSeconds(0.75f);

			UI_PopupsManager.Singleton.popupEndLevelInfo.SetActive(false);
			//****************//


			// Ex.: Condição de VITORIA		
			if (victory)
			{
				//yield return new WaitForSeconds(0.75f);

				//Mostra a tela de vitoria, passa quantas estrelas conseguiu e qual foi a pontuação da fase pra salvar isso no SharedState
				UI_PopupsManager.Singleton.ShowLevelVictoryScreen(starAmount, Level.CurrentLevel.score);
			}
			else
			{
				//yield return new WaitForSeconds(0.75f);

				// Perde a fase.
				UI_PopupsManager.Singleton.ShowLevelFailScreen();
			}
		}
	}

	public bool AreAllAnswersRight()
	{
		foreach (var item in spotList)
		{
			if (item.IsRightAnswer())
			{
				continue;
			}
			else
			{
				return false;
			}
		}
		return true;
	}

	public void ShowLevelEndInfoPopup()
	{
		levelIsOver = true;		

		StartCoroutine(Routine());

		IEnumerator Routine()
		{
			bool victory = AreAllAnswersRight();

			// Ex.: Condição de VITORIA
			if (victory)
			{
				yield return new WaitForSeconds(0.75f);

				//var btnOk = UI_PopupsManager.Singleton.popupEndLevelInfo.GetComponentInChildren<Button>();
				//btnOk.onClick.AddListener(CheckForVictoryOrDefeat);

				UI_PopupsManager.Singleton.popupEndLevelInfo.SetActive(true);
				ProgressController.AddProgress();

				for (int i = 0; i < allCarsList.Count; i++)
				{
					var item = allCarsList[i];
					item.frontTireL.gameObject.SetActive(false);
					item.frontTireR.gameObject.SetActive(false);
					item.carSprite.DOFade(0.1f, 2.5f);
					item.numberText.fontSize = 33;
				}
			}
			else
			{

				yield return new WaitForSeconds(0.75f);

				// Perde a fase.
				UI_PopupsManager.Singleton.ShowLevelFailScreen();
			}
		}
	}
}