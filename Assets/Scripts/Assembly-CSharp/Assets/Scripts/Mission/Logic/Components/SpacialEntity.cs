using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Entities;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class SpacialEntity : TurnUpdateableComponent, IOctreeDynamicEntity
	{
		private const float c_Radius = 0.25f;

		private readonly GameObjectEntity m_gameobjectEntity;

		private float m_radius = 0.25f;

		public override int Priority
		{
			get
			{
				return 5;
			}
		}

		public bool IsDirty { get; set; }

		public Vector3 Center { get; private set; }

		public float Radius
		{
			get
			{
				return m_radius;
			}
		}

		public OctreeDynamic CurrentCell { get; set; }

		public Renderer[] Renderers { get; internal set; }

		public SpacialEntity(IGameObjectEntity _entity)
			: base(_entity)
		{
			m_gameobjectEntity = Entity.Cast<GameObjectEntity>();
		}

		public override void OnRegistered()
		{
			base.OnRegistered();
			IsActive = true;
		}

		public override void Attach(EntityWrapperComponent _wrapperComponent)
		{
			base.Attach(_wrapperComponent);
			Renderers = _wrapperComponent.gameObject.GetComponentsInChildren<Renderer>(true);
		}

		public override void Detach()
		{
			base.Detach();
			Renderers = null;
		}

		public override void DoTurnUpdate(int _deltaTurns)
		{
			Vector3 position = m_gameobjectEntity.Position;
			if (!IsDirty)
			{
				IsDirty = Center != position;
			}
			if (IsDirty)
			{
				Center = position;
				m_radius = 0.25f;
				if (m_gameobjectEntity.IsAttached && Renderers.Length > 0)
				{
					for (int i = 0; i < Renderers.Length; i++)
					{
						Renderer renderer = Renderers[i];
						if (renderer.enabled)
						{
							m_radius = renderer.bounds.extents.magnitude;
							break;
						}
					}
				}
			}
			base.DoTurnUpdate(_deltaTurns);
		}
	}
}
