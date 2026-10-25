public class AnimationPlotStateWaitForAnimationEvent : AnimationPlotStateBase, IAnimatorEventListener
{
	private AnimatorEventType m_eventType;

	private AnimationPlotEntryBase m_trigger;

	public AnimationPlotStateWaitForAnimationEvent(AnimationPlotEntryBase _trigger, AnimatorEventType _eventType)
	{
		m_trigger = _trigger;
		m_eventType = _eventType;
		m_trigger.Owner = this;
	}

	public bool OnAnimatorEvent(AnimatorEvent _event)
	{
		if (_event.Type == m_eventType && !base.Finished)
		{
			base.Finished = true;
			base.Owner.OnStateFinished(this);
		}
		return false;
	}

	public override void OnStart()
	{
		base.OnStart();
		m_trigger.Parameters.AnimationController.GetComponent<AnimatorDataController>().RegisterEventListener(this);
		m_trigger.OnStart();
	}

	public override void OnStop()
	{
		base.OnStop();
		m_trigger.Parameters.AnimationController.GetComponent<AnimatorDataController>().UnregisterEventListener(this);
	}
}
