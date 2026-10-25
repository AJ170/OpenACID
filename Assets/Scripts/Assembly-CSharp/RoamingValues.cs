using UnityEngine;

public class RoamingValues : MonoBehaviour
{
	[SerializeField]
	public float leadingDamping = 0.5f;

	[SerializeField]
	public float relaxDamping = 0.2f;

	[SerializeField]
	public float walkLeadingDistance;

	[SerializeField]
	public float runLeadingDistance = 1.3f;

	[SerializeField]
	public float sprintLeadingDistance = 2f;

	[SerializeField]
	public float sprintFieldOfViewModifier = -5f;

	[SerializeField]
	public Vector3 sprintCameraOffset = new Vector3(0f, -0.25f, -0.5f);

	[SerializeField]
	public float speedTransitionDamping = 0.8f;

	[SerializeField]
	public float resetVerticalDamping = 0.8f;

	[SerializeField]
	public OrbitValues overrideRaycastOffsetWhileClimbing;

	[SerializeField]
	public CameraAnimationCurve m_alignmentSpeedInPatrol;
}
