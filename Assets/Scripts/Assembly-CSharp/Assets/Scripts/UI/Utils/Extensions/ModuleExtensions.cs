using System;
using AcierProtocol;
using Assets.Scripts.UI.Screens.ModuleScreen;
using Assets.Scripts.UI.State;
using UnityEngine;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class ModuleExtensions
	{
		public static float RemainingPercentage(this ModuleData _module)
		{
			float num = 0f;
			switch (_module.durationType)
			{
			case EDurationType.Time:
				if (_module.expireTimeStamp != 0L)
				{
					DateTime now = Singleton<StateManager>.Instance.ServerTimeManager.Now;
					DateTime dateTime = ServerTimeManager.ToLocalTimeMilliseconds(_module.expireTimeStamp);
					double totalSeconds = (dateTime - now).TotalSeconds;
					num = Mathf.Max(0f, (float)totalSeconds / (float)_module.duration);
				}
				else
				{
					num = 1f;
				}
				break;
			case EDurationType.MissionCount:
				num = 1f - (float)(_module.duration - _module.expireTimeStamp) / (float)_module.duration;
				break;
			}
			return 1f - num;
		}

		public static bool Expired(this ModuleData _module)
		{
			return _module.ComputeRemainingMilliSeconds() <= 0;
		}
	}
}
