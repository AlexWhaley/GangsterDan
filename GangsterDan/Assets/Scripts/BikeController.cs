using UnityEngine;

public class BikeController : MonoBehaviour
{
	private WheelJoint2D _backWheelJoint;
	private WheelJoint2D _frontWheelJoint;
	private FrameItem _frame;
	private Rigidbody2D _frameRB;

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
	private float _leanDeanForce;

	[SerializeField]
	private float _leanFrameForce;

	[SerializeField]
	private float _maxAngularLeanForce;

	private bool _useMotors;

	[SerializeField]
	private bool _useTestConfig;

	[SerializeField]
	private BikeConstructionData _testData;

	[SerializeField]
	private RagdollController _ragdollController;

	private bool _isDead;


	// Start is called before the first frame update
	void Awake()
	{
		if (_useTestConfig)
		{
			ConstructBike(_testData);
		}
	}

	private void ConstructBike(BikeConstructionData data)
	{
		GameObject frame = Instantiate(data.frame.gameObject, transform.position, transform.rotation);
		_frame = frame.GetComponent<FrameItem>();

		_frameRB = frame.GetComponent<Rigidbody2D>();
		_frontWheelJoint = _frame.backWheelJoint;
		_backWheelJoint = _frame.frontWheelJoint;

		_backWheelMotor = _backWheelJoint.motor;
		_frontWheelMotor = _frontWheelJoint.motor;

		//Create back wheel
		Vector3 backWheelWorldPos = frame.transform.TransformPoint(_backWheelJoint.anchor);
		GameObject backWheel = Instantiate(data.backWheel.gameObject, backWheelWorldPos, frame.transform.rotation);

		_backWheelJoint.connectedBody = backWheel.GetComponent<Rigidbody2D>();

		// Create front wheel
		Vector3 frontWheelWorldPos = frame.transform.TransformPoint(_frontWheelJoint.anchor);
		GameObject frontWheel = Instantiate(data.frontWheel.gameObject, frontWheelWorldPos, frame.transform.rotation);

		_frontWheelJoint.connectedBody = frontWheel.GetComponent<Rigidbody2D>();


		GameObject handlebars = Instantiate(data.handlebars.gameObject, _frame.handlebarTransform);
		var handlebarItem = handlebars.GetComponent<HandlebarItem>();

		Vector3 localHandlebarPosition = _frameRB.transform.InverseTransformPoint(handlebarItem.handPosition.position);
		_ragdollController.SetHandJointAnchor(_frameRB, localHandlebarPosition);

		GameObject seat = Instantiate(data.seatItem.gameObject, _frame.seatTransform);
		var seatItem = seat.GetComponent<SeatItem>();

		Vector3 localSeatPosition = _frameRB.transform.InverseTransformPoint(seatItem.assPosition.position);
		_ragdollController.SetSeatJointAnchor(_frameRB, localSeatPosition);

		SetConfiguredValues();
		CameraDirector.Instance.SetTarget(_ragdollController.torso.transform);
		_ragdollController.headCollisionEventSystem.onValidCollision += DestroyBike;
	}

	private void OnDestroy()
	{
		_ragdollController.headCollisionEventSystem.onValidCollision -= DestroyBike;
	}

	private void DestroyBike(Vector2 explosionPoint)
	{
		if (_backWheelJoint != null)
		{
			_backWheelJoint.enabled = false;
		}
		if (_frontWheelJoint != null)
		{
			_frontWheelJoint.enabled = false;
		}

		var rbs = GetComponentsInChildren<Rigidbody2D>();
		foreach (var rb in rbs)
		{
			rb.AddExplosionForce(10f, explosionPoint, 10f);
		}

		_isDead = true;
	}

	private void SetConfiguredValues()
	{
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
		if (_isDead) return;

		FixedWheelDriveUpdate();
		FixedDeanForceUpdate();
	}

	private void FixedWheelDriveUpdate()
	{
		if (_backWheelJoint == null) return;

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
			SetUseMotors(true);
			_currentMotorForce += _motorForceDecceleration * Time.deltaTime;

			if (_currentMotorForce > _motorForceMax)
			{
				_currentMotorForce = _motorForceMax;
			}
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

				if (_frontWheelJoint != null)
				{
					_frontWheelJoint.motor = _frontWheelMotor;
				}
			}
		}
	}

	private void SetUseMotors(bool useMotors)
	{
		_useMotors = useMotors;
		_backWheelJoint.useMotor = useMotors;

		if (_twoWheelDrive)
		{
			if (_frontWheelJoint != null)
			{
				_frontWheelJoint.useMotor = useMotors;
			}
		}
	}

	private void FixedDeanForceUpdate()
	{
		if (Input.GetKey(KeyCode.A))// && _rigiDean.angularVelocity > - _maxAngularDeanForce)
		{
			_ragdollController.torso.AddTorque(_leanDeanForce * Time.deltaTime);

			if (_frameRB.angularVelocity < _maxAngularLeanForce)
			{
				_frameRB.AddTorque(_leanFrameForce * Time.deltaTime);
			}
		}
		else if (Input.GetKey(KeyCode.D))// && _rigiDean.angularVelocity < _maxAngularDeanForce)
		{
			_ragdollController.torso.AddTorque(-_leanDeanForce * Time.deltaTime);

			if (_frameRB.angularVelocity > -_maxAngularLeanForce)
			{
				_frameRB.AddTorque(-_leanFrameForce * Time.deltaTime);
			}
		}
	}
}
