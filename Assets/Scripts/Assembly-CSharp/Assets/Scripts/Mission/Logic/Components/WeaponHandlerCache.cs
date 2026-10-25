namespace Assets.Scripts.Mission.Logic.Components
{
	public class WeaponHandlerCache : Component
	{
		private readonly WeaponHandler[] m_weaponHandler;

		public override int Priority
		{
			get
			{
				return 5;
			}
		}

		public WeaponHandlerCache(IEntity _entity)
			: base(_entity)
		{
			m_weaponHandler = new WeaponHandler[2];
		}

		public override void Attach(EntityWrapperComponent _wrapperComponent)
		{
			base.Attach(_wrapperComponent);
			WeaponHandler[] components = _wrapperComponent.gameObject.GetComponents<WeaponHandler>();
			int num = components.Length;
			for (int i = 0; i < num; i++)
			{
				WeaponHandler weaponHandler = components[i];
				m_weaponHandler[(int)weaponHandler.CurrentWeaponDesc.Category] = weaponHandler;
			}
		}

		public override void Detach()
		{
			base.Detach();
			for (int i = 0; i < 2; i++)
			{
				WeaponHandler weaponHandler = m_weaponHandler[i];
				if (weaponHandler != null)
				{
					weaponHandler.Detach();
					m_weaponHandler[i] = null;
				}
			}
		}

		public WeaponHandler GetWeaponHandler(EWeaponCategory _category)
		{
			return m_weaponHandler[(int)_category];
		}
	}
}
