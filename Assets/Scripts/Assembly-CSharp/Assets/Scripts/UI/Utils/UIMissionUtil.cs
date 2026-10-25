using AcierProtocol;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Utils
{
	internal class UIMissionUtil
	{
		public static void UpdateMissionIconByMissionObjectiveType(UISprite _icon, EMissionKind _missionKind, bool _isElite, UISprite _background = null)
		{
			MissionKindDescription value;
			if (_icon != null && UbiCore.Utils.Singleton<UIDataMapper>.Instance.MissionKindDescriptions.TryGetValue((int)_missionKind, out value))
			{
				_icon.spriteName = value.IconPath;
				if (_background != null)
				{
					_background.spriteName = ((!_isElite) ? "aUI_fra_missionType" : "aUI_fra_Elite_Token");
				}
			}
		}

		public static bool UpdateMissionIconByMissionObjectiveType(UISprite _icon, EObjectiveType _objectiveType, bool _isElite, UISprite _background = null)
		{
			bool result = false;
			ObjectiveDescription value;
			if (_icon != null && UbiCore.Utils.Singleton<UIDataMapper>.Instance.ObjectiveDescriptions.TryGetValue((int)_objectiveType, out value))
			{
				_icon.spriteName = value.IconPath;
				result = true;
			}
			if (_background != null)
			{
				if (!_isElite)
				{
					ObjectiveDescription value2;
					if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.ObjectiveDescriptions.TryGetValue((int)_objectiveType, out value2))
					{
						_background.spriteName = value2.IconBackgroundPath;
						result = true;
					}
				}
				else
				{
					_background.spriteName = "aUI_fra_Elite_Token";
				}
			}
			return result;
		}
	}
}
