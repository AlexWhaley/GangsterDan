using System.Collections;
using UnityEngine;

public class RaceOverlay : MonoBehaviour
{
	[SerializeField]
	private GameObject wastedText;

	[SerializeField]
	private GameObject successText;

	private CanvasGroup _canvasGroup;

	private float _fadeInTime = 3f;

	private void Awake()
	{
		_canvasGroup = GetComponent<CanvasGroup>();
		_canvasGroup.alpha = 0f;
		wastedText.SetActive(false);
		successText.SetActive(false);
	}

	public void Wasted()
	{
		wastedText.SetActive(true);
		StartCoroutine(FadeIn());
	}

	public void Success()
	{
		successText.SetActive(true);
		StartCoroutine(FadeIn());
	}

	private IEnumerator FadeIn()
	{
		float currentFade = 0;

		while (currentFade < _fadeInTime)
		{
			currentFade += Time.deltaTime;
			_canvasGroup.alpha = currentFade / _fadeInTime;
			yield return null;
		}
		
	}
}
