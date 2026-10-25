using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.UI.State;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class HirelingAssassinExtensions
	{
		public static List<Assets.Scripts.Mission.Definitions.SkillData.ISkillDefinition> GetSkillDefinitions(this HirelingAssassin _hireling)
		{
			List<Assets.Scripts.Mission.Definitions.SkillData.ISkillDefinition> list = new List<Assets.Scripts.Mission.Definitions.SkillData.ISkillDefinition>();
			AvatarClassDefinitions.AvatarClass avatarClass = Singleton<StateManager>.Instance.TemplateManager.AvatarDefinition.classes.First((AvatarClassDefinitions.AvatarClass c) => c.classTypeId == _hireling.classTypeId);
			string[] hirelingAbilityGuids = avatarClass.hirelingAbilityGuids;
			foreach (string guidString in hirelingAbilityGuids)
			{
				RuleData.IRuleDefinition definition = UbiCore.Utils.Singleton<RuleData>.Instance.GetDefinition(guidString);
				if (definition == null)
				{
					Assets.Scripts.Mission.Definitions.SkillData.ISkillDefinition definition2 = UbiCore.Utils.Singleton<Assets.Scripts.Mission.Definitions.SkillData>.Instance.GetDefinition(guidString, 0);
					if (definition2 != null)
					{
						list.Add(definition2);
					}
				}
			}
			return list;
		}
	}
}
