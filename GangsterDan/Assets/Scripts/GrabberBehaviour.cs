using UnityEngine;

public class GrabberBehaviour : MonoBehaviour
{
	[SerializeField]
	private float _moveSpeed = 1f;
	[SerializeField]
	private float _grabTime = 1f;
	private Transform _transform;
    // Start is called before the first frame update
    void Start()
    {
		_transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void HandleInput()
	{
		Vector2 movementVector = Vector2.zero;

		if (Input.GetKey(KeyCode.W))
		{
			movementVector.y = 1f;
		}
		else if (Input.GetKey(KeyCode.S))
		{
			movementVector.y = -1f;
		}

		if (Input.GetKey(KeyCode.D))
		{
			movementVector.x = 1f;
		}
		else if (Input.GetKey(KeyCode.A))
		{
			movementVector.x = -1f;
		}
	}
}
