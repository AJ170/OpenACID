using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.SkillTreeScreen
{
	public class SkillTreeData : ISkillTreeProfileFacade
	{
		public List<EventDelegate> onDataChanged = new List<EventDelegate>();

		public List<EventDelegate> onClassSelectionChanged = new List<EventDelegate>();

		public List<EventDelegate> onClassPreviewChanged = new List<EventDelegate>();

		public List<EventDelegate> onSkillSelectionChange = new List<EventDelegate>();

		public List<EventDelegate> onSkillTrained = new List<EventDelegate>();

		private Assassin.EClassTypeId m_previewClass = Assassin.EClassTypeId.None;

		private Assassin.EClassTypeId m_selectedClass = Assassin.EClassTypeId.None;

		private UISkillId m_selectedSkill;

		private readonly ISkillTreeProfileFacade m_profileFacade;

		private readonly SkillTreeProfileFacadeMock m_profileFacadeMock;

		private ISkillTreeProfileFacade m_currentProfileFacade;

		public Color MaxLevelColor { get; set; }

		public Color NextLevelColor { get; set; }

		public Color ValueColor { get; set; }

		public bool UseProfileFacadeMock
		{
			get
			{
				return m_currentProfileFacade == m_profileFacadeMock;
			}
			set
			{
				ISkillTreeProfileFacade currentProfileFacade;
				if (value)
				{
					ISkillTreeProfileFacade profileFacadeMock = m_profileFacadeMock;
					currentProfileFacade = profileFacadeMock;
				}
				else
				{
					currentProfileFacade = m_profileFacade;
				}
				m_currentProfileFacade = currentProfileFacade;
			}
		}

		public SkillTreeProfileFacadeMock ProfileFacadeMock
		{
			get
			{
				return m_profileFacadeMock;
			}
		}

		public int SkillPoints
		{
			get
			{
				return m_currentProfileFacade.SkillPoints;
			}
		}

		public int PointsSpent
		{
			get
			{
				return m_currentProfileFacade.PointsSpent;
			}
		}

		public int MaxSkillPoints
		{
			get
			{
				return Singleton<StateManager>.Instance.TemplateManager.XpPerLevelTable.GetMaxLevel() * Singleton<StateManager>.Instance.TemplateManager.Globals.numSkillpointsPerLevelup;
			}
		}

		public bool HasSpentMaxSkillPoints
		{
			get
			{
				return SkillPoints == 0 && PointsSpent >= MaxSkillPoints;
			}
		}

		public bool CanTrain
		{
			get
			{
				if (SelectedSkill == null || !SelectedSkill.Valid)
				{
					return false;
				}
				SkillData skill = GetSkill(SelectedSkill);
				AssassinSkillTier_ tier = Assassin.GetTier(skill.TierId, (!PrimarySelected) ? AssassinSkillInTraining.SkillClass.ExtendedClass : AssassinSkillInTraining.SkillClass.BaseClass);
				if (tier.Locked())
				{
					return false;
				}
				bool flag = Assassin.skillPoints > 0;
				flag &= skill.Level < 5;
				return flag & !skill.Locked;
			}
		}

		public Assassin.EClassTypeId PrimaryClass
		{
			get
			{
				return Assassin.classTypeId;
			}
		}

		public Assassin.EClassTypeId SecondaryClass
		{
			get
			{
				return Assassin.extendedClassTypeId;
			}
		}

		public Assassin.EClassTypeId PreviewClass
		{
			get
			{
				return m_previewClass;
			}
			set
			{
				m_previewClass = value;
				EventDelegate.Execute(onClassPreviewChanged);
			}
		}

		public Assassin.EClassTypeId SelectedClass
		{
			get
			{
				return m_selectedClass;
			}
			set
			{
				m_selectedClass = value;
				EventDelegate.Execute(onClassSelectionChanged);
			}
		}

		public bool IsSecondClassPreviewShown { get; set; }

		public bool PrimarySelected
		{
			get
			{
				return SelectedClass == PrimaryClass;
			}
		}

		public int PointsUntillSecondClass
		{
			get
			{
				return Assassin.pointsToSpendForExtendedClass;
			}
		}

		public Assassin Assassin
		{
			get
			{
				return Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin;
			}
		}

		public UISkillId SelectedSkill
		{
			get
			{
				return m_selectedSkill;
			}
			set
			{
				if (!(value == SelectedSkill))
				{
					m_selectedSkill = value;
					EventDelegate.Execute(onSkillSelectionChange);
				}
			}
		}

		public SkillData SelectedSkillData
		{
			get
			{
				UISkillId selectedSkill = SelectedSkill;
				if (selectedSkill == null || !selectedSkill.Valid)
				{
					return null;
				}
				return GetSkill(selectedSkill);
			}
		}

		public bool CanUnlockSelectedSkill
		{
			get
			{
				SkillData selectedSkillData = SelectedSkillData;
				if (selectedSkillData == null || !selectedSkillData.Locked)
				{
					return false;
				}
				AssassinSkillTier_ assassinSkillTier_ = null;
				assassinSkillTier_ = ((!PrimarySelected) ? Assassin.GetTier(selectedSkillData.TierId, AssassinSkillInTraining.SkillClass.ExtendedClass) : Assassin.GetTier(selectedSkillData.TierId, AssassinSkillInTraining.SkillClass.BaseClass));
				return !assassinSkillTier_.Locked();
			}
		}

		public SkillTreeData()
		{
			m_profileFacade = new SkillTreeProfileFacade(this);
			m_profileFacadeMock = new SkillTreeProfileFacadeMock();
			m_currentProfileFacade = m_profileFacade;
		}

		public void Update()
		{
			EventDelegate.Execute(onDataChanged);
		}

		public void SelectedSkillTrained()
		{
			EventDelegate.Execute(onSkillTrained);
		}

		public SkillData[][][] GetSkills()
		{
			return Assassin.GetSkills();
		}

		public SkillData[][] GetSkills(bool _primary)
		{
			return GetSkills()[(!_primary) ? 1 : 0];
		}

		public SkillData[] GetSkills(bool _primary, int _tierIndex)
		{
			SkillData[][] skills = GetSkills(_primary);
			return skills[_tierIndex];
		}

		public SkillData GetSkill(bool _primary, int _tierIndex, int _skillIndex)
		{
			return GetSkills(_primary, _tierIndex)[_skillIndex];
		}

		public SkillData GetSkill(bool _primary, int _tierIndex, AssassinSkillInTraining.SkillType _type)
		{
			return GetSkill(_primary, _tierIndex, (int)_type);
		}

		public SkillData GetSkill(UISkillId _skillId)
		{
			if (!_skillId.Valid)
			{
				return null;
			}
			SkillData[][][] skills = GetSkills();
			foreach (SkillData[][] array in skills)
			{
				foreach (SkillData[] array2 in array)
				{
					if (array2 == null)
					{
						continue;
					}
					foreach (SkillData skillData in array2)
					{
						if (skillData.SkillId == _skillId)
						{
							return skillData;
						}
					}
				}
			}
			return null;
		}

		public AssassinSkillTier_ GetTierByIndex(int _index)
		{
			return Assassin.GetTier(PrimarySelected, _index);
		}

		public AssassinSkillTier_ GetTierByIndex(int _index, bool _primarySelected)
		{
			return Assassin.GetTier(_primarySelected, _index);
		}

		public AvatarClassDefinitions.AvatarClass.AvatarTier GetAvatarTierByIndex(int _index)
		{
			AvatarClassDefinitions.AvatarClass definitionByType = Singleton<StateManager>.Instance.TemplateManager.AvatarDefinition.GetDefinitionByType((!PrimarySelected) ? SecondaryClass : PrimaryClass);
			return definitionByType.tiers[_index];
		}

		public AvatarClassDefinitions.AvatarClass.AvatarTier GetTierById(int _id)
		{
			AvatarClassDefinitions.AvatarClass definitionByType = Singleton<StateManager>.Instance.TemplateManager.AvatarDefinition.GetDefinitionByType((!PrimarySelected) ? SecondaryClass : PrimaryClass);
			return definitionByType.GetSkillTier(_id);
		}

		public TrainingCost GetCost(UISkillId _id, int _rank, Assassin _assassin)
		{
			AvatarClassDefinitions.AvatarClass definitionByType = Singleton<StateManager>.Instance.TemplateManager.AvatarDefinition.GetDefinitionByType((!PrimarySelected) ? SecondaryClass : PrimaryClass);
			return definitionByType.GetCost(_id, _rank, _assassin);
		}

		public void Clear()
		{
			onDataChanged.Clear();
			onClassSelectionChanged.Clear();
			onClassPreviewChanged.Clear();
			onSkillSelectionChange.Clear();
			onSkillTrained.Clear();
			m_currentProfileFacade = null;
		}
	}
}
