using UnityEngine;

public class RagdollController : MonoBehaviour
{
	[SerializeField]
	private HingeJoint2D _seatBoneJoint;

	[SerializeField]
	private HingeJoint2D _handBoneJoint;

	public CollisionEventSystem headCollisionEventSystem;

	public Rigidbody2D torso;

	[SerializeField]
	private HingeJoint2D _armJoint;

	[SerializeField]
	private HingeJoint2D _legJoint;

	[SerializeField]
	private GameObject _oofHead;

	[SerializeField]
	private GameObject _nonOofHead;

	private void Awake()
	{
		headCollisionEventSystem.onValidCollision += KillPlayer;
	}

	private void OnDestroy()
	{
		headCollisionEventSystem.onValidCollision -= KillPlayer;
	}

	private void KillPlayer(Vector2 position)
	{
		_oofHead.SetActive(true);
		_nonOofHead.SetActive(false);

		_armJoint.enabled = false;
		_legJoint.enabled = false;
		_seatBoneJoint.enabled = false;
		_handBoneJoint.enabled = false;

		var rbs = GetComponentsInChildren<Rigidbody2D>();
		
		foreach(var rb in rbs)
		{
			rb.AddExplosionForce(50f, position, 10f);
		}
	}

	public void SetHandJointAnchor(Rigidbody2D rb, Vector2 anchor)
	{
		_handBoneJoint.connectedBody = rb;
		_handBoneJoint.connectedAnchor = anchor;
	}

	public void SetSeatJointAnchor(Rigidbody2D rb, Vector2 anchor)
	{
		_seatBoneJoint.connectedBody = rb;
		_seatBoneJoint.connectedAnchor = anchor;
	}
	
	public void DisableHandJoint()
	{
		_handBoneJoint.enabled = false;
	}

	public void DisableSeatJoint()
	{
		_seatBoneJoint.enabled = false;
	}
}
