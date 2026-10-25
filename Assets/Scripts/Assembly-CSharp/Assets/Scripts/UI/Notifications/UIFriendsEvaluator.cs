namespace Assets.Scripts.UI.Notifications
{
	public class UIFriendsEvaluator : UINewsEvaluatorBase
	{
		public override ENewsType Type
		{
			get
			{
				return ENewsType.Friends;
			}
		}

		public override void Init()
		{
			base.Init();
		}

		public override void Destroy()
		{
			base.Destroy();
		}

		public override void Update()
		{
		}
	}
}
