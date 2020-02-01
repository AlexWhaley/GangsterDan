using UnityEngine;

public class BikeController : MonoBehaviour
{
	[SerializeField]
	private WheelJoint2D _backWheelJoint;

	[SerializeField]
	private float _motorForceMax;

	[SerializeField]
	private float _motorForceAcceleration = 500f;
	[SerializeField]
	private float _motorForceDecceleration = 500f;

	private JointMotor2D _backWheelMotor;
	private float _currentMotorForce = 0f;

	[SerializeField]
	private float _gravityScale = 1f;

	// Start is called before the first frame update
	void Awake()
	{
		_backWheelMotor = _backWheelJoint.motor;
		var rbs = GetComponentsInChildren<Rigidbody2D>();
		foreach(var rb in rbs)
		{
			rb.gravityScale = _gravityScale;
		}
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (_backWheelJoint == null) return;

		if (Input.GetKey(KeyCode.W))
		{
			_backWheelJoint.useMotor = true;
			_currentMotorForce += _motorForceAcceleration * Time.deltaTime;

			if (_currentMotorForce > _motorForceMax)
			{
				_currentMotorForce = _motorForceMax;
			}
		}
		else if (Input.GetKey(KeyCode.S))
		{
			_backWheelJoint.useMotor = true;
			_currentMotorForce -= _motorForceDecceleration * Time.deltaTime;

			if (_currentMotorForce < 0)
			{
				_currentMotorForce = 0;
			}
			_backWheelMotor.motorSpeed = 0;
		}
		else
		{
			_backWheelJoint.useMotor = false;
		}

		if (_backWheelJoint.useMotor)
		{
			_backWheelMotor.motorSpeed = _currentMotorForce;
			_backWheelJoint.motor = _backWheelMotor;
		}
	}
}
