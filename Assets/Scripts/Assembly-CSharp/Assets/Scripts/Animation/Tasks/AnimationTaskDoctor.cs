using Assets.Scripts.Animation.States;
using Assets.Scripts.Mission.Logic;

namespace Assets.Scripts.Animation.Tasks
{
	public class AnimationTaskDoctor : AnimationTaskBase
	{
		public class Parameters : AnimationTaskParameters
		{
			public EType Type { get; set; }

			public EntityId TargetEntityId { get; set; }

			public Parameters()
				: base(AnimationTaskType.Doctor)
			{
			}
		}

		public enum EType
		{
			Accept = 0,
			Reject = 1
		}

		public const AnimationTaskType sTaskType = AnimationTaskType.Doctor;

		public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
		{
			Layer = AnimationTaskManager.ELayer.Base,
			ParametersTemplate = new Parameters()
		};

		private readonly EType m_Type;

		public override AnimationTaskType TaskType
		{
			get
			{
				return AnimationTaskType.Doctor;
			}
		}

		public static Parameters ParametersTemplate
		{
			get
			{
				return (Parameters)Properties.ParametersTemplate;
			}
		}

		private AnimatorStateMachineStatic.StateHashValue AnimHash
		{
			get
			{
				switch (m_Type)
				{
				case EType.Accept:
					return AnimationStateHashes.State_DoctorIdle_DoctorThankBuy;
				case EType.Reject:
					return AnimationStateHashes.State_DoctorIdle_DoctorNoBuy;
				default:
					return AnimationStateHashes.State_DoctorIdle_DoctorNoBuy;
				}
			}
		}

		private ReactionType ReactionType
		{
			get
			{
				switch (m_Type)
				{
				case EType.Accept:
					return ReactionType.DoctorThankBuy;
				case EType.Reject:
					return ReactionType.DoctorNoBuy;
				default:
					return ReactionType.DoctorNoBuy;
				}
			}
		}

		public AnimationTaskDoctor()
			: this(ParametersTemplate)
		{
		}

		public AnimationTaskDoctor(Parameters _parameters)
			: base(_parameters)
		{
			m_Type = _parameters.Type;
			AnimationStatePlaySimple animationStatePlaySimple = OnAddState(new AnimationStatePlaySimple(_parameters.AnimationController, new ParameterValues
			{
				ReactionType = ReactionType
			}, AnimHash, "Doctor"));
			AutoAnimationRotation autoAnimationRotation = animationStatePlaySimple.AddAutoAnimationRotation(_parameters.TargetEntityId.GameEntity);
			autoAnimationRotation.Speed = 2f;
		}

		public override bool CanCancel()
		{
			return false;
		}
	}
}
