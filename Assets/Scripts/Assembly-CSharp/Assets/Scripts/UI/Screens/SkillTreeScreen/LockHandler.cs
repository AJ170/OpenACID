using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.SkillTreeScreen
{
	public class LockHandler : MonoBehaviour
	{
		public static LockHandler Current;

		public List<EventDelegate> onUnlock = new List<EventDelegate>();

		[SerializeField]
		private UIExtendedLabel m_unlockLabel;

		[SerializeField]
		private GameObject m_labelContainer;

		[SerializeField]
		private UIButtonProxy m_unlockButton;

		[SerializeField]
		private UISprite m_overlay;

		private SkillTreeData m_data;

		private int m_tierIndex;

		private bool m_isLocked;

		public bool Locked
		{
			get
			{
				return m_isLocked;
			}
			private set
			{
				m_isLocked = value;
				m_unlockButton.gameObject.SetActive(value);
				m_overlay.gameObject.SetActive(!value);
				m_labelContainer.gameObject.SetActive(value);
			}
		}

		public void Init(SkillTreeData _data, int _tierindex)
		{
			m_data = _data;
			m_tierIndex = _tierindex;
			EventDelegate.Add(m_data.onDataChanged, OnDataChanged);
			EventDelegate.Add(m_data.onClassSelectionChanged, OnClassSelectionChange);
			UpdateHandler();
		}

		public void Clear()
		{
			if (m_data != null)
			{
				EventDelegate.Remove(m_data.onDataChanged, OnDataChanged);
				EventDelegate.Remove(m_data.onClassSelectionChanged, OnClassSelectionChange);
			}
		}

		private void Awake()
		{
		}

		private void OnDestroy()
		{
			Clear();
			m_data = null;
		}

		private void OnEnable()
		{
			EventDelegate.Add(m_unlockButton.onClick, OnUnlockClicked);
		}

		private void OnDisable()
		{
			EventDelegate.Remove(m_unlockButton.onClick, OnUnlockClicked);
		}

		private void OnDataChanged()
		{
			UpdateHandler();
		}

		private void OnClassSelectionChange()
		{
			UpdateHandler();
		}

		private void OnUnlockClicked()
		{
			Current = this;
			EventDelegate.Execute(onUnlock);
			Current = null;
		}

		private void UpdateHandler()
		{
			AssassinSkillTier_ tierByIndex = m_data.GetTierByIndex(m_tierIndex);
			m_unlockLabel.SetLocalizationKey(1562, new StringMapper { { "Skill_Points_Required", tierByIndex.pointsRemaining } });
			Locked = tierByIndex.Locked();
			if (Locked && m_data.HasSpentMaxSkillPoints)
			{
				m_unlockButton.gameObject.SetActive(false);
				m_labelContainer.gameObject.SetActive(false);
			}
		}
	}
}
