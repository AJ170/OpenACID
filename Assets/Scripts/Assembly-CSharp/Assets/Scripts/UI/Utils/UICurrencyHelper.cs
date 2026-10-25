using AcierProtocol;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Utils
{
	public static class UICurrencyHelper
	{
		public static bool SetCurrenySymbol(UISprite _sprite, EResourceType _resource, bool _small)
		{
			CurrencyDescription value;
			if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.CurrencyDescriptions.TryGetValue((int)_resource, out value))
			{
				_sprite.spriteName = ((!_small) ? value.IconName : value.SmallIconName);
				return true;
			}
			return false;
		}

		public static bool SetCurrenySymbol(UISprite _sprite, Resource _resource, bool _small)
		{
			return SetCurrenySymbol(_sprite, _resource.type, _small);
		}
	}
}
