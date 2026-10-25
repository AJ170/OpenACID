using System;

namespace Assets.Scripts.UI.Behaviours
{
	public sealed class UIDropDownItemConfiguration
	{
		public EFilter FilterType;

		public object Data;

		public string IconName;

		public string IconNameDisabled;

		public string Label;

		public bool Selected;

		public bool IsEnabled;

		public Action<bool, UIDropDownItemConfiguration> OnSelectionChange;

		public Action<bool, UIDropDownItemConfiguration> OnStateChangedFinished;
	}
}
