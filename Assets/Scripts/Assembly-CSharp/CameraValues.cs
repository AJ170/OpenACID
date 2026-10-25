using UnityEngine;

public class CameraValues : MonoBehaviour
{
	[SerializeField]
	public FieldOfViewValues m_fieldOfViewValues;

	[SerializeField]
	public RaycastValues m_raycastValues;

	[SerializeField]
	public RaycastValues m_climbRaycastValues;

	[SerializeField]
	public ZoomAndRotateValues m_zoomAndRotateValues;

	[SerializeField]
	public OrbitValues m_orbitValues;

	[SerializeField]
	public RoamingValues m_roamingValues;

	[SerializeField]
	public DeadValues m_deadValues;
}
