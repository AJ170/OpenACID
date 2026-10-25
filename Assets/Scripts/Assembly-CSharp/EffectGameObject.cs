using UnityEngine;

public class EffectGameObject : EffectBase
{
	public GameObject GO;

	private bool m_enabled;

	public void Start()
	{
		UpdateGameObject();
	}

	private void UpdateGameObject()
	{
		if (GO != null)
		{
			GO.SetActive(m_enabled);
		}
		if (m_enabled)
		{
			Singleton<EffectManager>.Instance.RegisterEffect(this);
		}
	}

	public override void On()
	{
		Singleton<EffectManager>.Instance.RegisterEffect(this);
		m_enabled = true;
		UpdateGameObject();
	}

	public override void Off()
	{
		m_enabled = false;
		UpdateGameObject();
		DeregisterAndDestroy();
	}

	public void OnBecameVisible()
	{
		UpdateGameObject();
	}

	public override void Trigger()
	{
		Singleton<EffectManager>.Instance.RegisterEffect(this);
	}
}
