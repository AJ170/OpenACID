using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class ObjectiveGetInRangeStaticData : ObjectiveGetInRangeData
	{
		public string TypeGuid { get; set; }

		public ObjectiveGetInRangeStaticData()
		{
		}

		public ObjectiveGetInRangeStaticData(ObjectiveGetInRangeStaticContent _objectiveGetInRangeStaticContent)
			: base(_objectiveGetInRangeStaticContent.ObjectiveGetInRangeContent)
		{
			TypeGuid = _objectiveGetInRangeStaticContent.TypeGuid;
		}

		public override IExtensible SerializeProto()
		{
			ObjectiveGetInRangeStaticContent objectiveGetInRangeStaticContent = new ObjectiveGetInRangeStaticContent();
			objectiveGetInRangeStaticContent.ObjectiveGetInRangeContent = (ObjectiveGetInRangeContent)base.SerializeProto();
			objectiveGetInRangeStaticContent.TypeGuid = TypeGuid;
			return objectiveGetInRangeStaticContent;
		}
	}
}
