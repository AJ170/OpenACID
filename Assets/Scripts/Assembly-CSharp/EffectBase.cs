using UnityEngine;

public abstract class EffectBase : MonoBehaviour
{
	public EffectController ParentController { get; set; }

	public EffectBase()
	{
	}

	public abstract void On();

	public abstract void Off();

	public abstract void Trigger();

	public virtual void OnDestroy()
	{
		Deregister();
		Singleton<EffectManager>.Instance.DeregisterEffect(this);
	}

	public virtual void Parameters(EffectParameter _data)
	{
		Debug.Log("Got a parameter with " + _data.Values.Length + " entries!");
	}

	protected bool Deregister()
	{
		if ((bool)ParentController)
		{
			if (ParentController.RemoveActiveEffect(this))
			{
				ParentController = null;
				return true;
			}
			return false;
		}
		return true;
	}

	public void DeregisterAndDestroy()
	{
		if (Deregister())
		{
			Object.Destroy(base.gameObject);
		}
	}
}
