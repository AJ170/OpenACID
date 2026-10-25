using System;
using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.MVC.Controllers.Misc;
using Assets.Scripts.UI.MVC.Models.Events;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;

namespace Assets.Scripts.UI.MVC.Models.DailyQuest
{
	public class UIDailyQuestItemModel : UIEventBaseModel
	{
		public ProgressionQuests.RankQuests.Quest Quest { get; set; }

		public override int CurrentValue
		{
			get
			{
				return (int)Quest.progress;
			}
		}

		public override int MaxValue
		{
			get
			{
				return (int)Quest.numberOfTimesToPerform;
			}
		}

		public override bool IsLocked
		{
			get
			{
				return Quest.status == EQuestStatus.Created;
			}
		}

		public override bool IsClaimed
		{
			get
			{
				return Quest.status == EQuestStatus.Claimed;
			}
		}

		public override bool IsClaimable
		{
			get
			{
				return Quest.status == EQuestStatus.Finished;
			}
		}

		public override bool CanClaimItem
		{
			get
			{
				if (Quest.reward.elements != null && Quest.reward.elements.elements.Length > 0)
				{
					return Quest.reward.elements.elements[0].HasInventorySpaceForItem();
				}
				return true;
			}
		}

		public override string HeaderText
		{
			get
			{
				return LocalizationManager.Instance.Get(Quest.titleLocaKey);
			}
		}

		public override string HeaderDescription
		{
			get
			{
				return LocalizationManager.Instance.Get(Quest.descriptionLocaKey, new StringMapper { { "quest_amount", Quest.numberOfTimesToPerform } });
			}
		}

		public override int RewardItemCount()
		{
			return 1;
		}

		public override void InitializeRewardItem(int _rewardIndex, UIRewardItemController _item)
		{
			_item.Initialize(Quest.reward);
		}

		public override void OnClaimItem(Action<bool, bool, bool> _onItemClaimed)
		{
			int currentRank = Singleton<StateManager>.Instance.MissionSelectionManager.ContractHeroicMissionRank;
			UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncCallback)
			{
				Singleton<StateManager>.Instance.ProfileManager.ClaimQuestReward(currentRank, _asyncCallback);
			}, null, delegate(IAsyncResult _result)
			{
				if (_onItemClaimed != null)
				{
					_onItemClaimed(!_result.IsError(), true, true);
				}
			});
		}
	}
}
