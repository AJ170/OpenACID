using System.Collections;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.SkillTreeScreen
{
	public class TierHandler : MonoBehaviour
	{
		public static TierHandler Current;

		public List<EventDelegate> onUnlock = new List<EventDelegate>();

		[SerializeField]
		private SkillButtonHandler m_activeHandler;

		[SerializeField]
		private SkillButtonHandler m_passiveOneHandler;

		[SerializeField]
		private SkillButtonHandler m_passiveTwoHandler;

		[SerializeField]
		private LockHandler m_lockHandler;

		[SerializeField]
		private UIAlignmentBox m_skillsContainer;

		[SerializeField]
		private ParticleEffectsToolDynamic m_effectTool;

		[SerializeField]
		private ParticleSystem m_particleSystem;

		private SkillTreeData m_data;

		private SkillButtonHandler[] m_handlers;

		private SkillButtonHandler m_highlighted;

		private int m_index = -1;

		private bool m_oldLockedStatus;

		public int Index
		{
			get
			{
				return m_index;
			}
		}

		public void InitScreen()
		{
			m_handlers = new SkillButtonHandler[3] { m_activeHandler, m_passiveOneHandler, m_passiveTwoHandler };
			m_activeHandler.InitScreen(AssassinSkillInTraining.SkillType.Active);
			m_passiveOneHandler.InitScreen(AssassinSkillInTraining.SkillType.PassiveOne);
			m_passiveTwoHandler.InitScreen(AssassinSkillInTraining.SkillType.PassiveTwo);
		}

		public void Init(SkillTreeData _data, int _tierIndex)
		{
			m_data = _data;
			m_index = _tierIndex;
			m_oldLockedStatus = m_data.GetTierByIndex(m_index, m_data.PrimarySelected).Locked();
			m_particleSystem.gameObject.SetActive(false);
			m_activeHandler.Init(_data, _tierIndex, AssassinSkillInTraining.SkillType.Active);
			m_passiveOneHandler.Init(_data, _tierIndex, AssassinSkillInTraining.SkillType.PassiveOne);
			m_passiveTwoHandler.Init(_data, _tierIndex, AssassinSkillInTraining.SkillType.PassiveTwo);
			m_lockHandler.Init(_data, _tierIndex);
			EventDelegate.Add(m_data.onDataChanged, OnDataChanged);
			EventDelegate.Add(m_lockHandler.onUnlock, OnUnlock);
			UpdateHandler();
		}

		public void Clear()
		{
			m_activeHandler.Clear();
			m_passiveOneHandler.Clear();
			m_passiveTwoHandler.Clear();
			m_lockHandler.Clear();
			if (m_data != null)
			{
				EventDelegate.Remove(m_data.onDataChanged, OnDataChanged);
			}
			m_data = null;
			EventDelegate.Remove(m_lockHandler.onUnlock, OnUnlock);
		}

		private void DiableWithAnimation()
		{
			StartCoroutine(PlayAnimation(2f));
		}

		private IEnumerator PlayAnimation(float _time)
		{
			m_particleSystem.gameObject.SetActive(true);
			m_effectTool.createParticlesDynamically();
			do
			{
				yield return new WaitForEndOfFrame();
			}
			while (m_particleSystem.isPlaying);
			yield return new WaitForEndOfFrame();
			m_particleSystem.gameObject.SetActive(false);
		}

		private void OnDestroy()
		{
			Clear();
		}

		private void OnUnlock()
		{
			Current = this;
			EventDelegate.Execute(onUnlock);
			Current = null;
		}

		private void OnDataChanged()
		{
			bool flag = m_data.GetTierByIndex(m_index, m_data.PrimarySelected).Locked();
			if (m_oldLockedStatus && !flag)
			{
				StartCoroutine(PlayAnimation(0.4f));
			}
			m_oldLockedStatus = flag;
			UpdateHandler();
		}

		private void UpdateHandler()
		{
			for (int i = 0; i < m_handlers.Length; i++)
			{
				if (m_handlers[i].Highlighted)
				{
					m_highlighted = m_handlers[i];
				}
				m_handlers[i].Highlighted = false;
			}
			OnReposition();
		}

		private void OnReposition()
		{
			if (m_highlighted != null)
			{
				m_highlighted.Highlighted = true;
			}
			m_highlighted = null;
		}
	}
}
