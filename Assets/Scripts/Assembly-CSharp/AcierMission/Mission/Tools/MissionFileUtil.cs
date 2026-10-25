using System.Collections.Generic;
using Assets.Scripts.Network.Missions.Math;
using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.Network.Missions.Mission.Serialization;

namespace AcierMission.Mission.Tools
{
	public class MissionFileUtil
	{
		public class MapPositions
		{
			public List<Point3D> SpawnpointPositions = new List<Point3D>();

			public List<Point3D> ObjectPositions = new List<Point3D>();

			public List<Point3D> WaypointPositions = new List<Point3D>();
		}

		public static MapPositions GetMapPositions(string _filePath)
		{
			SerializedNode serializedNode = MissionSerializer.ReadBinaryFileWrapped(_filePath);
			List<SerializedNodeComponent> list = serializedNode.ComponentsRecursive();
			MapPositions mapPositions = new MapPositions();
			foreach (SerializedNodeComponent item in list)
			{
				if (item.DataType == typeof(TemplateTargetData) || item.DataType == typeof(MissionSpawnObjectData))
				{
					mapPositions.ObjectPositions.Add(item.Node.Position);
				}
				else if (item.DataType == typeof(TemplateSpawnPointData) || item.DataType == typeof(MissionNpcSpawnData))
				{
					mapPositions.SpawnpointPositions.Add(item.Node.Position);
				}
				else if (item.DataType == typeof(MissionWaypointData))
				{
					mapPositions.WaypointPositions.Add(item.Node.Position);
				}
			}
			return mapPositions;
		}
	}
}
