using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.SkillTreeScreen
{
	public class InfoPanelHandler : MonoBehaviour
	{
		private static readonly StringId s_Rank = 1192;

		[SerializeField]
		private UIExtendedLabel m_heading;

		[SerializeField]
		private UIExtendedLabel m_subHeading;

		[SerializeField]
		private UIExtendedLabel m_rank;

		[SerializeField]
		private UIExtendedLabel m_description;

		[SerializeField]
		private UITexture m_icon;

		[SerializeField]
		private RuleListHandler m_current;

		[SerializeField]
		private RuleListHandler m_next;

		[SerializeField]
		private UITable m_table;

		[SerializeField]
		private UIAlignmentBox m_alignmentBox;

		[SerializeField]
		private UIScrollView m_scrollView;

		private Color m_defaultColor;

		private SkillTreeData m_data;

		private int m_pendingEvents;

		private string m_colorCode;

		public void Init(SkillTreeData _data)
		{
			m_data = _data;
			EnableListeners();
			UpdateHandler();
		}

		public void Clear()
		{
			DisableListeners();
		}

		private void Awake()
		{
			m_defaultColor = m_rank.color;
			m_colorCode = ColorExtensions.ToNGUIColorString(UIDataMapper.UITextColors.SkillColor);
		}

		private void OnDestroy()
		{
			if (m_data != null)
			{
				EventDelegate.Remove(m_data.onDataChanged, OnDataUpdated);
				EventDelegate.Remove(m_data.onSkillSelectionChange, OnSkillSelectionChange);
			}
			m_data = null;
		}

		private void EnableListeners()
		{
			if (m_data != null)
			{
				EventDelegate.Add(m_data.onDataChanged, OnDataUpdated);
				EventDelegate.Add(m_data.onSkillSelectionChange, OnSkillSelectionChange);
			}
		}

		private void DisableListeners()
		{
			if (m_data != null)
			{
				EventDelegate.Remove(m_data.onDataChanged, OnDataUpdated);
				EventDelegate.Remove(m_data.onSkillSelectionChange, OnSkillSelectionChange);
			}
			DisableScreen();
		}

		private void DisableScreen()
		{
			m_current.Visible = false;
			m_next.Visible = false;
			if (m_pendingEvents > 0)
			{
				EventDelegate.Remove(m_next.onAlign, OnAligned);
				EventDelegate.Remove(m_current.onAlign, OnAligned);
			}
			m_pendingEvents = 0;
		}

		private void OnDataUpdated()
		{
			UpdateHandler();
		}

		private void OnSkillSelectionChange()
		{
			UpdateHandler();
		}

		private void UpdateHandler()
		{
			DisableScreen();
			Assets.Scripts.UI.Utils.Extensions.SkillData selectedSkillData = m_data.SelectedSkillData;
			if (selectedSkillData == null)
			{
				return;
			}
			Assets.Scripts.Mission.Definitions.SkillData.ISkillDefinition definition = UbiCore.Utils.Singleton<Assets.Scripts.Mission.Definitions.SkillData>.Instance.GetDefinition(selectedSkillData.SkillId, (selectedSkillData.Level <= 0) ? 1 : selectedSkillData.Level);
			if (definition != null)
			{
				m_icon.mainTexture = definition.IconUp.GetObject<Texture2D>();
				m_heading.supportEncoding = true;
				m_heading.text = m_colorCode + LocalizationManager.Instance.Get(definition.NameLocaKey.Key);
				m_subHeading.text = LocalizationManager.Instance.Get((!selectedSkillData.Active) ? 1191 : 1190);
				m_rank.supportEncoding = true;
				m_rank.text = string.Format("{0}{1}[-]", (selectedSkillData.Level != 5) ? m_defaultColor.ToNGUIColorString() : m_data.MaxLevelColor.ToNGUIColorString(), LocalizationManager.Instance.Get(s_Rank, new StringMapper
				{
					{ "Skill_Rank_Current", selectedSkillData.Level },
					{ "Skill_Rank_Max", 5 }
				}));
				m_rank.gameObject.SetActive(selectedSkillData.Level > 0);
				RuleData.IRuleDefinition ruleDefinition = definition.RuleDefinition;
				StringMapper stringMapper = new StringMapper();
				stringMapper.Add("rank", selectedSkillData.Level);
				StringMapper stringMapper2 = stringMapper;
				stringMapper2.Append(ruleDefinition.GetStringMapperForRules(definition.DescriptionLocaKey.Key, 0));
				m_description.supportEncoding = true;
				m_description.text = LocalizationManager.Instance.Get(definition.DescriptionLocaKey.Key, stringMapper2);
				int[] stats = FilterRules(selectedSkillData.SkillId);
				if (selectedSkillData.Level > 0)
				{
					m_pendingEvents++;
					EventDelegate.Add(m_current.onAlign, OnAligned, true);
					m_current.Visible = true;
					m_current.UpdateData(selectedSkillData.SkillId, selectedSkillData.Level, selectedSkillData.Level, stats, (selectedSkillData.Level != 5) ? RuleListHandler.s_CurrentId : RuleListHandler.s_MaxRank, m_defaultColor, m_defaultColor);
				}
				if (selectedSkillData.Level < 5)
				{
					m_pendingEvents++;
					EventDelegate.Add(m_next.onAlign, OnAligned, true);
					m_next.Visible = true;
					m_next.UpdateData(selectedSkillData.SkillId, selectedSkillData.Level + 1, selectedSkillData.Level, stats, RuleListHandler.s_NextRank, m_data.NextLevelColor, m_defaultColor);
				}
			}
		}

		private void OnAligned()
		{
			m_pendingEvents--;
			if (m_pendingEvents == 0)
			{
				EventDelegate.Add(m_alignmentBox.OnReposition, OnBoxAligned, true);
				m_alignmentBox.Reposition();
			}
		}

		private void OnBoxAligned()
		{
			m_table.Reposition();
			m_scrollView.ResetPosition();
		}

		private static int[] FilterRules(UISkillId _skill)
		{
			Assets.Scripts.Mission.Definitions.SkillData.ISkillDefinition definition = UbiCore.Utils.Singleton<Assets.Scripts.Mission.Definitions.SkillData>.Instance.GetDefinition(_skill, 1);
			Assets.Scripts.Mission.Definitions.SkillData.ISkillDefinition definition2 = UbiCore.Utils.Singleton<Assets.Scripts.Mission.Definitions.SkillData>.Instance.GetDefinition(_skill, 5);
			List<int> list = new List<int>();
			for (StatTypes.EStatType eStatType = StatTypeExtensions.RulesStart(); eStatType < StatTypeExtensions.RulesEnd(); eStatType++)
			{
				if (eStatType.IsUiRelevantRuleStat())
				{
					IReadonlyStatValue readonlyStatValue = definition.RuleDefinition.GetReadonlyStatValue(eStatType);
					IReadonlyStatValue readonlyStatValue2 = definition2.RuleDefinition.GetReadonlyStatValue(eStatType);
					if (!readonlyStatValue.HasEqualValue(readonlyStatValue2))
					{
						list.Add((int)eStatType);
					}
				}
			}
			return list.ToArray();
		}
	}
}
