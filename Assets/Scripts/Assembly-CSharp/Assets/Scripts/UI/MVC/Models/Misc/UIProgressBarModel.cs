namespace Assets.Scripts.UI.MVC.Models.Misc
{
	public class UIProgressBarModel : UIModelBase
	{
		public int CurrentValue { get; set; }

		public int AnimationEndValue { get; set; }

		public int MaxValue { get; set; }

		public int StartValue { get; set; }

		public bool ShowIcons { get; set; }
	}
}
