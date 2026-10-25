public static class TweenerExtensions
{
	public static void RewindAndPlay(this UITweener _tweener, EventDelegate.Callback _callback = null, bool _oneShot = false)
	{
		_tweener.enabled = true;
		_tweener.ResetToBeginning();
		if (_callback != null)
		{
			EventDelegate.Add(_tweener.onFinished, _callback, _oneShot);
		}
		_tweener.PlayForward();
	}
}
