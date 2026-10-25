using System.Collections.Generic;
using Assets.Scripts.Database;
using Assets.Scripts.UI.Interfaces;
using UbiCore.Utils;

namespace Assets.Scripts.UI.State
{
	public class PersistentStateManager : IInitialized
	{
		public void Initialize()
		{
			Singleton<StateManager>.Instance.ProfileManager.OnLevelUp += ProfileManagerOnOnLevelUp;
		}

		public void Clear()
		{
			Singleton<StateManager>.Instance.ProfileManager.OnLevelUp -= ProfileManagerOnOnLevelUp;
		}

		private void ProfileManagerOnOnLevelUp(int _old, int _new)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("old", _old.ToString());
			dictionary.Add("new", _new.ToString());
			dictionary.Add("seen", "false");
			Dictionary<string, object> value = dictionary;
			UbiCore.Utils.Singleton<LocalStorage>.Instance.GPSetValue("level_up", value);
		}
	}
}
