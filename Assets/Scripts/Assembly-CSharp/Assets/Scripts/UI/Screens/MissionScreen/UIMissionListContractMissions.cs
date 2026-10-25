using System;
using System.Collections;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Database;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Behaviours;
using Assets.Scripts.UI.EnvironmentBridge;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.Progression;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.MissionScreen.EarthPins;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Screens.Popup.EarlyAccess;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.MissionManager;
using Assets.Scripts.UI.State.ProfileManager;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.MissionScreen
{
	public class UIMissionListContractMissions : UIMissionListBase
	{
		[SerializeField]
		private BBTweenAnimation m_misisonListTweenAnimation;

		[SerializeField]
		private UnityEngine.Animation m_misisonListAnimation;

		private bool m_instantShowPin;

		private MissionCollection.RankMissions m_currentRankMissions;

		private List<AcierProtocol.Mission> m_missionList = new List<AcierProtocol.Mission>();

		private bool m_setFocusAfterEarthTransition;

		private bool m_isWaitingForAnimationToComplete;

		private bool m_showNewMissionMessage;

		public override MissionSelectionManager.EMissionMenu ListType
		{
			get
			{
				return MissionSelectionManager.EMissionMenu.ContractMissions;
			}
		}

		public override bool ShowBackButton
		{
			get
			{
				return false;
			}
		}

		public bool IsInteractionDisabled { get; set; }

		public override void OnActivate()
		{
			m_setFocusAfterEarthTransition = true;
			base.OnActivate();
			m_misisonListTweenAnimation.enabled = false;
			m_isWaitingForAnimationToComplete = false;
			ResetMissionList();
			SetupCurrentRankMissions();
			UpdateMissionsDisplayed();
			Resource[] resources = Singleton<StateManager>.Instance.TemplateManager.Globals.refreshMissionListCost.resources;
			Array.Sort(resources, UIUtil.SortResourcesByType);
			Resource resource = resources[0];
			m_actionButtonBar.SetupButton(UIMissionActionButtonBar.EButtonType.Primary, 1198, base.CurrentSelected != null);
			m_actionButtonBar.SetupButton(UIMissionActionButtonBar.EButtonType.Secondary, 1520, base.CurrentSelected != null, resource.type);
		}

		public override bool OnTapBack()
		{
			if (IsInteractionDisabled)
			{
				return false;
			}
			Singleton<StateManager>.Instance.MissionSelectionManager.SubMenuType = MissionSelectionManager.EMissionMenu.Campaigns;
			return true;
		}

		public override void Refresh()
		{
			UpdateMissionsDisplayed();
			if (!m_isWaitingForAnimationToComplete)
			{
				LocationSet locationSet = m_missionList[base.CurrentSelectedIndex].locationSet;
				if (locationSet != null)
				{
					base.EarthPinManager.CreateRegionPinsForContractMissions(m_missionList, base.CurrentSelectedIndex);
				}
			}
		}

		public bool SelectMission(bool _won, EMissionKind _missionKind)
		{
			MissionCollection.RankMissions currrentRankMissions = Singleton<StateManager>.Instance.ProfileManager.CurrrentRankMissions;
			currrentRankMissions.GetMissionsByHeroicRank(ref m_missionList);
			for (int i = 0; i < m_missionList.Count; i++)
			{
				AcierProtocol.Mission mission = m_missionList[i];
				if (mission.wasWon == _won && mission.missionKind == _missionKind)
				{
					base.CurrentSelectedId = mission.id;
					base.CurrentSelectedIndex = i;
					UpdateMissionsDisplayed();
					return true;
				}
			}
			return false;
		}

		protected override void OnActionButtonClicked(UIMissionActionButtonBar.EButtonType _type)
		{
			if (!m_actionButtonBar.GetButtonByType(_type).isEnabled || IsInteractionDisabled)
			{
				return;
			}
			if (_type == UIMissionActionButtonBar.EButtonType.Primary)
			{
				UIMissionItemContract uIMissionItemContract = (UIMissionItemContract)base.CurrentSelected;
				if (uIMissionItemContract.IsMissionValid())
				{
					MissionSelectionManager missionSelectionManager = Singleton<StateManager>.Instance.MissionSelectionManager;
					missionSelectionManager.ContractMission = uIMissionItemContract.MissionData;
					missionSelectionManager.SubMenuType = MissionSelectionManager.EMissionMenu.ContractDetails;
				}
			}
			else
			{
				OnRefreshButtonClick();
			}
		}

		protected override void OnWraperItemUpdate(Transform _transform, int _index)
		{
			if (!IsInteractionDisabled)
			{
				UIMissionItemContract component = _transform.GetComponent<UIMissionItemContract>();
				component.UpdateContractItem(m_missionList[_index], _index);
				bool flag = !string.IsNullOrEmpty(base.CurrentSelectedId) && base.CurrentSelectedId == component.ItemId;
				if (flag)
				{
					base.CurrentSelected = component;
				}
				component.ChangeSelectedStatusInstantly(flag);
			}
		}

		protected override void OnCurrentSelectedChanged(int _previous)
		{
			m_instantShowPin = false;
			if (_previous >= 0 && _previous < m_missionList.Count)
			{
				AcierProtocol.Mission mission = m_missionList[_previous];
				LocationSet locationSet = mission.locationSet;
				if (locationSet != null)
				{
					RegionDefinition regionDefinition = locationSet.GetRegionDefinition();
					m_instantShowPin = regionDefinition.CityIdentifier == m_currentSelected.RegionData.CityIdentifier;
					EarthPinBase usedPinByEarthPinLocaKey = base.EarthPinManager.GetUsedPinByEarthPinLocaKey(regionDefinition, string.Empty);
					if (usedPinByEarthPinLocaKey != null)
					{
						usedPinByEarthPinLocaKey.PlayTween = !m_instantShowPin;
						usedPinByEarthPinLocaKey.IsSelected = false;
					}
				}
			}
			base.OnCurrentSelectedChanged(_previous);
			m_actionButtonBar.ChangeButtonVisibility(UIMissionActionButtonBar.EButtonType.Primary, base.CurrentSelected != null);
			m_actionButtonBar.ChangeButtonVisibility(UIMissionActionButtonBar.EButtonType.Secondary, base.CurrentSelected != null);
		}

		protected override void SetupEarthGlobeConnection()
		{
			if (m_missionList.Count > base.CurrentSelectedIndex)
			{
				EEra era = m_missionList[base.CurrentSelectedIndex].locationSet.era;
				base.EarthPinManager.CreateRegionPinsForContractMissions(m_missionList, base.CurrentSelectedIndex, !m_setFocusAfterEarthTransition);
				m_setFocusAfterEarthTransition = false;
				EraExtensions.EEarthPin earthPinByEra = era.GetEarthPinByEra();
				BehaviourSingleton<EnvironmentBridgeMain>.Instance.EarthTransitionManager.SetEarthTransition(earthPinByEra, true);
			}
		}

		protected override void SetupEarthPinConnection()
		{
			if (!m_isWaitingForAnimationToComplete && base.CurrentSelected != null)
			{
				LocationSet locationSet = m_missionList[base.CurrentSelectedIndex].locationSet;
				RegionDefinition regionDefinition = locationSet.GetRegionDefinition();
				EarthPinBase usedPinByEarthPinLocaKey = base.EarthPinManager.GetUsedPinByEarthPinLocaKey(regionDefinition, string.Empty);
				usedPinByEarthPinLocaKey.PlayTween = (m_instantShowPin = !m_instantShowPin);
				usedPinByEarthPinLocaKey.IsSelected = true;
				base.CurrentSelected.SetEarthPin(m_scrollPanel, usedPinByEarthPinLocaKey);
			}
		}

		private void ResetMissionList()
		{
			List<Transform> children = m_wrapperContainer.Children;
			for (int i = 0; i < children.Count; i++)
			{
				UIMissionItemContract component = children[i].GetComponent<UIMissionItemContract>();
				if (component.gameObject.activeSelf)
				{
					component.SetupTweenersForAnimation(false);
				}
			}
			m_misisonListAnimation.Stop();
			m_misisonListTweenAnimation.IgnoreParent = true;
			m_misisonListTweenAnimation.StopAnimationIfPlaying(true);
		}

		private void DeselectEarthPinConnection()
		{
			if (base.CurrentSelected != null)
			{
				LocationSet locationSet = m_missionList[base.CurrentSelectedIndex].locationSet;
				RegionDefinition regionDefinition = locationSet.GetRegionDefinition();
				EarthPinBase usedPinByEarthPinLocaKey = base.EarthPinManager.GetUsedPinByEarthPinLocaKey(regionDefinition, string.Empty);
				usedPinByEarthPinLocaKey.PlayTween = true;
				usedPinByEarthPinLocaKey.IsSelected = false;
				base.CurrentSelected.HideLine();
			}
		}

		private void SetupCurrentRankMissions()
		{
			ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
			m_currentRankMissions = profileManager.CurrrentRankMissions;
			m_showNewMissionMessage = false;
			if (!UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialCategoryActive(ETutorialCategory.TutCatMs26Contracts))
			{
				LastProfileData lastProfileDataBeforeMission = profileManager.LastProfileDataBeforeMission;
				MissionCollection missionCollection = lastProfileDataBeforeMission.MissionCollection;
				if (missionCollection != null && !lastProfileDataBeforeMission.MissionUpdateShownToUse)
				{
					m_isWaitingForAnimationToComplete = true;
					m_showNewMissionMessage = true;
					lastProfileDataBeforeMission.MissionUpdateShownToUse = true;
					m_currentRankMissions = lastProfileDataBeforeMission.CurrrentRankMissions;
				}
			}
			UpdateMissionList();
		}

		private void UpdateMissionList()
		{
			m_missionList.Clear();
			m_currentRankMissions.GetMissionsByHeroicRank(ref m_missionList);
			ValidateLastSelected();
			m_wrapperContainer.ItemCount = m_missionList.Count;
			m_wrapperContainer.WrapScrollView.ResetPosition();
			m_wrapperContainer.SetFocusOnChildByIndex(base.CurrentSelectedIndex);
		}

		private void ValidateLastSelected()
		{
			bool flag = false;
			for (int i = 0; i < m_missionList.Count; i++)
			{
				if (base.CurrentSelectedId == m_missionList[i].id)
				{
					base.CurrentSelectedIndex = i;
					flag = true;
					break;
				}
			}
			if (!flag && m_missionList.Count > 0)
			{
				base.CurrentSelectedId = m_missionList[0].id;
				base.CurrentSelectedIndex = 0;
			}
		}

		private void UpdateMissionsDisplayed()
		{
			MissionCollection.RankMissions currentRankMissions = m_currentRankMissions;
			ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
			m_currentRankMissions = profileManager.CurrrentRankMissions;
			if (currentRankMissions != null && !m_currentRankMissions.IsEqual(currentRankMissions))
			{
				StopCoroutine("CheckMissionListChanged");
				StartCoroutine("CheckMissionListChanged");
			}
		}

		private void OnRefreshButtonClick()
		{
			if (!IsInteractionDisabled)
			{
				Resource[] resources = Singleton<StateManager>.Instance.TemplateManager.Globals.refreshMissionListCost.resources;
				Array.Sort(resources, UIUtil.SortResourcesByType);
				Resource cost = resources[0];
				UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
				List<UIPopupResourcesContent.ResourceConfiguration> list = new List<UIPopupResourcesContent.ResourceConfiguration>();
				list.Add(new UIPopupResourcesContent.ResourceConfiguration
				{
					HasEnoughResources = profile.HasEnoughResources(cost),
					Resource = cost
				});
				List<UIPopupResourcesContent.ResourceConfiguration> list2 = list;
				PopUpButtonConfiguration[] buttons = new PopUpButtonConfiguration[1] { UIBuyHelper.CreateResourceCostButton(list2[0], 1780, delegate
				{
					UIBuyHelper.DetermineAllowPurches(cost, RefreshMissionListAfterAllowanceCheck, null);
				}) };
				PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
				popUpConfiguration.ShowCloseButton = true;
				popUpConfiguration.OnCloseButtonClicked = delegate
				{
					Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
				};
				popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.CloseButton;
				popUpConfiguration.Heading = LocalizationManager.Instance.Get(2341);
				popUpConfiguration.Buttons = buttons;
				popUpConfiguration.Data = new EarlyAccessContent.EarlyAccessData
				{
					ShowResources = false,
					ShowModifier = false,
					RewardIncreaseFactor = 0f,
					ResourcesCost = list2,
					ResourceSymbolAdditive = false,
					Description = LocalizationManager.Instance.Get(5754, new StringMapper { { "mission_refresh_cost", cost.amount } })
				};
				PopUpConfiguration config = popUpConfiguration;
				Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
			}
		}

		private void RefreshMissionListAfterAllowanceCheck()
		{
			int rank = Singleton<StateManager>.Instance.MissionSelectionManager.HeroicMissionRank;
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_generic_refresh);
			UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncResult)
			{
				Singleton<StateManager>.Instance.ProfileManager.RefreshMissionList(Singleton<StateManager>.Instance.ProfileManager.CurrentAssassinId, rank, _asyncResult);
			}, null, delegate(IAsyncResult _result)
			{
				if (!_result.IsError())
				{
					Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
					m_showNewMissionMessage = true;
					UpdateMissionsDisplayed();
				}
			});
		}

		private IEnumerator CheckMissionListChanged()
		{
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			while (!UIFlowHandler.CanStartMissionProgressionAnimation)
			{
				yield return wait;
			}
			if (m_isWaitingForAnimationToComplete)
			{
				ResetMissionList();
			}
			m_isWaitingForAnimationToComplete = true;
			DeselectEarthPinConnection();
			PlayFadeOut();
			while (IsAnimationPlaying())
			{
				yield return wait;
			}
			UpdateMissionList();
			if (m_showNewMissionMessage)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(5747), true);
				m_showNewMissionMessage = false;
			}
			PlayFadeIn();
			while (IsAnimationPlaying())
			{
				yield return wait;
			}
			m_isWaitingForAnimationToComplete = false;
			SetupEarthPinConnection();
		}

		private bool IsAnimationPlaying()
		{
			return m_misisonListTweenAnimation.IsAnimating || m_misisonListAnimation.isPlaying;
		}

		[ContextMenu("PlayFadeOut")]
		private void PlayFadeOut()
		{
			SetupAnimation(false);
		}

		[ContextMenu("PlayFadeIn")]
		private void PlayFadeIn()
		{
			SetupAnimation(true);
		}

		public void SetupAnimation(bool _fadeIn)
		{
			List<Transform> children = m_wrapperContainer.Children;
			List<UnityEngine.Animation> list = new List<UnityEngine.Animation>();
			for (int i = 0; i < children.Count; i++)
			{
				UIMissionItemContract component = children[i].GetComponent<UIMissionItemContract>();
				if (component.gameObject.activeSelf)
				{
					component.SetupTweenersForAnimation(_fadeIn);
					list.Add(component.RefreshAnimation);
				}
			}
			m_misisonListTweenAnimation.SetAnimationList(list.ToArray(), (!_fadeIn) ? "MissionContractFadeOut" : "MissionContractFadeIn");
			m_misisonListAnimation.Play();
		}
	}
}
