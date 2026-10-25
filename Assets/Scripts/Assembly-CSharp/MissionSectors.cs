using System.Collections.Generic;
using Assets.Scripts.Engine;
using Assets.Scripts.Network.Missions.Math;
using Assets.Scripts.Network.Missions.Mission.Data;
using Mission.WingedEdge;
using SharedBaseLib.Core;
using UnityEngine;

public class MissionSectors : MissionBase<MissionSectorsData>
{
	public class Edge
	{
		public int IndexFrom;

		public int IndexTo;
	}

	private BBList<Edge> m_edges = new BBList<Edge>();

	public BBList<Vector2> Vertices = new BBList<Vector2>();

	private WingedEdgeData m_wingedEdgeData = new WingedEdgeData();

	public List<Edge> Edges
	{
		get
		{
			return m_edges;
		}
	}

	public void AddEdge(int _from, int _to, bool _update = true)
	{
		if (_from < 0 || _to < 0 || _from == _to)
		{
			return;
		}
		for (int i = 0; i < Edges.Count; i++)
		{
			if ((Edges[i].IndexFrom == _from && Edges[i].IndexTo == _to) || (Edges[i].IndexFrom == _to && Edges[i].IndexTo == _from))
			{
				return;
			}
		}
		Edges.Add(new Edge
		{
			IndexFrom = _from,
			IndexTo = _to
		});
		if (_update)
		{
			ComputeSectors();
		}
	}

	public void Clear()
	{
		Edges.Clear();
		Vertices.Clear();
		ComputeSectors();
	}

	public int AddVertex(Vector2 _worldPos2d)
	{
		int num = Select(_worldPos2d);
		if (num < 0)
		{
			Vertices.Add(_worldPos2d);
			return Vertices.Count - 1;
		}
		return num;
	}

	private void ComputeSectors()
	{
		m_wingedEdgeData = new WingedEdgeData();
		for (int i = 0; i < Vertices.Count; i++)
		{
			m_wingedEdgeData.AddVertex(Vertices[i]);
		}
		for (int j = 0; j < Edges.Count; j++)
		{
			m_wingedEdgeData.AddEdge(Edges[j].IndexFrom, Edges[j].IndexTo);
		}
		m_wingedEdgeData.BuildData();
		base.Data.Sectors.ClearIfNotEmpty();
		for (int k = 0; k < m_wingedEdgeData.Faces.Count; k++)
		{
			WingedEdgeFace wingedEdgeFace = m_wingedEdgeData.Faces[k];
			MissionSector missionSector = new MissionSector();
			foreach (WingedEdgeVertex vertex in wingedEdgeFace.Vertices)
			{
				missionSector.Vertices.Add(new Point2D(vertex.Position[0], vertex.Position[1]));
			}
			missionSector.Index = k;
			base.Data.Sectors.Add(missionSector);
		}
	}

	public MissionSector GetSector(Vector2 _checkPosition)
	{
		for (int i = 0; i < base.Data.Sectors.Count; i++)
		{
			MissionSector missionSector = base.Data.Sectors[i];
			if (missionSector.Contains(new Point2D(_checkPosition.x, _checkPosition.y)))
			{
				return missionSector;
			}
		}
		return null;
	}

	public override void OnPostRead(MissionBaseData _nodeData)
	{
		base.OnPostRead(_nodeData);
		BuildGeometryFromSectors();
	}

	public void RemoveEdge(int _edgeIndex)
	{
		Edges.RemoveAt(_edgeIndex);
		ComputeSectors();
	}

	public void RemoveVertex(int _index)
	{
		RemoveEdgesUsing(_index);
		Vertices.RemoveAt(_index);
		for (int i = 0; i < Edges.Count; i++)
		{
			if (Edges[i].IndexFrom > _index)
			{
				Edges[i].IndexFrom--;
			}
			if (Edges[i].IndexTo > _index)
			{
				Edges[i].IndexTo--;
			}
		}
	}

	public int Select(Vector2 _worldPosition)
	{
		float num = float.MaxValue;
		int result = -1;
		for (int num2 = Vertices.Count - 1; num2 >= 0; num2--)
		{
			float num3 = Vector3.SqrMagnitude(_worldPosition - Vertices[num2]);
			if (num3 < num)
			{
				result = num2;
				num = num3;
			}
		}
		if (num > 20f)
		{
			return -1;
		}
		return result;
	}

	public int SelectEdge(Vector2 _worldPos)
	{
		float num = float.MaxValue;
		int result = -1;
		for (int i = 0; i < Edges.Count; i++)
		{
			float closestMagnitude = GetClosestMagnitude(Edges[i], _worldPos);
			if (closestMagnitude < num)
			{
				num = closestMagnitude;
				result = i;
			}
		}
		return result;
	}

	private void BuildGeometryFromSectors()
	{
		Vertices.ClearIfNotEmpty();
		Edges.ClearIfNotEmpty();
		for (int i = 0; i < base.Data.Sectors.Count; i++)
		{
			MissionSector missionSector = base.Data.Sectors[i];
			for (int j = 0; j < missionSector.Vertices.Count; j++)
			{
				Point2D point2D = missionSector.Vertices[j];
				Point2D point2D2 = missionSector.Vertices[(j + 1) % missionSector.Vertices.Count];
				int from = AddVertex(new Vector2(point2D[0], point2D[1]));
				int to = AddVertex(new Vector2(point2D2[0], point2D2[1]));
				AddEdge(from, to, false);
			}
		}
		ComputeSectors();
	}

	private float GetClosestMagnitude(Edge _edge, Vector2 _point)
	{
		Vector2 vector = Vertices[_edge.IndexFrom];
		Vector2 vector2 = Vertices[_edge.IndexTo];
		Vector2 vector3 = new Vector2(_point.x - vector.x, _point.y - vector.y);
		Vector2 vector4 = new Vector2(vector2.x - vector.x, vector2.y - vector.y);
		float num = vector4.x * vector4.x + vector4.y * vector4.y;
		float num2 = vector3.x * vector4.x + vector3.y * vector4.y;
		float num3 = num2 / num;
		Vector2 vector5 = new Vector2(vector.x + vector4.x * num3, vector.y + vector4.y * num3);
		return Vector2.SqrMagnitude(vector5 - _point);
	}

	private void RemoveEdgesUsing(int _edgeIndex)
	{
		for (int num = Edges.Count - 1; num >= 0; num--)
		{
			if (Edges[num].IndexFrom == _edgeIndex || Edges[num].IndexTo == _edgeIndex)
			{
				Edges.RemoveAt(num);
			}
		}
		ComputeSectors();
	}
}
