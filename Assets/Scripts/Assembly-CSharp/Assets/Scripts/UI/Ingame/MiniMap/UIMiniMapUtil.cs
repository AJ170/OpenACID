using System;
using AcierProtocol;
using SharedBaseLib.Core;

namespace Assets.Scripts.UI.Ingame.MiniMap
{
	public class UIMiniMapUtil
	{
		private static BBDictionary<int, string> s_minimap_icon_names;

		static UIMiniMapUtil()
		{
			Array values = Enum.GetValues(typeof(EMapIconType));
			s_minimap_icon_names = new BBDictionary<int, string>(values.Length - 1);
			for (int i = 0; i < values.Length; i++)
			{
				EMapIconType eMapIconType = (EMapIconType)(int)values.GetValue(i);
				if (eMapIconType >= EMapIconType.Invalid_EMapIconType)
				{
					string text = eMapIconType.ToString();
					text = text.Replace("_EMapIconType", string.Empty);
					s_minimap_icon_names.Add((int)eMapIconType, "icon_minimap_" + text.ToLower());
				}
			}
		}

		public static int GetDepthFromMapIconType(EMapIconType _type, bool _isObjective, bool _isPlayerCharacter)
		{
			int num = ((!_isObjective) ? 2 : 3);
			switch (_type)
			{
			case EMapIconType.Player_EMapIconType:
				num = ((!_isPlayerCharacter) ? num : 4);
				break;
			case EMapIconType.Enemy_EMapIconType:
				num = 1;
				break;
			}
			return num;
		}

		public static void SetSpriteByMapIconType(EMapIconType _type, UISprite _sprite, bool _isObjective, bool _isPlayerCharacter)
		{
			if (!(_sprite == null) && _type != 0)
			{
				string text = s_minimap_icon_names[(int)_type];
				UISpriteData sprite = _sprite.atlas.GetSprite(text);
				if (sprite != null)
				{
					_sprite.spriteName = text;
				}
				int depth = (_sprite.depth = GetDepthFromMapIconType(_type, _isObjective, _isPlayerCharacter));
				_sprite.depth = depth;
				_sprite.MakePixelPerfect();
			}
		}
	}
}
