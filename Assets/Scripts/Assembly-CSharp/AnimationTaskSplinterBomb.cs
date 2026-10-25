using Assets.Scripts.Animation;

public class AnimationTaskSplinterBomb : AnimationTaskSimpleUpperbodyBase
{
	public class Parameters : AnimationTaskParameters
	{
		public Parameters()
			: base(AnimationTaskType.SplinterBomb)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.SplinterBomb;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.UpperBody,
		ParametersTemplate = new Parameters()
	};

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.SplinterBomb;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public AnimationTaskSplinterBomb()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskSplinterBomb(Parameters _parameters)
		: base(_parameters, ActionTypeUpperBody.SplinterBomb, AnimationStateHashes.State_SplinterBomb, 0.15f, 0.2f)
	{
	}
}
