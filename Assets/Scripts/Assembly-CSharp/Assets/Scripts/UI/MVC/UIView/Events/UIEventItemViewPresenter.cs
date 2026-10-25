using System;
using Assets.Scripts.UI.MVC.Controllers.Events;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.UIView.Events
{
	public class UIEventItemViewPresenter : UIViewPresenterBase
	{
		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_headerLabel;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_descriptionLabel;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_progressText;

		[SerializeField]
		[CheckAssigned]
		private UIProgressBar m_eventProgress;

		[CheckAssigned]
		[SerializeField]
		private UISprite m_awardProgressThumb;

		[SerializeField]
		[CheckAssigned]
		private UISprite m_lockIcon;

		[CheckAssigned]
		[SerializeField]
		private UISprite m_lockBackground;

		[SerializeField]
		[CheckAssigned]
		private UISprite m_checkBoxFrame;

		[SerializeField]
		[CheckAssigned]
		private UISprite m_checkBoxMarker;

		[CheckAssigned]
		[SerializeField]
		private UIWidget m_centerWidget;

		[SerializeField]
		[CheckAssigned]
		private UIAlignmentBox m_rewardAlignmentBox;

		[CheckAssigned]
		[SerializeField]
		private UITweenedScreenComponent m_tweenComponent;

		[CheckAssigned]
		[SerializeField]
		private UIButtonProxy m_claimButton;

		[CheckAssigned]
		[SerializeField]
		private UIEventItemController m_controller;

		private UIWidget m_viewWidget;

		public UIButtonProxy ClaimButton
		{
			get
			{
				return m_claimButton;
			}
		}

		public UIWidget CenterWidget
		{
			get
			{
				return m_centerWidget;
			}
		}

		public string HeaderLabel
		{
			set
			{
				m_headerLabel.text = value;
			}
		}

		public string HeaderDescription
		{
			set
			{
				m_descriptionLabel.text = value;
			}
		}

		public float EventProgress
		{
			set
			{
				m_eventProgress.value = 1f;
				m_eventProgress.value = value;
				m_eventProgress.ForceUpdate();
			}
		}

		public string EventProgressText
		{
			set
			{
				m_progressText.text = value;
			}
		}

		public bool ShowProgressThumb
		{
			get
			{
				return m_awardProgressThumb.alpha > 0f;
			}
			set
			{
				m_awardProgressThumb.alpha = (value ? 1 : 0);
			}
		}

		public bool ShowLockIcon
		{
			get
			{
				return m_lockIcon.gameObject.activeSelf;
			}
			set
			{
				m_lockIcon.gameObject.SetActive(value);
			}
		}

		public bool ShowLockBackground
		{
			get
			{
				return m_lockBackground.gameObject.activeSelf;
			}
			set
			{
				m_lockBackground.gameObject.SetActive(value);
			}
		}

		public bool ShowCheckBoxFrame
		{
			get
			{
				return m_checkBoxFrame.gameObject.activeSelf;
			}
			set
			{
				m_checkBoxFrame.gameObject.SetActive(value);
			}
		}

		public bool ShowChekBoxMarker
		{
			get
			{
				return m_checkBoxMarker.gameObject.activeSelf;
			}
			set
			{
				m_checkBoxMarker.gameObject.SetActive(value);
			}
		}

		public bool IsClaimButtonEnabled
		{
			set
			{
				m_claimButton.Button.isEnabled = value;
			}
		}

		public void ChangeViewState(bool _showClaimState)
		{
			m_claimButton.gameObject.SetActive(_showClaimState);
			m_eventProgress.transform.parent.gameObject.SetActive(!_showClaimState);
			m_descriptionLabel.gameObject.SetActive(!_showClaimState);
		}

		public void SetTweenToBeginning()
		{
			if (m_tweenComponent.Tweener != null)
			{
				m_tweenComponent.Tweener.enabled = false;
				m_tweenComponent.Tweener.tweenFactor = 0f;
				m_tweenComponent.Tweener.Sample(m_tweenComponent.Tweener.tweenFactor, false);
			}
		}

		public void SetTweenToEnd()
		{
			if (m_tweenComponent.Tweener != null)
			{
				m_tweenComponent.Tweener.enabled = false;
				m_tweenComponent.Tweener.tweenFactor = 1f;
				m_tweenComponent.Tweener.Sample(m_tweenComponent.Tweener.tweenFactor, false);
			}
		}

		public void AlignRewardContainer()
		{
			m_rewardAlignmentBox.RepositionNow = true;
		}

		protected override void OnAwakeUnity()
		{
			base.OnAwakeUnity();
			m_claimButton.AllowDisabledClick = true;
			m_eventProgress.ForceUpdate();
			m_viewWidget = GetComponent<UIWidget>();
			UIWidget viewWidget = m_viewWidget;
			viewWidget.onChange = (UIWidget.OnDimensionsChanged)Delegate.Combine(viewWidget.onChange, new UIWidget.OnDimensionsChanged(OnDimentionChange));
			EventDelegate.Add(m_claimButton.onClick, OnClaimClicked);
		}

		protected override void OnDestroyUnity()
		{
			base.OnDestroyUnity();
			if (m_claimButton != null)
			{
				EventDelegate.Remove(m_claimButton.onClick, OnClaimClicked);
			}
			if (m_viewWidget != null)
			{
				UIWidget viewWidget = m_viewWidget;
				viewWidget.onChange = (UIWidget.OnDimensionsChanged)Delegate.Remove(viewWidget.onChange, new UIWidget.OnDimensionsChanged(OnDimentionChange));
			}
			m_viewWidget = null;
		}

		private void OnClaimClicked()
		{
			if (m_controller.IsInitialized)
			{
				m_controller.OnClaimClick();
			}
		}

		private void OnDimentionChange()
		{
			m_eventProgress.ForceUpdate();
		}
	}
}
