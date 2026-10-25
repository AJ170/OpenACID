using System;
using Assets.Scripts.Network;
using SharedBaseLib.Core;

namespace Assets.Scripts.UI.Screens.NewsFeedScreen.Data
{
	public class NewsFeedManager
	{
		public delegate void OnFeedUpdatedEvent();

		public OnFeedUpdatedEvent OnFeedUpdated;

		private int m_waitCounterForFeedUpdate;

		private readonly BBSimpleList<NewsFeedBase> m_newsFeeds = new BBSimpleList<NewsFeedBase>();

		private readonly BBList<NewsFeedDataBase> m_sortedNewsFeedEntries = new BBList<NewsFeedDataBase>();

		public BBList<NewsFeedDataBase> NewsFeedEntries
		{
			get
			{
				return m_sortedNewsFeedEntries;
			}
		}

		public bool IsInitialized { get; private set; }

		public NewsFeedManager()
		{
			m_newsFeeds.Add(new NewsFeedTwitter());
			for (int i = 0; i < m_newsFeeds.Count; i++)
			{
				NewsFeedBase newsFeedBase = m_newsFeeds[i];
				newsFeedBase.OnUpdateFinished = (NewsFeedBase.OnUpdateFinishedEvent)Delegate.Combine(newsFeedBase.OnUpdateFinished, new NewsFeedBase.OnUpdateFinishedEvent(OnUpdateFinished));
			}
		}

		public void Abandon()
		{
			for (int i = 0; i < m_newsFeeds.Count; i++)
			{
				m_newsFeeds[i].AbandonRequest();
			}
		}

		public bool HasFinishedLoading()
		{
			for (int i = 0; i < m_newsFeeds.Count; i++)
			{
				if (!m_newsFeeds[i].Finished)
				{
					return false;
				}
			}
			return true;
		}

		public void ProcessFetchedData()
		{
			for (int i = 0; i < m_newsFeeds.Count; i++)
			{
				m_newsFeeds[i].ProcessFetchedData();
			}
		}

		public void FetchNewDataFromServer(NetworkDataStore _dataStore = null)
		{
			m_sortedNewsFeedEntries.Clear();
			m_waitCounterForFeedUpdate = m_newsFeeds.Count;
			for (int i = 0; i < m_newsFeeds.Count; i++)
			{
				m_newsFeeds[i].FetchNewDataFromServer(_dataStore);
			}
		}

		public void FetchCachedData()
		{
			if (m_waitCounterForFeedUpdate <= 0 && OnFeedUpdated != null)
			{
				OnFeedUpdated();
			}
		}

		private void OnUpdateFinished(BBList<NewsFeedDataBase> _newsFeeds)
		{
			m_sortedNewsFeedEntries.AddRange(_newsFeeds);
			m_waitCounterForFeedUpdate--;
			if (m_waitCounterForFeedUpdate <= 0)
			{
				m_sortedNewsFeedEntries.Sort(SortNewsFeedEntries);
				if (OnFeedUpdated != null)
				{
					OnFeedUpdated();
				}
			}
		}

		private static int SortNewsFeedEntries(NewsFeedDataBase _a, NewsFeedDataBase _b)
		{
			return (_a.CreatedAt != _b.CreatedAt) ? ((_a.CreatedAt <= _b.CreatedAt) ? 1 : (-1)) : 0;
		}
	}
}
