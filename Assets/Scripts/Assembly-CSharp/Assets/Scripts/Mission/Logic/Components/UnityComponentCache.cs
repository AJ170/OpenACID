using Assets.Scripts.Mission.Logic.Entities;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class UnityComponentCache : Component
	{
		private NavMeshAgent m_navMeshAgent;

		private Animator m_animator;

		private AnimationController m_animationController;

		private Transform m_transform;

		private Transform m_headTransform;

		private Collider m_collider;

		private AudioSource m_audioSource;

		private EntityWrapperComponent m_entityWrapper;

		public sealed override int Priority
		{
			get
			{
				return 1;
			}
		}

		public NavMeshAgent NavMeshAgent
		{
			get
			{
				return m_navMeshAgent;
			}
		}

		public Animator Animator
		{
			get
			{
				return m_animator;
			}
		}

		public AnimationController AnimationController
		{
			get
			{
				return m_animationController;
			}
		}

		public Transform Transform
		{
			get
			{
				return m_transform;
			}
		}

		public Transform HeadTransform
		{
			get
			{
				return m_headTransform;
			}
		}

		public Collider Collider
		{
			get
			{
				return m_collider;
			}
		}

		public AudioSource AudioSource
		{
			get
			{
				return m_audioSource;
			}
		}

		public EntityWrapperComponent EntityWrapper
		{
			get
			{
				return m_entityWrapper;
			}
		}

		public UnityComponentCache(IEntity _entity)
			: base(_entity)
		{
		}

		public override void Attach(EntityWrapperComponent _wrapperComponent)
		{
			m_navMeshAgent = base.GameObjectEntity.GameObject.GetComponent<NavMeshAgent>();
			Character character = Entity as Character;
			if (character != null)
			{
				m_navMeshAgent.areaMask = character.GetAutoPassableNavMeshLayerMask();
			}
			m_animator = base.GameObjectEntity.GameObject.GetComponent<Animator>();
			m_animationController = base.GameObjectEntity.GameObject.GetComponent<AnimationController>();
			if (m_animationController != null)
			{
				m_animationController.EntityId = Entity.Id;
				m_animationController.Attach();
				m_headTransform = AnimationController.Animator.GetBoneTransform(HumanBodyBones.Head);
			}
			m_transform = base.GameObjectEntity.GameObject.transform;
			m_collider = base.GameObjectEntity.GameObject.GetComponent<Collider>();
			m_audioSource = base.GameObjectEntity.GameObject.GetComponent<AudioSource>();
			m_entityWrapper = base.GameObjectEntity.GameObject.GetComponent<EntityWrapperComponent>();
		}

		public override void Detach()
		{
			m_navMeshAgent = null;
			m_animator = null;
			if (m_animationController != null)
			{
				m_animationController.Detach();
				m_animationController.EntityId = EntityId.InvalidId;
			}
			m_animationController = null;
			m_headTransform = null;
			m_transform = null;
			m_collider = null;
			m_audioSource = null;
			m_entityWrapper = null;
		}
	}
}
