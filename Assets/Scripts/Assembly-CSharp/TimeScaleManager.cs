using System.Collections.Generic;
using UbiCore.Utils;
using UnityEngine;

public static class TimeScaleManager
{
	private static List<ITimeScaleProvider> ms_multipliers = new List<ITimeScaleProvider>();

	private static List<ITimeScaleProvider> ms_slowMotions = new List<ITimeScaleProvider>();

	public static void RegisterMultiplier(ITimeScaleProvider _handler)
	{
		ms_multipliers.Add(_handler);
	}

	public static void UnregisterMultiplier(ITimeScaleProvider _handler)
	{
		ms_multipliers.Remove(_handler);
	}

	public static void RegisterSlowMotion(ITimeScaleProvider _handler)
	{
		ms_slowMotions.Add(_handler);
	}

	public static void UnregisterSlowMotion(ITimeScaleProvider _handler)
	{
		ms_slowMotions.Remove(_handler);
	}

	private static float Evaluate()
	{
		float num = 1f;
		for (int i = 0; i < ms_multipliers.Count; i++)
		{
			ITimeScaleProvider timeScaleProvider = ms_multipliers[i];
			num *= timeScaleProvider.Evaluate();
		}
		float num2;
		if (ms_slowMotions.Count > 0)
		{
			num2 = ms_slowMotions[0].Evaluate();
			for (int j = 1; j < ms_slowMotions.Count; j++)
			{
				num2 = Mathf.Min(num2, ms_slowMotions[j].Evaluate());
			}
		}
		else
		{
			num2 = 1f;
		}
		return num * num2;
	}

	private static void Refresh()
	{
		Timer instance = UbiCore.Utils.Singleton<Timer>.Instance;
		if (!instance.IsPaused())
		{
			float timeScale = Evaluate();
			instance.timeScale = timeScale;
		}
	}

	public static void Step()
	{
		Refresh();
	}
}
