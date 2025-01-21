using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial_PressArrows : MonoBehaviour
{

	private TextMeshProUGUI text;
	public float holdAmount;
	public float sumSpeed = 1;

	[Header("Tween Settings")]
	public float tweenDuration = 0.8f;
	public Ease easeType = Ease.Linear;

	public GameObject nextText;

	void OnEnable()
	{
		text = GetComponent<TextMeshProUGUI>();

		this.transform.localScale = Vector3.zero;// * 0.1f;
		this.transform.DOScale(0.9f, tweenDuration * 0.5f).SetEase(easeType).SetDelay(0.1f);

		this.transform.DOScale(1, tweenDuration).SetEase(easeType).SetDelay((tweenDuration * 0.5f) + 0.1f).SetLoops(-1, LoopType.Yoyo);
	}

	void Update()
	{
		if (holdAmount >= 1)
		{
			this.gameObject.SetActive(false);
			nextText.SetActive(true);
			return;
		}

		if(holdAmount > 0.3f)
		{
			nextText.SetActive(true);
		}

		var sum = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized.magnitude;

		holdAmount += sum * Time.deltaTime * sumSpeed;

		holdAmount = Mathf.MoveTowards(holdAmount, 0, Time.deltaTime * sumSpeed * 0.5f);

		var color = text.color;

		color.a = 1 - holdAmount;
		text.color = color;
	}
}
