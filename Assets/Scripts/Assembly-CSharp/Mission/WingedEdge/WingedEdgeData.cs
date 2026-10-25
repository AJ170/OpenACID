using System.Collections.Generic;
using SharedBaseLib.Core;
using UnityEngine;

namespace Mission.WingedEdge
{
	public class WingedEdgeData
	{
		private readonly BBList<WingedEdgeEdge> m_edges = new BBList<WingedEdgeEdge>();

		private readonly BBList<WingedEdgeFace> m_faces = new BBList<WingedEdgeFace>();

		private readonly BBList<WingedEdgeVertex> m_vertices = new BBList<WingedEdgeVertex>();

		public List<WingedEdgeFace> Faces
		{
			get
			{
				return m_faces;
			}
		}

		public static void TestCase()
		{
			WingedEdgeData wingedEdgeData = new WingedEdgeData();
			wingedEdgeData.AddVertex(new Vector2(-1f, 1f));
			wingedEdgeData.AddVertex(new Vector2(0f, 1f));
			wingedEdgeData.AddVertex(new Vector2(1f, 1f));
			wingedEdgeData.AddVertex(new Vector2(0f, 0f));
			wingedEdgeData.AddVertex(new Vector2(0f, -1f));
			wingedEdgeData.AddEdge(0, 3);
			wingedEdgeData.AddEdge(1, 3);
			wingedEdgeData.AddEdge(2, 3);
			wingedEdgeData.AddEdge(4, 3);
			wingedEdgeData.AddEdge(0, 1);
			wingedEdgeData.AddEdge(2, 1);
			wingedEdgeData.AddEdge(2, 4);
			wingedEdgeData.AddEdge(0, 4);
			wingedEdgeData.BuildData();
		}

		public void AddEdge(int _indexFrom, int _indexTo)
		{
			WingedEdgeVertex wingedEdgeVertex = m_vertices[_indexFrom];
			WingedEdgeVertex wingedEdgeVertex2 = m_vertices[_indexTo];
			WingedEdgeEdge wingedEdgeEdge = new WingedEdgeEdge();
			wingedEdgeEdge.VertexOne = wingedEdgeVertex;
			wingedEdgeEdge.VertexTwo = wingedEdgeVertex2;
			WingedEdgeEdge item = wingedEdgeEdge;
			wingedEdgeVertex.Edges.Add(item);
			wingedEdgeVertex2.Edges.Add(item);
			m_edges.Add(item);
		}

		public void AddVertex(Vector2 _vertex)
		{
			WingedEdgeVertex wingedEdgeVertex = new WingedEdgeVertex();
			wingedEdgeVertex.Position = _vertex;
			WingedEdgeVertex item = wingedEdgeVertex;
			m_vertices.Add(item);
		}

		public void BuildData()
		{
			for (int i = 0; i < m_vertices.Count; i++)
			{
				WingedEdgeVertex wingedEdgeVertex = m_vertices[i];
				wingedEdgeVertex.ConnectEdges();
			}
			for (int j = 0; j < m_edges.Count; j++)
			{
				WingedEdgeEdge wingedEdgeEdge = m_edges[j];
				wingedEdgeEdge.ConnectEdges();
			}
			for (int k = 0; k < m_edges.Count; k++)
			{
				WingedEdgeEdge item = m_edges[k];
				WingedEdgeFace item2 = BuildFace(new BBList<WingedEdgeEdge> { item }, true);
				WingedEdgeFace item3 = BuildFace(new BBList<WingedEdgeEdge> { item }, false);
				if (!Faces.Contains(item2))
				{
					Faces.Add(item2);
				}
				if (!Faces.Contains(item3))
				{
					Faces.Add(item3);
				}
			}
			if (Faces.Count != 0)
			{
				WingedEdgeFace wingedEdgeFace = FindOuterFace(GetMinimumVertex());
				if (wingedEdgeFace != null)
				{
					Faces.Remove(wingedEdgeFace);
				}
			}
		}

		public bool IsClockwise(List<WingedEdgeEdge> _currentPath)
		{
			float num = 0f;
			List<WingedEdgeVertex> list = ConvertEdgeToVertexList(_currentPath);
			for (int i = 0; i < list.Count; i++)
			{
				WingedEdgeVertex wingedEdgeVertex = list[i];
				WingedEdgeVertex wingedEdgeVertex2 = list[(i + 1) % _currentPath.Count];
				num += (wingedEdgeVertex2.Position[0] - wingedEdgeVertex.Position[0]) * (wingedEdgeVertex2.Position[1] + wingedEdgeVertex.Position[1]);
			}
			return num > 0f;
		}

		private static BBList<WingedEdgeVertex> ConvertEdgeToVertexList(List<WingedEdgeEdge> _currentPath)
		{
			BBList<WingedEdgeVertex> bBList = new BBList<WingedEdgeVertex>();
			for (int i = 0; i < _currentPath.Count; i++)
			{
				WingedEdgeEdge wingedEdgeEdge = _currentPath[i];
				WingedEdgeEdge wingedEdgeEdge2 = _currentPath[(i + 1) % _currentPath.Count];
				WingedEdgeVertex item = ((wingedEdgeEdge.VertexOne != wingedEdgeEdge2.VertexOne && wingedEdgeEdge.VertexOne != wingedEdgeEdge2.VertexTwo) ? wingedEdgeEdge.VertexOne : wingedEdgeEdge.VertexTwo);
				bBList.Add(item);
			}
			return bBList;
		}

		private WingedEdgeFace BuildFace(BBList<WingedEdgeEdge> _currentPath, bool _isLeft)
		{
			WingedEdgeEdge wingedEdgeEdge = _currentPath[_currentPath.Count - 1];
			WingedEdgeEdge wingedEdgeEdge2 = ((_currentPath.Count <= 1) ? null : _currentPath[_currentPath.Count - 2]);
			if (wingedEdgeEdge == null)
			{
				return null;
			}
			WingedEdgeEdge wingedEdgeEdge3 = (_isLeft ? ((wingedEdgeEdge2 != null && wingedEdgeEdge.PreviousEdgeLeft != wingedEdgeEdge2) ? wingedEdgeEdge.PreviousEdgeRight : wingedEdgeEdge.NextEdgeLeft) : ((wingedEdgeEdge2 != null && wingedEdgeEdge.PreviousEdgeRight != wingedEdgeEdge2) ? wingedEdgeEdge.PreviousEdgeLeft : wingedEdgeEdge.NextEdgeRight));
			if (wingedEdgeEdge3 == null || _currentPath.Contains(wingedEdgeEdge3))
			{
				return CreateFace(_currentPath, _isLeft);
			}
			_currentPath.Add(wingedEdgeEdge3);
			return BuildFace(_currentPath, _isLeft);
		}

		private WingedEdgeFace CreateFace(BBList<WingedEdgeEdge> _currentPath, bool _isLeft)
		{
			if (_currentPath == null)
			{
				return null;
			}
			if (_isLeft && _currentPath[0].FaceLeft != null)
			{
				return _currentPath[0].FaceLeft;
			}
			if (!_isLeft && _currentPath[0].FaceRight != null)
			{
				return _currentPath[0].FaceRight;
			}
			WingedEdgeFace wingedEdgeFace = new WingedEdgeFace();
			wingedEdgeFace.Edges = _currentPath;
			WingedEdgeFace wingedEdgeFace2 = wingedEdgeFace;
			for (int i = 0; i < _currentPath.Count; i++)
			{
				WingedEdgeEdge wingedEdgeEdge = _currentPath[i];
				WingedEdgeEdge wingedEdgeEdge2 = ((i < 1) ? null : _currentPath[i - 1]);
				if (_isLeft)
				{
					if (wingedEdgeEdge2 == null || wingedEdgeEdge.PreviousEdgeLeft == wingedEdgeEdge2)
					{
						wingedEdgeEdge.FaceLeft = wingedEdgeFace2;
					}
					else
					{
						wingedEdgeEdge.FaceRight = wingedEdgeFace2;
					}
				}
				else if (wingedEdgeEdge2 == null || wingedEdgeEdge.PreviousEdgeRight == wingedEdgeEdge2)
				{
					wingedEdgeEdge.FaceRight = wingedEdgeFace2;
				}
				else
				{
					wingedEdgeEdge.FaceLeft = wingedEdgeFace2;
				}
			}
			if (!IsClockwise(_currentPath))
			{
				_currentPath.Reverse();
			}
			wingedEdgeFace2.Vertices = ConvertEdgeToVertexList(_currentPath);
			return wingedEdgeFace2;
		}

		private WingedEdgeFace FindOuterFace(WingedEdgeVertex _minimumVertex)
		{
			if (_minimumVertex == null)
			{
				return null;
			}
			WingedEdgeEdge item = _minimumVertex.FindLeftNeighbour(new Vector2(0f, -1f));
			WingedEdgeFace wingedEdgeFace = BuildFace(new BBList<WingedEdgeEdge> { item }, true);
			WingedEdgeEdge item2 = _minimumVertex.FindRightNeighbour(new Vector2(0f, -1f));
			WingedEdgeFace wingedEdgeFace2 = BuildFace(new BBList<WingedEdgeEdge> { item2 }, false);
			if (wingedEdgeFace == null || wingedEdgeFace2 == null)
			{
				return null;
			}
			return (!IsFaceContaining(wingedEdgeFace, wingedEdgeFace2)) ? wingedEdgeFace2 : wingedEdgeFace;
		}

		private bool IsFaceContaining(WingedEdgeFace _outerFace, WingedEdgeFace _innerFace)
		{
			List<WingedEdgeVertex> vertices = _outerFace.Vertices;
			List<WingedEdgeVertex> list = new List<WingedEdgeVertex>(_innerFace.Vertices);
			for (int num = list.Count - 1; num >= 0; num--)
			{
				if (vertices.Contains(list[num]))
				{
					list.RemoveAt(num);
				}
			}
			if (list.Count == 0)
			{
				return true;
			}
			foreach (WingedEdgeVertex item in list)
			{
				if (_outerFace.Contains(item))
				{
					return true;
				}
			}
			return false;
		}

		private WingedEdgeVertex GetMinimumVertex()
		{
			WingedEdgeVertex wingedEdgeVertex = null;
			for (int i = 0; i < m_vertices.Count; i++)
			{
				WingedEdgeVertex wingedEdgeVertex2 = m_vertices[i];
				if (wingedEdgeVertex2 != null && (wingedEdgeVertex == null || wingedEdgeVertex2.Position[0] < wingedEdgeVertex.Position[0]))
				{
					wingedEdgeVertex = wingedEdgeVertex2;
				}
			}
			return wingedEdgeVertex;
		}
	}
}
