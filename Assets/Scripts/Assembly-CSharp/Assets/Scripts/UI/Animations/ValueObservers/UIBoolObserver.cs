using System;

namespace Assets.Scripts.UI.Animations.ValueObservers
{
	public class UIBoolObserver : IAnimationObserver
	{
		public event Action OnActive;

		public event Action OnDisabled;

		private void OnEnable()
		{
			if (this.OnActive != null)
			{
				this.OnActive();
			}
		}

		private void OnDisable()
		{
			if (this.OnDisabled != null)
			{
				this.OnDisabled();
			}
		}
	}
}
