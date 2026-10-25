using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.UI.DataHandling.Bridges;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.SkillTreeScreen
{
	public class RuleListHandler : MonoBehaviour
	{
		public List<EventDelegate> onAlign = new List<EventDelegate>();

		[SerializeField]
		private UIAlignmentBox m_table;

		[SerializeField]
		private GameObject m_dataPrefab;

		[SerializeField]
		private UIExtendedLabel m_heading;

		[SerializeField]
		private Transform m_anchorTarget;

		public static readonly StringId s_CurrentId = 708;

		public static readonly StringId s_NextRank = 709;

		public static readonly StringId s_MaxRank = 1552;

		public bool Visible
		{
			get
			{
				return base.gameObject.activeSelf;
			}
			set
			{
				if (m_table != null)
				{
					m_table.gameObject.SetActive(value);
				}
				if (base.gameObject != null)
				{
					base.gameObject.SetActive(value);
				}
				if (m_heading != null)
				{
					m_heading.gameObject.SetActive(value);
				}
			}
		}

		public void UpdateData(BBList<Assets.Scripts.Mission.Definitions.SkillData.SkillPreview> _skillPreviews, Color _colorDefault)
		{
			m_table.transform.ClearChildren();
			for (int i = 0; i < _skillPreviews.Count; i++)
			{
				StatTypes.EStatType ruleStatType = _skillPreviews[i].RuleStatType;
				float magnitude = _skillPreviews[i].Magnitude;
				GameObject gameObject = NGUITools.AddChild(m_table.gameObject, m_dataPrefab);
				UIWidget component = gameObject.GetComponent<UIWidget>();
				component.leftAnchor.target = m_anchorTarget.transform;
				component.rightAnchor.target = m_anchorTarget.transform;
				component.leftAnchor.Set(0f, 0f);
				component.rightAnchor.Set(1f, 0f);
				component.ForceUpdate(true);
				UIDataMapper instance = UbiCore.Utils.Singleton<UIDataMapper>.Instance;
				LocalizationManager instance2 = LocalizationManager.Instance;
				StatisticsInfoLabel component2 = gameObject.GetComponent<StatisticsInfoLabel>();
				bool isBonus = _skillPreviews[i].BonusType != Assets.Scripts.Mission.Definitions.SkillData.SkillPreview.EBonusType.Invalid;
				StatTypes.EStatUnit statUnit = StatTypes.GetStatUnit(ruleStatType);
				UIRule value = new UIRule(magnitude, ruleStatType, statUnit);
				component2.Set(instance2.Get(instance.RuleDescriptions[(int)ruleStatType].NameId), instance2.Get(instance.UnitDescriptions[(int)statUnit].NameId, new StringMapper { { "value", value } }), _colorDefault, isBonus);
			}
			EventDelegate.Add(m_table.OnReposition, OnReposition, true);
			m_table.RepositionNow = true;
		}

		public void UpdateData(UISkillId _skill, int _rank, int _rankCompare, int[] _stats, StringId _heading, Color _colorDefault, Color _colorSecondary)
		{
			m_table.transform.ClearChildren();
			m_heading.text = LocalizationManager.Instance.Get(_heading);
			Assets.Scripts.Mission.Definitions.SkillData.ISkillDefinition definition = UbiCore.Utils.Singleton<Assets.Scripts.Mission.Definitions.SkillData>.Instance.GetDefinition(_skill, _rank);
			object obj;
			if (_rank > _rankCompare && _rankCompare > 0)
			{
				Assets.Scripts.Mission.Definitions.SkillData.ISkillDefinition definition2 = UbiCore.Utils.Singleton<Assets.Scripts.Mission.Definitions.SkillData>.Instance.GetDefinition(_skill, _rankCompare);
				obj = definition2;
			}
			else
			{
				obj = null;
			}
			Assets.Scripts.Mission.Definitions.SkillData.ISkillDefinition skillDefinition = (Assets.Scripts.Mission.Definitions.SkillData.ISkillDefinition)obj;
			RuleData.IRuleDefinition ruleDefinition = definition.RuleDefinition;
			for (int i = 0; i < _stats.Length; i++)
			{
				StatTypes.EStatType statType = (StatTypes.EStatType)_stats[i];
				GameObject gameObject = NGUITools.AddChild(m_table.gameObject, m_dataPrefab);
				UIWidget component = gameObject.GetComponent<UIWidget>();
				component.leftAnchor.target = m_anchorTarget.transform;
				component.rightAnchor.target = m_anchorTarget.transform;
				component.leftAnchor.Set(0f, 0f);
				component.rightAnchor.Set(1f, 0f);
				component.ForceUpdate(true);
				StatisticsInfoLabel component2 = gameObject.GetComponent<StatisticsInfoLabel>();
				IReadonlyStatValue readonlyStatValue = ruleDefinition.GetReadonlyStatValueBase(statType) as IReadonlyStatValue;
				if (readonlyStatValue != null)
				{
					object obj2;
					if (skillDefinition != null && skillDefinition.RuleDefinition != null)
					{
						IReadonlyStatValue readonlyStatValue2 = skillDefinition.RuleDefinition.GetReadonlyStatValue(statType);
						obj2 = readonlyStatValue2;
					}
					else
					{
						obj2 = null;
					}
					IReadonlyStatValue readonlyStatValue3 = (IReadonlyStatValue)obj2;
					if (readonlyStatValue.StatType.IsUiRelevantRuleStat())
					{
						UIRule uIRule = new UIRule(readonlyStatValue, ruleDefinition);
						UIRule uIRule2 = ((readonlyStatValue3 == null || skillDefinition.RuleDefinition == null) ? null : new UIRule(readonlyStatValue3, skillDefinition.RuleDefinition));
						component2.Set(LocalizationManager.Instance.Get(UbiCore.Utils.Singleton<UIDataMapper>.Instance.RuleDescriptions[(int)readonlyStatValue.StatType].NameId), LocalizationManager.Instance.Get(UbiCore.Utils.Singleton<UIDataMapper>.Instance.UnitDescriptions[(int)readonlyStatValue.Unit].NameId, new StringMapper { { "value", uIRule } }), (uIRule2 != null && uIRule.DisplaysEqual(uIRule2)) ? _colorSecondary : _colorDefault);
					}
				}
			}
			EventDelegate.Add(m_table.OnReposition, OnReposition, true);
			m_table.Reposition();
		}

		private void Awake()
		{
		}

		private void OnReposition()
		{
			EventDelegate.Execute(onAlign);
		}
	}
}
