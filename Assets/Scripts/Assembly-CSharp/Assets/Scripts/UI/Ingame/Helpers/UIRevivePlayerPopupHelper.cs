using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.Mission;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Tracking;
using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Screens.Popup.RevivePlayer;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Ingame.Helpers
{
	public class UIRevivePlayerPopupHelper
	{
		private bool m_waitingForInput;

		public bool HasEnoughOfOneResources
		{
			get
			{
				UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
				Resource[] resources = Singleton<StateManager>.Instance.TemplateManager.Globals.reviveCosts.resources;
				for (int i = 0; i < resources.Length; i++)
				{
					if ((bool)profile.HasEnoughResources(resources[i]))
					{
						return true;
					}
				}
				return false;
			}
		}

		public bool IsVisible { get; private set; }

		public void Update()
		{
			if (!IsVisible && UbiCore.Utils.Singleton<MissionManager>.IsAvailable && UbiCore.Utils.Singleton<MissionManager>.Instance.OfferingRevive && Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.ReviveEntitlementGUID))
			{
				ShowRevivePopup();
			}
		}

		public void ShowRevivePopup()
		{
			if (AdColonyManager.IsVideoAvailable)
			{
				m_waitingForInput = true;
				Singleton<UIScreenManager>.Instance.ShowPopUpScreen(CreateOfferVideoPopupConfiguration());
				IsVisible = true;
				TrackingManager.AddRevivePopupTrackPoint();
			}
			else if (HasEnoughOfOneResources)
			{
				m_waitingForInput = true;
				Singleton<UIScreenManager>.Instance.ShowPopUpScreen(CreateDefaultPopupConfiguration());
				IsVisible = true;
				TrackingManager.AddRevivePopupTrackPoint();
			}
			else
			{
				UbiCore.Utils.Singleton<MissionManager>.Instance.WaitForRevive = false;
				IsVisible = false;
				UbiCore.Utils.Singleton<LocalStorage>.Instance.CTACounterAdd(LocalStorage.CTACounterReviveNotPossible, 1);
			}
		}

		private void OnTimerRanOut()
		{
			if (m_waitingForInput)
			{
				m_waitingForInput = false;
				OnRevivePlayerPopupClosed(false);
			}
		}

		private PopUpConfiguration CreateDefaultPopupConfiguration()
		{
			UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
			Resource[] resources = Singleton<StateManager>.Instance.TemplateManager.Globals.reviveCosts.resources;
			List<PopUpButtonConfiguration> list = new List<PopUpButtonConfiguration>();
			Array.Sort(resources, UIUtil.SortResourcesByType);
			List<UIPopupResourcesContent.ResourceConfiguration> list2 = new List<UIPopupResourcesContent.ResourceConfiguration>(resources.Length);
			for (int i = 0; i < resources.Length; i++)
			{
				bool flag = profile.HasEnoughResources(resources[i]);
				EResourceType type = resources[i].type;
				list.Add(new PopUpButtonConfiguration
				{
					Data = true,
					Label = LocalizationManager.Instance.Get(6548),
					ButtonType = PopUpButtonConfiguration.EButtonType.eSuggestedClick,
					OnButtonPressed = delegate
					{
						OnChoseToReviveByDefault(type);
					},
					SoundIntent = EButtonIntent.Buy,
					IsEnabled = flag
				});
				list2.Add(new UIPopupResourcesContent.ResourceConfiguration
				{
					HasEnoughResources = flag,
					Resource = resources[i]
				});
			}
			if (resources.Length == 1)
			{
				list.Add(new PopUpButtonConfiguration
				{
					Data = true,
					Label = LocalizationManager.Instance.Get(6667),
					ButtonType = PopUpButtonConfiguration.EButtonType.eNormal,
					OnButtonPressed = delegate
					{
						OnTimerRanOut();
					},
					SoundIntent = EButtonIntent.Cancel,
					IsEnabled = true
				});
			}
			PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
			popUpConfiguration.TimerDuration = Singleton<StateManager>.Instance.TemplateManager.Globals.reviveTimer;
			popUpConfiguration.OnTimerRanOut = OnTimerRanOut;
			popUpConfiguration.Data = new RevivePlayerContent.RevivePlayerData
			{
				Description = LocalizationManager.Instance.Get(6537),
				ResourcesCost = list2
			};
			popUpConfiguration.Heading = LocalizationManager.Instance.Get(6547);
			popUpConfiguration.Buttons = list.ToArray();
			popUpConfiguration.ShowCloseButton = false;
			popUpConfiguration.OnCloseButtonClicked = OnTimerRanOut;
			return popUpConfiguration;
		}

		private PopUpConfiguration CreateOfferVideoPopupConfiguration()
		{
			PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
			popUpConfiguration.TimerDuration = Singleton<StateManager>.Instance.TemplateManager.Globals.reviveTimer;
			popUpConfiguration.OnTimerRanOut = OnTimerRanOut;
			popUpConfiguration.Data = new RevivePlayerWithVideoContent.RevivePlayerWithVideoData
			{
				Description = LocalizationManager.Instance.Get(9953)
			};
			popUpConfiguration.Heading = LocalizationManager.Instance.Get(6547);
			popUpConfiguration.Buttons = new PopUpButtonConfiguration[2]
			{
				new PopUpButtonConfiguration
				{
					Label = LocalizationManager.Instance.Get(7228),
					ButtonType = PopUpButtonConfiguration.EButtonType.eSuggestedClick,
					OnButtonPressed = delegate
					{
						OnChoseToReviveWithVideo();
					},
					SoundIntent = EButtonIntent.Buy
				},
				new PopUpButtonConfiguration
				{
					Label = LocalizationManager.Instance.Get(6667),
					OnButtonPressed = delegate
					{
						OnRevivePlayerPopupClosed(false);
					},
					SoundIntent = EButtonIntent.Cancel
				}
			};
			popUpConfiguration.ShowCloseButton = false;
			popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.Top;
			popUpConfiguration.OnCloseButtonClicked = OnTimerRanOut;
			return popUpConfiguration;
		}

		private void OnRevivePlayerPopupClosed(bool _revived)
		{
			if (!_revived)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ClearQueue();
			}
			Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			UbiCore.Utils.Singleton<MissionManager>.Instance.WaitForRevive = false;
			IsVisible = false;
		}

		private void OnChoseToReviveByDefault(EResourceType _resourceType)
		{
			if (!m_waitingForInput)
			{
				return;
			}
			m_waitingForInput = false;
			Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _result)
			{
				Singleton<StateManager>.Instance.ProfileManager.Revive(Singleton<StateManager>.Instance.ProfileManager.MissionToken.token, _resourceType, _result);
			}, new RetryHandler(delegate
			{
				OnRevivePlayerPopupClosed(false);
			}), delegate(IAsyncResult _x)
			{
				if (!_x.IsError())
				{
					UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.Revive();
					OnRevivePlayerPopupClosed(true);
					LogicMain.MissionResult.IncreaseReviveCount(true);
				}
			});
		}

		private void OnChoseToReviveWithVideo()
		{
			if (m_waitingForInput)
			{
				m_waitingForInput = false;
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
				AdColonyManager.Instance.OnVideoFinishedCallback += OnAdWatched;
				AdColonyManager.Instance.ShowAdColonyVideo(AdColonyManager.ERewardType.Revive);
			}
		}

		private void OnAdWatched(bool _adWasShown)
		{
			AdColonyManager.Instance.OnVideoFinishedCallback -= OnAdWatched;
			if (_adWasShown)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.Revive();
				LogicMain.MissionResult.IncreaseReviveCount(false);
			}
			OnRevivePlayerPopupClosed(_adWasShown);
		}
	}
}
