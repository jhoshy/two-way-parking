using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;
using TMPro;

public class ParkingSpot : MonoBehaviour
{
	[Header("Game settings")]
	public int myNumber;
	[ReadOnly]
	public bool isOccupied;
	[HideInInspector]
	public Car parkedCar;

	[Header("References")]
	public ParkingMark mark1, mark2;

	[ShowNonSerializedField]
	private bool triggerEnter;

	[SerializeField]
	private GameObject colorPlane;
	[SerializeField]
	private TextMeshPro textDebug;

	public Vector3 MarkCenter => mark2.transform.position + ((mark1.transform.position - mark2.transform.position) * 0.5f);

	private void OnValidate()
	{
		if(textDebug)
			textDebug.text = myNumber.ToString();
	}

	private void Start()
	{
		colorPlane.GetComponent<Renderer>().material.DOFade(0.4f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
		colorPlane.SetActive(false);

		GameplayController.Singleton.spotCounter++;
		GameplayController.Singleton.spotList.Add(this);
	}

	public bool IsRightAnswer()
	{
		return (parkedCar && parkedCar.myNumber == this.myNumber);
	}

	public void OccupySpot(Car car)
	{
		isOccupied = true;
		parkedCar = car;
		// Removes a spot from the controller, it is now taken
		GameplayController.Singleton.spotCounter--;
		GameplayController.Singleton.WarnForSpacebar(false);

		colorPlane.GetComponent<Renderer>().material.DOKill();
	}

	private void Update()
	{
		if (isOccupied)
			return;

		if (mark1.myCar && mark2.myCar && mark1.myCar == mark2.myCar)
		{
			// Enter right spot
			if (!triggerEnter)
			{
				Debug.Log("Entra");
				triggerEnter = true;
				mark1.myCar.ParkCar(this);
				colorPlane.SetActive(true);

				GameplayController.Singleton.WarnForSpacebar(true);
			}
		}
		else
		{
			// Exit right spot
			if (triggerEnter)
			{
				Debug.Log("Sai");
				triggerEnter = false;
				mark1.myCar?.CancelCarParking();
				mark2.myCar?.CancelCarParking();
				colorPlane.SetActive(false);

				GameplayController.Singleton.WarnForSpacebar(false);
			}
		}
	}

	//private void OnTriggerEnter(Collider other)
	//{
	//	if (isOccupied)
	//		return;
	//
	//	var car = other.GetComponent<Car>();
	//	if (car)
	//	{
	//		//isOccupied = true;
	//		car.ParkCar(this);
	//	}
	//}
	//
	//private void OnTriggerExit(Collider other)
	//{
	//	if (isOccupied)
	//		return;
	//
	//	var car = other.GetComponent<Car>();
	//	if (car)
	//	{
	//		car.CancelCarParking();
	//	}
	//}
}