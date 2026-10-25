using UbiCore.CameraController;
using UnityEngine;

internal class FreeCameraController : CameraController
{
	private readonly float m_panSpeed = 0.05f;

	private readonly float m_zoomSpeed = 0.05f;

	private readonly float m_rotateSpeed = 0.15f;

	private float m_startTouchTime;

	private Vector3 m_lastPosition;

	private Vector3 m_startPosition;

	private bool m_decidedBetweenPanAndZoom;

	private bool m_isPanningAndNotZooming;

	private Quaternion m_direction;

	public override bool suppressesInteraction
	{
		get
		{
			return true;
		}
	}

	private void Start()
	{
		m_direction = base.transform.rotation;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			m_startTouchTime = Time.time;
			m_startPosition = (m_lastPosition = Input.mousePosition);
			m_decidedBetweenPanAndZoom = false;
		}
		if (Time.time - m_startTouchTime < 0.2f)
		{
			Vector3 vector3 = m_lastPosition - Input.mousePosition;
			if (Input.GetMouseButtonUp(0))
			{
				m_direction = Quaternion.LookRotation(GetComponent<Camera>().ScreenPointToRay(Input.mousePosition).direction);
			}
		}
		else if (Input.GetMouseButton(0))
		{
			if (!m_decidedBetweenPanAndZoom)
			{
				Vector3 vector = m_lastPosition - Input.mousePosition;
				m_isPanningAndNotZooming = Mathf.Abs(vector.x) > Mathf.Abs(vector.y);
				m_decidedBetweenPanAndZoom = true;
			}
			Vector3 vector2 = m_lastPosition - Input.mousePosition;
			if (m_isPanningAndNotZooming)
			{
				base.transform.Translate(vector2.x * m_panSpeed, vector2.y * m_panSpeed, 0f, Space.Self);
			}
			else
			{
				base.transform.Translate(0f, 0f, (0f - vector2.y) * m_zoomSpeed, Space.Self);
			}
		}
		if (Input.GetMouseButton(0))
		{
			m_lastPosition = Input.mousePosition;
		}
		base.transform.rotation = Quaternion.Lerp(base.transform.rotation, m_direction, m_rotateSpeed);
	}
}
