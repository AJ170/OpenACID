using System;
using Assets.Scripts.UI.Localization;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.SkillTreeScreen
{
	public class HelpPanelHandler : MonoBehaviour
	{
		public enum EState
		{
			Normal = 0,
			SecondClass = 1,
			SelectSecondClass = 2
		}

		private static readonly StringId s_helpId = 1193;

		private static readonly StringId s_secondClass = 3513;

		private static readonly StringId s_selectSecondClass = 2034;

		[SerializeField]
		private UIExtendedLabel m_label;

		private SkillTreeData m_data;

		private EState m_state;

		public EState State
		{
			get
			{
				return m_state;
			}
			set
			{
				m_state = value;
				switch (value)
				{
				case EState.Normal:
					m_label.text = LocalizationManager.Instance.Get(s_helpId);
					break;
				case EState.SelectSecondClass:
					m_label.text = LocalizationManager.Instance.Get(s_selectSecondClass);
					break;
				case EState.SecondClass:
					m_label.text = LocalizationManager.Instance.Get(s_secondClass, new StringMapper { { "Skill_Points_Required", m_data.PointsUntillSecondClass } });
					break;
				default:
					throw new ArgumentOutOfRangeException("value");
				}
			}
		}

		public void Init(SkillTreeData _data)
		{
			m_data = _data;
		}

		public void Clear()
		{
			m_data = null;
		}

		private void Awake()
		{
			State = EState.Normal;
		}

		private void OnDestroy()
		{
			Clear();
		}
	}
}
