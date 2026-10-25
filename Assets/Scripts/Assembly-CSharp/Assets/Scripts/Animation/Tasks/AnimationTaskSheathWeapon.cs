namespace Assets.Scripts.Animation.Tasks
{
	public class AnimationTaskSheathWeapon : AnimationTaskSimpleUpperbodyBase
	{
		public class Parameters : AnimationTaskParameters
		{
			public Parameters()
				: base(AnimationTaskType.SheathWeapon)
			{
			}
		}

		public const AnimationTaskType sTaskType = AnimationTaskType.SheathWeapon;

		public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
		{
			Layer = AnimationTaskManager.ELayer.UpperBody,
			ParametersTemplate = new Parameters()
		};

		public override AnimationTaskType TaskType
		{
			get
			{
				return AnimationTaskType.SheathWeapon;
			}
		}

		public static Parameters ParametersTemplate
		{
			get
			{
				return (Parameters)Properties.ParametersTemplate;
			}
		}

		public AnimationTaskSheathWeapon()
			: this(ParametersTemplate)
		{
		}

		public AnimationTaskSheathWeapon(Parameters _parameters)
			: base(_parameters, ActionTypeUpperBody.SheathWeapon, AnimationStateHashes.State_Sheath, 0.2f, 0.3f)
		{
		}

		public void SheathInstant()
		{
			base.IsFinished = true;
		}
	}
}
