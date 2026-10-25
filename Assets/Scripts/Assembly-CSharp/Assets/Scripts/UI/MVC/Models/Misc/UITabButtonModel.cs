using Assets.Scripts.UI.Localization;

namespace Assets.Scripts.UI.MVC.Models.Misc
{
	public abstract class UITabButtonModel : UIModelBase
	{
		public abstract long EndTimeUTC { get; }

		public abstract StringId TitleLoca { get; }

		public abstract StringId SubTitleLoca { get; }

		public abstract bool IsEnabled { get; }
	}
}
