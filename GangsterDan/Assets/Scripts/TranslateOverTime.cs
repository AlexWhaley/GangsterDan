using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateOverTime : MonoBehaviour
{
	[SerializeField]
	private float _scrollSpeed;

	private void Awake()
	{
		var rb = GetComponent<Rigidbody2D>().velocity = new Vector2(-_scrollSpeed, 0f);
	}
}
