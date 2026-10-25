using System.Collections.Generic;
using UnityEngine;

public class ClassChooseNavigation : MonoBehaviour
{
	[SerializeField]
	private List<ClassChooseContainer> m_containerList;

	private ClassChooseContainer m_currentTab;

	private ClassChooseContainer m_lastTab;

	private void Start()
	{
		for (int i = 0; i < m_containerList.Count; i++)
		{
			EventDelegate.Add(m_containerList[i].ActivateTabTrigger.onClick, OnClickedTab);
			m_containerList[i].GetDeActivated();
		}
		m_currentTab = m_containerList[0];
		m_lastTab = m_containerList[0];
		m_currentTab.GetActivated();
	}

	private void OnDestroy()
	{
		for (int i = 0; i < m_containerList.Count; i++)
		{
			EventDelegate.Remove(m_containerList[i].ActivateTabTrigger.onClick, OnClickedTab);
		}
	}

	private void OnClickedTab()
	{
		if (m_lastTab != null)
		{
			m_lastTab.GetDeActivated();
		}
		m_currentTab = UIEventTrigger.current.gameObject.GetComponent<ClassChooseContainer>();
		m_currentTab.GetActivated();
		m_lastTab = m_currentTab;
	}
}
