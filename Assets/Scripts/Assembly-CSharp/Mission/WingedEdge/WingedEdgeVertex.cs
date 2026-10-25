using System;
using SharedBaseLib.Core;
using UnityEngine;

namespace Mission.WingedEdge
{
	public class WingedEdgeVertex
	{
		public BBList<WingedEdgeEdge> Edges = new BBList<WingedEdgeEdge>();

		public Vector2 Position;

		public void ConnectEdges()
		{
			Edges.Sort(Sorter);
		}

		public WingedEdgeEdge FindLeftNeighbour(Vector2 _backVector)
		{
			float num = (float)Math.PI * 2f;
			float num2 = (float)Math.Atan2(_backVector[1], _backVector[0]);
			WingedEdgeEdge result = null;
			for (int i = 0; i < Edges.Count; i++)
			{
				WingedEdgeVertex wingedEdgeVertex = Edges[i].VertexNot(this);
				Vector2 vector = wingedEdgeVertex.Position - Position;
				float num3 = (float)Math.Atan2(vector[1], vector[0]);
				float num4 = num2 - num3;
				num4 += (float)Math.PI * 2f;
				num4 %= (float)Math.PI * 2f;
				if (!(Math.Abs(num4) < float.Epsilon) && num4 < num)
				{
					num = num4;
					result = Edges[i];
				}
			}
			return result;
		}

		public WingedEdgeEdge FindRightNeighbour(Vector2 _backVector)
		{
			float num = 0f;
			float num2 = (float)Math.Atan2(_backVector[1], _backVector[0]);
			WingedEdgeEdge result = null;
			for (int i = 0; i < Edges.Count; i++)
			{
				WingedEdgeVertex wingedEdgeVertex = Edges[i].VertexNot(this);
				Vector2 vector = wingedEdgeVertex.Position - Position;
				float num3 = (float)Math.Atan2(vector[1], vector[0]);
				float num4 = num2 - num3;
				num4 += (float)Math.PI * 2f;
				num4 %= (float)Math.PI * 2f;
				if (!(Math.Abs(num4) < float.Epsilon) && num4 > num)
				{
					num = num4;
					result = Edges[i];
				}
			}
			return result;
		}

		public override string ToString()
		{
			return Position.ToString();
		}

		private int Sorter(WingedEdgeEdge _x, WingedEdgeEdge _y)
		{
			Vector2 position = Position;
			Vector2 vector = ((!(_x.VertexOne.Position == Position)) ? _x.VertexOne.Position : _x.VertexTwo.Position);
			Vector2 vector2 = ((!(_y.VertexOne.Position == Position)) ? _y.VertexOne.Position : _y.VertexTwo.Position);
			Vector2 vector3 = vector - position;
			Vector2 vector4 = vector2 - position;
			double num = Math.Atan2(vector3[1], vector3[0]);
			double value = Math.Atan2(vector4[1], vector4[0]);
			return num.CompareTo(value);
		}
	}
}
