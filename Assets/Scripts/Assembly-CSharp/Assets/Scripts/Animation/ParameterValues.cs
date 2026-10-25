namespace Assets.Scripts.Animation
{
	public class ParameterValues
	{
		public ActionType? ActionType;

		public ActionTypeUpperBody? ActionTypeUpperBody;

		public GeneralState? GeneralState;

		public MovementState? MovementState;

		public ReactionType? ReactionType;

		public IdleType? IdleType;

		public AttackMeleeType? AttackMeleeType;

		public AttackRangeType? AttackRangeType;

		public int? AnimatorNpcId;

		public float? TargetDirectionY;

		public void Read(AnimationController _controller, ParameterValues _template)
		{
			ActionType? actionType = _template.ActionType;
			if (actionType.HasValue)
			{
				ActionType = _controller.Animator.GetCurrentActionType();
			}
			ActionTypeUpperBody? actionTypeUpperBody = _template.ActionTypeUpperBody;
			if (actionTypeUpperBody.HasValue)
			{
				ActionTypeUpperBody = _controller.Animator.GetCurrentActionTypeUpperBody();
			}
			GeneralState? generalState = _template.GeneralState;
			if (generalState.HasValue)
			{
				GeneralState = _controller.Animator.GetCurrentGeneralState();
			}
			MovementState? movementState = _template.MovementState;
			if (movementState.HasValue)
			{
				MovementState = _controller.Animator.GetCurrentMovementState();
			}
			ReactionType? reactionType = _template.ReactionType;
			if (reactionType.HasValue)
			{
				ReactionType = _controller.Animator.GetCurrentReactionType();
			}
			IdleType? idleType = _template.IdleType;
			if (idleType.HasValue)
			{
				IdleType = _controller.Animator.GetCurrentIdleType();
			}
			AttackMeleeType? attackMeleeType = _template.AttackMeleeType;
			if (attackMeleeType.HasValue)
			{
				AttackMeleeType = _controller.Animator.GetCurrentAttackMeleeType();
			}
			AttackRangeType? attackRangeType = _template.AttackRangeType;
			if (attackRangeType.HasValue)
			{
				AttackRangeType = _controller.Animator.GetCurrentAttackRangeType();
			}
			int? animatorNpcId = _template.AnimatorNpcId;
			if (animatorNpcId.HasValue)
			{
				AnimatorNpcId = _controller.Animator.GetCurrentNpcId();
			}
			float? targetDirectionY = _template.TargetDirectionY;
			if (targetDirectionY.HasValue)
			{
				TargetDirectionY = _controller.Animator.GetTargetDirectionY();
			}
		}

		public void Write(AnimationController _controller)
		{
			ActionType? actionType = ActionType;
			if (actionType.HasValue)
			{
				_controller.Animator.SetCurrentActionType(ActionType.Value);
			}
			ActionTypeUpperBody? actionTypeUpperBody = ActionTypeUpperBody;
			if (actionTypeUpperBody.HasValue)
			{
				_controller.Animator.SetCurrentActionTypeUpperBody(ActionTypeUpperBody.Value);
			}
			GeneralState? generalState = GeneralState;
			if (generalState.HasValue)
			{
				_controller.Animator.SetCurrentGeneralState(GeneralState.Value);
			}
			MovementState? movementState = MovementState;
			if (movementState.HasValue)
			{
				_controller.Animator.SetCurrentMovementState(MovementState.Value);
			}
			ReactionType? reactionType = ReactionType;
			if (reactionType.HasValue)
			{
				_controller.Animator.SetCurrentReactionType(ReactionType.Value);
			}
			IdleType? idleType = IdleType;
			if (idleType.HasValue)
			{
				_controller.Animator.SetCurrentIdleType(IdleType.Value);
			}
			AttackMeleeType? attackMeleeType = AttackMeleeType;
			if (attackMeleeType.HasValue)
			{
				_controller.Animator.SetCurrentAttackMeleeType(AttackMeleeType.Value);
			}
			AttackRangeType? attackRangeType = AttackRangeType;
			if (attackRangeType.HasValue)
			{
				_controller.Animator.SetCurrentAttackRangeType(AttackRangeType.Value);
			}
			int? animatorNpcId = AnimatorNpcId;
			if (animatorNpcId.HasValue)
			{
				_controller.Animator.SetCurrentNpcId(AnimatorNpcId.Value);
			}
			float? targetDirectionY = TargetDirectionY;
			if (targetDirectionY.HasValue)
			{
				_controller.Animator.SetTargetDirectionY(TargetDirectionY.Value);
			}
		}

		public ParameterValues Clone()
		{
			ParameterValues parameterValues = new ParameterValues();
			parameterValues.ActionType = ActionType;
			parameterValues.ActionTypeUpperBody = ActionTypeUpperBody;
			parameterValues.GeneralState = GeneralState;
			parameterValues.MovementState = MovementState;
			parameterValues.IdleType = IdleType;
			parameterValues.AttackMeleeType = AttackMeleeType;
			parameterValues.AttackRangeType = AttackRangeType;
			parameterValues.AnimatorNpcId = AnimatorNpcId;
			parameterValues.TargetDirectionY = TargetDirectionY;
			return parameterValues;
		}
	}
}
