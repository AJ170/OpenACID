using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Entities;
using UbiCore.Debug;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class SelectionCollider : TurnUpdateableComponent
	{
		private readonly float m_distanceMin = 4f;

		private readonly float m_distanceMax = 20f;

		private readonly float m_scaleMin = 0.4f;

		private readonly float m_scaleMax = 0.9f;

		private readonly float m_step = 0.05f;

		private GameObjectEntity m_gameobjectEntity;

		private CapsuleCollider m_capsuleCollider;

		public sealed override int Priority
		{
			get
			{
				return 29;
			}
		}

		public SelectionCollider(IGameObjectEntity _entity)
			: base(_entity)
		{
			m_gameobjectEntity = Entity.Cast<GameObjectEntity>();
		}

		public override void OnRegistered()
		{
			base.OnRegistered();
			IsActive = false;
		}

		public override void Attach(EntityWrapperComponent _wrapperComponent)
		{
			base.Attach(_wrapperComponent);
			IsActive = true;
			GameObject gameObject = m_gameobjectEntity.GameObject.FindChildByName("SelectionCollider");
			if (gameObject != null)
			{
				m_capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
			}
			UpdateColliderRadius();
		}

		public override void Detach()
		{
			base.Detach();
			m_capsuleCollider = null;
			IsActive = false;
		}

		public override void DoTurnUpdate(int _deltaTurns)
		{
			Profiling.Start("SelectionCollider.DoTurnUpdate", float.MaxValue);
			UpdateColliderRadius();
			base.DoTurnUpdate(_deltaTurns);
			Profiling.Stop();
		}

		public void UpdateColliderRadius()
		{
			if (m_capsuleCollider != null)
			{
				float num = Mathf.Clamp((m_gameobjectEntity.Position - UnityEngine.Camera.main.transform.position).magnitude, m_distanceMin, m_distanceMax) - m_distanceMin;
				num /= m_distanceMax - m_distanceMin;
				num = Mathf.Clamp01(num);
				float num2 = Mathf.Round((m_scaleMin + num * (m_scaleMax - m_scaleMin)) / m_step) * m_step;
				if (Mathf.Abs(m_capsuleCollider.radius - num2) > m_step * 0.5f)
				{
					m_capsuleCollider.radius = num2;
				}
			}
		}
	}
}
