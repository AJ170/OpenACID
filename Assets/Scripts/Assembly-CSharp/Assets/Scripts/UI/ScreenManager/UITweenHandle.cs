namespace Assets.Scripts.UI.ScreenManager
{
	public class UITweenHandle
	{
		public UITweener InTween { get; private set; }

		public UITweener OutTween { get; private set; }

		public UITweenHandle()
		{
			InTween = null;
			OutTween = null;
		}

		public UITweenHandle(UITweener _inTweener, UITweener _outTweener)
		{
			InTween = _inTweener;
			OutTween = _outTweener;
		}
	}
}
