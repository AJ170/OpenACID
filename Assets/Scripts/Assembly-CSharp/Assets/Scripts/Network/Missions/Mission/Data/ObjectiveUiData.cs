using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class ObjectiveUiData : ObjectiveBaseData
	{
		public string TargetPath = string.Empty;

		public EAction Action;

		public bool WaitForInput;

		public ObjectiveUiData()
		{
		}

		public ObjectiveUiData(ObjectiveUiContent _objectiveUiContent)
			: base(_objectiveUiContent.ObjectiveBaseContent)
		{
			TargetPath = _objectiveUiContent.TargetPath;
			Action = _objectiveUiContent.Action;
			WaitForInput = _objectiveUiContent.WaitForInput;
		}

		public override IExtensible SerializeProto()
		{
			ObjectiveUiContent objectiveUiContent = new ObjectiveUiContent();
			objectiveUiContent.Action = Action;
			objectiveUiContent.ObjectiveBaseContent = (ObjectiveBaseContent)base.SerializeProto();
			objectiveUiContent.TargetPath = TargetPath;
			objectiveUiContent.WaitForInput = WaitForInput;
			return objectiveUiContent;
		}
	}
}
