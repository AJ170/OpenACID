using UnityEngine;

public abstract class HighlightBehaviour : MonoBehaviour
{
	public abstract void Activate();

	public abstract void Deactivate();

	public virtual void PlayAppearEffect()
	{
	}

	public void Activate(bool _activate)
	{
		if (_activate)
		{
			Activate();
		}
		else
		{
			Deactivate();
		}
	}
}
