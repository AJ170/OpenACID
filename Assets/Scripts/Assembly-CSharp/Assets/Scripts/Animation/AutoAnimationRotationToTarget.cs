using Assets.Scripts.Mission.Logic;
using UnityEngine;

namespace Assets.Scripts.Animation
{
	public class AutoAnimationRotationToTarget : AutoAnimationRotation
	{
		protected readonly IPositionProvider m_TargetPositionEntity;

		protected readonly bool m_onlyXZ;

		public AutoAnimationRotationToTarget(IGameEntity _entity, bool _onlyXZ, IPositionProvider _targetPositionEntity)
			: base(_entity)
		{
			m_TargetPositionEntity = _targetPositionEntity;
			m_onlyXZ = _onlyXZ;
		}

		public AutoAnimationRotationToTarget(IGameEntity _entity, IPositionProvider _targetPositionEntity)
			: this(_entity, true, _targetPositionEntity)
		{
		}

		public override void Start()
		{
			base.Start();
			base.From = (m_entity.IsAttached ? m_entity.UnityComponentCache.AnimationController.NextRotation : m_entity.Rotation);
			UpdateTargetRotation();
		}

		private void UpdateTargetRotation()
		{
			Vector3 forward = m_TargetPositionEntity.Position - m_entity.Position;
			if (m_onlyXZ)
			{
				forward.y = 0f;
			}
			base.To = Quaternion.LookRotation(forward);
		}

		protected override bool UpdateFrameInternal(float _gameTimeDelta)
		{
			if (!m_entity.IsValid)
			{
				return true;
			}
			IEntity entity = m_TargetPositionEntity as IEntity;
			if (entity != null && !entity.IsValid)
			{
				return true;
			}
			UpdateTargetRotation();
			return base.UpdateFrameInternal(_gameTimeDelta);
		}
	}
}
