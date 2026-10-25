using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class GameObjectProxy : Component, ITransformProxy
	{
		private Vector3 m_position = Vector3.zero;

		private Vector3 m_localScale = new Vector3(1f, 1f, 1f);

		private Quaternion m_rotation = Quaternion.identity;

		private bool m_attached;

		public sealed override int Priority
		{
			get
			{
				return 4;
			}
		}

		public UnityComponentCache UnityComponentCache
		{
			get
			{
				return GameEntity.UnityComponentCache;
			}
		}

		public Vector3 Position
		{
			get
			{
				return (!m_attached) ? m_position : Transform.position;
			}
			set
			{
				if (m_attached)
				{
					Transform.position = value;
				}
				else
				{
					m_position = value;
				}
			}
		}

		public Quaternion Rotation
		{
			get
			{
				return (!m_attached) ? m_rotation : Transform.rotation;
			}
			set
			{
				if (m_attached)
				{
					Transform.rotation = value;
				}
				else
				{
					m_rotation = value;
				}
			}
		}

		public Vector3 Scale
		{
			get
			{
				return (!m_attached) ? m_localScale : Transform.localScale;
			}
			set
			{
				if (m_attached)
				{
					Transform.localScale = value;
				}
				else
				{
					m_localScale = value;
				}
			}
		}

		public Transform Transform
		{
			get
			{
				return UnityComponentCache.Transform;
			}
		}

		public GameObjectProxy(IEntity _entity)
			: base(_entity)
		{
		}

		public override void Attach(EntityWrapperComponent _wrapperComponent)
		{
			base.Attach(_wrapperComponent);
			UnityComponentCache.Transform.position = m_position;
			UnityComponentCache.Transform.rotation = m_rotation;
			UnityComponentCache.Transform.localScale = m_localScale;
			if (UnityComponentCache.NavMeshAgent != null)
			{
				UnityComponentCache.NavMeshAgent.Warp(Position);
			}
			m_attached = true;
		}

		public override void Detach()
		{
			base.Detach();
			m_position = UnityComponentCache.Transform.position;
			m_rotation = UnityComponentCache.Transform.rotation;
			m_localScale = UnityComponentCache.Transform.localScale;
			m_attached = false;
		}
	}
}
