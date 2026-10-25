using AcierProtocol;
using Assets.Scripts.UI.Localization;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class MoodExtensions
	{
		public static string DisplayName(this EMood _mood)
		{
			MoodDescription value;
			if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.MoodDescriptions.TryGetValue((int)_mood, out value))
			{
				return LocalizationManager.Instance.Get(value.NameId);
			}
			return string.Empty;
		}
	}
}
