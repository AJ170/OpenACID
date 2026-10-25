using AcierProtocol;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Helpers
{
	public static class ClassLockedStateHelper
	{
		public static bool GetClassTypeLocked(Assassin.EClassTypeId _type, out ClassEntitlementDescription _desc)
		{
			bool result = false;
			if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.ClassEntitlementDescriptions.TryGetValue((int)_type, out _desc))
			{
				result = !Singleton<StateManager>.Instance.TemplateManager.AvatarDefinition.IsValidClassType(_type) || !Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(_desc.entitlementID);
			}
			return result;
		}
	}
}
