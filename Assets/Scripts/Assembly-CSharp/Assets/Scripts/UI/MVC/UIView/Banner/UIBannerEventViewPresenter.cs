using Assets.Scripts.UI.MVC.Controllers.Banner;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.UIView.Banner
{
	public class UIBannerEventViewPresenter : UIViewPresenterBase
	{
		[CheckAssigned]
		[SerializeField]
		private UIWidget m_mainContainer;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_eventname;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_description;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_timer;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_timerTitle;

		[CheckAssigned]
		[SerializeField]
		private UITexture m_backgroundTexture;

		[CheckAssigned]
		[SerializeField]
		private UIEventBannerTexture m_imageTexture;

		[CheckAssigned]
		[SerializeField]
		private BoxCollider m_tapCollider;

		[SerializeField]
		[CheckAssigned]
		private BBTargetedTween m_pressedTween;

		[CheckAssigned]
		[SerializeField]
		private UIBannerEventController m_controller;

		public string EventName
		{
			set
			{
				m_eventname.text = value;
			}
		}

		public Color EventNameColor
		{
			set
			{
				m_eventname.color = value;
			}
		}

		public string Description
		{
			set
			{
				m_description.text = value;
			}
		}

		public Color DescriptionColor
		{
			set
			{
				m_description.color = value;
			}
		}

		public string Timer
		{
			set
			{
				m_timer.text = value;
			}
		}

		public Color TimerColor
		{
			set
			{
				m_timer.color = value;
			}
		}

		public string TimerTitle
		{
			set
			{
				m_timerTitle.text = value;
			}
		}

		public Color TimerTitleColor
		{
			set
			{
				m_timerTitle.color = value;
			}
		}

		public bool CanTap
		{
			get
			{
				return m_tapCollider.enabled;
			}
			set
			{
				m_tapCollider.enabled = value;
			}
		}

		public UITexture BackgroundTexture
		{
			get
			{
				return m_backgroundTexture;
			}
		}

		public UIEventBannerTexture ImageItemTexture
		{
			get
			{
				return m_imageTexture;
			}
		}

		public override void Show()
		{
			m_mainContainer.gameObject.SetActive(true);
		}

		public override void Hide()
		{
			m_mainContainer.gameObject.SetActive(false);
		}

		public void StartTweenPlayForward()
		{
			m_pressedTween.StartTweenPlayForward();
		}

		public void SetTweenToBeginning()
		{
			m_pressedTween.SetTweenToBeginning();
		}

		public void StartTweenPlayReverse()
		{
			m_pressedTween.StartTweenPlayReverse();
		}

		protected override void OnAwakeUnity()
		{
			base.OnAwakeUnity();
		}

		private void OnPress(bool _pressed)
		{
			m_controller.OnPress(_pressed);
		}

		private void OnDragStart()
		{
			m_controller.OnDragStart();
		}
	}
}
