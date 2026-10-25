namespace Assets.Scripts.UI.Notifications
{
	public class UINullnewsEvaluator : UINewsEvaluatorBase
	{
		public override ENewsType Type
		{
			get
			{
				return ENewsType.Invalid;
			}
		}

		public override void Init()
		{
		}

		public override void Destroy()
		{
		}

		public override void Update()
		{
		}
	}
}
