using System;

namespace Assets.Scripts.UI.Animations.ValueObservers
{
	public abstract class IValueObserver : IAnimationObserver
	{
		public Action<float> OnObserverEnable;

		public Action<float> OnObserverDisable;

		protected abstract float Value { get; }

		private void OnEnable()
		{
			if (OnObserverEnable != null)
			{
				OnObserverEnable(Value);
			}
		}

		private void OnDisable()
		{
			if (OnObserverDisable != null)
			{
				OnObserverDisable(Value);
			}
		}
	}
}
