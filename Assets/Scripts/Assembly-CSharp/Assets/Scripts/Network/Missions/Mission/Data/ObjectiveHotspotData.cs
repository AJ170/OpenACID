using System;
using AcierProtocol;
using ProtoBuf;
using SharedBaseLib.Core;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class ObjectiveHotspotData : ObjectiveBaseData
	{
		private BBSimpleList<int> m_targetHotspots = new BBSimpleList<int>();

		public bool Invert;

		public BBSimpleList<int> HotSpots
		{
			get
			{
				return m_targetHotspots;
			}
			set
			{
				m_targetHotspots = value;
			}
		}

		public ObjectiveHotspotData()
		{
		}

		public ObjectiveHotspotData(ObjectiveHotspotContent _objectiveHotspotContent)
			: base(_objectiveHotspotContent.ObjectiveBaseContent)
		{
			Invert = _objectiveHotspotContent.Invert;
			m_targetHotspots = new BBSimpleList<int>(_objectiveHotspotContent.TargetHotspots);
		}

		public override IExtensible SerializeProto()
		{
			ObjectiveHotspotContent objectiveHotspotContent = new ObjectiveHotspotContent();
			objectiveHotspotContent.Invert = Invert;
			objectiveHotspotContent.ObjectiveBaseContent = (ObjectiveBaseContent)base.SerializeProto();
			ObjectiveHotspotContent objectiveHotspotContent2 = objectiveHotspotContent;
			m_targetHotspots.TrimExcess();
			objectiveHotspotContent2.TargetHotspots = m_targetHotspots.Data;
			return objectiveHotspotContent2;
		}
	}
}
