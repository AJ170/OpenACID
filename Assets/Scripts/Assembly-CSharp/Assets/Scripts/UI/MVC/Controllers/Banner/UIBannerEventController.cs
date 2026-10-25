using System;
using Assets.Scripts.Tracking;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.MVC.Controllers.Events;
using Assets.Scripts.UI.MVC.UIView.Banner;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.NewsFeedScreen;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.EventManager.Data;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using Database;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Banner
{
	public class UIBannerEventController : UIBannerBaseController
	{
		[SerializeField]
		private UIBannerEventViewPresenter m_bannerEventView;

		private DateTime m_eventEndDate;

		private bool IsPressed { get; set; }

		public void OnPress(bool _pressed)
		{
			OnItemPress(_pressed);
			if (!_pressed && IsPressed)
			{
				OnItemClicked();
			}
			IsPressed = _pressed;
		}

		public void OnDragStart()
		{
			OnItemDragStart();
			IsPressed = false;
		}

		public override void InstantShowItem()
		{
			m_bannerEventView.Show();
		}

		public override void UpdateBannerData(EventBase _event)
		{
			EventProgressionChallenges eventProgressionChallenges = (EventProgressionChallenges)_event;
			m_eventEndDate = ServerTimeManager.ToUtcTime(eventProgressionChallenges.Event.endTimeUTC);
			EventProgressionChallenges.EventProgression.EventProgressionTheme layoutTheme = eventProgressionChallenges.EventProgressionData.LayoutTheme;
			UIItemUtil.UpdateUITexture(layoutTheme.BannerImageGuiId, m_bannerEventView.ImageItemTexture, delegate(ItemDef _def)
			{
				Vector2 textureOffset = Vector2.zero;
				if (LayoutType == EBannerType.EventBanner1X1)
				{
					textureOffset = _def.AlphaMaskOffset;
				}
				m_bannerEventView.ImageItemTexture.SetTextureOffset(textureOffset);
			});
			string guid = layoutTheme.BannerBackgroundLargeGuId;
			if (LayoutType == EBannerType.EventBanner1X1)
			{
				guid = layoutTheme.BannerBackgroundSmallGuId;
			}
			UIItemUtil.UpdateUITexture(guid, m_bannerEventView.BackgroundTexture);
			m_bannerEventView.BackgroundTexture.color = UIItemUtil.GetWidgetColor(layoutTheme.BannerTintingColorGuId);
			m_bannerEventView.EventNameColor = UIItemUtil.GetWidgetColor(layoutTheme.BannerFontColorHeaderGuId);
			m_bannerEventView.EventName = LocalizationManager.Instance.Get(eventProgressionChallenges.EventProgressionData.MainHeaderTitleKey);
			m_bannerEventView.DescriptionColor = UIItemUtil.GetWidgetColor(layoutTheme.BannerFontColorDescriptionGuId);
			m_bannerEventView.Description = LocalizationManager.Instance.Get(eventProgressionChallenges.EventProgressionData.MainHeaderDescriptionKey);
			m_bannerEventView.TimerColor = UIItemUtil.GetWidgetColor(layoutTheme.BannerFontColorTimerGuId);
			m_bannerEventView.TimerTitleColor = UIItemUtil.GetWidgetColor(layoutTheme.BannerFontColorTimerHeaderGuId);
			m_bannerEventView.CanTap = true;
			UpdateTimer();
		}

		public override void OnNavigateToScreen()
		{
			base.OnNavigateToScreen();
			OnRedirectToEvent();
		}

		protected override void OnUpdateUnity()
		{
			UpdateTimer();
		}

		private void UpdateTimer()
		{
			DateTime utcNow = Singleton<StateManager>.Instance.ServerTimeManager.UtcNow;
			double totalSeconds = (m_eventEndDate - utcNow).TotalSeconds;
			if (totalSeconds > 0.0)
			{
				m_bannerEventView.Timer = UIDuration.FromSeconds((long)totalSeconds).ToString();
			}
		}

		private void OnItemPress(bool _pressed)
		{
			if (_pressed)
			{
				m_bannerEventView.StartTweenPlayForward();
			}
			else
			{
				m_bannerEventView.SetTweenToBeginning();
			}
		}

		private void OnItemDragStart()
		{
			if (IsPressed)
			{
				m_bannerEventView.StartTweenPlayReverse();
			}
		}

		private void OnItemClicked()
		{
			if (IsPressed)
			{
				OnNavigateToScreen();
			}
		}

		private void OnRedirectToEvent()
		{
			m_bannerEventView.CanTap = false;
			TrackingManager.OpenEventProgressionScreen("NewsFeedEventBanner");
			UIEventScreenChild.SelectTabOnEnter = UIEventScreenChild.ETabButtonType.WeaklyEvent;
			UINewsFeedDetails.StartAnimusAndGoToScreen(EScreenID.EventProgressionScreen);
		}
	}
}
