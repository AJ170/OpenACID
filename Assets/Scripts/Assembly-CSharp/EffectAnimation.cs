using UnityEngine;

public class EffectAnimation : EffectBase
{
	public Animation TheAnimation;

	private bool m_wasStarted;

	public bool FaceCamera;

	public void Start()
	{
		m_wasStarted = TheAnimation.playAutomatically;
		if (m_wasStarted)
		{
			Singleton<EffectManager>.Instance.RegisterEffect(this);
		}
	}

	private void StartAnimation()
	{
		TheAnimation.Play();
	}

	private void EndAnimation()
	{
		TheAnimation.Stop();
	}

	private void Update()
	{
		if (FaceCamera)
		{
			base.transform.LookAt(Camera.main.transform.position);
		}
		if (m_wasStarted && !TheAnimation.isPlaying)
		{
			DeregisterAndDestroy();
		}
	}

	public override void On()
	{
		Singleton<EffectManager>.Instance.RegisterEffect(this);
		m_wasStarted = true;
		StartAnimation();
	}

	public override void Off()
	{
		EndAnimation();
		DeregisterAndDestroy();
	}

	public override void Trigger()
	{
		Singleton<EffectManager>.Instance.RegisterEffect(this);
		m_wasStarted = true;
		StartAnimation();
	}
}
