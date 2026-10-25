namespace Assets.Scripts.UI.Screens.NewsFeedScreen.Data
{
	public class NewsFeedDataBase
	{
		public enum EFeedIconType
		{
			None = -1,
			General = 0,
			News = 1,
			Maintenance = 2,
			Shop = 3,
			Contest = 4,
			FAQ = 5,
			Poll = 6,
			Quote = 7,
			Tip = 8
		}

		public long CreatedAt;

		public string GoToUrl = string.Empty;

		public EFeedIconType IconType;

		public virtual string Header { get; set; }

		public virtual string Message { get; set; }

		public NewsFeedDataBase()
		{
			Header = string.Empty;
			Message = string.Empty;
		}
	}
}
