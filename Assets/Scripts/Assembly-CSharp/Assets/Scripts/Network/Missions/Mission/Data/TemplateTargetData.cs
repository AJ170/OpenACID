using System;
using AcierProtocol;
using Mission.Helpers;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public class TemplateTargetData : MissionTargetData, IMissionTemplateData
	{
		public int SubCategories { get; set; }

		public TemplateTargetData()
		{
		}

		public TemplateTargetData(TemplateTargetContent _templateTargetContent)
			: base(_templateTargetContent.MissionTargetContent)
		{
			SubCategories = (int)_templateTargetContent.SubCategories.FromMask();
		}

		public override IExtensible SerializeProto()
		{
			TemplateTargetContent templateTargetContent = new TemplateTargetContent();
			templateTargetContent.MissionTargetContent = (MissionTargetContent)base.SerializeProto();
			TemplateTargetContent templateTargetContent2 = templateTargetContent;
			EObjectSubCategory[] array = ((EObjectSubCategory)SubCategories).ToFlagList();
			foreach (EObjectSubCategory item in array)
			{
				templateTargetContent2.SubCategories = templateTargetContent2.SubCategories.Add(item);
			}
			return templateTargetContent2;
		}
	}
}
