using Assets.Scripts.Animation;
using UbiCore.Messenger;
using UnityEngine;

public class AnimationTaskJumpDownAnywhere : AnimationTaskBase, IAnimation, IParkourAnimation
{
	public class Parameters : AnimationTaskParameters
	{
		public JumpAnywhereEvent JumpAnywhereEvent { get; set; }

		public Parameters()
			: base(AnimationTaskType.JumpDownAnywhere)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.JumpDownAnywhere;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	private JumpAnywhereEvent m_event;

	private Vector3 m_startPosition;

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.JumpDownAnywhere;
		}
	}

	public float Height { get; private set; }

	public Vector3 Destination { get; private set; }

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public Vector3 LandPosition
	{
		get
		{
			return m_event.LandPosition;
		}
	}

	public AnimationTaskJumpDownAnywhere()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskJumpDownAnywhere(Parameters _parameters)
		: base(_parameters)
	{
		m_event = _parameters.JumpAnywhereEvent;
		m_startPosition = AnimationController.NextPosition;
		OnAddState(new AnimationStateJumpDown(_parameters.AnimationController, m_startPosition, m_event.LandPosition));
		Height = m_startPosition.y - m_event.LandPosition.y;
		Destination = m_event.LandPosition;
	}

	public override bool CanCancel()
	{
		return false;
	}
}
