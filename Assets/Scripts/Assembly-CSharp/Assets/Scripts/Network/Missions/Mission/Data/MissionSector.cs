using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Network.Missions.Helpers;
using Assets.Scripts.Network.Missions.Math;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	public class MissionSector
	{
		public readonly List<Point2D> Vertices = new List<Point2D>();

		public int Index { get; set; }

		public MissionSector()
		{
		}

		public MissionSector(MissionSectorsContent.MissionSector _missionSector)
		{
			Index = _missionSector.Index;
			Vertices = new List<Point2D>();
			MissionSectorsContent.MissionSector.Vertex[] vertices = _missionSector.Vertices;
			foreach (MissionSectorsContent.MissionSector.Vertex vertex in vertices)
			{
				Vertices.Add(new Point2D(vertex.PositionX, vertex.PositionY));
			}
		}

		public bool Contains(Point2D _testPosition, bool _startsInside = false)
		{
			return MissionMathHelper.Contains2D(_testPosition, Vertices, _startsInside);
		}

		public bool Intersects(Point3D _lineStart, Point3D _lineEnd)
		{
			double num = IntersectionLength(_lineStart, _lineEnd);
			return num > 0.0;
		}

		public float IntersectionLength(Point3D _lineStart, Point3D _lineEnd)
		{
			return MissionMathHelper.IntersectionLength2D((Point2D)_lineStart, (Point2D)_lineEnd, Vertices);
		}

		public bool IsVertex(float _x, float _y)
		{
			for (int i = 0; i < Vertices.Count; i++)
			{
				if (System.Math.Abs(Vertices[i].x - _x) < float.Epsilon && System.Math.Abs(Vertices[i].y - _y) < float.Epsilon)
				{
					return true;
				}
			}
			return false;
		}

		public override string ToString()
		{
			return string.Format("ID: {0}", Index);
		}

		public MissionSectorsContent.MissionSector SerializeProto()
		{
			MissionSectorsContent.MissionSector missionSector = new MissionSectorsContent.MissionSector();
			missionSector.Index = Index;
			MissionSectorsContent.MissionSector missionSector2 = missionSector;
			foreach (Point2D vertex2 in Vertices)
			{
				MissionSectorsContent.MissionSector.Vertex vertex = new MissionSectorsContent.MissionSector.Vertex();
				vertex.PositionX = vertex2.x;
				vertex.PositionY = vertex2.y;
				MissionSectorsContent.MissionSector.Vertex item = vertex;
				missionSector2.Vertices = missionSector2.Vertices.Add(item);
			}
			return missionSector2;
		}
	}
}
