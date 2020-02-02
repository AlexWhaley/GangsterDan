using UnityEngine;

[System.Serializable]
public class FrameItem : MonoBehaviour
{
	public Transform handlebarTransform;

	public Transform seatTransform;

	public Transform bonusItemTransform;

	public WheelJoint2D backWheelJoint;

	public WheelJoint2D frontWheelJoint;
}
	