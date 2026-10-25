using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AcierProtocol;
using Assets.Scripts.Network;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using SharedBaseLib.Core;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Screens.NewsFeedScreen.Data
{
	public class NewsFeedTwitter : NewsFeedBase
	{
		private readonly Regex m_regLink = new Regex("http(s)?://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?");

		private readonly Regex m_regScreenName = new Regex("@\\w+");

		private readonly Regex m_regHashTag = new Regex("#\\w+");

		private Dictionary<string, int> m_newsFeedHashTagToType;

		private UIRequestToken m_currentRequest;

		private TwitterFeed FeedsToBeProcessed;

		public static string GetTwitterAccountUrl
		{
			get
			{
				return "https://twitter.com/intent/user?user_id=" + LocalizationManager.Instance.Get(4654);
			}
		}

		public override void AbandonRequest()
		{
			if (m_currentRequest != null)
			{
				m_currentRequest.Abandoned = true;
				m_currentRequest = null;
			}
		}

		public override void FetchNewDataFromServer(NetworkDataStore _dataStore = null)
		{
			if (_dataStore == null)
			{
				m_currentRequest = UINetworkAdaptor.NetworkRequest(delegate(AsyncCallback _result)
				{
					Singleton<StateManager>.Instance.ProfileManager.GetTwitterTimeline(LocalizationManager.Instance.CurrentLanguage, _result);
				}, null, OnGetTwitterTimeline);
			}
			else
			{
				_dataStore.GetTwitterTimeline(LocalizationManager.Instance.CurrentLanguage, OnGetTwitterTimeline, _dataStore.Dispatcher);
			}
		}

		public override void ProcessFetchedData()
		{
			if (FeedsToBeProcessed != null)
			{
				BBList<NewsFeedDataBase> bBList = new BBList<NewsFeedDataBase>();
				for (int i = 0; i < FeedsToBeProcessed.statusMessages.Length; i++)
				{
					TwitterStatus twitterStatus = FeedsToBeProcessed.statusMessages[i];
					NewsFeedDataTwitter newsFeedDataTwitter = new NewsFeedDataTwitter();
					newsFeedDataTwitter.CreatedAt = twitterStatus.createdAt;
					newsFeedDataTwitter.Message = FormatedTwitterFeed(twitterStatus);
					newsFeedDataTwitter.Status = twitterStatus;
					newsFeedDataTwitter.IconType = GetIconTypeFromHashTag(twitterStatus.hashTagEntity);
					newsFeedDataTwitter.GoToUrl = GetTwitterAccountUrl;
					NewsFeedDataTwitter item = newsFeedDataTwitter;
					bBList.Add(item);
				}
				CallUpdateFinished(bBList);
				FeedsToBeProcessed = null;
			}
		}

		private void OnGetTwitterTimeline(IAsyncResult _asyncResult)
		{
			try
			{
				FeedsToBeProcessed = ((AsyncDataResult<GetTwitterTimelineResponse>)_asyncResult).Result().twitterFeed;
			}
			catch (Exception)
			{
			}
			OnDateFetchCompleted();
		}

		private NewsFeedDataBase.EFeedIconType GetIconFromHashTag(string _tag)
		{
			if (m_newsFeedHashTagToType == null || m_newsFeedHashTagToType.Count == 0)
			{
				m_newsFeedHashTagToType = new Dictionary<string, int>();
				foreach (KeyValuePair<int, NewsFeedDescription> item in UbiCore.Utils.Singleton<UIDataMapper>.Instance.NewsFeedDescription)
				{
					if (StringId.ValidStringId(item.Key))
					{
						string key = LocalizationManager.Instance.Get(item.Value.HashTagId).ToLower();
						int value = -1;
						if (!m_newsFeedHashTagToType.TryGetValue(key, out value))
						{
							m_newsFeedHashTagToType.Add(key, item.Key);
						}
					}
				}
			}
			int value2 = -1;
			if (m_newsFeedHashTagToType.TryGetValue("#" + _tag.ToLower(), out value2))
			{
				return (NewsFeedDataBase.EFeedIconType)value2;
			}
			return NewsFeedDataBase.EFeedIconType.None;
		}

		private NewsFeedDataBase.EFeedIconType GetIconTypeFromHashTag(TwitterHashTagEntity[] _hashTagEntity)
		{
			for (int i = 0; i < _hashTagEntity.Length; i++)
			{
				string text = _hashTagEntity[i].text;
				NewsFeedDataBase.EFeedIconType iconFromHashTag = GetIconFromHashTag(text);
				if (iconFromHashTag != NewsFeedDataBase.EFeedIconType.None)
				{
					return iconFromHashTag;
				}
			}
			return NewsFeedDataBase.EFeedIconType.General;
		}

		private string FormatedTwitterFeed(TwitterStatus _status)
		{
			string input = m_regLink.Replace(_status.text, delegate(Match m)
			{
				string text = m.Value;
				string text2 = string.Empty;
				for (int i = 0; i < _status.urlEntity.Length; i++)
				{
					TwitterUrlEntity twitterUrlEntity = _status.urlEntity[i];
					if (_status.urlEntity[i].text == text)
					{
						text = twitterUrlEntity.displayUrl;
						text2 = twitterUrlEntity.expandedUrl;
						break;
					}
				}
				return (!string.IsNullOrEmpty(text2)) ? string.Format("[08A1EF][url={0}]{1}[/url][-]", text2, text) : text;
			});
			input = m_regScreenName.Replace(input, delegate(Match m)
			{
				string value2 = m.Value;
				return string.Format("[08A1EF][url=https://twitter.com/{0}]{1}[-]", value2.TrimStart('@'), value2);
			});
			return m_regHashTag.Replace(input, delegate(Match m)
			{
				string value = m.Value;
				return string.Format("[08A1EF][url=https://twitter.com/hashtag/{0}]{1}[-]", value.TrimStart('#'), value);
			});
		}
	}
}
