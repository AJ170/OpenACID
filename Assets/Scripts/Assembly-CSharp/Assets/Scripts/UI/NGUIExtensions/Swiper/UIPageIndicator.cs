using System.Collections.Generic;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.NGUIExtensions.Swiper
{
	[RequireComponent(typeof(UIAlignmentBox))]
	[AddComponentMenu("NGUI/Interaction/Swipe Page Indicator")]
	public class UIPageIndicator : MonoBehaviour
	{
		public GameObject PageIndicatorPrefab;

		public bool ShowOnlyIfNeeded = true;

		private UIAlignmentBox m_indicatorsBoxContainer;

		private readonly List<BBTweenAlpha> m_pageIndicators = new List<BBTweenAlpha>();

		private UIAlignmentBox AlignmentBox
		{
			get
			{
				return m_indicatorsBoxContainer ?? (m_indicatorsBoxContainer = base.gameObject.AddComponentIfNotExist<UIAlignmentBox>());
			}
		}

		private void OnDestroy()
		{
			PageIndicatorPrefab = null;
			for (int i = 0; i < m_pageIndicators.Count; i++)
			{
				NGUITools.Destroy(m_pageIndicators[i]);
			}
			m_pageIndicators.Clear();
		}

		public void SelectPage(int _selectedPage, int _maxPages)
		{
			UpdatePageIndicators(_maxPages);
			bool flag = !ShowOnlyIfNeeded || _maxPages >= 1;
			if (flag)
			{
				for (int i = 0; i < m_pageIndicators.Count; i++)
				{
					m_pageIndicators[i].value = ((i != _selectedPage) ? 0f : 1f);
				}
				if (m_pageIndicators.Count > _selectedPage)
				{
					m_pageIndicators[_selectedPage].value = 1f;
				}
			}
			base.gameObject.SetActive(flag);
		}

		private void UpdatePageIndicators(int _count)
		{
			int count = m_pageIndicators.Count;
			if (count == _count)
			{
				return;
			}
			for (int i = m_pageIndicators.Count; i < _count; i++)
			{
				GameObject gameObject = NGUITools.AddChild(base.gameObject, PageIndicatorPrefab);
				m_pageIndicators.Add(gameObject.GetComponent<BBTweenAlpha>());
			}
			if (m_pageIndicators.Count > _count)
			{
				for (int num = m_pageIndicators.Count - 1; num >= _count; num--)
				{
					GameObject obj = m_pageIndicators[num].gameObject;
					m_pageIndicators.RemoveAt(num);
					NGUITools.Destroy(obj);
				}
			}
			AlignmentBox.RepositionNow = true;
		}
	}
}
