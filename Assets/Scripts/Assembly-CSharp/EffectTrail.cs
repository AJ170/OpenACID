using UnityEngine;

public class EffectTrail : EffectBase
{
	public TrailRenderer TheTrail;

	private bool m_enabled;

	public void Start()
	{
		if (TheTrail.gameObject != base.gameObject)
		{
			VisibilityBroadcaster visibilityBroadcaster = TheTrail.gameObject.AddComponent<VisibilityBroadcaster>();
			visibilityBroadcaster.TheTarget = base.gameObject;
		}
	}

	private void UpdateTrail()
	{
		if (TheTrail != null)
		{
			TheTrail.enabled = m_enabled;
		}
	}

	public override void On()
	{
		Singleton<EffectManager>.Instance.RegisterEffect(this);
		m_enabled = true;
		UpdateTrail();
	}

	public override void Off()
	{
		m_enabled = false;
		UpdateTrail();
		DeregisterAndDestroy();
	}

	public void OnBecameVisible()
	{
		UpdateTrail();
	}

	public void OnBecameInvisible()
	{
	}

	public override void Trigger()
	{
		Singleton<EffectManager>.Instance.RegisterEffect(this);
	}
}
