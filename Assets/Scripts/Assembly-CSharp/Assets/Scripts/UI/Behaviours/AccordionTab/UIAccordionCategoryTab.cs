using Assets.Scripts.Audio;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours.AccordionTab
{
	public class UIAccordionCategoryTab : UIAccordionBaseTab
	{
		[SerializeField]
		protected UITable m_subTabContainer;

		[SerializeField]
		protected BBTweenScale m_scaleListTween;

		[SerializeField]
		protected BBTweenRotation m_arrowTween;

		[SerializeField]
		protected UISprite m_arrowSprite;

		protected bool m_isLocked;

		private bool m_isOpen;

		public UITable ChildTable
		{
			get
			{
				return m_subTabContainer;
			}
		}

		public bool HasChildren
		{
			get
			{
				return m_subTabContainer.transform.childCount > 0;
			}
			set
			{
				m_arrowSprite.gameObject.SetActive(value);
			}
		}

		public virtual bool IsLocked
		{
			get
			{
				return m_isLocked;
			}
			set
			{
				m_isLocked = value;
			}
		}

		public bool IsOpend
		{
			get
			{
				return m_isOpen;
			}
			set
			{
				if (HasChildren && m_isOpen != value)
				{
					m_isOpen = value;
					if (m_isOpen)
					{
						OpenTab(false);
					}
					else
					{
						CloseTab(false);
					}
				}
			}
		}

		public override void Init(bool _isActivated)
		{
			base.Init(_isActivated);
			m_isOpen = HasChildren && _isActivated;
			if (m_isOpen)
			{
				OpenTab(true);
			}
			else
			{
				CloseTab(true);
			}
		}

		public void RepositionLayout()
		{
			ChildTable.Reposition();
		}

		protected void OpenTab(bool _finishInstantly)
		{
			m_subTabContainer.gameObject.SetActive(true);
			EventDelegate.Remove(m_scaleListTween.onFinished, OnDeActivteScaleTweenFinished);
			if (_finishInstantly)
			{
				m_scaleListTween.SetTweenToEnd();
				m_arrowTween.SetTweenToEnd();
			}
			else
			{
				m_scaleListTween.StartTweenPlayForward();
				m_arrowTween.StartTweenPlayForward();
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_accordionmenu_movement);
			}
		}

		protected void CloseTab(bool _finishInstantly)
		{
			if (_finishInstantly)
			{
				OnDeActivteScaleTweenFinished();
				m_scaleListTween.SetTweenToBeginning();
				m_arrowTween.SetTweenToBeginning();
			}
			else
			{
				EventDelegate.Add(m_scaleListTween.onFinished, OnDeActivteScaleTweenFinished, true);
				m_scaleListTween.StartTweenPlayReverse();
				m_arrowTween.StartTweenPlayReverse();
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_accordionmenu_movement);
			}
		}

		protected override void OnAwake()
		{
			base.OnAwake();
			m_scaleListTween.updateTable = true;
		}

		protected override void OnActivateTab(bool _finishInstantly)
		{
			base.OnActivateTab(_finishInstantly);
			m_arrowSprite.color = base.SelectedTextColor;
		}

		protected override void OnDeactivateTab(bool _finishInstantly)
		{
			base.OnDeactivateTab(_finishInstantly);
			m_arrowSprite.color = base.DeselectedTextColor;
		}

		private void OnDeActivteScaleTweenFinished()
		{
			m_subTabContainer.gameObject.SetActive(false);
		}
	}
}
