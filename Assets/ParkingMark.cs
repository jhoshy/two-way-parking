using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkingMark : MonoBehaviour
{
	//public ParkingSpot parentSpot;
	public Car myCar;

	private void OnTriggerEnter(Collider other)
	{
		//if (parentSpot.isOccupied)
		//	return;

		var car = other.GetComponent<Car>();
		if(car)
			myCar = car;

		//if (parentSpot.mark1Car && !parentSpot.mark2Car)
		//	parentSpot.mark1Car = car;
		//
		//if (!parentSpot.mark1Car && parentSpot.mark2Car)
		//	parentSpot.mark2Car = car;
	}

	private void OnTriggerExit(Collider other)
	{
		//if (parentSpot.isOccupied)
		//	return;

		var car = other.GetComponent<Car>();
		if (car)		
			myCar = null;
	}
}