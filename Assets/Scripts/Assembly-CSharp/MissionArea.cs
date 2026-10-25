using System.Collections.Generic;
using Assets.Scripts.Mission.Utilities;
using Assets.Scripts.Network.Missions.Helpers;
using Assets.Scripts.Network.Missions.Mission.Data;
using SharedBaseLib.Core;
using UnityEngine;

public class MissionArea : MissionAreaBase<MissionAreaData>
{
	public Color[] FillColor = new Color[8]
	{
		new Color(1f, 0f, 0f, 0.4f),
		new Color(0f, 1f, 0f, 0.4f),
		new Color(0f, 0f, 1f, 0.4f),
		new Color(1f, 0.75f, 0.34f, 0.4f),
		new Color(1f, 1f, 1f, 0.4f),
		new Color(1f, 0f, 1f, 0.4f),
		new Color(0f, 0.8f, 0f),
		new Color(0f, 1f, 1f)
	};

	public int LineShift = -1;

	public int LineWidth = 2;

	public Color OtherFillColor = new Color(0.7f, 0.7f, 0.7f, 0.7f);

	[SerializeField]
	private BBList<AreaNode> m_areaNodes = new BBList<AreaNode>();

	private Vector2[] m_corners;

	private int m_lastChildCount;

	private Vector3 m_center;

	public IList<AreaNode> AreaNodes
	{
		get
		{
			return m_areaNodes;
		}
	}

	public Vector3[] ComputeAreaNodePositions()
	{
		Vector3[] array = new Vector3[AreaNodes.Count];
		for (int i = 0; i < AreaNodes.Count; i++)
		{
			array[i] = AreaNodes[i].transform.position;
		}
		return array;
	}

	public void CenterNode(int _index)
	{
		AreaNode areaNode = AreaNodes[_index];
		int count = AreaNodes.Count;
		AreaNode areaNode2 = AreaNodes[(_index + 1) % count];
		AreaNode areaNode3 = AreaNodes[(_index + count - 1) % count];
		Vector3 localPosition = areaNode2.transform.localPosition;
		Vector3 localPosition2 = areaNode3.transform.localPosition;
		Vector3 localPosition3 = (localPosition + localPosition2) / 2f;
		areaNode.transform.localPosition = localPosition3;
	}

	public AreaNode CreateNode()
	{
		GameObject gameObject = new GameObject("AreaNode");
		AreaNode result = gameObject.AddComponent<AreaNode>();
		gameObject.transform.parent = base.transform;
		UpdateAreaData();
		CenterNode(AreaNodes.Count - 1);
		return result;
	}

	public override bool IsInside(Vector3 _checkPosition)
	{
		return MissionMathHelper.IsInside(m_corners, _checkPosition);
	}

	public override void OnDrawGizmosSelected()
	{
		if (AreaNodes != null)
		{
			Gizmos.color = Color.red;
			for (int i = 0; i < AreaNodes.Count; i++)
			{
				AreaNode areaNode = AreaNodes[i];
				AreaNode areaNode2 = AreaNodes[(i + 1) % AreaNodes.Count];
				Gizmos.DrawLine(areaNode.transform.position, areaNode2.transform.position);
				Gizmos.DrawSphere(areaNode.transform.position, 0.2f);
			}
		}
	}

	public override void OnPreActivate()
	{
		UpdateAreaData();
		m_center = ComputeCenter();
		if (m_areaNodes.Count > 0)
		{
			Vector3 vector = base.transform.position - m_center;
			base.transform.position = m_center;
			for (int i = 0; i < m_areaNodes.Count; i++)
			{
				AreaNode areaNode = m_areaNodes[i];
				areaNode.transform.position += vector;
			}
		}
		base.OnPreActivate();
	}

	private Vector3 ComputeCenter()
	{
		Vector3 zero = Vector3.zero;
		for (int i = 0; i < m_areaNodes.Count; i++)
		{
			zero += m_areaNodes[i].transform.position;
		}
		return zero / m_areaNodes.Count;
	}

	public void RemoveAt(int _index)
	{
		Object.Destroy(m_areaNodes[_index].gameObject);
		m_areaNodes.RemoveAt(_index);
	}

	private Vector2[] ComputeWorldPosCorners()
	{
		Vector2[] array = new Vector2[AreaNodes.Count];
		for (int i = 0; i < AreaNodes.Count; i++)
		{
			Vector3 position = AreaNodes[i].transform.position;
			array[i] = new Vector2(position.x, position.z);
		}
		return array;
	}

	private void UpdateAreaData()
	{
		AreaNode[] componentsInChildren = base.gameObject.GetComponentsInChildren<AreaNode>();
		DeactivateNodes();
		AreaNode[] array = componentsInChildren;
		foreach (AreaNode areaNode in array)
		{
			if (!(areaNode == null) && !(areaNode.gameObject == null))
			{
				AreaNodes.Add(areaNode);
			}
		}
		m_lastChildCount = base.transform.childCount;
		m_corners = ComputeWorldPosCorners();
	}

	private void DeactivateNodes()
	{
		AreaNodes.Clear();
		m_lastChildCount = 0;
		m_corners = new Vector2[0];
	}
}
