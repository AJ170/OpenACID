using System;
using System.Collections.Generic;
using AcierProtocol;
using BB.Mobile.Net;

namespace Assets.Scripts.Network
{
	public interface INetworkDataStore
	{
		IAsyncDataResult<DiscoveryServiceResponse> GetDiscoveryService(EServerEnvironment _serverEnvironment, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<GetTwitterTimelineResponse> GetTwitterTimeline(ELanguages _language, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<GetUserProfileResponse> GetPlayerProfile(AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<GetUserProfileResponse> GetPlayerProfile(bool _needInventory, bool _needAssassins, bool _needResources, bool _needMissions, bool _needCounter, bool _needAchievements, bool _needTutorial, bool _needEntitlements, bool _needSettings, bool _needProgressionQuests, bool _needEventsProgress, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<ModifyUserSettingsResponse> ModifyUserSettings(UserSettings _userSettings, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<ModifyProfileNameResponse> ModifyPlayerProfileName(string _profileName, ESocialNetwork _socialNetwork, string _accessToken = null, EModuleOrigin _origin = EModuleOrigin.ModuleOrigin_Invalid, bool _forceLinkToCurrent = false, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void UpdateStaticsVersion(long _version);

		IAsyncDataResult<GetStaticsResponse> GetStatics(long _cachedStaticsVersion = -1, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<BuyResourcesResponse> BuyResources(Dictionary<int, long> _resourceAmounts, EModuleOrigin _origin, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<TrainAssassinResponse> TrainAssassin(string _assassinId, AssassinSkillInTraining.SkillClass _skillTierClass, int _tierId, AssassinSkillInTraining.SkillType _tierSkillType, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<TrainAssassinDuringTutorialResponse> TrainAssassinDuringTutorial(string _assassinId, AssassinSkillInTraining.SkillClass _skillTierClass, int _tierId, AssassinSkillInTraining.SkillType _tierSkillType, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<UnlockTierResponse> UnlockTier(string _assassinId, int _tierId, AssassinSkillInTraining.SkillClass _skillClass, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<UnlockPassiveSkillResponse> UnlockPassiveSkill(string _assassinId, int _tierId, AssassinSkillInTraining.SkillClass _skillClass, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<EquipElementResponse> EquipElement(string _assassinId, string _elementId, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<UnequipElementResponse> UnequipElement(string _assassinId, string _elementId, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<SellItemResponse> SellItem(string _itemId, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<GetMissionInfoResponse> GetMissionInfo(string _missionGuid, string _assassinId, string _hirelingId, string _campaignId, int _rank, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<OpenMissionResponse> OpenMission(string _missionGuid, string _assassinId, string _hirelingId, string _campaignId, int _rank, bool _isRetry, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<ShareMissionResponse> ShareMission(string _missionId, string _receiverId, string _missionFullName, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<RemoveHirelingAndMissionResponse> RemoveHirelingAndMission(string _campaignId, string _missionId, string _assassinId, int _rank, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<EndMissionResponse> EndMission(MissionResult _missionResult, string _ticket, bool _won, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<RefreshMissionListResponse> RefreshMissionList(string _assassinId, int _rank, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<ReviveResponse> Revive(string _missionToken, EResourceType _paymentResource, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<CreateAssassinResponse> CreateAssassin(Assassin.EClassTypeId _avatarType, string _headGuid, string _customName, int _firstNameLocaId, int _lastNameLocaId, int _numAssassinsBefore, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<DeleteAssassinResponse> DeleteAssassin(string _assassinId, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<GetForeignAssassinResponse> GetForeignAssassinByAssassinId(string _userIdHash, string _assassinId, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<GetForeignAssassinResponse> GetForeignAssassinByHirelingId(string _hirelingId, string _assassinId, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<GetForeignAssassinResponse> GetForeignAssassinByUserId(string _userId, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<ResetSkillTreeResponse> ResetSkillTree(string _assassinId, AssassinSkillInTraining.SkillClass _skillClass, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<SetExtendedClassTypeResponse> SetExtendedClass(string _assassinId, Assassin.EClassTypeId classType, EResourceType _resource, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<UpdateSkillWheelResponse> UpdateSkillWheel(string _assassinId, SkillWheel skillWheel, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<PurchaseInGameItemResponse> PurchaseInGameShopItem(Resources _costs, string _shopItemId, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<PurchaseAssassinSlotResponse> PurchaseAssassinSlot(int _numAlreadyPurchasedSlots, EResourceType _paymentResource, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<PurchaseInventorySlotResponse> PurchaseInventorySlot(int _numSlotPurchases, EResourceType _paymentResource, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<EarlyAccessClassResponse> EarlyAccessClass(Assassin.EClassTypeId _classType, EResourceType _currency, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<FBStatusResponse> QueryFBStatus(string _accessToken, ESocialNetwork _socialNetwork, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<FBStatusByIdResponse> QueryFBStatusById(string _facebookId, ESocialNetwork _socialNetwork, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<GetMailHeadersResponse> GetMailHeaders(long _lastTimeStamp, int _maxHeaders = 0, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<GetMailBodyResponse> GetMailBody(string _headerId, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<SendMailResponse> SendMail(string _receiverId, string _subject, string _body, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<DeleteMailsResponse> DeleteMails(List<string> _headerIds, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<SendFriendRequestResponse> SendFriendRequest(string _userId, EModuleOrigin _origin, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<ProcessMailResponse> ProcessMail(string _headerId, bool _accept, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<GetMyFriendsResponse> GetMyFriends(AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<RemoveFriendResponse> RemoveFriend(string _userId, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<ReadAchievementResponse> ReadAchievement(string _achievmentId, int _rankIdx, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<ClaimAchievementResponse> ClaimAchievement(string _achievmentId, int _rankIdx, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<ReachNewAchievementsResponse> ReachNewAchievements(List<string> _achievementIds, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<SetTutorialStepResponse> SetTutorialStep(ETutorialCategory _category, int _counter, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<SkipTutorialResponse> SkipTutorial(AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<SetTutorialFinishedResponse> SetTutorialFinished(ETutorialCategory _category, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<OpenMysteryBoxResponse> OpenMysteryBox(Quality.EQuality _quality, EMysteryBoxType _mysteryBoxType, string assassinId, string _itemId, bool _startChain, bool _overspending, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<ClaimMysteryBoxResponse> ClaimMysteryBox(AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<GetMysteryBoxInfoResponse> GetMysteryBoxInfo(AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<ClaimQuestRewardResponse> ClaimQuestReward(int _rankIdx, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<ClaimWeeklyEventRewardResponse> ClaimWeeklyEventReward(string _challengeId, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<GetLeaderBoardsResponse> GetLeaderBoards(AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<GetResetProfileResponse> GetResetProfile(AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<ForgeResponse> Forge(List<string> _elementIds, Resource _expectedCost, int _expectedItemLevel, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<ForgeRerollResponse> ForgeReroll(Resource _expectedCost, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<ForgeDuringTutorialResponse> ForgeDuringTutorial(List<string> _elementIds, Resource _expectedCost, int _expectedItemLevel, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<ForgeRerollDuringTutorialResponse> ForgeRerollDuringTutorial(Resource _expectedCost, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<VerifyAppResponse> VerifyApp(string _appReceipt, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<CollectAdRewardsResponse> CollectAdRewards(AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		IAsyncDataResult<CheatResponse> UserProfileCheat(CheatRequest.CheatType _cheatType, string _assassinId = "", CheatParameters _params = null);

		IAsyncDataResult<CheatResponse> GetMissionByName(string _name, bool _isElite);

		IAsyncDataResult<SimulateLootRollResponse> SimulateLootRoll(string _rollCode, bool _giveLoot, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void Update();

		bool AbandonUserProfile();
	}
}
