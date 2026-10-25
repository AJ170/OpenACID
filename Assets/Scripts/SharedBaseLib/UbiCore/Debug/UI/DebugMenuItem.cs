using UnityEngine;

namespace UbiCore.Debug.UI
{
	public abstract class DebugMenuItem : MonoBehaviour
	{
		protected DebugMenu m_debugMenu;

		internal DebugMenu DebugMenu
		{
			set
			{
				m_debugMenu = value;
			}
		}

		public abstract string GetName();

		public abstract void OnMenu();

		public abstract void OnOpen();

		public abstract void OnApply();

		protected void Toggle(ref bool value, string label)
		{
			value = GUILayout.Toggle(value, label);
		}
	}
}
