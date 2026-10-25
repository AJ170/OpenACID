using Assets.Scripts.Network;
using SharedBaseLib.Core;

namespace Assets.Scripts.UI.Screens.NewsFeedScreen.Data
{
	public abstract class NewsFeedBase
	{
		public delegate void OnUpdateFinishedEvent(BBList<NewsFeedDataBase> _newsFeeds);

		public OnUpdateFinishedEvent OnUpdateFinished;

		public bool Finished { get; private set; }

		public NewsFeedBase()
		{
			Finished = false;
		}

		public virtual void AbandonRequest()
		{
		}

		public abstract void FetchNewDataFromServer(NetworkDataStore _dataStore = null);

		public abstract void ProcessFetchedData();

		protected void OnDateFetchCompleted()
		{
			Finished = true;
		}

		protected void CallUpdateFinished(BBList<NewsFeedDataBase> _items)
		{
			if (OnUpdateFinished != null)
			{
				OnUpdateFinished(_items);
			}
		}
	}
}
