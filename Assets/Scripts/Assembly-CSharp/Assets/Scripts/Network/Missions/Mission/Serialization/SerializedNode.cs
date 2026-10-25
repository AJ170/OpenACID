using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Network.Missions.Math;
using Assets.Scripts.Network.Missions.Mission.Data;

namespace Assets.Scripts.Network.Missions.Mission.Serialization
{
	public class SerializedNode
	{
		public readonly List<SerializedNode> Children = new List<SerializedNode>();

		public readonly List<SerializedNodeComponent> Components = new List<SerializedNodeComponent>();

		public float EulerAnglesX;

		public float EulerAnglesY;

		public float EulerAnglesZ;

		public string Name;

		public float PositionX;

		public float PositionY;

		public float PositionZ;

		public Point3D Position
		{
			get
			{
				return new Point3D(PositionX, PositionY, PositionZ);
			}
		}

		public SerializedNode(float _positionX, float _positionY, float _positionZ, float _eulerAngleX, float _eulerAngleY, float _eulerAngleZ, string _name)
		{
			EulerAnglesX = _eulerAngleX;
			EulerAnglesY = _eulerAngleY;
			EulerAnglesZ = _eulerAngleZ;
			PositionX = _positionX;
			PositionY = _positionY;
			PositionZ = _positionZ;
			Name = _name;
		}

		public SerializedNode(AcierProtocol.SerializedNode _serializedNode)
		{
			Children = new List<SerializedNode>();
			AcierProtocol.SerializedNode[] children = _serializedNode.Children;
			foreach (AcierProtocol.SerializedNode serializedNode in children)
			{
				Children.Add(new SerializedNode(serializedNode));
			}
			Components = new List<SerializedNodeComponent>();
			AcierProtocol.SerializedNodeComponent[] components = _serializedNode.Components;
			foreach (AcierProtocol.SerializedNodeComponent component in components)
			{
				Components.Add(new SerializedNodeComponent(this, component));
			}
			EulerAnglesX = _serializedNode.EulerAnglesX;
			EulerAnglesY = _serializedNode.EulerAnglesY;
			EulerAnglesZ = _serializedNode.EulerAnglesZ;
			PositionX = _serializedNode.PositionX;
			PositionY = _serializedNode.PositionY;
			PositionZ = _serializedNode.PositionZ;
			Name = _serializedNode.Name;
		}

		public static double Distance(float[] _point, float[] _pointB, bool _useHeightFilter)
		{
			if (_useHeightFilter && (double)System.Math.Abs(_point[1] - _pointB[1]) > 0.5)
			{
				return double.PositiveInfinity;
			}
			double num = _point[0] - _pointB[0];
			double num2 = _point[2] - _pointB[2];
			return System.Math.Sqrt(num * num + num2 * num2);
		}

		public static double Distance(float[] _point, float[] _segmentStart, float[] _segmentEnd, bool _useHeightFilter)
		{
			double num = (double)System.Math.Min(_segmentStart[1], _segmentEnd[1]) - 0.5;
			double num2 = (double)System.Math.Max(_segmentStart[1], _segmentEnd[1]) + 0.5;
			if (_useHeightFilter && ((double)_point[1] < num || (double)_point[1] > num2))
			{
				return double.PositiveInfinity;
			}
			double num3 = _segmentEnd[0] - _segmentStart[0];
			double num4 = _segmentEnd[2] - _segmentStart[2];
			double num5 = System.Math.Abs(num3 * (double)(_segmentStart[2] - _point[2]) - (double)(_segmentStart[0] - _point[0]) * num4);
			double num6 = System.Math.Sqrt(num3 * num3 + num4 * num4);
			return num5 / num6;
		}

		public List<SerializedNodeComponent> ComponentsRecursive()
		{
			List<SerializedNodeComponent> list = new List<SerializedNodeComponent>(Components);
			for (int i = 0; i < Children.Count; i++)
			{
				list.AddRange(Children[i].ComponentsRecursive());
			}
			return list;
		}

		public SerializedNodeComponent GetComponent(string _componentType)
		{
			foreach (SerializedNodeComponent component in Components)
			{
				if (component.ComponentType == _componentType)
				{
					return component;
				}
			}
			return null;
		}

		public AcierProtocol.SerializedNode SerializeProto()
		{
			if (!AssertIDs())
			{
				return null;
			}
			return SerializeProtoInternal();
		}

		private AcierProtocol.SerializedNode SerializeProtoInternal()
		{
			AcierProtocol.SerializedNode serializedNode = new AcierProtocol.SerializedNode();
			serializedNode.Name = Name;
			serializedNode.PositionX = PositionX;
			serializedNode.PositionY = PositionY;
			serializedNode.PositionZ = PositionZ;
			serializedNode.EulerAnglesX = EulerAnglesX;
			serializedNode.EulerAnglesY = EulerAnglesY;
			serializedNode.EulerAnglesZ = EulerAnglesZ;
			foreach (SerializedNodeComponent component in Components)
			{
				if (component != null)
				{
					AcierProtocol.SerializedNodeComponent item = component.SerializeProto();
					serializedNode.Components = serializedNode.Components.Add(item);
				}
			}
			foreach (SerializedNode child in Children)
			{
				AcierProtocol.SerializedNode item2 = child.SerializeProtoInternal();
				serializedNode.Children = serializedNode.Children.Add(item2);
			}
			return serializedNode;
		}

		public override string ToString()
		{
			return string.Format("{0} (Children: {1}, Comp: {2}, {3})", Name, Children.Count, Components.Count, (Components.Count <= 0) ? "-" : Components[0].DataType.ToString());
		}

		private bool AssertIDs()
		{
			SerializedNodeComponent component = GetComponent("MissionRoot");
			if (component == null)
			{
				return false;
			}
			HashSet<long> hashSet = new HashSet<long>();
			List<SerializedNodeComponent> list = ComponentsRecursive();
			foreach (SerializedNodeComponent item in list)
			{
				MissionBaseData nodeData = item.NodeData;
				if (nodeData != null)
				{
					if (hashSet.Contains(nodeData.ID))
					{
						throw new Exception(string.Format("MISSION EXPORT ERROR: {0} has duplicate id: {1}", item.Node.Name, nodeData));
					}
					hashSet.Add(nodeData.ID);
				}
			}
			return true;
		}
	}
}
