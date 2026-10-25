using UnityEngine;

public class EffectAnimationOnOff : EffectBase
{
	public Animation TheAnimation;

	private bool m_wasStarted;

	private bool m_wasOn;

	public void Update()
	{
		if (m_wasStarted)
		{
			if (TheAnimation["on"].weight > 0.98f)
			{
				m_wasOn = true;
			}
			if (TheAnimation["off"].weight > 0.98f && m_wasOn)
			{
				DeregisterAndDestroy();
			}
		}
	}

	public override void On()
	{
		Singleton<EffectManager>.Instance.RegisterEffect(this);
		m_wasStarted = true;
		TurnOn();
	}

	public override void Off()
	{
		TurnOff();
	}

	public override void Trigger()
	{
		Singleton<EffectManager>.Instance.RegisterEffect(this);
		m_wasStarted = true;
		TurnOn();
	}

	private void TurnOn()
	{
		if (TheAnimation != null)
		{
			TheAnimation.CrossFade("on", 0.1f);
		}
	}

	private void TurnOff()
	{
		if (TheAnimation != null)
		{
			TheAnimation.CrossFade("off", 0.1f);
		}
	}
}
