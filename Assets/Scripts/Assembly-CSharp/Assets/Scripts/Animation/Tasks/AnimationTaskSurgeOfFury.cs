using Assets.Scripts.Animation.States;
using Assets.Scripts.Mission.Logic;

namespace Assets.Scripts.Animation.Tasks
{
	public class AnimationTaskSurgeOfFury : AnimationTaskBase
	{
		public class Parameters : AnimationTaskParameters
		{
			public EntityId TargetEntityId { get; set; }

			public Parameters()
				: base(AnimationTaskType.SurgeOfFury)
			{
			}
		}

		public const AnimationTaskType sTaskType = AnimationTaskType.SurgeOfFury;

		public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
		{
			Layer = AnimationTaskManager.ELayer.Base,
			ParametersTemplate = new Parameters()
		};

		public override AnimationTaskType TaskType
		{
			get
			{
				return AnimationTaskType.SurgeOfFury;
			}
		}

		public static Parameters ParametersTemplate
		{
			get
			{
				return (Parameters)Properties.ParametersTemplate;
			}
		}

		public AnimationTaskSurgeOfFury()
			: this(ParametersTemplate)
		{
		}

		public AnimationTaskSurgeOfFury(Parameters _parameters)
			: base(_parameters)
		{
			OnAddState(new AnimationStatePlaySimple(_parameters.AnimationController, new ParameterValues
			{
				AttackMeleeType = AttackMeleeType.WaveOfFury
			}, AnimationStateMachineHashes.StateMachine_WaveOfFury, "WaveOfFury"));
		}

		public override bool CanCancel()
		{
			return false;
		}
	}
}
