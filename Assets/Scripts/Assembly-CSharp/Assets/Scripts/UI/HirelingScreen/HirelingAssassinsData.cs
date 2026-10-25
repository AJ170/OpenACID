using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.RuleSystem;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;

namespace Assets.Scripts.UI.HirelingScreen
{
	public class HirelingAssassinsData
	{
		private Assets.Scripts.Mission.Definitions.SkillData.ISkillDefinition m_passiveSkill;

		private Assets.Scripts.Mission.Definitions.SkillData.ISkillDefinition m_activeSkill;

		public HirelingAssassin HirelingAssassinData { get; private set; }

		public string HirelingID
		{
			get
			{
				return HirelingAssassinData.hirelingId;
			}
		}

		public string Username
		{
			get
			{
				return HirelingAssassinData.userName;
			}
		}

		public string AssassinsName
		{
			get
			{
				return HirelingAssassinData.GetHirelingName();
			}
		}

		public string ClassName
		{
			get
			{
				return LocalizationManager.Instance.Get(UbiCore.Utils.Singleton<UIDataMapper>.Instance.AssassinClassDescriptions[(int)HirelingAssassinData.classTypeId].NameId);
			}
		}

		public Assassin.EClassTypeId ClassType
		{
			get
			{
				return HirelingAssassinData.classTypeId;
			}
		}

		public int RewardCount
		{
			get
			{
				return (int)Singleton<StateManager>.Instance.TemplateManager.Globals.usedFriendHirelingFcReward.amount;
			}
		}

		public EResourceType RewardType
		{
			get
			{
				return Singleton<StateManager>.Instance.TemplateManager.Globals.usedFriendHirelingFcReward.type;
			}
		}

		public long ItemPow
		{
			get
			{
				return HirelingAssassinData.itemPower;
			}
		}

		public int LevelValue
		{
			get
			{
				return HirelingAssassinData.level;
			}
		}

		public string Relation
		{
			get
			{
				if (RelationType == HirelingAssassin.EHirelingType.Friend)
				{
					return LocalizationManager.Instance.Get(4550);
				}
				return string.Empty;
			}
		}

		public bool ShowRelationSymbol
		{
			get
			{
				return RelationType == HirelingAssassin.EHirelingType.Friend;
			}
		}

		public HirelingAssassin.EHirelingType RelationType
		{
			get
			{
				return HirelingAssassinData.hirelingType;
			}
		}

		public Assets.Scripts.Mission.Definitions.SkillData.ISkillDefinition ActiveSkill
		{
			get
			{
				if (m_activeSkill == null)
				{
					List<Assets.Scripts.Mission.Definitions.SkillData.ISkillDefinition> skillDefinitions = HirelingAssassinData.GetSkillDefinitions();
					m_activeSkill = skillDefinitions.Find((Assets.Scripts.Mission.Definitions.SkillData.ISkillDefinition _x) => _x.ActivationType != ESkillActivationType.PassiveHireling);
				}
				return m_activeSkill;
			}
		}

		public Assets.Scripts.Mission.Definitions.SkillData.ISkillDefinition PassiveSkill
		{
			get
			{
				if (m_passiveSkill == null)
				{
					List<Assets.Scripts.Mission.Definitions.SkillData.ISkillDefinition> skillDefinitions = HirelingAssassinData.GetSkillDefinitions();
					m_passiveSkill = skillDefinitions.Find((Assets.Scripts.Mission.Definitions.SkillData.ISkillDefinition _x) => _x.ActivationType == ESkillActivationType.PassiveHireling);
				}
				return m_passiveSkill;
			}
		}

		public HirelingAssassinsData(HirelingAssassin _hirelingAssassinData)
		{
			HirelingAssassinData = _hirelingAssassinData;
		}
	}
}
