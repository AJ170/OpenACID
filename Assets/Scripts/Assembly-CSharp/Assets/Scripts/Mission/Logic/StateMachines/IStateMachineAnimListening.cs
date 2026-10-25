using System;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public interface IStateMachineAnimListening : IAnimatorEventListener
	{
		Action StartAnimationListening(AnimationTaskType _taskType, AnimatorEventType _animatorEventType, AnimationTaskManager.ELayer _layer, Action<bool> _delayStateAction);

		void StopAnimationListening();
	}
	public interface IStateMachineAnimListening<out TEntity> : IAnimatorEventListener, IUpdateable, ITurnUpdateable, IStateMachineAnimListening, IStateMachine, IStateMachine<TEntity> where TEntity : IGameEntity
	{
	}
}
