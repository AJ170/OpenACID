using System.Collections;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.SkillTreeScreen
{
	public class SkillButtonHandler : MonoBehaviour
	{
		public List<EventDelegate> onSelect = new List<EventDelegate>();

		[SerializeField]
		private IconHandler m_activeHandler;

		[SerializeField]
		private IconHandler m_passiveHandler;

		[SerializeField]
		private SkillValueHandler m_ValueHandler;

		[SerializeField]
		private UIWidget m_widget;

		[SerializeField]
		private GameObject m_SkillUpgradePrefabActive;

		[SerializeField]
		private GameObject m_SkillUpgradePrefabPassive;

		[SerializeField]
		private GameObject m_SkillUpgradePrefabActiveToMax;

		[SerializeField]
		private GameObject m_SkillUpgradePrefabPassiveToMax;

		[SerializeField]
		private UIWidget m_transparencyContainer;

		private GameObject m_selectedPrefab;

		private GameObject m_selectedPrefabMax;

		private SkillTreeData m_data;

		private AssassinSkillInTraining.SkillType m_type;

		private int m_tierIndex;

		public bool Highlighted
		{
			get
			{
				return (m_activeHandler.Visible && m_activeHandler.Highlighted) || (m_passiveHandler.Visible && m_passiveHandler.Highlighted);
			}
			set
			{
				if (m_activeHandler.Visible)
				{
					m_activeHandler.Highlighted = value;
				}
				else if (m_passiveHandler.Visible)
				{
					m_passiveHandler.Highlighted = value;
				}
			}
		}

		public void InitScreen(AssassinSkillInTraining.SkillType _type)
		{
			if (m_selectedPrefab == null)
			{
				if (_type == AssassinSkillInTraining.SkillType.Active)
				{
					m_selectedPrefab = NGUITools.AddChild(base.gameObject, m_SkillUpgradePrefabActive);
					m_selectedPrefabMax = NGUITools.AddChild(base.gameObject, m_SkillUpgradePrefabActiveToMax);
				}
				else
				{
					m_selectedPrefab = NGUITools.AddChild(base.gameObject, m_SkillUpgradePrefabPassive);
					m_selectedPrefabMax = NGUITools.AddChild(base.gameObject, m_SkillUpgradePrefabPassiveToMax);
				}
				m_selectedPrefabMax.SetActive(false);
				m_selectedPrefab.SetActive(false);
			}
			m_ValueHandler.InitScreen();
		}

		public void Init(SkillTreeData _data, int _tierIndex, AssassinSkillInTraining.SkillType _type)
		{
			if (m_selectedPrefab != null && m_selectedPrefabMax != null)
			{
				m_selectedPrefab.SetActive(false);
				m_selectedPrefabMax.SetActive(false);
			}
			m_data = _data;
			m_type = _type;
			m_tierIndex = _tierIndex;
			EventDelegate.Add(m_data.onClassSelectionChanged, OnClassSelectionChange);
			EventDelegate.Add(m_data.onDataChanged, OnDataUpdated);
			EventDelegate.Add(m_data.onSkillTrained, OnSkillTrained);
			EventDelegate.Add(m_data.onSkillSelectionChange, OnSkillSelectionChange);
			m_ValueHandler.Init(_data, _tierIndex, _type);
			m_activeHandler.Init(_data, _tierIndex, _type);
			m_passiveHandler.Init(_data, _tierIndex, _type);
			UpdateHandler();
		}

		public void Clear()
		{
			if (m_data != null)
			{
				EventDelegate.Remove(m_data.onClassSelectionChanged, OnClassSelectionChange);
				EventDelegate.Remove(m_data.onDataChanged, OnDataUpdated);
				EventDelegate.Remove(m_data.onSkillTrained, OnSkillTrained);
				EventDelegate.Remove(m_data.onSkillSelectionChange, OnSkillSelectionChange);
			}
			m_data = null;
		}

		private void OnDestroy()
		{
			m_data = null;
			m_selectedPrefab = null;
			m_selectedPrefabMax = null;
		}

		private void OnEnable()
		{
			EventDelegate.Add(m_activeHandler.onSelected, OnSelected);
			EventDelegate.Add(m_passiveHandler.onSelected, OnSelected);
		}

		private void OnDisable()
		{
			EventDelegate.Remove(m_activeHandler.onSelected, OnSelected);
			EventDelegate.Remove(m_passiveHandler.onSelected, OnSelected);
			Clear();
		}

		private void OnDataUpdated()
		{
			UpdateHandler();
		}

		private void OnClassSelectionChange()
		{
			UpdateHandler();
		}

		private void OnSkillSelectionChange()
		{
			UpdateHandler();
		}

		private void OnSelected()
		{
			m_ValueHandler.StartAnimationExternally();
			EventDelegate.Execute(onSelect);
		}

		private void UpdateHandler()
		{
			SkillData skill = m_data.GetSkill(m_data.PrimarySelected, m_tierIndex, m_type);
			m_ValueHandler.UpdateHandler(skill);
			m_ValueHandler.Visible = true;
			m_activeHandler.UpdateHandler(skill);
			m_activeHandler.Visible = m_type == AssassinSkillInTraining.SkillType.Active;
			m_passiveHandler.UpdateHandler(skill);
			m_passiveHandler.Visible = m_type != AssassinSkillInTraining.SkillType.Active;
			AssassinSkillTier_ tierByIndex = m_data.GetTierByIndex(m_tierIndex);
			m_transparencyContainer.alpha = ((!skill.Locked && !tierByIndex.Locked()) ? 1f : 0.3f);
		}

		private void OnSkillTrained()
		{
			SkillData skill = m_data.GetSkill(m_data.PrimarySelected, m_tierIndex, m_type);
			if (skill.SkillId == m_data.SelectedSkill)
			{
				GameObject gameObject = ((skill.Level <= 4) ? m_selectedPrefab : m_selectedPrefabMax);
				UnityEngine.Animation component = gameObject.GetComponent<UnityEngine.Animation>();
				gameObject.SetActive(true);
				component.wrapMode = WrapMode.Once;
				component.Play();
				StartCoroutine(CheckAnimation(component.clip.length));
			}
		}

		private IEnumerator CheckAnimation(float _time)
		{
			float counter = 0f;
			do
			{
				counter += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			while (!(counter > _time));
			yield return new WaitForEndOfFrame();
			m_selectedPrefab.SetActive(false);
			m_selectedPrefabMax.SetActive(false);
		}
	}
}
