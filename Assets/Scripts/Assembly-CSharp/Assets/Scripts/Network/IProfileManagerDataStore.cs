using System;
using System.Collections.Generic;
using AcierProtocol;
using BB.Mobile.Net;

namespace Assets.Scripts.Network
{
	public interface IProfileManagerDataStore
	{
		void UpdateUserProfile(AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null, int _callDepth = 0);

		void UpdateUserProfile(bool _needInventory, bool _needAssassins, bool _needResources, bool _needMissions, bool _needCounter, bool _needAchievements, bool _needTutorial, bool _needEntitlements, bool _needUserSettings, bool _needProgressionQuests, bool _needEventsProgress, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null, int _callDepth = 0);

		void ModifyUserSettings(UserSettings _userSettings, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void ModifyPlayerProfileName(string _profileName, ESocialNetwork _socialNetwork, string _fbAccessToken = null, EModuleOrigin _origin = EModuleOrigin.ModuleOrigin_Invalid, bool _forceFacebookLinkToCurrent = false, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void GetStatics(AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null, int _callDepth = 0);

		void BuyResources(Dictionary<int, long> _resourceAmounts, EModuleOrigin _origin, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void TrainAssassin(string _assassinId, AssassinSkillInTraining.SkillClass _skillTierClass, int _tierId, AssassinSkillInTraining.SkillType _tierSkillType, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void TrainAssassinDuringTutorial(string _assassinId, AssassinSkillInTraining.SkillClass _skillTierClass, int _tierId, AssassinSkillInTraining.SkillType _tierSkillType, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void UnlockTier(string _assassinId, int _tierId, AssassinSkillInTraining.SkillClass _skillClass, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void UnlockPassiveSkill(string _assassinId, int _tierId, AssassinSkillInTraining.SkillClass _skillClass, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void EquipElement(string _assassinId, string _elementId, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void UnequipElement(string _assassinId, string _elementId, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void SellItem(string _itemId, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void OpenMission(string _missionGuid, string _assassinId, string _hirelingId, string _campaignId, int _rank, bool _isRetry, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void EndMission(MissionResult _missionResult, string _ticket, bool _won, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void GetMissionInfo(string _missionGuid, string _assassinId, string _hirelingId, string _campaignId, int _rank, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void RemoveHirelingAndMission(string _campaignId, string _missionId, string _assassinId, int _rank, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void RefreshMissionList(string _assassinId, int _rank, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void Revive(string _missionToken, EResourceType _paymentResource, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void CreateAssassin(Assassin.EClassTypeId _avatarType, string _headGuid, string _customName, int _firstNameLocaId, int _lastNameLocaId, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void DeleteAssassin(string _assassinId, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void GetForeignAssassinByAssassinId(string _userIdHash, string _assassinId, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void GetForeignAssassinByHirelingId(string _hirelingId, string _assassinId, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void GetForeignAssassinByUserId(string _userId, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void ResetSkillTree(string _assassinId, AssassinSkillInTraining.SkillClass _skillClass, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void SetExtendedClass(string _assassinId, Assassin.EClassTypeId classType, EResourceType _resource, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void UpdateSkillWheel(string _assassinId, SkillWheel skillWheel, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void PurchaseInGameShopItem(Resources _costs, string _shopItemId, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void PurchaseAssassinSlot(int _numAlreadyPurchasedSlots, EResourceType _paymentResource, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void PurchaseInventorySlot(int _numSlotPurchases, EResourceType _paymentResource, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void VerifyReceipt(string _productId, string _transactionId, string _originalTransactionId, string _receiptData, string _currencyCode, string _cost, string _formattedPrice, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void VerifyGoogleReceipt(string _productId, string _receiptSignature, string _receiptData, string _currencyCode, string _cost, string _formattedPrice, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void QueryFbStatus(string _accessToken, ESocialNetwork _socialNetwork, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void QueryFbStatusById(string _facebookId, ESocialNetwork _socialNetwork, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void GetMailHeaders(long _lastTimeStamp, int _maxHeaders = 0, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void GetMailBody(string _headerId, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void SendMail(string _receiverId, string _subject, string _body, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void DeleteMails(List<string> _headerIds, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void SendFriendRequest(string _userId, EModuleOrigin _origin, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void ProcessMail(string _headerId, bool _accept, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void GetMyFriends(AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void RemoveFriend(string _userId, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void ReadAchievement(string _achievmentId, int _rankIdx, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void ClaimAchievement(string _achievmentId, int _rankIdx, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void ReachNewAchievements(List<string> _achievementIds, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void SetTutorialStep(ETutorialCategory _category, int _counter, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void SkipTutorial(AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void SetTutorialFinished(ETutorialCategory _category, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void GetProductPricings(string[] _productIds, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null, int _callDepth = 5);

		void PurchaseProductFromShop(string _productId, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null, int _callDepth = 0);

		void OpenMysteryBox(Quality.EQuality _quality, EMysteryBoxType _mysteryBoxType, string _assassinId, string _itemId, bool _startChain, bool _overspending, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void ClaimMysteryBox(AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void GetMysteryBoxInfo(AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void ClaimQuestReward(int _rankIdx, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void ClaimWeeklyEventReward(string _challengeId, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void GetLeaderBoards(AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void GetResetProfile(AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void Forge(List<string> _elementIds, Resource _expectedCost, int _expectedItemLevel, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void ForgeReroll(Resource _expectedCost, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void ForgeDuringTutorial(List<string> _elementIds, Resource _expectedCost, int _expectedItemLevel, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void ForgeRerollDuringTutorial(Resource _expectedCost, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void GetTwitterTimeline(ELanguages _language, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void CollectAdRewards(AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void UserProfileCheat(CheatRequest.CheatType _cheatType, string _assassinId = "", CheatParameters _params = null, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void GetMissionByName(string _name, bool _isElite, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);

		void SimulateLootRoll(string _rollCode, bool _giveLoot, AsyncCallback _callback = null, IAsyncCallbackScheduler _scheduler = null);
	}
}
