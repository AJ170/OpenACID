using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Engine;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.State;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class MailExtensions
	{
		private static readonly Dictionary<int, StringId> s_FolderNames = new Dictionary<int, StringId>
		{
			{
				0,
				new StringId(4407)
			},
			{
				1,
				new StringId(1839)
			}
		};

		private static readonly StringId s_systemSender = new StringId(4520);

		public static int MailLimit(this MailHeader _header)
		{
			return MailLimit(_header.priority);
		}

		public static int MailLimit(EMailPriority _priority)
		{
			MailBoxMapping.PriorityLimit[] priorityLimitMapping = Singleton<StateManager>.Instance.TemplateManager.MailBoxMapping.priorityLimitMapping;
			for (int i = 0; i < priorityLimitMapping.Length; i++)
			{
				if (priorityLimitMapping[i].emailPriority == _priority)
				{
					return priorityLimitMapping[i].limit;
				}
			}
			return 0;
		}

		public static bool UnlimitedAllowed(EMailPriority _priority)
		{
			return MailLimit(_priority) == -1;
		}

		public static bool UnlimitedAllowed(this MailHeader _header)
		{
			return UnlimitedAllowed(_header.priority);
		}

		public static bool CanBeReplied(this MailHeader _header)
		{
			return _header.type == EMailType.MailTypePlayer2Player;
		}

		public static bool CanBeDeleted(this MailHeader _header)
		{
			if (_header.hasAttachment)
			{
				return false;
			}
			return true;
		}

		public static bool IsFriendRequest(this MailHeader _header)
		{
			return _header.hasAttachment && _header.attachmentType == EMailAttachmentType.MailAttachmentTypeFriendRequest;
		}

		public static bool IsProgressionEvent(this MailBody _body)
		{
			return _body.attachment != null && _body.attachment.progressionEvent != null;
		}

		public static bool IsGift(this MailHeader _header)
		{
			return _header.hasAttachment && _header.attachmentType == EMailAttachmentType.MailAttachmentTypeGift;
		}

		public static bool IsResourceGift(this MailBody _body)
		{
			return _body.attachment != null && _body.attachment.gift != null && _body.attachment.gift.resources != null;
		}

		public static bool IsHirelingReward(this MailBody _body)
		{
			return _body.attachment != null && _body.attachment.hirelingAttachement != null;
		}

		public static bool IsItemGift(this MailBody _body)
		{
			return _body.attachment != null && _body.attachment.gift != null && _body.attachment.gift.elements != null;
		}

		public static bool IsEntitlementGift(this MailBody _body)
		{
			return _body.attachment != null && _body.attachment.gift != null && _body.attachment.gift.entitlements != null && _body.attachment.gift.entitlements.Length > 0;
		}

		public static bool IsExpired(this MailHeader _header)
		{
			if (_header.expirationDate == 0L)
			{
				return false;
			}
			DateTime dateTime = ServerTimeManager.SecondsToUtcTime(_header.expirationDate);
			return dateTime <= Singleton<StateManager>.Instance.ServerTimeManager.UtcNow;
		}

		public static string GetSubject(this MailHeader _header)
		{
			if (!string.IsNullOrEmpty(_header.subject))
			{
				return _header.subject;
			}
			StringMapper stringMapper = new StringMapper();
			stringMapper.Add("player_from", _header.GetSenderName());
			StringMapper mapper = stringMapper;
			if (Assets.Scripts.Engine.Helpers.Verify(_header.subjectLangKey != null, "[_header.subjectLangKey == NULL]  header has no language set. {0}", _header.DebugString()))
			{
				return LocalizationManager.Instance.Get(_header.subjectLangKey.neutral, mapper);
			}
			return string.Empty;
		}

		public static string GetFolderName(this MailHeader _header)
		{
			StringId value;
			if (s_FolderNames.TryGetValue((int)_header.folder, out value))
			{
				return LocalizationManager.Instance.Get(value);
			}
			return string.Empty;
		}

		public static bool IsSenderReal(this MailHeader _header)
		{
			return !string.IsNullOrEmpty(_header.senderId);
		}

		public static string GetSenderName(this MailHeader _header)
		{
			switch (_header.type)
			{
			case EMailType.MailTypePlayer2Player:
				return _header.senderName;
			case EMailType.MailTypeSystem:
				if (_header.IsSenderReal())
				{
					return _header.senderName;
				}
				return LocalizationManager.Instance.Get(s_systemSender);
			case EMailType.MailTypeMission:
				return LocalizationManager.Instance.Get(s_systemSender);
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private static string DebugString(this MailHeader _header)
		{
			return string.Format("Id: {0} Type: {1} Sender: {2} CreationData: {3} Expiration Date: {4}", _header.headerId, _header.type, _header.senderId, _header.creationDate, _header.expirationDate);
		}

		public static string GetBody(this MailBody _body, MailHeader _header)
		{
			if (!string.IsNullOrEmpty(_body.body))
			{
				return _body.body;
			}
			if (Assets.Scripts.Engine.Helpers.Verify(_body.bodyLangKey != null, "[_body.bodyLangKey == NULL]  Body has no language set"))
			{
				StringMapper stringMapper = new StringMapper();
				stringMapper.Add("Player_Name", _header.receiverName);
				stringMapper.Add("player_from", _header.GetSenderName());
				StringMapper stringMapper2 = stringMapper;
				if (_header.IsFriendRequest())
				{
					stringMapper2["player_from"] = _body.attachment.friendRequest.userName;
				}
				if (_header.IsGift())
				{
					string senderName = _header.GetSenderName();
					stringMapper2["player_from"] = senderName;
					if (_body.attachment.gift.resources != null)
					{
						stringMapper2["NumResources"] = _body.attachment.gift.resources.resources.Length;
					}
					else if (_body.attachment.gift.elements != null)
					{
						stringMapper2["NumItems"] = _body.attachment.gift.elements.elements.Length;
					}
				}
				if (_body.IsProgressionEvent())
				{
					stringMapper2["event_name"] = LocalizationManager.Instance.Get(_body.attachment.progressionEvent.eventHeaderLocaKey);
				}
				string text = LocalizationManager.Instance.Get(_body.bodyLangKey.neutral, stringMapper2);
				if (_body.IsHirelingReward())
				{
					for (int i = 0; i < _body.attachment.hirelingAttachement.hirelingUsages.Length; i++)
					{
						HirelingUsageStatisticsAttachment.StatisticsEntry statisticsEntry = _body.attachment.hirelingAttachement.hirelingUsages[i];
						string assassinName = Singleton<StateManager>.Instance.ProfileManager.Profile.GetAssassin(statisticsEntry.assassinId).GetAssassinName();
						int num = statisticsEntry.timesUsedByFriends + statisticsEntry.timesUsedByNonFriends;
						int num2 = (int)((statisticsEntry.rewardForTimesUsedByFriends != null) ? statisticsEntry.rewardForTimesUsedByFriends.amount : 0);
						int num3 = (int)((statisticsEntry.rewardForTimesUsedByNonFriends != null) ? statisticsEntry.rewardForTimesUsedByNonFriends.amount : 0);
						int num4 = num2 + num3;
						stringMapper = new StringMapper();
						stringMapper.Add("Assassin_Name", assassinName);
						stringMapper.Add("Hiring_amount", LocalizationManager.Instance.GetInteger(num));
						stringMapper.Add("Friend_Bonus", LocalizationManager.Instance.GetInteger(num4));
						StringMapper mapper = stringMapper;
						string text2 = "\n \n" + LocalizationManager.Instance.Get(8236, mapper);
						text += text2;
					}
				}
				return text;
			}
			return string.Empty;
		}

		public static bool CanBeRejected(this MailBody _body)
		{
			if (_body.attachment == null)
			{
				return false;
			}
			if (_body.attachment.friendRequest != null)
			{
				return true;
			}
			if (_body.attachment.gift == null)
			{
				return false;
			}
			return _body.attachment.gift.senderId != Singleton<StateManager>.Instance.ProfileManager.Profile.id;
		}

		public static bool CanClaim(this MailBody _body)
		{
			if (_body.attachment == null)
			{
				return false;
			}
			if (_body.attachment.gift != null)
			{
				InventoryElements elements = _body.attachment.gift.elements;
				if (elements != null && elements.elements.Length > 0)
				{
					return elements.elements.HasInventorySpaceForItem();
				}
				return true;
			}
			return false;
		}
	}
}
