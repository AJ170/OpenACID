using AcierProtocol;
using Assets.Scripts.UI.Localization;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class CurrencyExtensions
	{
		public static string DisplayName(this EResourceType _resource)
		{
			CurrencyDescription value;
			if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.CurrencyDescriptions.TryGetValue((int)_resource, out value))
			{
				return LocalizationManager.Instance.Get(value.LocaKey);
			}
			return string.Empty;
		}
	}
}
