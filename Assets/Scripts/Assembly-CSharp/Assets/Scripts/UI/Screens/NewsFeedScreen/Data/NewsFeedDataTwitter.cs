using AcierProtocol;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Types;

namespace Assets.Scripts.UI.Screens.NewsFeedScreen.Data
{
	public class NewsFeedDataTwitter : NewsFeedDataBase
	{
		public TwitterStatus Status;

		public override string Header
		{
			get
			{
				return new UIDate(ServerTimeManager.ToLocalTimeMilliseconds(Status.createdAt), UIDate.EDisplayType.DateTime).ToString();
			}
		}
	}
}
