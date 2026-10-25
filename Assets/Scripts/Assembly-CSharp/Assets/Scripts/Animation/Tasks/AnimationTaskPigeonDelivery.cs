using Assets.Scripts.Animation.States;
using Assets.Scripts.Mission.Logic;
using Database;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Animation.Tasks
{
	public class AnimationTaskPigeonDelivery : AnimationTaskBase, IAnimatorEventListener
	{
		public class Parameters : AnimationTaskParameters
		{
			public EntityId PigeonryId { get; set; }

			public Parameters()
				: base(AnimationTaskType.PigeonDelivery)
			{
			}
		}

		public const AnimationTaskType sTaskType = AnimationTaskType.PigeonDelivery;

		public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
		{
			Layer = AnimationTaskManager.ELayer.Base,
			ParametersTemplate = new Parameters()
		};

		private GameObject m_Pigeon;

		public override AnimationTaskType TaskType
		{
			get
			{
				return AnimationTaskType.PigeonDelivery;
			}
		}

		public static Parameters ParametersTemplate
		{
			get
			{
				return (Parameters)Properties.ParametersTemplate;
			}
		}

		public AnimationTaskPigeonDelivery()
			: this(ParametersTemplate)
		{
		}

		public AnimationTaskPigeonDelivery(Parameters _parameters)
			: base(_parameters)
		{
			AnimationStatePlaySimple animationStatePlaySimple = OnAddState(new AnimationStatePlaySimple(_parameters.AnimationController, new ParameterValues
			{
				ActionType = ActionType.PigeonDelivery
			}, AnimationStateMachineHashes.StateMachine_PigeonDelivery, "PigeonDelivery"));
			AnimationController.DataController.RegisterEventListener(this);
			AutoAnimationRotation autoAnimationRotation = animationStatePlaySimple.AddAutoAnimationRotation(_parameters.PigeonryId.GetEntity<IPositionEntity>());
			autoAnimationRotation.Speed = 2f;
		}

		public override bool CanCancel()
		{
			return false;
		}

		private void SpawnPigeon()
		{
			if (m_Pigeon == null)
			{
				ItemDef item = Main.Instance.GameDB.GetItem<ItemDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_envInPigeonDelivery);
				Object @object = item.ItemPrefab.GetObject<Object>();
				m_Pigeon = Object.Instantiate(@object) as GameObject;
				ItemHelper.SetupLOD(m_Pigeon, 0.05f);
				m_Pigeon.transform.parent = AnimationController.transform;
				m_Pigeon.transform.localPosition = Vector3.zero;
				m_Pigeon.transform.localRotation = Quaternion.AngleAxis(90f, Vector3.up);
				SkinnedMeshRenderer componentInChildren = m_Pigeon.GetComponentInChildren<SkinnedMeshRenderer>();
				Animator componentInChildren2 = m_Pigeon.GetComponentInChildren<Animator>();
				if (componentInChildren != null && componentInChildren2 != null)
				{
					componentInChildren2.cullingMode = AnimatorCullingMode.AlwaysAnimate;
				}
				Rendering.ConfigureLightProbes(m_Pigeon);
			}
		}

		public bool OnAnimatorEvent(AnimatorEvent _event)
		{
			AnimatorEventType type = _event.Type;
			if (type == AnimatorEventType.ReactionStart)
			{
				SpawnPigeon();
			}
			return false;
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			AnimationHelper.StayOnNavMesh(base.Owner);
		}

		public override void OnDispose()
		{
			base.OnDispose();
			if (m_Pigeon != null)
			{
				Object.Destroy(m_Pigeon);
				m_Pigeon = null;
			}
			AnimationController.DataController.UnregisterEventListener(this);
		}
	}
}
