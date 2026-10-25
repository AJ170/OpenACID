using System.Collections.Generic;
using UnityEngine;

public class EffectParticles : EffectBase
{
	public enum SelectionMode
	{
		None = 0,
		All = 1,
		Random = 2,
		InOrder = 3
	}

	public List<ParticleSystem> TheSystems;

	public SelectionMode SystemMode = SelectionMode.All;

	protected int m_lastSystem;

	private bool m_wasStarted;

	public bool detachOnStart;

	public void Start()
	{
		if (detachOnStart)
		{
			base.transform.parent = null;
		}
		int count = TheSystems.Count;
		for (int i = 0; count > i; i++)
		{
			if (TheSystems[i].playOnAwake)
			{
				m_wasStarted = true;
			}
		}
		if (m_wasStarted)
		{
			Singleton<EffectManager>.Instance.RegisterEffect(this);
		}
	}

	public override void On()
	{
		switch (SystemMode)
		{
		case SelectionMode.All:
		{
			int count = TheSystems.Count;
			for (int i = 0; count > i; i++)
			{
				TheSystems[i].Play();
			}
			break;
		}
		case SelectionMode.Random:
			m_lastSystem = Random.Range(0, TheSystems.Count);
			TheSystems[m_lastSystem].Play();
			break;
		case SelectionMode.InOrder:
			m_lastSystem = (m_lastSystem + 1) % TheSystems.Count;
			TheSystems[m_lastSystem].Play();
			break;
		}
		m_wasStarted = true;
		Singleton<EffectManager>.Instance.RegisterEffect(this);
	}

	public override void Off()
	{
		switch (SystemMode)
		{
		case SelectionMode.All:
		{
			int count = TheSystems.Count;
			for (int i = 0; count > i; i++)
			{
				TheSystems[i].Stop();
			}
			break;
		}
		case SelectionMode.Random:
		case SelectionMode.InOrder:
			TheSystems[m_lastSystem].Stop();
			break;
		}
	}

	public override void Trigger()
	{
		switch (SystemMode)
		{
		case SelectionMode.All:
		{
			int count = TheSystems.Count;
			for (int i = 0; count > i; i++)
			{
				TheSystems[i].Play();
			}
			break;
		}
		case SelectionMode.Random:
			m_lastSystem = Random.Range(0, TheSystems.Count);
			TheSystems[m_lastSystem].Play();
			break;
		case SelectionMode.InOrder:
			m_lastSystem = (m_lastSystem + 1) % TheSystems.Count;
			TheSystems[m_lastSystem].Play();
			break;
		}
		m_wasStarted = true;
		Singleton<EffectManager>.Instance.RegisterEffect(this);
	}

	public void Update()
	{
		if (!m_wasStarted)
		{
			return;
		}
		bool flag = false;
		switch (SystemMode)
		{
		case SelectionMode.All:
		{
			int count = TheSystems.Count;
			int num = 0;
			for (int i = 0; count > i; i++)
			{
				num += (TheSystems[i].IsAlive() ? 1 : 0);
			}
			flag = num == 0;
			break;
		}
		case SelectionMode.Random:
		case SelectionMode.InOrder:
			flag = !TheSystems[m_lastSystem].IsAlive();
			break;
		}
		if (flag)
		{
			DeregisterAndDestroy();
		}
	}
}
