using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.TweenHandling;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.SkillTreeScreen
{
	public class TabHandler : MonoBehaviour
	{
		public List<EventDelegate> onSelectFirstClass = new List<EventDelegate>();

		public List<EventDelegate> onSelectSecondClass = new List<EventDelegate>();

		[SerializeField]
		private ClassTabHandler m_primaryClass;

		[SerializeField]
		private ClassTabHandler m_secondClass;

		[SerializeField]
		private TweenHandler m_tweener;

		[SerializeField]
		private GameObject m_effectPrefab;

		private SkillTreeData m_data;

		private SkillTreeDetails m_details;

		private ClassTabHandler m_currentTab;

		private GameObject m_instantiatedPrefab;

		private UnityEngine.Animation m_animation;

		private bool m_gotoSecondClassOnInit;

		public bool PrimaryTabIsActive
		{
			get
			{
				return m_currentTab == m_primaryClass;
			}
		}

		public void Init(SkillTreeData _data, SkillTreeDetails _details)
		{
			if (m_instantiatedPrefab == null)
			{
				m_instantiatedPrefab = NGUITools.AddChild(m_secondClass.gameObject, m_effectPrefab);
				m_animation = m_instantiatedPrefab.GetComponent<UnityEngine.Animation>();
			}
			m_instantiatedPrefab.SetActive(false);
			m_data = _data;
			m_primaryClass.Init(m_data, true);
			m_secondClass.Init(m_data, false);
			m_data.SelectedClass = m_data.PrimaryClass;
			m_tweener.ResetToBeginning();
			m_currentTab = m_primaryClass;
			if (m_gotoSecondClassOnInit)
			{
				if (m_data.SecondaryClass != Assassin.EClassTypeId.None)
				{
					m_currentTab = m_secondClass;
					m_data.SelectedClass = m_currentTab.ClassType;
					m_tweener.StartTweeningForward();
				}
				else
				{
					m_gotoSecondClassOnInit = false;
				}
			}
			if (m_details != null)
			{
				EventDelegate.Remove(m_details.onChooseClass, InitTreeAfterSecondClassisChosen);
			}
			m_details = _details;
			EventDelegate.Add(m_details.onChooseClass, InitTreeAfterSecondClassisChosen);
		}

		public void Clear()
		{
			if (m_details != null)
			{
				EventDelegate.Remove(m_details.onChooseClass, InitTreeAfterSecondClassisChosen);
			}
			m_details = null;
			m_data = null;
			m_currentTab = null;
			m_primaryClass.Clear();
			m_secondClass.Clear();
		}

		public void ViewSecondClass()
		{
			m_currentTab = m_secondClass;
			m_data.SelectedClass = m_currentTab.ClassType;
			m_tweener.StartTweeningForward();
		}

		private void Awake()
		{
		}

		private void OnDestroy()
		{
			if (m_instantiatedPrefab != null)
			{
				NGUITools.Destroy(m_instantiatedPrefab);
			}
			m_currentTab = null;
			m_details = null;
			m_data = null;
			m_animation = null;
		}

		private void OnEnable()
		{
			EventDelegate.Add(m_primaryClass.onClick, OnFirstClicked);
			EventDelegate.Add(m_secondClass.onClick, OnSecondClicked);
			Singleton<StateManager>.Instance.ProfileManager.OnAssassinChange += new SimpleDelegate.CallbackFunction(OnAssassinChange);
		}

		private void OnDisable()
		{
			EventDelegate.Remove(m_primaryClass.onClick, OnFirstClicked);
			EventDelegate.Remove(m_secondClass.onClick, OnSecondClicked);
			Singleton<StateManager>.Instance.ProfileManager.OnAssassinChange -= new SimpleDelegate.CallbackFunction(OnAssassinChange);
			Clear();
		}

		private void OnFirstClicked()
		{
			if (m_currentTab != m_primaryClass && !m_tweener.IsTweening)
			{
				m_currentTab = m_primaryClass;
				m_tweener.StartTweeningReverse();
				m_data.SelectedClass = m_currentTab.ClassType;
				m_data.PreviewClass = Assassin.EClassTypeId.None;
				EventDelegate.Execute(onSelectFirstClass);
			}
			m_gotoSecondClassOnInit = false;
		}

		private void OnSecondClicked()
		{
			if (m_currentTab != m_secondClass && !m_tweener.IsTweening)
			{
				m_currentTab = m_secondClass;
				if (m_currentTab.ClassType != Assassin.EClassTypeId.None)
				{
					m_data.SelectedClass = m_currentTab.ClassType;
				}
				m_tweener.StartTweeningForward();
				EventDelegate.Execute(onSelectSecondClass);
			}
			m_gotoSecondClassOnInit = true;
		}

		private void PlayAnimation()
		{
			m_instantiatedPrefab.SetActive(true);
			m_animation.Play();
		}

		private void InitTreeAfterSecondClassisChosen()
		{
			PlayAnimation();
			m_currentTab = m_secondClass;
			m_data.SelectedClass = m_currentTab.ClassType;
			m_tweener.StartTweeningForward();
		}

		private void OnAssassinChange(params object[] _params)
		{
			m_gotoSecondClassOnInit = false;
		}
	}
}
