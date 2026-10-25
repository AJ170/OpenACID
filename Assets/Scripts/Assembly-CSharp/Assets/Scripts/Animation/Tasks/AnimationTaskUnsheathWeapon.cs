namespace Assets.Scripts.Animation.Tasks
{
	public class AnimationTaskUnsheathWeapon : AnimationTaskSimpleUpperbodyBase
	{
		public class Parameters : AnimationTaskParameters
		{
			public Parameters()
				: base(AnimationTaskType.UnsheathWeapon)
			{
			}
		}

		public const AnimationTaskType sTaskType = AnimationTaskType.UnsheathWeapon;

		public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
		{
			Layer = AnimationTaskManager.ELayer.UpperBody,
			ParametersTemplate = new Parameters()
		};

		public override AnimationTaskType TaskType
		{
			get
			{
				return AnimationTaskType.UnsheathWeapon;
			}
		}

		public static Parameters ParametersTemplate
		{
			get
			{
				return (Parameters)Properties.ParametersTemplate;
			}
		}

		public AnimationTaskUnsheathWeapon()
			: this(ParametersTemplate)
		{
		}

		public AnimationTaskUnsheathWeapon(Parameters _parameters)
			: base(_parameters, ActionTypeUpperBody.UnsheathWeapon, AnimationStateHashes.State_Unsheath, 0.2f, 0.3f)
		{
		}
	}
}
