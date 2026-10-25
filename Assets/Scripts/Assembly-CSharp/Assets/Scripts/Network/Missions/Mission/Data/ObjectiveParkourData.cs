using System;
using AcierProtocol;
using ProtoBuf;
using SharedBaseLib.Core;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class ObjectiveParkourData : ObjectiveBaseData
	{
		private BBList<long> m_challengers = new BBList<long>();

		private BBList<long> m_waypoints = new BBList<long>();

		public int WaypointWaitingDuration;

		public BBList<long> Challengers
		{
			get
			{
				return m_challengers;
			}
			private set
			{
				m_challengers = value;
			}
		}

		public BBList<long> Waypoints
		{
			get
			{
				return m_waypoints;
			}
			private set
			{
				m_waypoints = value;
			}
		}

		public string TypeGuid { get; set; }

		public ObjectiveParkourData()
		{
		}

		public ObjectiveParkourData(ObjectiveParkourContent _objectiveParkourContent)
			: base(_objectiveParkourContent.ObjectiveBaseContent)
		{
			Challengers = new BBList<long>(_objectiveParkourContent.Challengers);
			Waypoints = new BBList<long>(_objectiveParkourContent.Waypoints);
			WaypointWaitingDuration = _objectiveParkourContent.WaypointWaitingDuration;
			TypeGuid = _objectiveParkourContent.TypeGuid;
		}

		public override IExtensible SerializeProto()
		{
			ObjectiveParkourContent objectiveParkourContent = new ObjectiveParkourContent();
			objectiveParkourContent.ObjectiveBaseContent = (ObjectiveBaseContent)base.SerializeProto();
			objectiveParkourContent.TypeGuid = TypeGuid;
			objectiveParkourContent.WaypointWaitingDuration = WaypointWaitingDuration;
			ObjectiveParkourContent objectiveParkourContent2 = objectiveParkourContent;
			objectiveParkourContent2.Challengers = Challengers.ToArray();
			objectiveParkourContent2.Waypoints = Waypoints.ToArray();
			return objectiveParkourContent2;
		}
	}
}
