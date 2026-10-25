using AcierProtocol;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.SkillTreeScreen
{
	public class SkillTreeHandler : MonoBehaviour
	{
		public delegate void OnUnlock(int _tierIndex);

		[SerializeField]
		private TierHandler m_Tier1;

		[SerializeField]
		private TierHandler m_Tier2;

		[SerializeField]
		private TierHandler m_Tier3;

		[SerializeField]
		private TierHandler m_Tier4;

		private TierHandler[] m_tierHandlers;

		private SkillTreeData m_data;

		public OnUnlock OnUnlockHandler;

		public bool Visible
		{
			get
			{
				return base.gameObject.activeSelf;
			}
			set
			{
				base.gameObject.SetActive(value);
			}
		}

		public void InitScreen()
		{
			m_tierHandlers = new TierHandler[4] { m_Tier1, m_Tier2, m_Tier3, m_Tier4 };
			for (int i = 0; i < m_tierHandlers.Length; i++)
			{
				m_tierHandlers[i].InitScreen();
			}
		}

		public void Init(SkillTreeData _data)
		{
			Visible = true;
			m_data = _data;
			EventDelegate.Add(m_data.onClassSelectionChanged, OnClassSelectionChange);
			Assassin.EClassTypeId selectedClass = m_data.SelectedClass;
			if (selectedClass != Assassin.EClassTypeId.None)
			{
				SkillData[][] skills = m_data.GetSkills(m_data.PrimaryClass == selectedClass);
				for (int i = 0; i < m_tierHandlers.Length; i++)
				{
					m_tierHandlers[i].Init(_data, i);
					EventDelegate.Add(m_tierHandlers[i].onUnlock, OnUnlockClicked);
				}
			}
		}

		public void Clear()
		{
			if (m_data != null)
			{
				EventDelegate.Remove(m_data.onClassSelectionChanged, OnClassSelectionChange);
			}
			for (int i = 0; i < m_tierHandlers.Length; i++)
			{
				m_tierHandlers[i].Clear();
				EventDelegate.Remove(m_tierHandlers[i].onUnlock, OnUnlockClicked);
			}
		}

		private void Awake()
		{
			Visible = true;
		}

		private void OnDestroy()
		{
			Clear();
		}

		private void OnUnlockClicked()
		{
			OnUnlockHandler(TierHandler.Current.Index);
		}

		private void OnClassSelectionChange()
		{
			for (int i = 0; i < m_tierHandlers.Length; i++)
			{
				m_tierHandlers[i].Init(m_data, i);
			}
		}
	}
}
