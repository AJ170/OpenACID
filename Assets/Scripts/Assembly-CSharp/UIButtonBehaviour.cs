using System.Collections;
using Assets.Scripts.UI.Ingame;

public abstract class UIButtonBehaviour : UIControlBehaviour, ISpriteButtonBehaviour, ISpriteControlBehaviour
{
	public virtual void AnimateActivate()
	{
	}

	public virtual void AnimateActivityStart()
	{
	}

	public virtual void AnimateBlocked()
	{
	}

	public virtual void AnimateDeactivate()
	{
	}

	public virtual void AnimateInstantActivation()
	{
	}

	public virtual void AnimateLoad()
	{
	}

	public virtual void AnimatePrepare()
	{
	}

	public virtual void AnimateReset()
	{
	}

	public virtual void AnimateUnload()
	{
	}

	public virtual void AnimateUnprepare(bool _isEnabled)
	{
	}

	public virtual void StartCooldown(float _duration)
	{
	}

	public virtual void StartActivity(float _duration)
	{
	}

	protected virtual IEnumerator AppearAnimation(float _delay)
	{
		yield break;
	}

	public override void Construct()
	{
		StartCoroutine(AppearAnimation(1f));
		base.Construct();
	}
}
