using System.Collections;
using UbiCore.Utils;
using UnityEngine;

public class ChestHighlighting : HighlightBehaviour
{
	public float AnimationSpeed;

	public ParticleSystem BottomLeft;

	public ParticleSystem BottomRight;

	public Transform Highlight;

	public ParticleSystem SideLeft;

	public ParticleSystem SideRight;

	public override void Activate()
	{
		SideLeft.Play();
		SideRight.Play();
		BottomLeft.Play();
		BottomRight.Play();
		Highlight.gameObject.SetActive(true);
		StopCoroutine("FadeOut");
		StartCoroutine("FadeIn");
	}

	public override void Deactivate()
	{
		SideLeft.Stop();
		SideRight.Stop();
		BottomLeft.Stop();
		BottomRight.Stop();
		StopCoroutine("FadeIn");
		StartCoroutine("FadeOut");
	}

	private IEnumerator FadeIn()
	{
		float elapsedTime = 0f;
		while (true)
		{
			float deltaTime = UbiCore.Utils.Singleton<Timer>.Instance.realDeltaTime;
			elapsedTime += deltaTime;
			Highlight.localPosition = Vector3.Lerp(Highlight.localPosition, Vector3.zero, AnimationSpeed * elapsedTime);
			if (Highlight.localPosition.y >= -0.1f)
			{
				break;
			}
			yield return StartCoroutine(Timer.WaitForSecondsUnscaled(deltaTime));
		}
	}

	private IEnumerator FadeOut()
	{
		float elapsedTime = 0f;
		while (true)
		{
			float deltaTime = UbiCore.Utils.Singleton<Timer>.Instance.realDeltaTime;
			elapsedTime += deltaTime;
			Highlight.localPosition = Vector3.Lerp(Highlight.localPosition, new Vector3(0f, -1f, 0f), AnimationSpeed * elapsedTime);
			if (Highlight.localPosition.y <= -0.9f)
			{
				break;
			}
			yield return StartCoroutine(Timer.WaitForSecondsUnscaled(deltaTime));
		}
		Highlight.gameObject.SetActive(false);
	}
}
