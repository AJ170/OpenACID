using System;
using System.Collections;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.Mission.Mission;
using Assets.Scripts.Network;
using Assets.Scripts.UI.Behaviours;
using Assets.Scripts.UI.EnvironmentBridge;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.Ingame.Behaviours.Misc;
using Assets.Scripts.UI.Ingame.Behaviours.MissionResult;
using Assets.Scripts.UI.Loading;
using Assets.Scripts.UI.LoadoutScreen;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Generic;
using Assets.Scripts.UI.Screens.MissionScreen;
using Assets.Scripts.UI.Screens.MissionScreen.EarthPins;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.MissionManager;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using SharedBaseLib.Core;
using SharedBaseLib.System.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.MissionDetailsScreen
{
	public class MissionDetailsData : UIMissionSubMenuBase
	{
		private class WaitTimer
		{
			private float m_startTime;

			private float m_duration;

			public bool IsOff
			{
				get
				{
					return Time.unscaledTime - m_startTime > m_duration;
				}
			}

			public void Reset(float _seconds)
			{
				m_startTime = Time.unscaledTime;
				m_duration = _seconds;
			}
		}

		private const int NumMaxItemsRewardsInRow = 4;

		private const int ContinueButtonWaitForServerDisableTime = 5;

		[SerializeField]
		private UILabel m_MainGoal;

		[SerializeField]
		private UILabel m_MissionName;

		[SerializeField]
		private UILabel m_missionXPReward;

		[SerializeField]
		private UILabel m_challengeXPReward;

		[SerializeField]
		private UILabel m_LongDescription;

		[SerializeField]
		private UIExtendedLabel m_missionLevel;

		[SerializeField]
		private UIAlignmentBox[] m_lootInfoAlignmentBox;

		[SerializeField]
		private UIMissionRewardItemBehaviour[] m_missionRewardItems;

		[SerializeField]
		private UIMissionRewardItemBehaviour[] m_challengeRewardItems;

		[SerializeField]
		private UIObjectiveBehaviour m_challenges;

		[SerializeField]
		private UIObjectiveBehaviour m_modifiers;

		[SerializeField]
		private UIScrollView m_scrollview;

		[SerializeField]
		private UITable m_containerTable;

		[SerializeField]
		private UITable m_missionTable;

		[SerializeField]
		private UITable m_missionItemRewardsTable;

		[SerializeField]
		private UITable m_challengeItemRewardsTable;

		[SerializeField]
		private UISprite m_MissionIcon;

		[SerializeField]
		private UISprite m_MissionIconBackground;

		[SerializeField]
		private UIWidget m_loadAnimationContainer;

		[SerializeField]
		private BBTweenAlpha m_showAnimation;

		[SerializeField]
		private UIMissionMessage m_missionMessage;

		[SerializeField]
		private GameObject[] m_ChallengeGoalSeparator;

		[SerializeField]
		private AnimatedLineRenderer m_lineAnimation;

		[SerializeField]
		private UICheckboxBehaviour m_mainGoalCheckbox;

		[SerializeField]
		private UICheckboxBehaviour m_challengeGoalCheckbox;

		[SerializeField]
		private UIWidget m_itemPowWidget;

		[SerializeField]
		private UIExtendedLabel m_reccomendedItemPOWLabel;

		[SerializeField]
		private UIExtendedLabel m_assassinItemPOWLabel;

		private bool m_wasFromCampaignSubmenu;

		private int m_numMainRewards;

		private int m_numChallengeRewards;

		private int m_numberOfChallenges;

		private int m_numberOfModifiers;

		private int missionContainerPositioned;

		private int lootRepositionCount;

		private int challengesPositioned;

		private LocationSet m_locationSet;

		private LoadAnimationManager m_loadAnimMgr;

		private LoadAnimationManager.SLoadObjectInfo m_loadAnimInfo;

		private readonly WaitTimer m_timer = new WaitTimer();

		private UIRequestToken m_currentRequestToken;

		public MissionDetailsNavigation MissionDetailsNavigation;

		public bool HasCompletedMission { get; private set; }

		public bool HasCompletedMissionChallenge { get; private set; }

		public override MissionSelectionManager.EMissionMenu ListType
		{
			get
			{
				return (!m_wasFromCampaignSubmenu) ? MissionSelectionManager.EMissionMenu.ContractDetails : MissionSelectionManager.EMissionMenu.CampaignDetails;
			}
		}

		public override bool ShowBackButton
		{
			get
			{
				return true;
			}
		}

		private void OnLootInfContainerReposition()
		{
			lootRepositionCount--;
			if (lootRepositionCount == 0)
			{
				UITable missionItemRewardsTable = m_missionItemRewardsTable;
				missionItemRewardsTable.onReposition = (UITable.OnReposition)Delegate.Combine(missionItemRewardsTable.onReposition, new UITable.OnReposition(OnMissionRewardsContainerReposition));
				m_missionItemRewardsTable.repositionNow = true;
			}
		}

		private void OnMissionRewardsContainerReposition()
		{
			missionContainerPositioned--;
			if (missionContainerPositioned == 0)
			{
				UITable containerTable = m_containerTable;
				containerTable.onReposition = (UITable.OnReposition)Delegate.Combine(containerTable.onReposition, new UITable.OnReposition(OnMainContainerRepositon));
				m_containerTable.repositionNow = true;
			}
		}

		private void OnMainContainerRepositon()
		{
			m_scrollview.deactivateDrag = false;
			m_scrollview.ResetPosition();
		}

		private void RepositionTable()
		{
			challengesPositioned--;
			if (challengesPositioned <= 0)
			{
				UITable missionTable = m_missionTable;
				missionTable.onReposition = (UITable.OnReposition)Delegate.Combine(missionTable.onReposition, new UITable.OnReposition(OnMissionRewardsContainerReposition));
				m_missionTable.repositionNow = true;
			}
		}

		private void OnMissionDataRetrieved(IAsyncResult _asyncResult)
		{
			m_actionButtonBar.GetButtonByType(UIMissionActionButtonBar.EButtonType.Primary).DisabledDelay = 0f;
			if (_asyncResult.IsError())
			{
				Singleton<UIScreenManager>.Instance.ValidateMissionListOnError();
			}
			else if (base.gameObject.activeInHierarchy)
			{
				GetMissionInfoResponse getMissionInfoResponse = ((IAsyncDataResult<GetMissionInfoResponse>)_asyncResult).Result();
				MissionSelectionManager missionSelectionManager = Singleton<StateManager>.Instance.MissionSelectionManager;
				if (missionSelectionManager.IsFromCampaignMenu)
				{
					missionSelectionManager.CampaignHirelingReward = (int)getMissionInfoResponse.missionRewardPreview.scFromHireling;
				}
				else
				{
					missionSelectionManager.ContractHirelingReward = (int)getMissionInfoResponse.missionRewardPreview.scFromHireling;
				}
				bool isElite = !missionSelectionManager.IsSelectedMissionCampaign && missionSelectionManager.ContractMission.eliteMission;
				MissionMetaDataManager.Instance.SetMissionGoals(getMissionInfoResponse.missionGoals, isElite);
				SetContent(getMissionInfoResponse);
				m_loadAnimMgr.RemoveLoadAnim(m_loadAnimInfo);
				m_showAnimation.StartTweenPlayForward();
			}
		}

		public override void OnInit(EarthPinManager _earthPinManager)
		{
			base.OnInit(_earthPinManager);
			m_loadAnimMgr = LoadAnimationManager.CreateNewInstance(base.transform);
			m_loadAnimInfo = new LoadAnimationManager.SLoadObjectInfo(m_loadAnimationContainer);
		}

		public override void OnActivate()
		{
			base.OnActivate();
			UIExtendedButton buttonByType = m_actionButtonBar.GetButtonByType(UIMissionActionButtonBar.EButtonType.Primary);
			buttonByType.DisabledDelay = 5f;
			MissionDetailsNavigation.Show();
			m_wasFromCampaignSubmenu = Singleton<StateManager>.Instance.MissionSelectionManager.IsFromCampaignMenu;
			bool isVisible = false;
			if (m_wasFromCampaignSubmenu)
			{
				MissionInfo campaignMission = Singleton<StateManager>.Instance.MissionSelectionManager.CampaignMission;
				bool flag = campaignMission.HasCompletedMission();
				EntitlementHighlightDescription entitlementHighlightDescription = campaignMission.GetEntitlementHighlightDescription();
				if (entitlementHighlightDescription != null && ((flag && entitlementHighlightDescription.ShowUnlock) || (!flag && entitlementHighlightDescription.ShowBeforeUnlock)))
				{
					isVisible = true;
				}
			}
			m_missionMessage.IsVisible = isVisible;
			ClearAndRequestData();
			UIMissionHelper.OnMissionRequested += OnMissionRequested;
			EventDelegate.Add(Singleton<StateManager>.Instance.ProfileManager.onNewModuleActivated, OnNewModuleActivated);
			m_actionButtonBar.SetupButton(UIMissionActionButtonBar.EButtonType.Primary, 1198, true);
			m_actionButtonBar.SetupButton(UIMissionActionButtonBar.EButtonType.Secondary, 1520, false);
			CreateEarthPins();
			BehaviourSingleton<EnvironmentBridgeMain>.Instance.EarthTransitionManager.SetEarthTransition(m_locationSet.era.GetEarthPinByEra(), true);
		}

		private void CreateEarthPins()
		{
			if (Singleton<StateManager>.Instance.MissionSelectionManager.IsSelectedMissionCampaign)
			{
				CampaignCatalog.Campaign campaignById = Singleton<StateManager>.Instance.TemplateManager.CampaignCatalog.GetCampaignById(Singleton<StateManager>.Instance.MissionSelectionManager.CampaignId);
				m_locationSet = Singleton<StateManager>.Instance.MissionSelectionManager.CurrentMissionLocationSet;
				if (m_locationSet != null)
				{
					string missionId = Singleton<StateManager>.Instance.MissionSelectionManager.MissionId;
					MissionInfo missionById = campaignById.GetMissionById(missionId);
					base.EarthPinManager.CreateRegionPinsForCampaignMissions(campaignById.missions, m_locationSet, missionById, false);
				}
			}
			else
			{
				MissionCollection.RankMissions currrentRankMissions = Singleton<StateManager>.Instance.ProfileManager.CurrrentRankMissions;
				List<AcierProtocol.Mission> _missionList = new List<AcierProtocol.Mission>();
				currrentRankMissions.GetMissionsByHeroicRank(ref _missionList);
				int num = -1;
				if (_missionList != null)
				{
					for (int i = 0; i < _missionList.Count; i++)
					{
						if (_missionList[i].id == Singleton<StateManager>.Instance.MissionSelectionManager.MissionId)
						{
							num = i;
							break;
						}
					}
					m_locationSet = _missionList[num].locationSet;
					base.EarthPinManager.CreateRegionPinsForContractMissions(_missionList, num, false);
				}
			}
			m_lineAnimation.StopLineAnimation();
		}

		public override void OnDeactivate()
		{
			base.OnDeactivate();
			MissionDetailsNavigation.Hide();
			m_missionMessage.IsVisible = false;
			UIMissionHelper.OnMissionRequested -= OnMissionRequested;
			EventDelegate.Remove(Singleton<StateManager>.Instance.ProfileManager.onNewModuleActivated, OnNewModuleActivated);
			UITable missionItemRewardsTable = m_missionItemRewardsTable;
			missionItemRewardsTable.onReposition = (UITable.OnReposition)Delegate.Remove(missionItemRewardsTable.onReposition, new UITable.OnReposition(OnMissionRewardsContainerReposition));
			UITable missionTable = m_missionTable;
			missionTable.onReposition = (UITable.OnReposition)Delegate.Remove(missionTable.onReposition, new UITable.OnReposition(OnMissionRewardsContainerReposition));
			UIExtendedButton buttonByType = m_actionButtonBar.GetButtonByType(UIMissionActionButtonBar.EButtonType.Primary);
			if (buttonByType != null)
			{
				buttonByType.DisabledDelay = 0f;
			}
			if (m_currentRequestToken != null)
			{
				m_currentRequestToken.Abandoned = true;
				m_currentRequestToken = null;
			}
		}

		private void OnMissionRequested(bool _errorOccured)
		{
			m_actionButtonBar.IsUpdating = _errorOccured;
		}

		protected override void SetupEarthPinConnection()
		{
			if (m_locationSet != null)
			{
				string text = string.Empty;
				RegionDefinition regionDefinition = m_locationSet.GetRegionDefinition();
				if (Singleton<StateManager>.Instance.MissionSelectionManager.IsSelectedMissionCampaign)
				{
					CampaignCatalog.Campaign campaignById = Singleton<StateManager>.Instance.TemplateManager.CampaignCatalog.GetCampaignById(Singleton<StateManager>.Instance.MissionSelectionManager.CampaignId);
					string missionId = Singleton<StateManager>.Instance.MissionSelectionManager.MissionId;
					MissionInfo missionById = campaignById.GetMissionById(missionId);
					text = missionById.regionAppearanceOption;
				}
				EarthPinBase usedPinByEarthPinLocaKey = base.EarthPinManager.GetUsedPinByEarthPinLocaKey(regionDefinition, text);
				if (usedPinByEarthPinLocaKey != null)
				{
					usedPinByEarthPinLocaKey.PlayTween = false;
					usedPinByEarthPinLocaKey.SetTargetRegion(regionDefinition, text);
					usedPinByEarthPinLocaKey.IsSelected = true;
					m_lineAnimation.StartLineAnimation(null, usedPinByEarthPinLocaKey);
				}
			}
		}

		protected override void OnActionButtonClicked(UIMissionActionButtonBar.EButtonType _buttontype)
		{
			if (_buttontype != 0 || !m_actionButtonBar.GetButtonByType(_buttontype).isEnabled)
			{
				return;
			}
			bool flag = false;
			if (EntitlementExtensions.IsHirelingUnlocked())
			{
				flag = true;
				if (m_wasFromCampaignSubmenu && Singleton<StateManager>.Instance.MissionSelectionManager.CampaignMission.IsTutorialMission())
				{
					flag = false;
					UIAssasinId firstHireling = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin.GetFirstHireling();
					Singleton<StateManager>.Instance.ProfileManager.LastSelectedHirelingId = firstHireling;
				}
			}
			if (flag)
			{
				GoToHirelings();
			}
			else
			{
				UIMissionHelper.StartCurrentSelectedMissionWithTutorialPopup();
			}
		}

		private static void GoToHirelings()
		{
			Singleton<UIScreenManager>.Instance.ChangeScreen(EScreenID.MissionHireling, new UITweenHandle(null, null));
		}

		public override bool OnTapBack()
		{
			if (m_wasFromCampaignSubmenu)
			{
				Singleton<StateManager>.Instance.MissionSelectionManager.SubMenuType = MissionSelectionManager.EMissionMenu.CampaignMissions;
			}
			else
			{
				Singleton<StateManager>.Instance.MissionSelectionManager.SubMenuType = MissionSelectionManager.EMissionMenu.ContractMissions;
			}
			return true;
		}

		private void OnNewModuleActivated()
		{
			ClearAndRequestData();
		}

		private void ClearAndRequestData()
		{
			ClearData();
			m_loadAnimMgr.AddLoadAnim(m_loadAnimInfo, 0);
			m_showAnimation.SetTweenToBeginning();
			m_scrollview.deactivateDrag = true;
			if (base.gameObject.activeSelf)
			{
				if (m_timer.IsOff)
				{
					StartCoroutine(RequestDataAsync());
				}
				else
				{
					m_timer.Reset(0.3f);
				}
			}
		}

		private IEnumerator RequestDataAsync()
		{
			WaitForSeconds wait = new WaitForSeconds(0.05f);
			while (!m_timer.IsOff)
			{
				yield return wait;
			}
			m_timer.Reset(0.3f);
			RequestData();
		}

		private void RequestData()
		{
			string missionId = Singleton<StateManager>.Instance.MissionSelectionManager.MissionId;
			string hirelingId = string.Empty;
			UIAssasinId assassinId = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassinId;
			string campaignId = Singleton<StateManager>.Instance.MissionSelectionManager.CurrentSelectedMissionCampaignId;
			int rank = Singleton<StateManager>.Instance.MissionSelectionManager.HeroicMissionRank;
			if (m_currentRequestToken != null)
			{
				m_currentRequestToken.Abandoned = true;
				m_currentRequestToken = null;
			}
			m_currentRequestToken = UINetworkAdaptor.NetworkRequest(delegate(AsyncCallback _asyncResult)
			{
				Singleton<StateManager>.Instance.ProfileManager.GetMissionInfo(missionId, assassinId, hirelingId, campaignId, rank, _asyncResult);
			}, null, OnMissionDataRetrieved);
			UpdateMissionText();
		}

		private void ClearData()
		{
			m_MissionName.gameObject.SetActive(false);
			m_missionLevel.gameObject.SetActive(false);
			m_MainGoal.gameObject.SetActive(false);
			m_challenges.SetActive(false);
			m_modifiers.SetActive(false);
			m_itemPowWidget.gameObject.SetActive(false);
			UIAlignmentBox[] lootInfoAlignmentBox = m_lootInfoAlignmentBox;
			foreach (UIAlignmentBox uIAlignmentBox in lootInfoAlignmentBox)
			{
				uIAlignmentBox.gameObject.SetActive(false);
			}
			m_LongDescription.transform.parent.gameObject.SetActive(false);
		}

		private void SetContent(GetMissionInfoResponse _response)
		{
			MissionReward missionRewardPreview = _response.missionRewardPreview;
			missionContainerPositioned = 0;
			UITable missionItemRewardsTable = m_missionItemRewardsTable;
			missionItemRewardsTable.onReposition = (UITable.OnReposition)Delegate.Remove(missionItemRewardsTable.onReposition, new UITable.OnReposition(OnMissionRewardsContainerReposition));
			UpdateMission();
			m_missionXPReward.text = LocalizationManager.Instance.GetInteger(missionRewardPreview.xpWithoutChallenge);
			m_missionXPReward.transform.parent.gameObject.SetActive(missionRewardPreview.xpWithoutChallenge > 0);
			long num = missionRewardPreview.finalXp - missionRewardPreview.xpWithoutChallenge;
			m_challengeXPReward.text = LocalizationManager.Instance.GetInteger(num);
			m_challengeXPReward.transform.parent.gameObject.SetActive(num > 0);
			UpdateRewardItemsDisplay(_response);
		}

		private void UpdateRewardItemsDisplay(GetMissionInfoResponse _response)
		{
			MissionGoals.MissionGoal[] missionGoals = _response.missionGoals.missionGoals;
			MissionReward missionRewardPreview = _response.missionRewardPreview;
			m_numMainRewards = 0;
			m_numChallengeRewards = 0;
			long scAmount = missionRewardPreview.scWithoutChallenge - missionRewardPreview.scFromPickpocketing - missionRewardPreview.scFromHireling;
			long htAmount = missionRewardPreview.htWithoutChallenge - missionRewardPreview.htFromPickpocketing - missionRewardPreview.htFromHireling;
			long scAmount2 = missionRewardPreview.finalSc - missionRewardPreview.scWithoutChallenge;
			long htAmount2 = missionRewardPreview.finalHt - missionRewardPreview.htWithoutChallenge;
			if (m_wasFromCampaignSubmenu)
			{
				StoryMissionRewardMapping.Entry missionReward = Singleton<StateManager>.Instance.MissionSelectionManager.CampaignMission.GetMissionReward();
				if (missionReward != null)
				{
					CollectItemRewards(missionReward.GetCurrentRankNormalRewards(), m_missionRewardItems, scAmount, htAmount, ref m_numMainRewards, HasCompletedMission);
					CollectItemRewards(missionReward.GetCurrentRankChallengesRewards(), m_challengeRewardItems, scAmount2, htAmount2, ref m_numChallengeRewards, HasCompletedMissionChallenge);
				}
			}
			else
			{
				bool eliteMission = Singleton<StateManager>.Instance.MissionSelectionManager.ContractMission.eliteMission;
				int heroicMissionRank = Singleton<StateManager>.Instance.MissionSelectionManager.HeroicMissionRank;
				Quality.EQuality minQualityForHeroicRank = Singleton<StateManager>.Instance.TemplateManager.GetMinQualityForHeroicRank(heroicMissionRank, eliteMission);
				BBList<LootFilter> bBList = new BBList<LootFilter>(Singleton<StateManager>.Instance.MissionSelectionManager.ContractMission.missionKind.GetLootFiltersForContractMissionKind());
				CollectContractItemRewards(bBList, m_missionRewardItems, scAmount, htAmount, ref m_numMainRewards, minQualityForHeroicRank);
				bBList.Clear();
				for (int i = 0; i < missionGoals.Length; i++)
				{
					if (missionGoals[i].goalType == EMissionGoal.Challenge_EMissionGoal && missionGoals[i].lootFilter != null && !missionGoals[i].hidden && !missionGoals[i].lootFilter.IsUniqeResourceType())
					{
						bBList.Add(missionGoals[i].lootFilter);
					}
				}
				CollectContractItemRewards(bBList, m_challengeRewardItems, scAmount2, htAmount2, ref m_numChallengeRewards, minQualityForHeroicRank);
			}
			m_missionItemRewardsTable.gameObject.SetActive(m_numMainRewards > 0);
			m_lootInfoAlignmentBox[0].gameObject.SetActive(m_numMainRewards > 0);
			m_challengeItemRewardsTable.gameObject.SetActive(m_numChallengeRewards > 0);
			m_lootInfoAlignmentBox[1].gameObject.SetActive(m_numChallengeRewards > 0);
			if (m_numMainRewards > 0 || m_numChallengeRewards > 0)
			{
				missionContainerPositioned++;
			}
			for (int j = 0; j < m_lootInfoAlignmentBox.Length; j++)
			{
				if (m_lootInfoAlignmentBox[j].gameObject.activeSelf)
				{
					lootRepositionCount++;
					EventDelegate.Remove(m_lootInfoAlignmentBox[j].OnReposition, OnLootInfContainerReposition);
					EventDelegate.Add(m_lootInfoAlignmentBox[j].OnReposition, OnLootInfContainerReposition, true);
					m_lootInfoAlignmentBox[j].RepositionNow = true;
				}
			}
		}

		private void CollectContractItemRewards(BBList<LootFilter> _rewardLootFilters, UIMissionRewardItemBehaviour[] _rewardSlots, long _scAmount, long _htAmount, ref int _rewardcount, Quality.EQuality _quality)
		{
			_rewardcount = 0;
			CreateResourceReward(_scAmount, EResourceType.SoftCurrency, _rewardSlots, ref _rewardcount);
			CreateResourceReward(_htAmount, EResourceType.SocialCurrency, _rewardSlots, ref _rewardcount);
			for (int i = 0; i < _rewardLootFilters.Count; i++)
			{
				if (_rewardcount < _rewardSlots.Length)
				{
					_rewardSlots[_rewardcount].UpdateRewardByGameID(_rewardLootFilters[i].GetItemDefPreview(_quality), _quality, _rewardLootFilters[i]);
					_rewardSlots[_rewardcount].SetVisibility(true);
					_rewardcount++;
				}
			}
			for (int j = _rewardcount; j < _rewardSlots.Length; j++)
			{
				_rewardSlots[j].SetVisibility(false);
			}
		}

		private void CollectItemRewards(StoryMissionRewardMapping.Entry.RankEntry _rewards, UIMissionRewardItemBehaviour[] _rewardSlots, long _scAmount, long _htAmount, ref int _rewardcount, bool _hasCompleted)
		{
			_rewardcount = 0;
			CreateResourceReward(_scAmount, EResourceType.SoftCurrency, _rewardSlots, ref _rewardcount);
			CreateResourceReward(_htAmount, EResourceType.SocialCurrency, _rewardSlots, ref _rewardcount);
			if (!_hasCompleted)
			{
				for (int i = 0; i < _rewards.lootTemplates.Length; i++)
				{
					if (_rewards.lootTemplates[i].entitlementTemplate == null && _rewardcount < _rewardSlots.Length)
					{
						_rewardSlots[_rewardcount].UpdateRewardByTemplate(_rewards.lootTemplates[i]);
						_rewardSlots[_rewardcount].SetVisibility(true);
						_rewardcount++;
					}
				}
			}
			for (int j = _rewardcount; j < _rewardSlots.Length; j++)
			{
				_rewardSlots[j].SetVisibility(false);
			}
		}

		private void CreateResourceReward(long _amount, EResourceType _resourceType, UIMissionRewardItemBehaviour[] _rewardSlots, ref int _rewardCount)
		{
			if (_amount > 0 && _rewardCount < _rewardSlots.Length)
			{
				_rewardSlots[_rewardCount].UpdateRewardByResource(new Resource
				{
					amount = _amount,
					type = _resourceType
				});
				_rewardSlots[_rewardCount].SetVisibility(true);
				_rewardCount++;
			}
		}

		private void UpdateMissionText()
		{
			string empty = string.Empty;
			string empty2 = string.Empty;
			int num = 0;
			int num2 = 0;
			if (Singleton<StateManager>.Instance.MissionSelectionManager.IsSelectedMissionCampaign)
			{
				MissionInfo campaignMission = Singleton<StateManager>.Instance.MissionSelectionManager.CampaignMission;
				num = campaignMission.difficulty;
				empty = campaignMission.LocaName();
				empty2 = campaignMission.LocaMissionFlavorText();
				UIMissionUtil.UpdateMissionIconByMissionObjectiveType(m_MissionIcon, campaignMission.objectiveBaseType, false, m_MissionIconBackground);
				num2 = campaignMission.rank;
			}
			else
			{
				AcierProtocol.Mission contractMission = Singleton<StateManager>.Instance.MissionSelectionManager.ContractMission;
				num2 = contractMission.rank;
				num = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin.level;
				for (int i = 0; i < Singleton<StateManager>.Instance.TemplateManager.HeroicRankMapping.Length; i++)
				{
					HeroicRankMapping.Entry entry = Singleton<StateManager>.Instance.TemplateManager.HeroicRankMapping[i];
					if (entry.rankIdx == contractMission.rank)
					{
						if (num > entry.randomMissionsScalingLevelUpper)
						{
							num = entry.randomMissionsScalingLevelUpper;
						}
						else if (num < entry.randomMissionsScalingLevelLower)
						{
							num = entry.randomMissionsScalingLevelLower;
						}
						break;
					}
				}
				RegionDefinition regionDefinition = contractMission.GetRegionDefinition();
				empty = contractMission.LocaObjectiveLang(regionDefinition);
				MissionSelectionManager missionSelectionManager = Singleton<StateManager>.Instance.MissionSelectionManager;
				bool flag = !missionSelectionManager.IsSelectedMissionCampaign && missionSelectionManager.ContractMission.eliteMission;
				if (flag)
				{
					empty = LocalizationManager.Instance.Get(8029, new StringMapper { { "elite_mission_name", empty } });
				}
				empty2 = contractMission.LocaMissionFlavorText(regionDefinition);
				UIMissionUtil.UpdateMissionIconByMissionObjectiveType(m_MissionIcon, contractMission.missionKind, flag, m_MissionIconBackground);
			}
			m_MissionName.gameObject.SetActive(!string.IsNullOrEmpty(empty));
			m_MissionName.text = empty;
			m_missionLevel.gameObject.SetActive(num > 0);
			m_missionLevel.SetLocalizationKey(2336, new StringMapper { { "Unlock_Level", num } });
			m_LongDescription.text = empty2;
			m_LongDescription.transform.parent.gameObject.SetActive(!string.IsNullOrEmpty(empty2));
			int num3 = (int)UIUtil.CalculateRecommendedItemPOW(num, num2);
			m_reccomendedItemPOWLabel.text = LocalizationManager.Instance.GetInteger(num3);
			int num4 = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin.TotalItemPower();
			m_assassinItemPOWLabel.text = LocalizationManager.Instance.GetInteger(num4);
			m_itemPowWidget.gameObject.SetActive(true);
		}

		private void UpdateMission()
		{
			UIMissionGoals missionGoals = MissionMetaDataManager.Instance.MissionGoals;
			m_numberOfChallenges = ((missionGoals.Challenges != null) ? missionGoals.Challenges.Where((UIMissionGoals.GoalData x) => !x.Hidden).Count() : 0);
			m_numberOfModifiers = ((missionGoals.AdditionalObjectives != null) ? missionGoals.AdditionalObjectives.Where((UIMissionGoals.GoalData x) => !x.Hidden).Count : 0);
			List<UIMissionGoals.GoalData> list = missionGoals.Objectives.Where((UIMissionGoals.GoalData x) => !x.Hidden);
			m_MainGoal.text = ((list.Count <= 0) ? "<animus error>" : list[0].Name);
			UpdateChallengesAndRequirements();
		}

		private void UpdateChallengesAndRequirements()
		{
			bool flag = m_numberOfChallenges > 0;
			bool flag2 = m_numberOfModifiers > 0;
			m_challenges.SetActive(flag);
			for (int i = 0; i < m_ChallengeGoalSeparator.Length; i++)
			{
				m_ChallengeGoalSeparator[i].SetActive(flag);
			}
			m_modifiers.SetActive(flag2);
			m_MainGoal.gameObject.SetActive(true);
			challengesPositioned = 0;
			UITable missionTable = m_missionTable;
			missionTable.onReposition = (UITable.OnReposition)Delegate.Remove(missionTable.onReposition, new UITable.OnReposition(OnMissionRewardsContainerReposition));
			UIMissionGoals missionGoals = MissionMetaDataManager.Instance.MissionGoals;
			if (flag)
			{
				challengesPositioned++;
				m_challenges.SetData(missionGoals.Challenges.ToArray(), RepositionTable, false);
			}
			if (flag2)
			{
				challengesPositioned++;
				m_modifiers.SetData(missionGoals.AdditionalObjectives.ToArray(), RepositionTable, false, true);
			}
			HasCompletedMission = false;
			HasCompletedMissionChallenge = false;
			if (Singleton<StateManager>.Instance.MissionSelectionManager.IsSelectedMissionCampaign)
			{
				StoryMissionRewardMapping.Entry missionReward = Singleton<StateManager>.Instance.MissionSelectionManager.CampaignMission.GetMissionReward();
				if (missionReward != null)
				{
					HasCompletedMission = missionReward.GetCurrentRankNormalRewards().lootTemplates.HasEntitlementsFomLootTemplates();
					HasCompletedMissionChallenge = missionReward.GetCurrentRankChallengesRewards().lootTemplates.HasEntitlementsFomLootTemplates();
				}
			}
			m_mainGoalCheckbox.Checked = (HasCompletedMission ? UICheckboxBehaviour.ECheckedState.Success : UICheckboxBehaviour.ECheckedState.None);
			m_challengeGoalCheckbox.Checked = (HasCompletedMissionChallenge ? UICheckboxBehaviour.ECheckedState.Success : UICheckboxBehaviour.ECheckedState.None);
			if (!flag && !flag2)
			{
				missionContainerPositioned++;
				RepositionTable();
			}
		}

		private void GetChallengeLoots(MissionGoalLoot[] _missionGoalLoot)
		{
			if (_missionGoalLoot == null)
			{
				return;
			}
			foreach (MissionGoalLoot missionGoalLoot in _missionGoalLoot)
			{
				if (missionGoalLoot.elements != null)
				{
					GetInventoryElementRewards(missionGoalLoot.elements.elements, m_challengeRewardItems, ref m_numMainRewards);
				}
				GetResourceRewards(missionGoalLoot.resources, m_challengeRewardItems, ref m_numMainRewards);
			}
		}

		private void GetInventoryElementRewards(InventoryElement[] _inventoryElements, UIMissionRewardItemBehaviour[] rewardBehArr, ref int _itemsAdded)
		{
			if (_inventoryElements != null)
			{
				for (int i = 0; i < _inventoryElements.Length; i++)
				{
					if (_itemsAdded < rewardBehArr.Length)
					{
						UIMissionRewardItemBehaviour uIMissionRewardItemBehaviour = rewardBehArr[_itemsAdded];
						uIMissionRewardItemBehaviour.UpdateRewardByItem(_inventoryElements[i]);
						_itemsAdded++;
					}
				}
			}
			for (int j = _itemsAdded; j < rewardBehArr.Length; j++)
			{
				rewardBehArr[j].SetVisibility(false);
			}
		}

		private void GetResourceRewards(ResourceFromTemplate[] _resourceTemplates, UIMissionRewardItemBehaviour[] rewardBehArr, ref int _itemsAdded)
		{
			if (_resourceTemplates != null)
			{
				for (int i = 0; i < _resourceTemplates.Length; i++)
				{
					if (_itemsAdded < rewardBehArr.Length)
					{
						UIMissionRewardItemBehaviour uIMissionRewardItemBehaviour = rewardBehArr[_itemsAdded];
						uIMissionRewardItemBehaviour.UpdateRewardByResource(_resourceTemplates[i].resource);
						_itemsAdded++;
					}
				}
			}
			for (int j = _itemsAdded; j < rewardBehArr.Length; j++)
			{
				rewardBehArr[j].SetVisibility(false);
			}
		}
	}
}
