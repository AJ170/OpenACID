using System.Collections.Generic;
using SharedBaseLib.Core;
using UnityEngine;

namespace Mission.WingedEdge
{
	public class WingedEdgeFace
	{
		public BBList<WingedEdgeEdge> Edges = new BBList<WingedEdgeEdge>();

		public BBList<WingedEdgeVertex> Vertices = new BBList<WingedEdgeVertex>();

		public bool IsMatch(List<WingedEdgeVertex> _path)
		{
			if (_path == null)
			{
				return false;
			}
			if (Vertices == null)
			{
				return false;
			}
			if (_path.Count != Vertices.Count)
			{
				return false;
			}
			for (int i = 0; i < _path.Count; i++)
			{
				WingedEdgeVertex item = _path[i];
				if (!Vertices.Contains(item))
				{
					return false;
				}
			}
			return true;
		}

		public bool Contains(WingedEdgeVertex _vertex)
		{
			Vector2 position = _vertex.Position;
			bool flag = false;
			int num = 0;
			int index = Vertices.Count - 1;
			while (num < Vertices.Count)
			{
				if (Vertices[num].Position[1] > position[1] != Vertices[index].Position[1] > position[1] && position[0] < (Vertices[index].Position[0] - Vertices[num].Position[0]) * (position[1] - Vertices[num].Position[1]) / (Vertices[index].Position[1] - Vertices[num].Position[1]) + Vertices[num].Position[0])
				{
					flag = !flag;
				}
				index = num++;
			}
			return flag;
		}
	}
}
