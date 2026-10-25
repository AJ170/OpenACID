using Assets.Scripts.Animation.States;
using Assets.Scripts.Mission.Utilities.AutoUpdating;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Animation.Tasks
{
	public class AnimationTaskMissionDeliverInstigator : AnimationTaskBase
	{
		public class Parameters : AnimationTaskParameters
		{
			public Vector3 DeliverTargetPosition { get; set; }

			public Parameters()
				: base(AnimationTaskType.MissionDeliverInstigator)
			{
			}
		}

		public const AnimationTaskType sTaskType = AnimationTaskType.MissionDeliverInstigator;

		public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
		{
			Layer = AnimationTaskManager.ELayer.Base,
			ParametersTemplate = new Parameters()
		};

		private AutoScalarInterpolation m_positioningScalar;

		private Vector3 m_deliverTargetPosition = Vector3.zero;

		private Vector3 m_initialPosition = Vector3.zero;

		private Vector3 m_slideVector = Vector3.zero;

		public override AnimationTaskType TaskType
		{
			get
			{
				return AnimationTaskType.MissionDeliverInstigator;
			}
		}

		public static Parameters ParametersTemplate
		{
			get
			{
				return (Parameters)Properties.ParametersTemplate;
			}
		}

		public AnimationTaskMissionDeliverInstigator()
			: this(ParametersTemplate)
		{
		}

		public AnimationTaskMissionDeliverInstigator(Parameters _parameters)
			: base(_parameters)
		{
			m_deliverTargetPosition = _parameters.DeliverTargetPosition;
			m_initialPosition = AnimationController.NextPosition;
			OnAddState(new AnimationStatePlaySimple(_parameters.AnimationController, new ParameterValues
			{
				ActionType = ActionType.DeliverItem
			}, AnimationStateMachineHashes.StateMachine_Deliver, "Deliver"));
		}

		public override bool CanCancel()
		{
			return false;
		}

		public override void OnEnter()
		{
			base.OnEnter();
			Vector3 vector = m_deliverTargetPosition - AnimationController.NextPosition;
			if (Mathf.Abs(vector.magnitude - 1.5f) > 0.1f)
			{
				float num = vector.magnitude - 1.5f;
				vector.Normalize();
				Vector3 position = AnimationController.NextPosition + vector * num;
				NavMeshHit _hit;
				if (NavigationHelper.CascadingSamplePosition(position, out _hit, AnimationController.Agent.areaMask))
				{
					m_slideVector = _hit.position - AnimationController.NextPosition;
					m_positioningScalar = new AutoScalarInterpolation();
					m_positioningScalar.From = 0f;
					m_positioningScalar.To = 1f;
					m_positioningScalar.Speed = Mathf.Abs(num);
					m_positioningScalar.Start();
				}
			}
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			if (m_positioningScalar != null)
			{
				AnimationController.NextPosition = m_initialPosition + m_slideVector * m_positioningScalar.Scalar;
			}
		}

		public override void OnExit(bool _isCanceled)
		{
			base.OnExit(_isCanceled);
			if (m_positioningScalar != null && m_positioningScalar.IsRunning)
			{
				m_positioningScalar.Stop();
				m_positioningScalar = null;
			}
		}
	}
}
