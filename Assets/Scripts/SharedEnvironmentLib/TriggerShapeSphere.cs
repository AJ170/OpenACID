using UnityEngine;

internal class TriggerShapeSphere : TriggerShape
{
	[SerializeField]
	private float m_radius = 1f;

	private float m_radiusSq;

	public override Bounds bounds { get; protected set; }

	private void Start()
	{
		m_radiusSq = m_radius * m_radius;
		UpdateBounds();
	}

	private void UpdateBounds()
	{
		float num = m_radius * 2f;
		bounds = new Bounds(base.transform.position, new Vector3(num, num, num));
	}

	public override bool Inside(Vector3 _worldPos)
	{
		return Vector3.SqrMagnitude(base.transform.position - _worldPos) < m_radiusSq;
	}

	public override bool Inside(Plane[] _planes)
	{
		Vector3 position = base.transform.position;
		for (int i = 0; i < _planes.Length; i++)
		{
			if (_planes[i].GetDistanceToPoint(position) + m_radius <= 0f)
			{
				return false;
			}
		}
		return true;
	}

	public override void DrawGizmo()
	{
		Gizmos.DrawWireSphere(base.transform.position, m_radius);
	}
}
