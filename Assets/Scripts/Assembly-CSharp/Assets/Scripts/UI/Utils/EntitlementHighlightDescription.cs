using Assets.Scripts.UI.Localization;
using Database;

namespace Assets.Scripts.UI.Utils
{
	public class EntitlementHighlightDescription
	{
		public GameID EntitlementGUID;

		public bool ShowUnlock;

		public bool ShowBeforeUnlock;

		public StringId DescriptionUnlocked;

		public StringId DescriptionLocked;
	}
}
