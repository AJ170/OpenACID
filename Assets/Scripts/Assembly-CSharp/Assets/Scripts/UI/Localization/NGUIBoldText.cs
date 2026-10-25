namespace Assets.Scripts.UI.Localization
{
	public class NGUIBoldText : INGUIFormatProvider
	{
		public string Format(string _text)
		{
			return string.Format("[b]{0}[/b]", _text);
		}
	}
}
