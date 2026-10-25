using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.SkillTreeScreen
{
	public class SkillPointsHandler : MonoBehaviour
	{
		public List<EventDelegate> onResetPoints = new List<EventDelegate>();

		[SerializeField]
		private UIExtendedLabel m_pointLabel;

		[SerializeField]
		private UIButtonProxy m_resetButton;

		private SkillTreeData m_data;

		public bool Enabled
		{
			set
			{
				m_resetButton.isEnabled = value;
			}
		}

		public void Init(SkillTreeData _data)
		{
			m_data = _data;
			EventDelegate.Add(m_data.onDataChanged, OnDataUpdated);
			EventDelegate.Add(m_data.onClassSelectionChanged, OnDataUpdated);
			UpdateHandler();
		}

		public void Clear()
		{
			if (m_data != null)
			{
				EventDelegate.Remove(m_data.onDataChanged, OnDataUpdated);
				EventDelegate.Remove(m_data.onClassSelectionChanged, OnDataUpdated);
			}
		}

		public void UpdateHandler()
		{
			if (m_data.SecondaryClass == Assassin.EClassTypeId.None && m_data.IsSecondClassPreviewShown)
			{
				Enabled = false;
			}
			else
			{
				Enabled = m_data.Assassin.PointSpent(m_data.PrimarySelected) > 0;
			}
			m_pointLabel.SetNumber(m_data.SkillPoints);
		}

		private void Awake()
		{
		}

		private void OnEnable()
		{
			EventDelegate.Add(m_resetButton.onClick, OnResetClick);
		}

		private void OnDisable()
		{
			EventDelegate.Remove(m_resetButton.onClick, OnResetClick);
			Clear();
		}

		private void OnResetClick()
		{
			EventDelegate.Execute(onResetPoints);
		}

		private void OnDataUpdated()
		{
			UpdateHandler();
		}
	}
}
