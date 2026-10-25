using System.Collections.Generic;

namespace Assets.Scripts.UI.Screens
{
	public class ExclusiveSubScreenManager
	{
		private readonly List<SubScreen> m_subScreens = new List<SubScreen>();

		private int m_currentIndex = -1;

		public void Set(List<SubScreen> _list)
		{
			m_currentIndex = -1;
			for (int i = 0; i < _list.Count; i++)
			{
				m_subScreens.Add(_list[i]);
				_list[i].gameObject.SetActive(false);
			}
		}

		public void Show(int _index)
		{
			if (_index < 0 || _index >= m_subScreens.Count || _index == m_currentIndex)
			{
				return;
			}
			if (m_currentIndex < _index)
			{
				if (m_currentIndex >= 0 && m_currentIndex < m_subScreens.Count)
				{
					m_subScreens[m_currentIndex].Hide();
				}
				m_subScreens[_index].Show();
			}
			else
			{
				m_subScreens[m_currentIndex].ReverseShow();
				m_subScreens[_index].ReverseHide();
			}
			m_currentIndex = _index;
		}

		public void TweenCamera(int _index)
		{
			if (_index >= 0 && _index < m_subScreens.Count)
			{
				m_subScreens[_index].TweenCamera();
			}
		}

		public void InstantCamera(int _index)
		{
			if (_index >= 0 && _index < m_subScreens.Count)
			{
				m_subScreens[_index].InstantCamera();
			}
		}
	}
}
