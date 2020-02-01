using UnityEngine;

public class BikeController : MonoBehaviour
{
	[SerializeField]
	private Rigidbody2D _frame;

	[SerializeField]
	private WheelJoint2D _backWheelJoint;

	[SerializeField]
	private WheelJoint2D _frontWheelJoint;

	[SerializeField]
	private float _motorForceMax;

	[SerializeField]
	private float _motorForceAcceleration = 500f;
	[SerializeField]
	private float _motorForceDecceleration = 500f;

	private JointMotor2D _backWheelMotor;
	private JointMotor2D _frontWheelMotor;
	private float _currentMotorForce = 0f;

	[SerializeField]
	private float _breakForce = 1500f;

	[SerializeField]
	private float _gravityScale = 1f;

	[SerializeField]
	private bool _twoWheelDrive;

	[SerializeField]
	private Rigidbody2D _rigiDean;

	[SerializeField]
	private float _leanDeanForce;

	[SerializeField]
	private float _maxAngularDeanForce;

	private bool _useMotors;

	// Start is called before the first frame update
	void Awake()
	{
		_backWheelMotor = _backWheelJoint.motor;
		_frontWheelMotor = _frontWheelJoint.motor;

		var rbs = GetComponentsInChildren<Rigidbody2D>();
		foreach (var rb in rbs)
		{
			rb.gravityScale = _gravityScale;
		}

		var wheels = GetComponentsInChildren<WheelJoint2D>();
		foreach (var wheel in wheels)
		{
			wheel.breakForce = _breakForce;
		}
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		FixedWheelDriveUpdate();
		FixedDeanForceUpdate();
	}

	private void FixedWheelDriveUpdate()
	{
		if (_backWheelJoint == null || _frontWheelJoint == null) return;

		if (Input.GetKey(KeyCode.W) && _backWheelJoint.jointSpeed > -_motorForceMax)
		{
			Debug.Log("Angular Velcoity: " + _backWheelJoint.jointSpeed);
			SetUseMotors(true);
			_currentMotorForce -= _motorForceAcceleration * Time.deltaTime;

			if (_currentMotorForce < -_motorForceMax)
			{
				_currentMotorForce = -_motorForceMax;
			}
		}
		else if (Input.GetKey(KeyCode.S))
		{
			//_backWheelJoint.jointSpeed 
			SetUseMotors(true);
			_currentMotorForce += _motorForceDecceleration * Time.deltaTime;

			if (_currentMotorForce < 0)
			{
				_currentMotorForce = 0;
			}
			_backWheelMotor.motorSpeed = 0;
		}
		else
		{
			SetUseMotors(false);
			_currentMotorForce = _backWheelJoint.jointSpeed;
		}

		if (_useMotors)
		{
			_backWheelMotor.motorSpeed = _currentMotorForce;
			_backWheelJoint.motor = _backWheelMotor;

			if (_twoWheelDrive)
			{
				_frontWheelMotor.motorSpeed = _currentMotorForce;
				_frontWheelJoint.motor = _frontWheelMotor;
			}
		}
	}

	private void SetUseMotors(bool useMotors)
	{
		_useMotors = useMotors;
		_backWheelJoint.useMotor = useMotors;

		if (_twoWheelDrive)
		{
			_frontWheelJoint.useMotor = useMotors;
		}
	}

	private void FixedDeanForceUpdate()
	{
		if (Input.GetKey(KeyCode.A))// && _rigiDean.angularVelocity > - _maxAngularDeanForce)
		{
			float torque = _leanDeanForce * Time.deltaTime;
			_rigiDean.AddTorque(torque);

			if (_frame.angularVelocity < 1.5f)
			{
				_frame.AddTorque(torque);
			}
		}
		else if (Input.GetKey(KeyCode.D))// && _rigiDean.angularVelocity < _maxAngularDeanForce)
		{
			float torque = -_leanDeanForce * Time.deltaTime;
			_rigiDean.AddTorque(torque);

			if (_frame.angularVelocity > -1.5f)
			{
				_frame.AddTorque(torque);
			}
		}
	}
}
