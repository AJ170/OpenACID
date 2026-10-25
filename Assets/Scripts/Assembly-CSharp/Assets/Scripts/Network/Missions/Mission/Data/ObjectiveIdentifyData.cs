using System;
using System.Collections.Generic;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class ObjectiveIdentifyData : ObjectiveTargetData
	{
		public const float MaximumRangeSqr = 2500f;

		public bool FailIfTargetDies = true;

		public bool UsePlainSight;

		public int Minimum;

		public List<long> TargetAreas { get; private set; }

		public ObjectiveIdentifyData()
		{
			TargetAreas = new List<long>();
		}

		public ObjectiveIdentifyData(ObjectiveIdentifyContent _objectiveIdentifyContent)
			: base(_objectiveIdentifyContent.ObjectiveTargetContent)
		{
			FailIfTargetDies = _objectiveIdentifyContent.FailIfTargetDies;
			UsePlainSight = _objectiveIdentifyContent.UsePlainSight;
			Minimum = _objectiveIdentifyContent.Minimum;
			TargetAreas = new List<long>(_objectiveIdentifyContent.TargetAreas);
		}

		public override IExtensible SerializeProto()
		{
			ObjectiveIdentifyContent objectiveIdentifyContent = new ObjectiveIdentifyContent();
			objectiveIdentifyContent.FailIfTargetDies = FailIfTargetDies;
			objectiveIdentifyContent.UsePlainSight = UsePlainSight;
			objectiveIdentifyContent.Minimum = Minimum;
			objectiveIdentifyContent.ObjectiveTargetContent = (ObjectiveTargetContent)base.SerializeProto();
			ObjectiveIdentifyContent objectiveIdentifyContent2 = objectiveIdentifyContent;
			objectiveIdentifyContent2.TargetAreas = objectiveIdentifyContent2.TargetAreas.AddRange(TargetAreas);
			return objectiveIdentifyContent2;
		}
	}
}
