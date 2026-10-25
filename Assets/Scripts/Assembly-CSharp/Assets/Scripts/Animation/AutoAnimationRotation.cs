using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Utilities.AutoUpdating;

namespace Assets.Scripts.Animation
{
	public class AutoAnimationRotation : AutoRotationInterpolation
	{
		private bool m_agentAutoRotate;

		protected readonly IGameEntity m_entity;

		public bool AutoStop { get; set; }

		public AutoAnimationRotation(IGameEntity _entity)
		{
			m_entity = _entity;
			AutoStop = true;
		}

		public override void Start()
		{
			base.Start();
			if (base.FireAndForget)
			{
				Enter();
			}
		}

		public void Enter()
		{
			if (m_entity.IsValid && m_entity.IsAttached)
			{
				m_agentAutoRotate = m_entity.UnityComponentCache.AnimationController.Agent.updateRotation;
				m_entity.UnityComponentCache.AnimationController.Agent.Stop();
				m_entity.UnityComponentCache.AnimationController.Agent.updateRotation = false;
			}
		}

		public void Exit()
		{
			if (m_entity.IsValid && m_entity.IsAttached && !m_entity.UnityComponentCache.AnimationController.Agent.updateRotation)
			{
				m_entity.UnityComponentCache.AnimationController.Agent.updateRotation = m_agentAutoRotate;
			}
		}

		protected override bool UpdateFrameInternal(float _gameTimeDelta)
		{
			if (!m_entity.IsValid)
			{
				return true;
			}
			bool flag = base.UpdateFrameInternal(_gameTimeDelta);
			m_entity.Rotation = base.Rotation;
			if (flag && base.FireAndForget)
			{
				Exit();
			}
			return flag && AutoStop;
		}
	}
}
