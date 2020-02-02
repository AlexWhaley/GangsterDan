using Cinemachine;
using UnityEngine;

public class CameraDirector : Singleton<CameraDirector>
{
	[SerializeField]
	private CinemachineVirtualCamera _vcam;

	public void SetTarget(Transform transform)
	{
		_vcam.Follow = transform;
	}
}
