using System.Collections.Generic;
using UnityEngine;

public class WeaponTrail : MonoBehaviour
{
	private class Point
	{
		public Vector3 Position;

		public Quaternion Orientation;

		public float startTime;
	}

	public float StartHeight = 1f;

	public float EndHeight = 1f;

	public float LifeTime = 2f;

	public bool AlwaysUp;

	public float MinDistance = 0.1f;

	public float MinAngleDegrees = 5f;

	public Color StartColor = Color.white;

	public Color EndColor = new Color(1f, 1f, 1f, 0f);

	public Material TrailMaterial;

	public bool FadeOut;

	private GameObject m_TrailHolder;

	private MeshFilter m_TargetFilter;

	private Mesh m_RingMesh;

	private Transform m_ReferenceTransform;

	private static int ms_MaxSegments = 512;

	private static List<int> ms_FullTriangleList;

	private int m_RingStart;

	private int m_RingEnd;

	private Vector3 m_LastPosition = Vector3.zero;

	private Quaternion m_LastOrientation = Quaternion.identity;

	private Point[] m_SamplePoints;

	private void CreateTrailHolder()
	{
		m_TrailHolder = new GameObject();
		m_TrailHolder.name = "TrailHolder";
		if (!m_TrailHolder.GetComponent<MeshFilter>())
		{
			m_TargetFilter = m_TrailHolder.AddComponent<MeshFilter>();
		}
		if (!m_TrailHolder.GetComponent<MeshRenderer>())
		{
			m_TrailHolder.AddComponent<MeshRenderer>();
		}
		if ((bool)TrailMaterial && (bool)m_TrailHolder.GetComponent<Renderer>())
		{
			m_TrailHolder.GetComponent<Renderer>().material = TrailMaterial;
		}
		m_RingMesh = new Mesh();
		m_RingMesh.MarkDynamic();
		m_TargetFilter.sharedMesh = m_RingMesh;
		if (ms_FullTriangleList == null)
		{
			ms_FullTriangleList = new List<int>(ms_MaxSegments * 6);
			for (int i = 0; i < ms_MaxSegments; i++)
			{
				ms_FullTriangleList.Add(i * 2);
				ms_FullTriangleList.Add(i * 2 + 1);
				ms_FullTriangleList.Add(i * 2 + 2);
				ms_FullTriangleList.Add(i * 2 + 2);
				ms_FullTriangleList.Add(i * 2 + 1);
				ms_FullTriangleList.Add(i * 2 + 3);
			}
		}
		m_RingMesh.vertices = new Vector3[ms_MaxSegments * 2];
		m_RingMesh.colors = new Color[ms_MaxSegments * 2];
		Vector2[] array = new Vector2[ms_MaxSegments * 2];
		for (int j = 0; ms_MaxSegments > j; j++)
		{
			array[j * 2] = new Vector2(0f, 0f);
			array[j * 2 + 1] = new Vector2(0f, 1f);
		}
		m_RingMesh.uv = array;
		m_SamplePoints = new Point[ms_MaxSegments];
		for (int k = 0; k < m_SamplePoints.Length; k++)
		{
			m_SamplePoints[k] = new Point();
		}
		m_RingStart = 0;
		m_RingEnd = 0;
		m_LastPosition = Vector3.zero;
	}

	private void Start()
	{
		CreateTrailHolder();
		m_ReferenceTransform = base.transform;
	}

	private void OnDestroy()
	{
		if ((bool)m_TrailHolder)
		{
			Object.Destroy(m_TrailHolder);
		}
	}

	private int RingSize()
	{
		return (m_RingEnd <= m_RingStart) ? (m_RingEnd + ms_MaxSegments - m_RingStart) : (m_RingEnd - m_RingStart);
	}

	private bool RingEmpty()
	{
		return m_RingStart == m_RingEnd;
	}

	private void AdvanceRingMarker(ref int _marker)
	{
		_marker = (_marker + 1) % ms_MaxSegments;
	}

	private void LateUpdate()
	{
		if (!base.enabled && !FadeOut)
		{
			return;
		}
		float time = Time.time;
		while (!RingEmpty() && time > m_SamplePoints[m_RingStart].startTime + LifeTime)
		{
			AdvanceRingMarker(ref m_RingStart);
		}
		if (RingEmpty() || Vector3.Distance(m_LastPosition, m_ReferenceTransform.position) > MinDistance || Quaternion.Angle(m_LastOrientation, m_ReferenceTransform.rotation) > MinAngleDegrees)
		{
			if (!FadeOut)
			{
				m_SamplePoints[m_RingEnd].startTime = time;
				m_SamplePoints[m_RingEnd].Position = m_ReferenceTransform.position;
				m_SamplePoints[m_RingEnd].Orientation = m_ReferenceTransform.rotation;
				AdvanceRingMarker(ref m_RingEnd);
			}
			m_LastPosition = m_ReferenceTransform.position;
			m_LastOrientation = m_ReferenceTransform.rotation;
		}
		int num = RingSize();
		if (FadeOut && num < 2)
		{
			FadeOut = false;
			base.enabled = false;
		}
		if (num >= 2)
		{
			int _marker = m_RingStart;
			Vector3[] vertices = m_RingMesh.vertices;
			Vector2[] uv = m_RingMesh.uv;
			Color32[] colors = m_RingMesh.colors32;
			Vector3 vector = new Vector3(0f, 0f, StartHeight / 2f);
			Vector3 vector2 = new Vector3(0f, 0f, EndHeight / 2f);
			int num2 = 0;
			while (num2 < num)
			{
				Point point = m_SamplePoints[_marker];
				float num3 = (time - point.startTime) / LifeTime;
				Vector3 a = point.Orientation * vector;
				Vector3 b = point.Orientation * vector2;
				Vector3 vector3 = Vector3.Lerp(a, b, num3);
				vertices[num2 * 2] = point.Position + vector3;
				vertices[num2 * 2 + 1] = point.Position - vector3;
				uv[num2 * 2].x = num3;
				uv[num2 * 2 + 1].x = num3;
				Color32 color = Color.Lerp(StartColor, EndColor, num3);
				colors[num2 * 2] = color;
				colors[num2 * 2 + 1] = color;
				num2++;
				AdvanceRingMarker(ref _marker);
			}
			m_RingMesh.Clear();
			m_RingMesh.vertices = vertices;
			m_RingMesh.colors32 = colors;
			m_RingMesh.uv = uv;
			m_RingMesh.triangles = ms_FullTriangleList.GetRange(0, (num - 1) * 6).ToArray();
		}
	}
}
