using System;
using AcierProtocol;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Screens.ModuleScreen
{
	public static class ModuleFormatHelper
	{
		public static string BuildTimeString(this ModuleData _module)
		{
			switch (_module.durationType)
			{
			case EDurationType.Time:
			{
				long milliSeconds = _module.ComputeRemainingMilliSeconds();
				if (_module.expireTimeStamp <= 0)
				{
					milliSeconds = (long)_module.duration * 1000L;
				}
				return UIDuration.FromMilliSeconds(milliSeconds).ToString();
			}
			case EDurationType.MissionCount:
			{
				long num = _module.expireTimeStamp;
				if (num <= 0)
				{
					num = _module.duration;
				}
				return LocalizationManager.Instance.Get(5410, new StringMapper { { "Count", num } });
			}
			default:
				return null;
			}
		}

		public static long ComputeRemainingMilliSeconds(this ModuleData _module)
		{
			if (_module == null)
			{
				return 0L;
			}
			if (_module.durationType == EDurationType.MissionCount)
			{
				return _module.duration;
			}
			long val = (long)(ServerTimeManager.ToLocalTimeMilliseconds(_module.expireTimeStamp) - Singleton<StateManager>.Instance.ServerTimeManager.Now).TotalMilliseconds;
			return Math.Max(val, 0L);
		}

		public static long ComputeModuleExpireInMilliSeconds(this ModuleData _module)
		{
			if (_module == null || _module.durationType == EDurationType.MissionCount)
			{
				return 0L;
			}
			long val = (long)(ServerTimeManager.ToLocalTimeMilliseconds(_module.expireTimeStamp) - Singleton<StateManager>.Instance.ServerTimeManager.Now).TotalMilliseconds;
			return Math.Max(val, 0L);
		}

		public static int BuildUnitValue(this ModuleData _module)
		{
			if (_module == null || _module.modifications == null || _module.modifications.Length == 0 || _module.modifications[0] == null)
			{
				return 0;
			}
			return _module.modifications[0].effectValue;
		}

		public static string BuildUnitStringRaw(this ModuleData _module, int _id)
		{
			Modification modification = _module.modifications[_id];
			return LocalizationManager.Instance.Get(modification.BuildUnitRawKey());
		}

		public static StringId BuildUnitRawKey(this Modification _modification)
		{
			switch (_modification.moduleModifier)
			{
			case EModuleModifier.ClientModifier:
			{
				Assets.Scripts.Mission.Definitions.SkillData.ISkillDefinition definition = UbiCore.Utils.Singleton<Assets.Scripts.Mission.Definitions.SkillData>.Instance.GetDefinition(_modification.skillGuid, 1);
				return definition.NameLocaKey.Key;
			}
			case EModuleModifier.ItemFind:
				return 5804;
			case EModuleModifier.SCReward:
				return 5803;
			case EModuleModifier.SkillTrainingTimeFactor:
				return 5802;
			case EModuleModifier.XPReward:
				return 5801;
			case EModuleModifier.HTReward:
				return 9700;
			default:
				return StringId.Invalid;
			}
		}

		public static InventoryElement RetrieveSetModule()
		{
			Assassin currentAssassin = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin;
			if (currentAssassin == null)
			{
				return null;
			}
			InventoryElement elementForSlot = currentAssassin.GetElementForSlot(EInventorySlot.SlotModule);
			if (elementForSlot == null)
			{
				return null;
			}
			if (elementForSlot.moduleData.ComputeRemainingMilliSeconds() <= 0)
			{
				return null;
			}
			return elementForSlot;
		}
	}
}
