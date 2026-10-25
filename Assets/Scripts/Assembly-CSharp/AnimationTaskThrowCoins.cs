using Assets.Scripts.Animation;

public class AnimationTaskThrowCoins : AnimationTaskSimpleUpperbodyBase
{
	public class Parameters : AnimationTaskParameters
	{
		public Parameters()
			: base(AnimationTaskType.ThrowCoins)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.ThrowCoins;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.UpperBody,
		ParametersTemplate = new Parameters()
	};

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.ThrowCoins;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public AnimationTaskThrowCoins()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskThrowCoins(Parameters _parameters)
		: base(_parameters, ActionTypeUpperBody.CoinThrow, AnimationStateHashes.State_ThrowCoins, 0.15f, 0.8f)
	{
	}
}
