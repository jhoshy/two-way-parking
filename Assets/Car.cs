using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Car : MonoBehaviour
{
	[Header("Game settings")]
	public int myNumber;
	public bool isParked;

	[ShowNonSerializedField]
	private Vector3 velocity;
	private Vector3 direction;
	private float torque;

	[Header("Physics settings")]
	[SerializeField]
	public float speed = 3f;
	[SerializeField]
	private float accelerationRate = 2f;
	[SerializeField]
	private float decelerationRate = 4f;
	[SerializeField]
	private float rotationSpeed = 0.5f;
	[SerializeField]
	private float rotationAccelerationRate = 2f;
	[SerializeField]
	private float rotationDecelerationRate = 2f;

	private float inputH, inputV;

	[Header("References")]
	public Transform frontTireL, frontTireR;
	public Text numberText;
	public Transform numberPoint;
	public ParticleSystem driftL, driftR;
	public SpriteRenderer carSprite;
	public SpriteRenderer carLightL, carLightR;
	private Rigidbody rigidbody;
	private Camera camera;
	public AudioSource engine;

	public UnityEvent OnCarClicked;

	public bool carIsSelected
	{
		get { return GameplayController.Singleton.selectedCar == this; }
	}

	void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
		camera = Camera.main;

		if (isParked)
			DisableCar();

		TurnLights(false);

		GameplayController.Singleton.allCarsList.Add(this);
	}

	private void OnValidate()
	{
		if (numberText)
		{
			numberText.text = myNumber.ToString();
			UpdateTextNumberPosition();
		}
	}

	void Update()
	{
		UpdateTextNumberPosition();

		if (isParked || GameplayController.Singleton.selectedCar != this)
			return;
		//UpdateInputs();
		UpdateMovement();
	}

	public void UpdateTextNumberPosition()
	{
		if (!camera)
			camera = Camera.main;
		if(numberText && camera)
			numberText.transform.position = camera.WorldToScreenPoint(numberPoint.position);
	}

	public bool trigger;
	public void UpdateInputs(float horizontal, float vertical)
	{
		inputH = horizontal; // Input.GetAxis("Horizontal");
		inputV = vertical; // Input.GetAxisRaw("Vertical");

		if (inputH == 0)
		{
			driftL.Play();
			driftR.Play();
		}
	}

	private void UpdateMovement()
	{
		// Get inputs
		var input = new Vector2(inputH, inputV).normalized;
		var newVel = /*(transform.right * input.x) +*/ (transform.forward * input.y);
		// Apply velocity
		if (newVel.magnitude > 0)
		{
			velocity = Vector3.MoveTowards(velocity, newVel, accelerationRate * Time.deltaTime);
		}
		else
		{
			velocity = Vector3.MoveTowards(velocity, Vector3.zero, decelerationRate * Time.deltaTime);
		}
		rigidbody.velocity = velocity * Time.deltaTime * speed;

		// Apply rotation
		if (velocity.magnitude > 0f)
		{
			torque = Mathf.MoveTowards(torque, inputV, rotationAccelerationRate * Time.deltaTime);
		}
		else
		{
			torque = Mathf.MoveTowards(torque, 0, rotationDecelerationRate * Time.deltaTime);
		}
		direction = (this.transform.right * inputH * torque) + this.transform.forward;
		rigidbody.rotation = Quaternion.Slerp(rigidbody.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);

		UpdateFrontTires((this.transform.right * inputH * Mathf.Abs(torque)) + this.transform.forward);
	}

	private void UpdateFrontTires(Vector3 direction)
	{
		frontTireL.rotation = Quaternion.LookRotation(direction);
		frontTireR.rotation = Quaternion.LookRotation(direction);
	}

	private void OnMouseDown()
	{
		if (isParked || GameplayController.Singleton.isTryingToPark)
			return;

		// Deselect the other car...
		GameplayController.Singleton.selectedCar?.TurnLights(false);
		GameplayController.Singleton.selectedCar?.DisableOutline();
		GameplayController.Singleton.selectedCar?.engine.Stop();
		if (GameplayController.Singleton.selectedCar)
		{
			GameplayController.Singleton.selectedCar.engine.enabled = false;
		}
		
		// Car selected.
		TurnLights(true);
		// Audio stuff
		engine.enabled = true;
		engine.Play();

		DisableOutline();

		var outline = carSprite.GetComponent<SpriteOutline>();
		outlineTween = DOTween.To(outline.UpdateOutline, 0, 4, 0.75f).SetLoops(-1, LoopType.Yoyo);

		GameplayController.Singleton.selectedCar = this;
		AudioController.Singleton.PlayCarOn();

		OnCarClicked?.Invoke();
	}
	public Tweener outlineTween;

	Coroutine coroutine;
	public void ParkCar(ParkingSpot parkingSpot)
	{
		coroutine = StartCoroutine(ParkingRoutine(parkingSpot));
	}

	IEnumerator ParkingRoutine(ParkingSpot parkingSpot)
	{
		yield return new WaitUntil(() => (GameplayController.Singleton.spacebarPressed /*Input.GetKeyDown(KeyCode.Space)*/ && GameplayController.Singleton.selectedCar == this));
		AudioController.Singleton.PlayCarOff();
		engine.Stop();
		engine.enabled = false;

		GameplayController.Singleton.isTryingToPark = false;
		GameplayController.Singleton.spacebarPressed = false;

		parkingSpot.OccupySpot(this);

		isParked = true;

		DisableCar();
		var duration = 0.5f;
		// Moves the car to the center spot
		this.transform.DOMove(parkingSpot.MarkCenter, duration);

		// Flips rotation depending on car side
		Vector3 carForward = transform.forward;
		Vector3 spotForward = parkingSpot.transform.forward;
		var newRotation = parkingSpot.transform.eulerAngles;
		if (Vector3.Angle(carForward, spotForward) > 90)
			newRotation.y = -newRotation.y;
		this.transform.DORotate(newRotation, duration);

		// Visual stuf
		//var outline = carSprite.GetComponent<SpriteOutline>();
		DisableOutline();
	}

	public Material grayScaleMaterial;
	public void DisableCar()
	{
		this.rigidbody.isKinematic = true;
		DisableOutline();
		TurnLights(false);

		carSprite.material = grayScaleMaterial;
		carSprite.material.DOFloat(0.9f, "_EffectAmount", 0.6f);
	}

	public void CancelCarParking()
	{
		StopCoroutine(coroutine);
	}

	public void DisableOutline()
	{
		outlineTween?.Kill();
		var outline = carSprite.GetComponent<SpriteOutline>();
		outline.UpdateOutline(0);
	}
	public void TurnLights(bool value)
	{
		carLightL.DOKill();
		carLightR.DOKill();

		// Keys On!
		if (value)
		{
			carLightL.gameObject.SetActive(true);
			carLightR.gameObject.SetActive(true);

			carLightL.DOFade(1, 0.2f);
			carLightR.DOFade(1, 0.2f);

			// Tocar audio de chave/alarme/motor ligando, etc
			carSprite.transform.DOKill();			
			carSprite.transform.DOShakeScale(0.5f, 0.1f).OnComplete(()=> { carSprite.transform.localScale = Vector3.one; });
		}
		else
		{
			carLightL.DOFade(0, 0.35f).OnComplete(TurnOff);
			carLightR.DOFade(0, 0.35f).OnComplete(TurnOff);
		}

		void TurnOff()
		{
			if (!value)
			{
				carLightL.gameObject.SetActive(false);
				carLightR.gameObject.SetActive(false);
			}
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Car"))
		{
			AudioController.Singleton.PlayCarCrash();
		}
	}
}