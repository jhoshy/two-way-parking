using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial_ParkHere : MonoBehaviour
{
	private TextMeshProUGUI text;
	public float holdAmount;
	public float sumSpeed = 1;

	[Header("Tween Settings")]
	public float tweenDuration = 0.8f;
	public Ease easeType = Ease.Linear;

	void OnEnable()
	{
		text = GetComponent<TextMeshProUGUI>();

		this.transform.localScale = Vector3.zero;// * 0.1f;
		this.transform.DOScale(0.9f, tweenDuration * 0.5f).SetEase(easeType).SetDelay(0.1f);

		this.transform.DOScale(1, tweenDuration).SetEase(easeType).SetDelay((tweenDuration * 0.5f) + 0.1f).SetLoops(-1, LoopType.Yoyo);
	}

	void Update()
	{
		if (GameplayController.Singleton.levelIsOver)
		{
			this.gameObject.SetActive(false);
			return;
		}

		//var sum = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized.magnitude;
		//
		//holdAmount += sum * Time.deltaTime * sumSpeed;
		//
		//holdAmount = Mathf.MoveTowards(holdAmount, 0, Time.deltaTime * sumSpeed * 0.5f);
		//
		//var color = text.color;
		//
		//color.a = 1 - holdAmount;
		//text.color = color;
	}
}