using System;
using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public abstract class StateMachineAnimListeningBase<T, TThis> : StateMachineUnityComponentListeningBase<T, TThis>, IAnimatorEventListener, IUpdateable, ITurnUpdateable, IStateMachineAnimListening, IStateMachine, IStateMachineAnimListening<T>, IStateMachine<T> where T : IGameEntity, IStateMachinesCarrier where TThis : StateMachineBase<T, TThis>
	{
		private readonly EventHandler<AnimationTaskManager.AnimationTaskEventArgs> m_AnimationFinishedHandler;

		private AnimationTaskType TaskType { get; set; }

		private AnimationTaskManager.ELayer TargetLayer { get; set; }

		private AnimatorEventType? AnimatorEventType { get; set; }

		private Action<bool> DelayStateAction { get; set; }

		protected StateMachineAnimListeningBase(T _entity)
			: base(_entity)
		{
			m_AnimationFinishedHandler = TaskManager_AnimationTaskExited;
		}

		protected static void InitStateMachineAnimListeningBase()
		{
			StateMachineUnityComponentListeningBase<T, TThis>.InitStateMachineUnityComponentListeningBase();
		}

		private void TaskManager_AnimationTaskExited(object sender, AnimationTaskManager.AnimationTaskEventArgs e)
		{
			if (e.AnimationTask.Layer == TargetLayer && (TaskType == AnimationTaskType.Unknown || TaskType == e.AnimationTask.TaskType))
			{
				Trigger(e.Canceled);
			}
		}

		private void Trigger(bool _canceled)
		{
			if (DelayStateAction != null)
			{
				DelayStateAction(_canceled);
			}
			StopAnimationListening();
		}

		public override void OnRegistered()
		{
			base.OnRegistered();
			DelayStateAction = null;
		}

		public override void Detach()
		{
			Trigger(false);
			base.Detach();
		}

		public Action StartAnimationListening(AnimationTaskType _taskType, AnimatorEventType _animatorEventType, AnimationTaskManager.ELayer _layer, Action<bool> _delayStateAction)
		{
			TaskType = _taskType;
			TargetLayer = _layer;
			DelayStateAction = _delayStateAction;
			Entity.UnityComponentCache.AnimationController.TaskManager.AnimationTaskExited += m_AnimationFinishedHandler;
			AnimatorEventType = _animatorEventType;
			Entity.UnityComponentCache.AnimationController.DataController.RegisterEventListener(this);
			return StopAnimationListening;
		}

		public void StopAnimationListening()
		{
			TaskType = AnimationTaskType.Unknown;
			DelayStateAction = null;
			AnimatorEventType = null;
			Entity.UnityComponentCache.AnimationController.TaskManager.AnimationTaskExited -= m_AnimationFinishedHandler;
			Entity.UnityComponentCache.AnimationController.DataController.UnregisterEventListener(this);
		}

		public bool OnAnimatorEvent(AnimatorEvent _event)
		{
			if (_event.Layer == (int)TargetLayer && AnimatorEventType.HasValue && _event.Type == AnimatorEventType)
			{
				Trigger(false);
			}
			return false;
		}
	}
}
