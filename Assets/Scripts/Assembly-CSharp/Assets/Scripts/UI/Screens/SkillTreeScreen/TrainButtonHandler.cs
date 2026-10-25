using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.SkillTreeScreen
{
	public class TrainButtonHandler : MonoBehaviour
	{
		private enum EState
		{
			Train = 0,
			Unlock = 1
		}

		public List<EventDelegate> onTrain = new List<EventDelegate>();

		public List<EventDelegate> onUnlockSkill = new List<EventDelegate>();

		private static StringId s_TRAIN_ID = 681;

		private static StringId s_UNLOCK_ID = 679;

		[SerializeField]
		private UIExtendedButton m_button;

		[SerializeField]
		private UIExtendedLabel m_noSkilPointsInfo;

		[SerializeField]
		private UIExtendedLabel m_tierLockedInfo;

		private SkillTreeData m_data;

		private EState m_state;

		private float m_seconds;

		public bool Enabled
		{
			get
			{
				return m_button.isEnabled;
			}
			set
			{
				m_button.isEnabled = value;
			}
		}

		public bool Visible
		{
			get
			{
				return base.gameObject.activeSelf;
			}
			set
			{
				TurnAllLabelsOff();
				if (!value)
				{
					if (m_data.SkillPoints < 1)
					{
						if (m_data.SelectedSkillData != null && m_data.SelectedSkillData.Level < 5)
						{
							m_noSkilPointsInfo.gameObject.SetActive(true);
						}
					}
					else if (m_data.SelectedSkill != null)
					{
						SkillData skill = m_data.GetSkill(m_data.SelectedSkill);
						AssassinSkillTier_ tier = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin.GetTier(skill.TierId, (!m_data.PrimarySelected) ? AssassinSkillInTraining.SkillClass.ExtendedClass : AssassinSkillInTraining.SkillClass.BaseClass);
						if (tier.Locked())
						{
							m_tierLockedInfo.gameObject.SetActive(true);
						}
						if (skill.Level > 4)
						{
							TurnAllLabelsOff();
						}
					}
				}
				base.gameObject.SetActive(value);
			}
		}

		public void Init(SkillTreeData _data)
		{
			m_data = _data;
			EnableListeners();
			UpdateHandler();
		}

		public void Clear()
		{
			if (m_data != null)
			{
				EventDelegate.Remove(m_data.onDataChanged, OnDataUpdated);
				EventDelegate.Remove(m_data.onSkillSelectionChange, OnSelectedSkillChange);
			}
			m_data = null;
		}

		private void TurnAllLabelsOff()
		{
			m_noSkilPointsInfo.gameObject.SetActive(false);
			m_tierLockedInfo.gameObject.SetActive(false);
		}

		private void Awake()
		{
		}

		private void OnDestroy()
		{
			Clear();
		}

		private void OnEnable()
		{
			EnableListeners();
			EventDelegate.Add(m_button.onClick, OnButtonClick);
			UpdateHandler();
		}

		private void OnDisable()
		{
			EventDelegate.Remove(m_button.onClick, OnButtonClick);
		}

		private void EnableListeners()
		{
			if (m_data != null)
			{
				EventDelegate.Add(m_data.onDataChanged, OnDataUpdated);
				EventDelegate.Add(m_data.onSkillSelectionChange, OnSelectedSkillChange);
			}
		}

		private void OnButtonClick()
		{
			switch (m_state)
			{
			case EState.Train:
				EventDelegate.Execute(onTrain);
				break;
			case EState.Unlock:
				EventDelegate.Execute(onUnlockSkill);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private void OnDataUpdated()
		{
			UpdateHandler();
		}

		private void OnSelectedSkillChange()
		{
			UpdateHandler();
		}

		private void UpdateHandler()
		{
			if (m_data != null)
			{
				if (m_data.CanUnlockSelectedSkill)
				{
					m_state = EState.Unlock;
					m_button.SetLabel(LocalizationManager.Instance.Get(s_UNLOCK_ID));
					m_button.isEnabled = true;
					Visible = !m_data.HasSpentMaxSkillPoints;
				}
				else
				{
					m_state = EState.Train;
					m_button.SetLabel(LocalizationManager.Instance.Get(s_TRAIN_ID));
					m_button.isEnabled = m_data.CanTrain;
					Visible = m_data.CanTrain;
				}
				if (m_button.gameObject.GetComponent<GenericButtonHighlighter>() != null)
				{
					m_button.gameObject.GetComponent<GenericButtonHighlighter>().UpdateState();
				}
			}
		}
	}
}
