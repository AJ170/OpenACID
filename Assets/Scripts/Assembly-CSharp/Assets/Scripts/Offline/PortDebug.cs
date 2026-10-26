using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Offline
{
	// Port diagnostics, editor only: combat behaviour changes, enemy attack scheduling and animator transitions the
	// game's state tables don't know. Lines start with "[port]" (Logs/playtest.txt keeps them).
	public static class PortDebug
	{
		private static readonly HashSet<int> s_reported = new HashSet<int>();

		private static readonly Dictionary<string, float> s_throttle = new Dictionary<string, float>();

		public static bool Enabled
		{
			get
			{
				return Application.isEditor;
			}
		}

		public static void Log(string _message)
		{
			if (Enabled)
			{
				Debug.Log("[port] " + _message);
			}
		}

		// True the first time a key is seen.
		public static bool Once(int _key)
		{
			return Enabled && s_reported.Add(_key);
		}

		// True at most once per _seconds for a key.
		public static bool Every(string _key, float _seconds)
		{
			if (!Enabled)
			{
				return false;
			}
			float last;
			if (s_throttle.TryGetValue(_key, out last) && Time.realtimeSinceStartup - last < _seconds)
			{
				return false;
			}
			s_throttle[_key] = Time.realtimeSinceStartup;
			return true;
		}
	}
}
