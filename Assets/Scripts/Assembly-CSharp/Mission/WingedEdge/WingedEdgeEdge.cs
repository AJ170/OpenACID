using UnityEngine;

namespace Mission.WingedEdge
{
	public class WingedEdgeEdge
	{
		public WingedEdgeFace FaceLeft;

		public WingedEdgeFace FaceRight;

		public WingedEdgeEdge NextEdgeLeft;

		public WingedEdgeEdge NextEdgeRight;

		public WingedEdgeEdge PreviousEdgeLeft;

		public WingedEdgeEdge PreviousEdgeRight;

		public WingedEdgeVertex VertexOne;

		public WingedEdgeVertex VertexTwo;

		public void ConnectEdges()
		{
			Vector2 backVector = VertexOne.Position - VertexTwo.Position;
			NextEdgeLeft = VertexTwo.FindLeftNeighbour(backVector);
			NextEdgeRight = VertexTwo.FindRightNeighbour(backVector);
			Vector2 backVector2 = VertexTwo.Position - VertexOne.Position;
			PreviousEdgeLeft = VertexOne.FindRightNeighbour(backVector2);
			PreviousEdgeRight = VertexOne.FindLeftNeighbour(backVector2);
		}

		public override string ToString()
		{
			return string.Format("{0} -> {1}", VertexOne.Position, VertexTwo.Position);
		}

		public WingedEdgeVertex VertexNot(WingedEdgeVertex _edgeVertex)
		{
			return (VertexTwo != _edgeVertex) ? VertexTwo : VertexOne;
		}
	}
}
