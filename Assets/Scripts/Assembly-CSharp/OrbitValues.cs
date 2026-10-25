using UnityEngine;

public class OrbitValues : MonoBehaviour
{
	[SerializeField]
	public CameraSpline m_cameraSpline;

	[SerializeField]
	public CameraSpline m_lookAtSpline;

	[SerializeField]
	public Vector3 cameraOffset = new Vector3(0f, 3.5f, -5f);

	[SerializeField]
	public Vector3 lookAtOffset = new Vector3(0f, 1.3f, 0f);

	[SerializeField]
	public float cameraDamping = 0.1f;

	[SerializeField]
	public float lookAtDamping = 0.1f;

	[SerializeField]
	public float referenceDamping = 0.2f;

	[SerializeField]
	public Vector3 raycastOffset = Vector3.zero;
}
