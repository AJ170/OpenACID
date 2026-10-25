using UbiCore.Utils.Extensions;
using UnityEngine;

public class GlyphBehaviour : HighlightBehaviour
{
	public GameObject GlyphGameObject;

	public Animator animator;

	public override void Activate()
	{
		SetVisibility(true);
	}

	public override void Deactivate()
	{
		SetVisibility(false);
	}

	public override void PlayAppearEffect()
	{
		GameObject gameObject = base.gameObject.FindChildByName("AppearEffect");
		if (gameObject != null)
		{
			ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
			component.Play(true);
		}
	}

	private void SetVisibility(bool isVisible)
	{
		animator.enabled = isVisible;
		Renderer[] componentsInChildren = GlyphGameObject.GetComponentsInChildren<Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = isVisible;
		}
	}
}
