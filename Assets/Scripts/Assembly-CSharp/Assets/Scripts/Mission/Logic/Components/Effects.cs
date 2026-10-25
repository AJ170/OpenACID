using Assets.Scripts.Engine;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class Effects : Component
	{
		private EffectController m_effectController;

		public override int Priority
		{
			get
			{
				return 3;
			}
		}

		public Effects(IEntity _entity)
			: base(_entity)
		{
		}

		public override void Attach(EntityWrapperComponent _wrapperComponent)
		{
			base.Attach(_wrapperComponent);
			m_effectController = GameEntity.GameObject.GetComponentLimitedRecursive<EffectController>(1);
			if ((bool)m_effectController)
			{
				m_effectController.RelinkBones();
				m_effectController.Reset();
			}
		}

		public override void OnUnregistered()
		{
			if (m_effectController != null)
			{
				m_effectController.Reset();
			}
			m_effectController = null;
			base.OnUnregistered();
		}

		public override void Detach()
		{
			if (m_effectController != null)
			{
				m_effectController.Reset();
			}
			m_effectController = null;
			base.Detach();
		}

		public void OnEffectTrigger(string _name)
		{
			if ((bool)m_effectController)
			{
				m_effectController.OnEffectTrigger(_name);
			}
		}

		public void OnEffectOn(string _name)
		{
			if ((bool)m_effectController)
			{
				m_effectController.OnEffectOn(_name);
			}
		}

		public void OnEffectOff(string _name)
		{
			if ((bool)m_effectController)
			{
				m_effectController.OnEffectOff(_name);
			}
		}
	}
}
