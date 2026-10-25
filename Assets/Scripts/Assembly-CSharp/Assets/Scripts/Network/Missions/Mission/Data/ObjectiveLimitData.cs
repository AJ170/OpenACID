using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class ObjectiveLimitData : ObjectiveBaseData
	{
		public float Limit;

		public ELimitType LimitType;

		public bool FlipComparer;

		public bool InvertedResult;

		public bool PreventFailingOnIncrease = true;

		public ObjectiveLimitData()
		{
		}

		public ObjectiveLimitData(ObjectiveLimitContent _objectiveLimitContent)
			: base(_objectiveLimitContent.ObjectiveBaseContent)
		{
			Limit = _objectiveLimitContent.Limit;
			LimitType = _objectiveLimitContent.LimitType;
			FlipComparer = _objectiveLimitContent.Invert;
			InvertedResult = _objectiveLimitContent.InvertedResult;
			PreventFailingOnIncrease = _objectiveLimitContent.PreventFailingOnIncrease;
		}

		public override IExtensible SerializeProto()
		{
			ObjectiveLimitContent objectiveLimitContent = new ObjectiveLimitContent();
			objectiveLimitContent.Invert = FlipComparer;
			objectiveLimitContent.Limit = Limit;
			objectiveLimitContent.LimitType = LimitType;
			objectiveLimitContent.InvertedResult = InvertedResult;
			objectiveLimitContent.PreventFailingOnIncrease = PreventFailingOnIncrease;
			objectiveLimitContent.ObjectiveBaseContent = (ObjectiveBaseContent)base.SerializeProto();
			return objectiveLimitContent;
		}
	}
}
