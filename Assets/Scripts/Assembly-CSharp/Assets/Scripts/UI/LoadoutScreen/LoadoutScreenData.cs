using System;
using System.Collections;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.Mission;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Network;
using Assets.Scripts.UI.Behaviours;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.Loading;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.MissionManager;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using Database;
using SharedBaseLib.Core;
using SharedBaseLib.System.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.LoadoutScreen
{
	public class LoadoutScreenData : UIScreenChild
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

		private static readonly StringId s_MissionLockedId = new StringId(6112);

		[SerializeField]
		private UILabel m_MainGoal;

		[SerializeField]
		private UILabel m_MissionName;

		[SerializeField]
		private UILabel m_xpReward;

		[SerializeField]
		private UILabel m_softCurrencyReward;

		[SerializeField]
		private UIAlignmentBox m_xpAlignmentBox;

		[SerializeField]
		private UIAlignmentBox m_softAlignmentBox;

		[SerializeField]
		private UIAlignmentBox m_lootInfoAlignmentBox;

		[SerializeField]
		private UIExtendedButton m_startButton;

		[SerializeField]
		private UIObjectiveBehaviour m_challenges;

		[SerializeField]
		private UIObjectiveBehaviour m_modifiers;

		[SerializeField]
		private UITable m_missionTable;

		[SerializeField]
		private UILabel m_LongDescription;

		[SerializeField]
		private UISprite m_MissionIcon;

		[SerializeField]
		private UISprite m_MissionIconBackground;

		[SerializeField]
		private UITexture m_DistrictBanner;

		[SerializeField]
		private UISprite[] DifficultyIndicaters;

		[SerializeField]
		private UIExtendedLabel m_difficultyLabel;

		[SerializeField]
		private UIAssassinSelectionHelper m_assassinSelection;

		[SerializeField]
		private UIWidget m_loadAnimationContainer;

		public LoadoutScreenNavigation LoadoutScreenNavigation;

		private int m_numberOfChallenges;

		private int m_numberOfModifiers;

		private bool m_isElite;

		private LoadAnimationManager m_loadAnimMgr;

		private LoadAnimationManager.SLoadObjectInfo m_loadAnimInfo;

		private readonly WaitTimer m_timer = new WaitTimer();

		private UIRequestToken m_currentRequestToken;

		protected override void OnScreenInit()
		{
			m_loadAnimMgr = LoadAnimationManager.CreateNewInstance(base.transform);
			m_difficultyLabel.LocalizationKey = 868;
			EventDelegate.Add(m_screen.onActivate, OnScreenActivate);
			EventDelegate.Add(m_screen.onDeActivate, OnScreenDeActivate);
			EventDelegate.Add(m_startButton.onClick, OnStartClick);
			m_startButton.AllowDisabledClick = true;
			m_loadAnimInfo = new LoadAnimationManager.SLoadObjectInfo(m_loadAnimationContainer);
		}

		protected override void OnScreenDestroy()
		{
			if (m_screen != null)
			{
				EventDelegate.Remove(m_screen.onActivate, OnScreenActivate);
				EventDelegate.Remove(m_screen.onDeActivate, OnScreenDeActivate);
			}
			EventDelegate.Remove(m_startButton.onClick, OnStartClick);
		}

		private void OnStartClick()
		{
			if (EntitlementExtensions.IsHirelingUnlocked())
			{
				Singleton<UIScreenManager>.Instance.ChangeScreen(EScreenID.MissionHireling, new UITweenHandle(null, null));
			}
			else
			{
				UIMissionHelper.StartCurrentSelectedMission();
			}
		}

		private void OnScreenActivate()
		{
			LoadoutScreenNavigation.ResetNow();
			UpdateData();
			Singleton<StateManager>.Instance.MissionSelectionManager.OnMissionStateChanged += new SimpleDelegate.CallbackFunction(OnMissionStateChanged);
			EventDelegate.Add(Singleton<StateManager>.Instance.ProfileManager.onNewModuleActivated, OnNewModuleActivated);
			Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.OnSelectionChanged += OnAssassinChange;
		}

		private void OnScreenDeActivate()
		{
			LoadoutScreenNavigation.ResetNow();
			Singleton<StateManager>.Instance.MissionSelectionManager.OnMissionStateChanged -= new SimpleDelegate.CallbackFunction(OnMissionStateChanged);
			Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.OnSelectionChanged -= OnAssassinChange;
			EventDelegate.Remove(Singleton<StateManager>.Instance.ProfileManager.onNewModuleActivated, OnNewModuleActivated);
			if (m_currentRequestToken != null)
			{
				m_currentRequestToken.Abandoned = true;
				m_currentRequestToken = null;
			}
		}

		private void OnMissionStateChanged(object[] __params)
		{
			UpdateData();
		}

		private void OnNewModuleActivated()
		{
			UpdateData();
		}

		private void UpdateData()
		{
			LoadoutScreenNavigation.Hide(ClearAndRequestData);
		}

		private void ClearAndRequestData()
		{
			ClearData();
			m_loadAnimMgr.AddLoadAnim(m_loadAnimInfo, 0);
			if (m_timer.IsOff)
			{
				StartCoroutine(RequestDataAsync());
			}
			else
			{
				m_timer.Reset(0.3f);
			}
		}

		private IEnumerator RequestDataAsync()
		{
			WaitForSeconds wait = new WaitForSeconds(0.05f);
			m_timer.Reset(0.3f);
			while (!m_timer.IsOff)
			{
				yield return wait;
			}
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
		}

		private void ClearData()
		{
			bool flag = CanPlayCurrentMission();
			m_MainGoal.gameObject.SetActive(false);
			m_challenges.SetActive(false);
			m_modifiers.SetActive(false);
			Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.ShowAsValidLevel(flag);
			m_startButton.isEnabled = flag;
		}

		private bool CanPlayCurrentMission()
		{
			return true;
		}

		private void OnMissionDataRetrieved(IAsyncResult _asyncResult)
		{
			if (base.gameObject.activeInHierarchy && !_asyncResult.IsError())
			{
				GetMissionInfoResponse getMissionInfoResponse = ((IAsyncDataResult<GetMissionInfoResponse>)_asyncResult).Result();
				MissionSelectionManager missionSelectionManager = Singleton<StateManager>.Instance.MissionSelectionManager;
				m_isElite = !missionSelectionManager.IsSelectedMissionCampaign && missionSelectionManager.ContractMission.eliteMission;
				MissionMetaDataManager.Instance.SetMissionGoals(getMissionInfoResponse.missionGoals, m_isElite);
				SetContent(getMissionInfoResponse);
				LoadoutScreenNavigation.Show(OnDataVisible);
				m_loadAnimMgr.RemoveLoadAnim(m_loadAnimInfo);
			}
		}

		private void OnDataVisible()
		{
			CheckMissionLockStatus();
		}

		private void CheckMissionLockStatus()
		{
			if (!CanPlayCurrentMission())
			{
				ShowOnScreenMessage();
			}
		}

		private void ShowOnScreenMessage()
		{
			UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(s_MissionLockedId));
		}

		private void SetContent(GetMissionInfoResponse _response)
		{
			UpdateMission();
			MissionReward missionRewardPreview = _response.missionRewardPreview;
			m_xpReward.text = LocalizationManager.Instance.GetInteger(missionRewardPreview.finalXp);
			m_softCurrencyReward.text = LocalizationManager.Instance.GetInteger(missionRewardPreview.finalSc);
			EventDelegate.Remove(m_lootInfoAlignmentBox.OnReposition, OnLootInfContainerReposition);
			EventDelegate.Add(m_lootInfoAlignmentBox.OnReposition, OnLootInfContainerReposition, true);
			m_lootInfoAlignmentBox.RepositionNow = true;
		}

		private void OnLootInfContainerReposition()
		{
			m_xpAlignmentBox.RepositionNow = true;
			m_softAlignmentBox.RepositionNow = true;
		}

		private void OnAssassinChange(UIAssassinRankDisplay.EDirection _eDirection)
		{
			UpdateData();
		}

		private void UpdateMission()
		{
			UIMissionGoals missionGoals = MissionMetaDataManager.Instance.MissionGoals;
			m_numberOfChallenges = ((missionGoals.Challenges != null) ? missionGoals.Challenges.Where((UIMissionGoals.GoalData x) => !x.Hidden).Count() : 0);
			m_numberOfModifiers = ((missionGoals.AdditionalObjectives != null) ? missionGoals.AdditionalObjectives.Where((UIMissionGoals.GoalData x) => !x.Hidden).Count : 0);
			UpdateChallengesAndRequirements();
			List<UIMissionGoals.GoalData> list = missionGoals.Objectives.Where((UIMissionGoals.GoalData x) => !x.Hidden);
			string empty = string.Empty;
			string empty2 = string.Empty;
			if (Singleton<StateManager>.Instance.MissionSelectionManager.IsSelectedMissionCampaign)
			{
				MissionInfo campaignMission = Singleton<StateManager>.Instance.MissionSelectionManager.CampaignMission;
				empty = campaignMission.LocaName();
				empty2 = campaignMission.LocaMissionFlavorText();
				UIMissionUtil.UpdateMissionIconByMissionObjectiveType(m_MissionIcon, campaignMission.objectiveBaseType, false, m_MissionIconBackground);
			}
			else
			{
				AcierProtocol.Mission contractMission = Singleton<StateManager>.Instance.MissionSelectionManager.ContractMission;
				RegionDefinition regionDefinition = contractMission.GetRegionDefinition();
				empty = contractMission.LocaMissionName(regionDefinition);
				if (m_isElite)
				{
					empty = LocalizationManager.Instance.Get(8029, new StringMapper { { "elite_mission_name", empty } });
				}
				empty2 = contractMission.LocaMissionFlavorText(regionDefinition);
				UIMissionUtil.UpdateMissionIconByMissionObjectiveType(m_MissionIcon, contractMission.missionKind, m_isElite, m_MissionIconBackground);
			}
			m_MissionName.text = empty;
			m_MainGoal.text = ((list.Count <= 0) ? "<animus error>" : list[0].Name);
			m_LongDescription.text = empty2;
		}

		private void UpdateChallengesAndRequirements()
		{
			bool active = m_numberOfChallenges > 0;
			bool active2 = m_numberOfModifiers > 0;
			m_challenges.SetActive(active);
			m_modifiers.SetActive(active2);
			m_MainGoal.gameObject.SetActive(true);
			UIMissionGoals missionGoals = MissionMetaDataManager.Instance.MissionGoals;
			m_challenges.SetData(missionGoals.Challenges.ToArray(), RepositionTable, false);
			m_modifiers.SetData(missionGoals.AdditionalObjectives.ToArray(), RepositionTable, false, true);
		}

		private void RepositionTable()
		{
			m_missionTable.repositionNow = true;
		}

		private IEnumerator LoadBanner(UnityResource _resource)
		{
			yield return StartCoroutine(m_loadAnimMgr.BeginLoadUnityResource(_resource, new LoadAnimationManager.SLoadObjectInfo(m_DistrictBanner)));
			m_DistrictBanner.mainTexture = _resource.AccessObject<Texture>();
		}
	}
}
