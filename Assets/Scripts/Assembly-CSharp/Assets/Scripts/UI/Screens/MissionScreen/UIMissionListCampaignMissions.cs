using System.Collections;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Behaviours;
using Assets.Scripts.UI.EnvironmentBridge;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Progression;
using Assets.Scripts.UI.Screens.MissionScreen.EarthPins;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.MissionManager;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.MissionScreen
{
	public class UIMissionListCampaignMissions : UIMissionListBase
	{
		private const string c_mainAnimationName = "MissionCampaignMain";

		private static readonly StringId s_exploreId = new StringId(711);

		[SerializeField]
		private bool m_isPlayingAnimation;

		[SerializeField]
		private UIMissionMessage m_missionMessage;

		[SerializeField]
		private UnityEngine.Animation m_mainAnimation;

		[SerializeField]
		private BBTweenAnimation m_unlockMissionAnimationTween;

		[SerializeField]
		private BBTweenAnimation m_completeMissionAnimationTween;

		private int m_completedMissionIndexToAnimate;

		private int m_unlockedMissionIndexToAnimate;

		private MissionsAtLocation[] m_missionsAtLocation;

		private List<MissionInfo> m_missionList = new List<MissionInfo>();

		private bool m_instantShowPin;

		private bool m_setFocusAfterEarthTransition;

		public override MissionSelectionManager.EMissionMenu ListType
		{
			get
			{
				return MissionSelectionManager.EMissionMenu.CampaignMissions;
			}
		}

		public override bool ShowBackButton
		{
			get
			{
				return true;
			}
		}

		public bool IsPlayingAnimation
		{
			get
			{
				return m_isPlayingAnimation;
			}
		}

		public bool IsWaitingForAnimation { get; private set; }

		private MissionInfo CurrentSelectedMissionInfo
		{
			get
			{
				if (base.CurrentSelectedIndex >= 0 && base.CurrentSelectedIndex < m_missionList.Count)
				{
					return m_missionList[base.CurrentSelectedIndex];
				}
				return null;
			}
		}

		public override void OnInit(EarthPinManager _earthPinManager)
		{
			base.OnInit(_earthPinManager);
			m_missionMessage.IsVisible = false;
		}

		public override void OnActivate()
		{
			base.OnActivate();
			m_setFocusAfterEarthTransition = true;
			m_actionButtonBar.ChangeButtonVisibility(UIMissionActionButtonBar.EButtonType.Primary, false);
			m_actionButtonBar.ChangeButtonVisibility(UIMissionActionButtonBar.EButtonType.Secondary, false);
			UpdateCampaignList();
			UpdateButtonBySelectedItem();
		}

		public override void OnDeactivate()
		{
			base.OnDeactivate();
			m_missionList.Clear();
			if (m_missionMessage != null)
			{
				m_missionMessage.IsVisible = false;
			}
			m_missionsAtLocation = null;
			ClearMissionItemItems();
			IsWaitingForAnimation = false;
		}

		public override bool OnTapBack()
		{
			Singleton<StateManager>.Instance.MissionSelectionManager.SubMenuType = MissionSelectionManager.EMissionMenu.Campaigns;
			return true;
		}

		public override void Refresh()
		{
			UpdateCampaignList();
		}

		protected override void OnActionButtonClicked(UIMissionActionButtonBar.EButtonType _type)
		{
			if (_type == UIMissionActionButtonBar.EButtonType.Primary && m_actionButtonBar.GetButtonByType(_type).isEnabled)
			{
				MissionInfo missionInfo = m_missionList[base.CurrentSelectedIndex];
				if (missionInfo.IsUnlocked())
				{
					Singleton<StateManager>.Instance.MissionSelectionManager.CampaignMission = missionInfo;
					Singleton<StateManager>.Instance.MissionSelectionManager.SubMenuType = MissionSelectionManager.EMissionMenu.CampaignDetails;
				}
				else
				{
					UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(missionInfo.LocaMissionLockedStatus());
				}
			}
		}

		protected override void OnWraperItemUpdate(Transform _transform, int _index)
		{
			MissionInfo missionInfo = m_missionList[_index];
			LocationSet locationSetFromMissionInfoId = m_missionsAtLocation.GetLocationSetFromMissionInfoId(missionInfo);
			UIMissionItemCampaign component = _transform.GetComponent<UIMissionItemCampaign>();
			component.UpdateCampaignMission(m_missionList[_index], locationSetFromMissionInfoId, _index);
			bool flag = !string.IsNullOrEmpty(base.CurrentSelectedId) && base.CurrentSelectedId == component.ItemId;
			if (flag)
			{
				base.CurrentSelected = component;
			}
			component.ChangeSelectedStatusInstantly(flag);
		}

		protected override void OnCurrentSelectedChanged(int _previous)
		{
			m_instantShowPin = false;
			if (_previous >= 0 && _previous < m_missionList.Count)
			{
				MissionInfo missionInfo = m_missionList[_previous];
				LocationSet locationSetFromMissionInfoId = m_missionsAtLocation.GetLocationSetFromMissionInfoId(missionInfo);
				if (locationSetFromMissionInfoId != null)
				{
					RegionDefinition regionDefinition = locationSetFromMissionInfoId.GetRegionDefinition();
					m_instantShowPin = regionDefinition.CityIdentifier == m_currentSelected.RegionData.CityIdentifier;
					MissionInfo missionInfoFromLocationSetById = m_missionsAtLocation.GetMissionInfoFromLocationSetById(base.CurrentSelectedId);
					EarthPinBase usedPinByEarthPinLocaKey = base.EarthPinManager.GetUsedPinByEarthPinLocaKey(regionDefinition, missionInfoFromLocationSetById.regionAppearanceOption);
					if (usedPinByEarthPinLocaKey != null)
					{
						usedPinByEarthPinLocaKey.PlayTween = !m_instantShowPin;
						usedPinByEarthPinLocaKey.IsSelected = false;
					}
				}
			}
			base.OnCurrentSelectedChanged(_previous);
			UpdateMissionMessage();
			UpdateButtonBySelectedItem();
		}

		protected override void SetupEarthGlobeConnection()
		{
			if (m_missionList.Count > base.CurrentSelectedIndex && base.CurrentSelectedId != null)
			{
				LocationSet locationSetFromMissionInfoId = m_missionsAtLocation.GetLocationSetFromMissionInfoId(base.CurrentSelectedId);
				if (locationSetFromMissionInfoId != null)
				{
					MissionInfo missionInfoFromLocationSetById = m_missionsAtLocation.GetMissionInfoFromLocationSetById(base.CurrentSelectedId);
					base.EarthPinManager.CreateRegionPinsForCampaignMissions(m_missionsAtLocation, locationSetFromMissionInfoId, missionInfoFromLocationSetById, !m_setFocusAfterEarthTransition);
					m_setFocusAfterEarthTransition = false;
					EraExtensions.EEarthPin earthPinByEra = locationSetFromMissionInfoId.era.GetEarthPinByEra();
					BehaviourSingleton<EnvironmentBridgeMain>.Instance.EarthTransitionManager.SetEarthTransition(earthPinByEra, true);
				}
			}
		}

		protected override void SetupEarthPinConnection()
		{
			if (base.CurrentSelected != null)
			{
				LocationSet locationSetFromMissionInfoId = m_missionsAtLocation.GetLocationSetFromMissionInfoId(base.CurrentSelectedId);
				MissionInfo missionInfoFromLocationSetById = m_missionsAtLocation.GetMissionInfoFromLocationSetById(base.CurrentSelectedId);
				EarthPinBase usedPinByEarthPinLocaKey = base.EarthPinManager.GetUsedPinByEarthPinLocaKey(locationSetFromMissionInfoId.GetRegionDefinition(), missionInfoFromLocationSetById.regionAppearanceOption);
				usedPinByEarthPinLocaKey.PlayTween = (m_instantShowPin = !m_instantShowPin);
				usedPinByEarthPinLocaKey.IsSelected = true;
				base.CurrentSelected.SetEarthPin(m_scrollPanel, usedPinByEarthPinLocaKey);
			}
		}

		private void UpdateMissionMessage()
		{
			bool isVisible = false;
			MissionInfo currentSelectedMissionInfo = CurrentSelectedMissionInfo;
			if (currentSelectedMissionInfo != null)
			{
				bool flag = currentSelectedMissionInfo.HasCompletedMission();
				EntitlementHighlightDescription entitlementHighlightDescription = currentSelectedMissionInfo.GetEntitlementHighlightDescription();
				if (entitlementHighlightDescription != null && ((flag && entitlementHighlightDescription.ShowUnlock) || (!flag && entitlementHighlightDescription.ShowBeforeUnlock)))
				{
					if (m_isPlayingAnimation && m_completedMissionIndexToAnimate != base.CurrentSelectedIndex)
					{
						m_missionMessage.StopAnimationIfPlaying();
					}
					LocalStorage.IMissionProgressData missionProgressData = UbiCore.Utils.Singleton<LocalStorage>.Instance.GetMissionProgressData(currentSelectedMissionInfo.baseName);
					m_missionMessage.UpdateDateByEntitlement(entitlementHighlightDescription, missionProgressData != null && missionProgressData.WasCompletedShown, flag);
					isVisible = true;
				}
			}
			m_missionMessage.IsVisible = isVisible;
		}

		private void ClearMissionItemItems()
		{
			for (int i = 0; i < m_wrapperContainer.Children.Count; i++)
			{
				Transform transform = m_wrapperContainer.Children[i];
				UIMissionItemCampaign component = transform.GetComponent<UIMissionItemCampaign>();
				if (component != null)
				{
					component.Clear();
				}
			}
		}

		private void UpdateCampaignList()
		{
			if (!IsWaitingForAnimation)
			{
				m_missionList.Clear();
				string campaignId = Singleton<StateManager>.Instance.MissionSelectionManager.CampaignId;
				CampaignCatalog.Campaign campaignById = Singleton<StateManager>.Instance.TemplateManager.CampaignCatalog.GetCampaignById(campaignId);
				m_missionsAtLocation = campaignById.missions;
				campaignById.GetSortedMissionList(ref m_missionList);
				ValidateAnimation();
				ValidateLastSelected();
				m_wrapperContainer.ItemCount = m_missionList.Count;
				m_wrapperContainer.WrapScrollView.ResetPosition();
				UpdateMissionListFocus();
			}
		}

		private void UpdateMissionListFocus()
		{
			EventDelegate.Add(m_wrapperContainer.OnFocusFinishedEvent, OnFocusMissionFinished, true);
			int num = base.CurrentSelectedIndex;
			if (IsWaitingForAnimation)
			{
				int num2 = m_missionList.Count - 1;
				if (m_unlockedMissionIndexToAnimate >= 0)
				{
					num = m_unlockedMissionIndexToAnimate;
				}
				else if (num != num2)
				{
					num++;
				}
			}
			m_wrapperContainer.SetFocusOnChildByIndex(num);
		}

		private void UpdateButtonBySelectedItem()
		{
			if (!(base.CurrentSelected == null))
			{
				m_actionButtonBar.SetupButton(UIMissionActionButtonBar.EButtonType.Primary, s_exploreId, true);
			}
		}

		private void ValidateLastSelected()
		{
			if (IsWaitingForAnimation)
			{
				return;
			}
			bool flag = true;
			int num = 0;
			for (int i = 0; i < m_missionList.Count; i++)
			{
				if (base.CurrentSelectedId == m_missionList[i].baseName)
				{
					num = i;
					break;
				}
				if (m_missionList[i].IsUnlocked())
				{
					bool flag2 = m_missionList[i].HasCompletedMission();
					if (!flag2 || flag)
					{
						flag = flag2;
						num = i;
					}
				}
			}
			if (m_missionList.Count > 0)
			{
				base.CurrentSelectedId = m_missionList[num].baseName;
				base.CurrentSelectedIndex = num;
			}
		}

		private void ValidateAnimation()
		{
			m_completedMissionIndexToAnimate = GetCompletedMissionIndexToAnimate();
			m_unlockedMissionIndexToAnimate = GetUnlockedMissionIndexToAnimate();
			IsWaitingForAnimation = m_completedMissionIndexToAnimate >= 0 || m_unlockedMissionIndexToAnimate >= 0;
			if (IsWaitingForAnimation)
			{
				if (m_completedMissionIndexToAnimate == m_unlockedMissionIndexToAnimate)
				{
					m_completedMissionIndexToAnimate = -1;
				}
				base.CurrentSelectedIndex = ((m_completedMissionIndexToAnimate < 0) ? Mathf.Clamp(m_unlockedMissionIndexToAnimate - 1, 0, m_unlockedMissionIndexToAnimate) : m_completedMissionIndexToAnimate);
				base.CurrentSelectedId = m_missionList[base.CurrentSelectedIndex].baseName;
			}
		}

		private UIMissionItemCampaign GetChildByVirtualIndex(int _virtualIndex)
		{
			if (_virtualIndex < m_missionList.Count)
			{
				for (int i = 0; i < m_wrapperContainer.Children.Count; i++)
				{
					Transform transform = m_wrapperContainer.Children[i];
					UIMissionItemCampaign component = transform.GetComponent<UIMissionItemCampaign>();
					if (component != null && component.Index == _virtualIndex)
					{
						return component;
					}
				}
			}
			return null;
		}

		private int GetCompletedMissionIndexToAnimate()
		{
			for (int i = 0; i < m_missionList.Count; i++)
			{
				if (m_missionList[i].HasCompletedMission() && !m_missionList[i].HasCompletedAnimationBeenPlayed())
				{
					return i;
				}
			}
			return -1;
		}

		private int GetUnlockedMissionIndexToAnimate()
		{
			for (int i = 0; i < m_missionList.Count; i++)
			{
				if (m_missionList[i].IsUnlocked() && !m_missionList[i].HasUnlockAnimationBeenPlayed())
				{
					return i;
				}
			}
			return -1;
		}

		private void OnFocusMissionFinished()
		{
			if (!IsWaitingForAnimation)
			{
				return;
			}
			UnityEngine.Animation[] animations = new UnityEngine.Animation[0];
			if (m_completedMissionIndexToAnimate >= 0)
			{
				UIMissionItemCampaign childByVirtualIndex = GetChildByVirtualIndex(m_completedMissionIndexToAnimate);
				if (childByVirtualIndex != null)
				{
					animations = new UnityEngine.Animation[1] { childByVirtualIndex.CompleteMissionAnimation };
				}
			}
			m_completeMissionAnimationTween.SetAnimationList(animations, string.Empty);
			m_missionMessage.SetupAnimation(m_completedMissionIndexToAnimate >= 0);
			UnityEngine.Animation[] animations2 = new UnityEngine.Animation[0];
			if (m_unlockedMissionIndexToAnimate >= 0)
			{
				UIMissionItemCampaign childByVirtualIndex2 = GetChildByVirtualIndex(m_unlockedMissionIndexToAnimate);
				if (childByVirtualIndex2 != null)
				{
					animations2 = new UnityEngine.Animation[1] { childByVirtualIndex2.UnlockMissionAnimation };
				}
			}
			m_unlockMissionAnimationTween.SetAnimationList(animations2, string.Empty);
			StopAllCoroutines();
			StartCoroutine(OnPlayStatusUpdateAnimation());
		}

		private IEnumerator OnPlayStatusUpdateAnimation()
		{
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			while (!UIFlowHandler.CanStartMissionProgressionAnimation)
			{
				yield return wait;
			}
			m_isPlayingAnimation = false;
			m_mainAnimation.SampleAnimation(0f, "MissionCampaignMain");
			yield return wait;
			m_mainAnimation.Play("MissionCampaignMain");
			m_mainAnimation.Sample();
			if (m_completedMissionIndexToAnimate >= 0)
			{
				UbiCore.Utils.Singleton<LocalStorage>.Instance.SetMissionProgressCompleted(m_missionList[m_completedMissionIndexToAnimate].baseName, true);
			}
			if (m_unlockedMissionIndexToAnimate >= 0)
			{
				UbiCore.Utils.Singleton<LocalStorage>.Instance.SetMissionProgressUnlocked(m_missionList[m_unlockedMissionIndexToAnimate].baseName, true);
			}
			do
			{
				yield return wait;
			}
			while (m_mainAnimation.isPlaying || m_isPlayingAnimation);
			for (int i = 0; i < m_wrapperContainer.Children.Count; i++)
			{
				Transform childTrans = m_wrapperContainer.Children[i];
				UIMissionItemCampaign child = childTrans.GetComponent<UIMissionItemCampaign>();
				if (child != null)
				{
					child.UpdateMissionStatus();
				}
			}
			UpdateMissionMessage();
			UpdateButtonBySelectedItem();
			ValidateAnimation();
			if (IsWaitingForAnimation)
			{
				UpdateMissionListFocus();
			}
			else
			{
				base.CurrentSelectedId = string.Empty;
				ValidateLastSelected();
			}
			UIMissionItemCampaign childItem = GetChildByVirtualIndex(base.CurrentSelectedIndex);
			if (childItem != null)
			{
				if (base.CurrentSelected != null && base.CurrentSelected != childItem)
				{
					base.CurrentSelected.IsSelected = false;
				}
				base.CurrentSelected = childItem;
				if (base.CurrentSelected != null && !base.CurrentSelected.IsSelected)
				{
					base.CurrentSelected.IsSelected = true;
				}
			}
		}
	}
}
