using System;
using UnityEngine;

public class CollisionEventSystem : MonoBehaviour
{
	[SerializeField]
	private string _compareTag;

	public Action<Vector2> onValidCollision;

	[SerializeField]
	private bool _disableAfterActivation;

	private bool _canFire = true;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (_canFire && collision.gameObject.CompareTag(_compareTag))
		{
			onValidCollision?.Invoke(collision.contacts[0].point);


			if (_disableAfterActivation)
			{
				_canFire = false;
			}
		}
	}
}
