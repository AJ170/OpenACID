using System.Collections.Generic;

namespace Assets.Scripts.UI.Utils
{
	public class ModuleNotificationManager
	{
		public List<EventDelegate> onModulesChanged = new List<EventDelegate>();
	}
}
