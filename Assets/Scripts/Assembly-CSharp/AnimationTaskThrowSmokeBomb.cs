using Assets.Scripts.Animation;

public class AnimationTaskThrowSmokeBomb : AnimationTaskSimpleUpperbodyBase
{
	public class Parameters : AnimationTaskParameters
	{
		public Parameters()
			: base(AnimationTaskType.ThrowSmokeBomb)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.ThrowSmokeBomb;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.UpperBody,
		ParametersTemplate = new Parameters()
	};

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.ThrowSmokeBomb;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public AnimationTaskThrowSmokeBomb()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskThrowSmokeBomb(Parameters _parameters)
		: base(_parameters, ActionTypeUpperBody.ThrowSmokeBomb, AnimationStateHashes.State_ThrowSmokeBomb, 0.3f, 0.7f)
	{
	}
}
