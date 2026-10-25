public class EffectWeaponTrail : EffectBase
{
	public WeaponTrail TheTrail;

	private bool m_enabled;

	private bool m_wasStarted;

	public void Start()
	{
		if ((bool)TheTrail)
		{
			m_wasStarted = TheTrail.enabled;
		}
		if (m_wasStarted)
		{
			Singleton<EffectManager>.Instance.RegisterEffect(this);
		}
	}

	private void Update()
	{
		if (TheTrail != null)
		{
			if (m_enabled)
			{
				TheTrail.enabled = m_enabled;
			}
			else
			{
				TheTrail.FadeOut = true;
			}
			if (!TheTrail.enabled && m_wasStarted)
			{
				DeregisterAndDestroy();
			}
		}
	}

	public override void On()
	{
		m_enabled = true;
		m_wasStarted = true;
		Singleton<EffectManager>.Instance.RegisterEffect(this);
	}

	public override void Off()
	{
		m_enabled = false;
	}

	public override void Trigger()
	{
		Singleton<EffectManager>.Instance.RegisterEffect(this);
	}
}
