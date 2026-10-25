namespace Assets.Scripts.UI.Screens.CreditsScreen
{
	public interface ICreditsItem
	{
		ECreditsItemType Type { get; }

		float Position { get; }

		float Size { get; }

		CreditsTab Tab { get; set; }
	}
}
