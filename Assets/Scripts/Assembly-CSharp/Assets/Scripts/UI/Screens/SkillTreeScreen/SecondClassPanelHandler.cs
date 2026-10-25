using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.SkillTreeScreen
{
	public class SecondClassPanelHandler : MonoBehaviour
	{
		[SerializeField]
		private UIExtendedLabel m_description;

		[SerializeField]
		private RuleListHandler m_primaryStats;

		[SerializeField]
		private RuleListHandler m_secondaryStats;

		[SerializeField]
		private UITable m_table;

		[SerializeField]
		private UIAlignmentBox m_alignmentBox;

		[SerializeField]
		private UIScrollView m_scrollView;

		[SerializeField]
		private UIExtendedButton m_buyClassButton;

		private SkillTreeData m_data;

		private int m_pendingEvents;

		private string m_baseDescription;

		public void Init(SkillTreeData _data)
		{
			m_data = _data;
			EnableListeners();
			DisableScreen();
		}

		public void Clear()
		{
			DisableListeners();
			DisableScreen();
		}

		private void Awake()
		{
		}

		private void OnDestroy()
		{
			Clear();
			m_data = null;
		}

		private void EnableListeners()
		{
			if (m_data != null)
			{
				EventDelegate.Add(m_data.onDataChanged, OnDataUpdated);
				EventDelegate.Add(m_data.onClassPreviewChanged, OnClassPreviewChanged);
			}
		}

		private void DisableListeners()
		{
			if (m_data != null)
			{
				EventDelegate.Remove(m_data.onDataChanged, OnDataUpdated);
				EventDelegate.Remove(m_data.onClassPreviewChanged, OnClassPreviewChanged);
			}
		}

		private void DisableScreen()
		{
			if (m_primaryStats != null)
			{
				m_primaryStats.Visible = false;
			}
			if (m_secondaryStats != null)
			{
				m_secondaryStats.Visible = false;
			}
			if (m_pendingEvents > 0)
			{
				EventDelegate.Remove(m_primaryStats.onAlign, OnAligned);
				EventDelegate.Remove(m_secondaryStats.onAlign, OnAligned);
			}
			m_pendingEvents = 0;
		}

		private void OnDataUpdated()
		{
			if (base.gameObject.activeSelf && m_data.PreviewClass != Assassin.EClassTypeId.None)
			{
				UpdateHandler();
			}
		}

		private void OnClassPreviewChanged()
		{
			UpdateHandler();
		}

		private void UpdateHandler()
		{
			DisableScreen();
			if (m_data.PreviewClass == Assassin.EClassTypeId.None)
			{
				return;
			}
			if (m_baseDescription == null)
			{
				m_baseDescription = m_description.text;
			}
			Dictionary<int, ClassDescription> assassinClassDescriptions = UbiCore.Utils.Singleton<UIDataMapper>.Instance.AssassinClassDescriptions;
			LocalizationManager instance = LocalizationManager.Instance;
			string value = instance.Get(assassinClassDescriptions[(int)m_data.PreviewClass].NameId);
			m_description.SetLocalizationKey(6500, new StringMapper { { "class_name", value } });
			AvatarClassDefinitions avatarDefinition = Singleton<StateManager>.Instance.TemplateManager.AvatarDefinition;
			AvatarClassDefinitions.AvatarClass definitionByType = avatarDefinition.GetDefinitionByType(m_data.Assassin.classTypeId);
			AvatarClassDefinitions.AvatarClass.SecondaryClassData secondaryClassData = definitionByType.secondaryClassInfos.Find((AvatarClassDefinitions.AvatarClass.SecondaryClassData _c) => _c.classTypeId == m_data.PreviewClass);
			if (secondaryClassData == null)
			{
				return;
			}
			Assets.Scripts.Mission.Definitions.SkillData instance2 = UbiCore.Utils.Singleton<Assets.Scripts.Mission.Definitions.SkillData>.Instance;
			BBList<Assets.Scripts.Mission.Definitions.SkillData.SkillPreview> bBList = new BBList<Assets.Scripts.Mission.Definitions.SkillData.SkillPreview>(3);
			BBList<Assets.Scripts.Mission.Definitions.SkillData.SkillPreview> bBList2 = new BBList<Assets.Scripts.Mission.Definitions.SkillData.SkillPreview>(3);
			AvatarClassDefinitions.AvatarClass.SecondaryClassData.SkillData[] skills = secondaryClassData.skills;
			for (int i = 0; i < skills.Length; i++)
			{
				Assets.Scripts.Mission.Definitions.SkillData.SkillPreview skillPreview = instance2.GetSkillPreview(skills[i].guid, skills[i].parameter, false);
				if (skillPreview.RuleStatType.IsUiRelevantRuleStat())
				{
					if (AssassinStatsHelper.IsPrimaryStat(skillPreview.StatType))
					{
						bBList.Add(skillPreview);
					}
					else
					{
						bBList2.Add(skillPreview);
					}
				}
			}
			Color colorDefault = UIDataMapper.UITextColors.PositiveColor;
			if (bBList.Count > 0)
			{
				m_pendingEvents++;
				EventDelegate.Add(m_primaryStats.onAlign, OnAligned, true);
				m_primaryStats.Visible = true;
				m_primaryStats.UpdateData(bBList, colorDefault);
			}
			if (bBList2.Count > 0)
			{
				m_pendingEvents++;
				EventDelegate.Add(m_secondaryStats.onAlign, OnAligned, true);
				m_secondaryStats.Visible = true;
				m_secondaryStats.UpdateData(bBList2, colorDefault);
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
	}
}
