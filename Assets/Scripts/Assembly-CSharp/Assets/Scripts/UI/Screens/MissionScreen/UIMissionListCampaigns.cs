using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.UI.Behaviours;
using Assets.Scripts.UI.EnvironmentBridge;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.MissionScreen.EarthPins;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.MissionManager;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.MissionScreen
{
	public class UIMissionListCampaigns : UIMissionListBase
	{
		public static string SelectIdOnStartUp = string.Empty;

		private List<CampaignCatalog.Campaign> m_campaigns = new List<CampaignCatalog.Campaign>();

		public override MissionSelectionManager.EMissionMenu ListType
		{
			get
			{
				return MissionSelectionManager.EMissionMenu.Campaigns;
			}
		}

		public static void OpenAndSelectCampaignsItem(CampaignCatalog.Campaign _campaign)
		{
			Singleton<StateManager>.Instance.MissionSelectionManager.SubMenuType = MissionSelectionManager.EMissionMenu.Campaigns;
			SelectIdOnStartUp = _campaign.id;
			if (Singleton<UIScreenManager>.Instance.CurrentScreenId != EScreenID.Mission)
			{
				Singleton<UIScreenManager>.Instance.ChangeScreen(EScreenID.Mission, new UITweenHandle());
			}
		}

		public override void OnActivate()
		{
			base.EarthPinManager.ReleaseObjects();
			base.OnActivate();
			Singleton<StateManager>.Instance.ProfileManager.OnProfileUpdate += new SimpleDelegate.CallbackFunction(OnProfileUpdate);
			if (!string.IsNullOrEmpty(SelectIdOnStartUp))
			{
				base.CurrentSelectedId = SelectIdOnStartUp;
			}
			UpdateCampaignList();
			UpdateActionButton();
		}

		public override void OnDeactivate()
		{
			base.OnDeactivate();
			Singleton<StateManager>.Instance.ProfileManager.OnProfileUpdate -= new SimpleDelegate.CallbackFunction(OnProfileUpdate);
			if (BehaviourSingleton<EnvironmentBridgeMain>.IsInitialized)
			{
				BehaviourSingleton<EnvironmentBridgeMain>.Instance.EarthSpinManager.UIPin = null;
			}
			if (base.EarthPinManager != null)
			{
				base.EarthPinManager.ReleaseObjects();
			}
		}

		public override void Refresh()
		{
			UpdateCampaignList();
		}

		protected override void OnActionButtonClicked(UIMissionActionButtonBar.EButtonType _type)
		{
			if (m_actionButtonBar.GetButtonByType(_type).isEnabled && _type == UIMissionActionButtonBar.EButtonType.Primary)
			{
				CampaignCatalog.Campaign campaign = m_campaigns[base.CurrentSelectedIndex];
				if (campaign.IsCampaignUnlocked())
				{
					MissionSelectionManager missionSelectionManager = Singleton<StateManager>.Instance.MissionSelectionManager;
					missionSelectionManager.CampaignId = base.CurrentSelectedId;
					missionSelectionManager.SubMenuType = MissionSelectionManager.EMissionMenu.CampaignMissions;
				}
				else
				{
					Singleton<UIScreenManager>.Instance.SceneData.UnlockMissionBundlePreview.OpenPreviewByEvent(campaign);
				}
			}
		}

		protected override void OnWraperItemUpdate(Transform _transform, int _index)
		{
			UIMissionItemCampaigns component = _transform.GetComponent<UIMissionItemCampaigns>();
			component.UpdateCampaignData(m_campaigns[_index], _index);
			bool flag = !string.IsNullOrEmpty(base.CurrentSelectedId) && base.CurrentSelectedId == component.ItemId;
			if (flag)
			{
				base.CurrentSelected = component;
			}
			component.ChangeSelectedStatusInstantly(flag);
		}

		protected override void OnCurrentSelectedChanged(int _previous)
		{
			base.OnCurrentSelectedChanged(_previous);
			UpdateActionButton();
		}

		protected override void SetupEarthPinConnection()
		{
			if (base.CurrentSelected != null)
			{
				base.CurrentSelected.SetEarthPin(m_scrollPanel, BehaviourSingleton<EnvironmentBridgeMain>.Instance.EarthSpinManager.UIPin);
			}
		}

		protected override void SetupEarthGlobeConnection()
		{
			EnvironmentBridgeMain instance = BehaviourSingleton<EnvironmentBridgeMain>.Instance;
			if (!(base.CurrentSelected != null))
			{
				return;
			}
			UIMissionItemCampaigns component = base.CurrentSelected.GetComponent<UIMissionItemCampaigns>();
			RegionDefinition regionData = component.RegionData;
			if (regionData == null)
			{
				return;
			}
			EraExtensions.EEarthPin earthPinByEra = component.RegionData.Age.GetEarthPinByEra();
			regionData = regionData.Age.GetFirstRegionDefinition(true);
			if (instance.EarthSpinManager.UIPin != null && earthPinByEra == instance.EarthSpinManager.Current)
			{
				instance.EarthSpinManager.UIPin.SetTargetRegion(regionData, string.Empty);
				BehaviourSingleton<EnvironmentBridgeMain>.Instance.EarthTransitionManager.SetEarthTransition(earthPinByEra, false);
			}
			else if (instance.EarthSpinManager.UIPin == null || earthPinByEra != instance.EarthSpinManager.Current)
			{
				if (instance.EarthSpinManager.UIPin == null)
				{
					instance.EarthSpinManager.UIPin = base.EarthPinManager.GetPin().GetComponent<EarthPinBase>();
				}
				instance.EarthSpinManager.UIPin.SetTargetRegion(regionData, string.Empty);
				BehaviourSingleton<EnvironmentBridgeMain>.Instance.EarthTransitionManager.SetEarthTransition(earthPinByEra, false);
			}
		}

		private void OnProfileUpdate(object[] _params)
		{
			UpdateActionButton();
		}

		private void UpdateActionButton()
		{
			int num = 711;
			if (m_campaigns != null && m_campaigns.Count > base.CurrentSelectedIndex)
			{
				CampaignCatalog.Campaign campaign = m_campaigns[base.CurrentSelectedIndex];
				if (campaign != null && !campaign.IsCampaignUnlocked())
				{
					num = 8376;
				}
			}
			m_actionButtonBar.SetupButton(UIMissionActionButtonBar.EButtonType.Primary, num, base.CurrentSelected != null);
			m_actionButtonBar.ChangeButtonVisibility(UIMissionActionButtonBar.EButtonType.Secondary, false);
		}

		private void UpdateCampaignList()
		{
			m_campaigns.Clear();
			Singleton<StateManager>.Instance.TemplateManager.CampaignCatalog.GetSortedCampaigns(ref m_campaigns);
			ValidateLastSelected();
			m_wrapperContainer.ItemCount = m_campaigns.Count;
			m_wrapperContainer.WrapScrollView.ResetPosition();
			m_wrapperContainer.SetFocusOnChildByIndex(base.CurrentSelectedIndex);
		}

		private void ValidateLastSelected()
		{
			bool flag = false;
			for (int i = 0; i < m_campaigns.Count; i++)
			{
				if (base.CurrentSelectedId == m_campaigns[i].id)
				{
					base.CurrentSelectedIndex = i;
					flag = true;
					break;
				}
			}
			if (!flag && m_campaigns.Count > 0)
			{
				base.CurrentSelectedId = m_campaigns[0].id;
				base.CurrentSelectedIndex = 0;
			}
		}
	}
}
