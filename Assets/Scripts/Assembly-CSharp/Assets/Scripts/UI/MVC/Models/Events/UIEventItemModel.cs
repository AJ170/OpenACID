using System;
using AcierProtocol;
using Assets.Scripts.Engine;
using Assets.Scripts.Network;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.MVC.Controllers.Misc;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;

namespace Assets.Scripts.UI.MVC.Models.Events
{
	public class UIEventItemModel : UIEventBaseModel
	{
		private static int[] s_descriptionSufix = new int[5] { 9785, 9975, 9978, 9979, 9980 };

		private static int[] s_descriptionPrefix = new int[6] { 9976, 9977, 9981, 9982, 9983, 9984 };

		private EventsProgress.ChallengeEvent m_eventChallenge;

		private string m_sufixDec;

		private string m_prefixDec;

		public string EventChallengeId { get; set; }

		public override int CurrentValue
		{
			get
			{
				return (int)((m_eventChallenge != null) ? m_eventChallenge.GetChallengeCount() : 0);
			}
		}

		public override int MaxValue
		{
			get
			{
				return (int)((m_eventChallenge != null) ? m_eventChallenge.GetFormatedTimesToCompleteVaule() : 0);
			}
		}

		public override bool IsClaimed
		{
			get
			{
				return m_eventChallenge != null && m_eventChallenge.status == EEventChallengeStatus.Challenge_Claimed;
			}
		}

		public override bool IsClaimable
		{
			get
			{
				return m_eventChallenge != null && m_eventChallenge.status == EEventChallengeStatus.Challenge_Finished;
			}
		}

		public override bool CanClaimItem
		{
			get
			{
				return true;
			}
		}

		public override string HeaderText
		{
			get
			{
				return m_eventChallenge.GetChallengeHeaderText();
			}
		}

		public override string HeaderDescription
		{
			get
			{
				string text = ((m_eventChallenge == null) ? string.Empty : m_eventChallenge.GetChallengeDescriptionText());
				if (Singleton<StateManager>.Instance.ThemesManager.IsThemeEnabled)
				{
					text = m_prefixDec + text + m_sufixDec;
				}
				return text;
			}
		}

		public override void UpdateData()
		{
			m_eventChallenge = Singleton<StateManager>.Instance.ProfileManager.Profile.GetEventsProgressChallenge(EventChallengeId);
			Random random = new Random(EventChallengeId.GetBbHashCode());
			int num = random.Next(2);
			m_sufixDec = ((num != 0) ? string.Empty : GeneratedDescPrefix(random, s_descriptionSufix));
			m_prefixDec = ((num <= 0) ? string.Empty : (GeneratedDescPrefix(random, s_descriptionPrefix) + " "));
		}

		public override int RewardItemCount()
		{
			return 2;
		}

		public override void InitializeRewardItem(int _rewardIndex, UIRewardItemController _item)
		{
			if (m_eventChallenge != null)
			{
				if (_rewardIndex == 0)
				{
					_item.Initialize(m_eventChallenge.rewardLoot);
				}
				else
				{
					_item.Initialize(ECustomItemType.EventScore, m_eventChallenge.rewardScore);
				}
			}
		}

		public override bool ShouldPlayClaimAnimationForReward(int _rewardIndex)
		{
			return _rewardIndex != 1;
		}

		public override void OnClaimItem(Action<bool, bool, bool> _onItemClaimed)
		{
			UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncCallback)
			{
				Singleton<StateManager>.Instance.ProfileManager.ClaimWeeklyEventReward(EventChallengeId, _asyncCallback);
			}, null, delegate(IAsyncResult _result)
			{
				bool arg = false;
				bool arg2 = false;
				if (!_result.IsError())
				{
					ClaimWeeklyEventRewardResponse claimWeeklyEventRewardResponse = ((AsyncDataResult<ClaimWeeklyEventRewardResponse>)_result).Result();
					arg = claimWeeklyEventRewardResponse.challengeRewardInInventory;
					arg2 = claimWeeklyEventRewardResponse.mainEventRewardInInventory;
				}
				if (_onItemClaimed != null)
				{
					_onItemClaimed(!_result.IsError(), arg, arg2);
				}
			});
		}

		public int GetEventScoreItemIndex()
		{
			return 1;
		}

		private string GeneratedDescPrefix(Random _randomGen, int[] _descriptionAddons)
		{
			int num = _randomGen.Next(_descriptionAddons.Length);
			return LocalizationManager.Instance.Get(_descriptionAddons[num]);
		}
	}
}
