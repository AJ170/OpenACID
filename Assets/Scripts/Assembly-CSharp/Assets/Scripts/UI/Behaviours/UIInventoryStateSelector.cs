using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.InventoryScreen;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours
{
	public class UIInventoryStateSelector : MonoBehaviour
	{
		[SerializeField]
		[CheckAssigned]
		private UIInventoryScreenData.EScreenState m_state;

		[SerializeField]
		[CheckAssigned]
		private UIScreenChanger[] m_changers;

		private void Awake()
		{
		}

		private void OnEnable()
		{
			if (m_changers != null && m_changers.Length != 0)
			{
				for (int i = 0; i < m_changers.Length; i++)
				{
					EventDelegate.Add(m_changers[i].onTrigger, OnChangeTriggered);
				}
			}
		}

		private void OnDisable()
		{
			if (m_changers != null && m_changers.Length != 0)
			{
				for (int i = 0; i < m_changers.Length; i++)
				{
					EventDelegate.Remove(m_changers[i].onTrigger, OnChangeTriggered);
				}
			}
		}

		private void OnChangeTriggered()
		{
			UIInventoryScreen.StartingState = m_state;
		}
	}
}
