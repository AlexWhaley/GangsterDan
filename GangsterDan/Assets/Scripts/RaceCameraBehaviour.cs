
using UnityEngine;

public class RaceCameraBehaviour : MonoBehaviour
{
	[SerializeField]
	private Rigidbody2D _rigidbody;

	// Update is called once per frame
	void Update()
	{
		var currentPosition = transform.position;
		transform.position = new Vector3(_rigidbody.position.x, currentPosition.y, currentPosition.z);
	}
}
