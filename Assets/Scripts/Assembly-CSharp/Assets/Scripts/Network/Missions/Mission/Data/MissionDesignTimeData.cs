using System;
using System.Collections.Generic;
using AcierProtocol;
using ProtoBuf;
using SharedBaseLib.System.Extensions;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class MissionDesignTimeData : MissionBaseData
	{
		public class GraphLayoutData
		{
			public class Rect
			{
				private float m_height;

				private float m_width;

				private float m_xMin;

				private float m_yMin;

				public float XMin
				{
					get
					{
						return m_xMin;
					}
				}

				public float YMin
				{
					get
					{
						return m_yMin;
					}
				}

				public float Width
				{
					get
					{
						return m_width;
					}
				}

				public float Height
				{
					get
					{
						return m_height;
					}
				}

				public Rect(MissionDesignTimeContent.Rectangle _rectangle)
				{
					m_xMin = _rectangle.XMin;
					m_yMin = _rectangle.YMin;
					m_width = _rectangle.Width;
					m_height = _rectangle.Height;
				}

				public Rect(float _xMin, float _yMin, float _width, float _height)
				{
					m_xMin = _xMin;
					m_yMin = _yMin;
					m_width = _width;
					m_height = _height;
				}

				public MissionDesignTimeContent.Rectangle Serialize()
				{
					MissionDesignTimeContent.Rectangle rectangle = new MissionDesignTimeContent.Rectangle();
					rectangle.Height = m_height;
					rectangle.Width = m_width;
					rectangle.XMin = m_xMin;
					rectangle.YMin = m_yMin;
					return rectangle;
				}
			}

			private class NodeLayout
			{
				public long ID { get; set; }

				public Rect Rectangle { get; set; }
			}

			private List<NodeLayout> m_data = new List<NodeLayout>();

			public Rect this[long _id]
			{
				get
				{
					NodeLayout nodeLayout = m_data.FirstOrDefault((NodeLayout _x) => _x.ID == _id);
					if (nodeLayout == null)
					{
						throw new KeyNotFoundException("The key " + _id + " was not present.");
					}
					return nodeLayout.Rectangle;
				}
				set
				{
					NodeLayout nodeLayout = m_data.FirstOrDefault((NodeLayout x) => x.ID == _id);
					if (nodeLayout != null)
					{
						m_data.Remove(nodeLayout);
					}
					m_data.Add(new NodeLayout
					{
						ID = _id,
						Rectangle = value
					});
				}
			}

			public GraphLayoutData(IEnumerable<MissionDesignTimeContent.NodeLayout> _graphLayout)
			{
				foreach (MissionDesignTimeContent.NodeLayout item in _graphLayout)
				{
					m_data.Add(new NodeLayout
					{
						ID = item.ID,
						Rectangle = new Rect(item.Rectangle)
					});
				}
			}

			public GraphLayoutData()
			{
			}

			public List<MissionDesignTimeContent.NodeLayout> Serialize()
			{
				List<MissionDesignTimeContent.NodeLayout> list = new List<MissionDesignTimeContent.NodeLayout>();
				foreach (NodeLayout datum in m_data)
				{
					list.Add(new MissionDesignTimeContent.NodeLayout
					{
						ID = datum.ID,
						Rectangle = datum.Rectangle.Serialize()
					});
				}
				return list;
			}

			public bool ContainsKey(long _id)
			{
				return m_data.Any((NodeLayout _x) => _x.ID == _id);
			}

			public void Clear()
			{
				m_data.Clear();
			}
		}

		public GraphLayoutData GraphLayout { get; set; }

		public MissionDesignTimeData()
		{
			GraphLayout = new GraphLayoutData();
		}

		public MissionDesignTimeData(MissionDesignTimeContent _missionDesignTimeContent)
			: base(_missionDesignTimeContent.MissionBaseContent)
		{
			GraphLayout = new GraphLayoutData(_missionDesignTimeContent.GraphLayout);
		}

		public override IExtensible SerializeProto()
		{
			MissionDesignTimeContent missionDesignTimeContent = new MissionDesignTimeContent();
			missionDesignTimeContent.MissionBaseContent = (MissionBaseContent)base.SerializeProto();
			MissionDesignTimeContent missionDesignTimeContent2 = missionDesignTimeContent;
			missionDesignTimeContent2.GraphLayout = GraphLayout.Serialize().ToArray();
			return missionDesignTimeContent2;
		}
	}
}
