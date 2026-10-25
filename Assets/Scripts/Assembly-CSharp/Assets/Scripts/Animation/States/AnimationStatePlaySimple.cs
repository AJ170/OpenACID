using Assets.Scripts.Mission.Logic;

namespace Assets.Scripts.Animation.States
{
	public class AnimationStatePlaySimple : AnimationStatePlayGeneric
	{
		public AnimationStatePlaySimple(AnimationController _animationController, ParameterValues _parameters, AnimatorStateMachineStatic.StateHashValue _stateHash, string _name, int _loopCount = 1)
			: base(_animationController, _parameters, null, new StateEnteredTrigger(_stateHash), new StateExitedTrigger(_stateHash), _name, _loopCount)
		{
		}

		public AnimationStatePlaySimple(AnimationController _animationController, ParameterValues _parameters, AnimatorStateMachineStatic.StateMachineHashValue _stateMachineHash, string _name, int _loopCount = 1)
			: base(_animationController, _parameters, null, new StateMachineEnteredTrigger(_stateMachineHash), new StateMachineExitedTrigger(_stateMachineHash), _name, _loopCount)
		{
		}

		public AnimationStatePlaySimple(AnimationController _animationController, ParameterValues _parameters, EntityId _startEventEntityId, AnimatorEventType _startEventType, AnimatorStateMachineStatic.StateHashValue _stateHash, string _name, int _loopCount = 1)
			: base(_animationController, _parameters, new AnimatorEventTrigger(_startEventEntityId, _startEventType), new StateEnteredTrigger(_stateHash), new StateExitedTrigger(_stateHash), _name, _loopCount)
		{
		}

		public AnimationStatePlaySimple(AnimationController _animationController, ParameterValues _parameters, EntityId _startEventEntityId, AnimatorEventType _startEventType, AnimatorStateMachineStatic.StateMachineHashValue _stateMachineHash, string _name, int _loopCount = 1)
			: base(_animationController, _parameters, new AnimatorEventTrigger(_startEventEntityId, _startEventType), new StateMachineEnteredTrigger(_stateMachineHash), new StateMachineExitedTrigger(_stateMachineHash), _name, _loopCount)
		{
		}
	}
}
