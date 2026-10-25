using System;
using Assets.Scripts.UI.Behaviours.AccordionTab;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.UIView.Accordion
{
	public class UIAccordionRootView : UIViewPresenterBase
	{
		[SerializeField]
		[CheckAssigned]
		private UITable m_rootTable;

		[CheckAssigned]
		[SerializeField]
		private UIScrollView m_scrollView;

		[SerializeField]
		[CheckAssigned]
		private BBTweenAlpha m_selectedTabHighlighterTween;

		[CheckAssigned]
		[SerializeField]
		private UIAccordionLeaderboardCategoryTab m_categoryTabPrefab;

		[CheckAssigned]
		[SerializeField]
		private UIAccordionLeaderboardEntryTab m_entryTabPrefab;

		public UITable RootTable
		{
			get
			{
				return m_rootTable;
			}
		}

		public UIAccordionLeaderboardCategoryTab CategoryTabPrefab
		{
			get
			{
				return m_categoryTabPrefab;
			}
		}

		public UIAccordionLeaderboardEntryTab EntryTabPrefab
		{
			get
			{
				return m_entryTabPrefab;
			}
		}

		public void UpdateTable()
		{
			UITable rootTable = m_rootTable;
			rootTable.onReposition = (UITable.OnReposition)Delegate.Combine(rootTable.onReposition, new UITable.OnReposition(OnFirstReposition));
			m_rootTable.repositionNow = true;
		}

		public void ClearTable()
		{
			if (m_rootTable != null && m_rootTable.transform != null)
			{
				m_rootTable.transform.ClearChildren();
			}
		}

		public void SetTabHighlighterVisibility(bool _isVisible)
		{
			m_selectedTabHighlighterTween.gameObject.SetActive(_isVisible);
		}

		public void PlayTabHighlighterTween(bool _forward)
		{
			if (_forward)
			{
				m_selectedTabHighlighterTween.StartTweenPlayForward();
			}
			else
			{
				m_selectedTabHighlighterTween.StartTweenPlayReverse();
			}
		}

		public void UpdateTabHighlighterLocation(float _positionY)
		{
			Vector3 position = m_selectedTabHighlighterTween.transform.position;
			position.y = _positionY;
			m_selectedTabHighlighterTween.transform.position = position;
			m_selectedTabHighlighterTween.gameObject.SetActive(true);
			m_selectedTabHighlighterTween.StartTweenPlayForward();
		}

		public void RepositionScrollViewIfOutofBounds()
		{
			m_scrollView.UpdateScrollbars(true);
			if (m_scrollView.canMoveVertically && !m_scrollView.shouldMoveVertically)
			{
				m_scrollView.RestrictWithinBounds(false);
			}
		}

		protected override void OnStartUnity()
		{
			base.OnStartUnity();
			UITable rootTable = m_rootTable;
			rootTable.onReposition = (UITable.OnReposition)Delegate.Combine(rootTable.onReposition, new UITable.OnReposition(OnFirstReposition));
			m_rootTable.repositionNow = true;
		}

		protected override void OnDestroyUnity()
		{
			if (m_rootTable != null)
			{
				UITable rootTable = m_rootTable;
				rootTable.onReposition = (UITable.OnReposition)Delegate.Remove(rootTable.onReposition, new UITable.OnReposition(OnFirstReposition));
			}
		}

		private void OnFirstReposition()
		{
			UITable rootTable = m_rootTable;
			rootTable.onReposition = (UITable.OnReposition)Delegate.Remove(rootTable.onReposition, new UITable.OnReposition(OnFirstReposition));
			m_scrollView.ResetPosition();
		}
	}
}
