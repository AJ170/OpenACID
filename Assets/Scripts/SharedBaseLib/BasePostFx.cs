using UnityEngine;

public class BasePostFx
{
	public virtual bool IsActive { get; set; }

	public virtual bool IsSupported()
	{
		return true;
	}

	public virtual void Reset(bool _skipExpensive)
	{
	}

	public virtual void Prepare(RenderTexture _src)
	{
	}

	public virtual void ExecuteOrSkip(Material _ComboPass)
	{
	}

	public virtual void ReleaseTemporaries()
	{
	}
}
