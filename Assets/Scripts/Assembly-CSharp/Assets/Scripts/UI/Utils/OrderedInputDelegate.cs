using UnityEngine;

namespace Assets.Scripts.UI.Utils
{
	public class OrderedInputDelegate : MonoBehaviour
	{
		[SerializeField]
		[HideInInspector]
		public EventDelegate[] onClickDelegates;

		[SerializeField]
		[HideInInspector]
		public EventDelegate[] onPointOverDelegates;

		[SerializeField]
		[HideInInspector]
		public EventDelegate[] onPointOutDelegates;

		[SerializeField]
		[HideInInspector]
		public EventDelegate[] onPressDelegates;

		[HideInInspector]
		[SerializeField]
		public EventDelegate[] onReleaseDelegates;

		[SerializeField]
		[HideInInspector]
		public EventDelegate[] onSelectDelegates;

		[SerializeField]
		[HideInInspector]
		public EventDelegate[] onDeselectDelegates;

		private void OnClick()
		{
			ExecuteDelegates(onClickDelegates);
		}

		private void OnHover(bool _isHover)
		{
			ExecuteDelegates((!_isHover) ? onPointOutDelegates : onPointOverDelegates);
		}

		private void OnPress(bool _isPressed)
		{
			ExecuteDelegates((!_isPressed) ? onReleaseDelegates : onPressDelegates);
		}

		private void OnSelect(bool _isSelected)
		{
			ExecuteDelegates((!_isSelected) ? onDeselectDelegates : onSelectDelegates);
		}

		private void ExecuteDelegates(EventDelegate[] _delegates)
		{
			if (!base.enabled || _delegates == null)
			{
				return;
			}
			foreach (EventDelegate eventDelegate in _delegates)
			{
				if (eventDelegate.isValid)
				{
					eventDelegate.Execute();
				}
			}
		}
	}
}
