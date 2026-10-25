using AcierProtocol;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Types;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.SkillTreeScreen
{
	public class Panelhandler : MonoBehaviour
	{
		[SerializeField]
		private HelpPanelHandler m_helpHandler;

		[SerializeField]
		private InfoPanelHandler m_infoHandler;

		[SerializeField]
		private SecondClassPanelHandler m_secondClassPanelHandler;

		private SkillTreeData m_data;

		private TabHandler m_tabHandler;

		public void Init(SkillTreeData _data, TabHandler _tabHandler)
		{
			m_data = _data;
			m_tabHandler = _tabHandler;
			EventDelegate.Add(m_data.onDataChanged, OnDataUpdate);
			EventDelegate.Add(m_data.onSkillSelectionChange, OnSelectedSkillChange);
			EventDelegate.Add(m_tabHandler.onSelectFirstClass, OnSelectFirstClass);
			EventDelegate.Add(m_tabHandler.onSelectSecondClass, OnSelectSecondClass);
			m_helpHandler.Init(_data);
			m_infoHandler.Init(_data);
			m_secondClassPanelHandler.Init(_data);
			UpdateHandler();
		}

		public void Clear()
		{
			if (m_data != null)
			{
				EventDelegate.Remove(m_data.onDataChanged, OnDataUpdate);
				EventDelegate.Remove(m_data.onSkillSelectionChange, OnSelectedSkillChange);
			}
			m_data = null;
			if (m_tabHandler != null)
			{
				EventDelegate.Remove(m_tabHandler.onSelectFirstClass, OnSelectFirstClass);
				EventDelegate.Remove(m_tabHandler.onSelectSecondClass, OnSelectSecondClass);
			}
			m_tabHandler = null;
			m_helpHandler.Clear();
			m_infoHandler.Clear();
			m_secondClassPanelHandler.Clear();
		}

		private void Awake()
		{
		}

		private void OnDisable()
		{
			Clear();
		}

		private void OnDataUpdate()
		{
			UpdateHandler();
		}

		private void OnSelectedSkillChange()
		{
			UpdateHandler();
		}

		private void OnSelectFirstClass()
		{
			UISkillId selectedSkill = m_data.SelectedSkill;
			if (selectedSkill != null && selectedSkill.Valid)
			{
				SetActive(m_helpHandler, false);
				SetActive(m_infoHandler, true);
			}
			else
			{
				m_helpHandler.State = HelpPanelHandler.EState.Normal;
				SetActive(m_helpHandler, true);
				SetActive(m_infoHandler, false);
			}
			SetActive(m_secondClassPanelHandler, false);
		}

		private void OnSelectSecondClass()
		{
			if (Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin.extendedClassTypeId == Assassin.EClassTypeId.None)
			{
				SetActive(m_helpHandler, false);
				SetActive(m_infoHandler, false);
				SetActive(m_secondClassPanelHandler, true);
			}
			else
			{
				OnSelectFirstClass();
			}
		}

		private void UpdateHandler()
		{
			if (m_tabHandler.PrimaryTabIsActive)
			{
				OnSelectFirstClass();
			}
			else
			{
				OnSelectSecondClass();
			}
		}

		private void SetActive(MonoBehaviour _monoBehaviour, bool _active)
		{
			_monoBehaviour.gameObject.SetActive(_active);
		}
	}
}
