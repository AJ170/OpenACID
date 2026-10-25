using System.Collections;
using UbiCore.Utils;
using UnityEngine;

public class AnimusFadeIn : MonoBehaviour
{
	public Color FogColor = new Color(13f / 128f, 23f / 128f, 7f / 32f, 1f);

	public float AnimationSpeed;

	public float FadeSpeed = 3f;

	public float animationAcceleration = 20f;

	public float StartDelay = 3f;

	public bool FadeIn;

	public Transform Ground;

	private void Start()
	{
		RenderSettings.fog = true;
		RenderSettings.fogColor = FogColor;
		RenderSettings.fogMode = FogMode.Linear;
		RenderSettings.fogDensity = 0.1f;
		if (FadeIn)
		{
			Ground.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(1f, 1f, 1f, 1f));
			RenderSettings.fogStartDistance = 0f;
			RenderSettings.fogEndDistance = 3f;
			StartCoroutine("FogFadeIn");
		}
		else
		{
			Ground.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(1f, 1f, 1f, 0f));
			RenderSettings.fogStartDistance = 150f;
			RenderSettings.fogEndDistance = 300f;
			StartCoroutine("FogFadeOut");
		}
	}

	public IEnumerator FogFadeIn()
	{
		yield return StartCoroutine(Timer.WaitForSecondsUnscaled(StartDelay));
		while (true)
		{
			Color c = Ground.GetComponent<Renderer>().material.GetColor("_TintColor");
			c.a -= FadeSpeed * UbiCore.Utils.Singleton<Timer>.Instance.realDeltaTime;
			if (c.a > 0f)
			{
				Ground.GetComponent<Renderer>().material.SetColor("_TintColor", c);
			}
			else if (c.a != 0f)
			{
				c.a = 0f;
				Ground.GetComponent<Renderer>().material.SetColor("_TintColor", c);
			}
			RenderSettings.fogEndDistance += AnimationSpeed * UbiCore.Utils.Singleton<Timer>.Instance.realDeltaTime;
			RenderSettings.fogStartDistance += AnimationSpeed / 2f * UbiCore.Utils.Singleton<Timer>.Instance.realDeltaTime;
			AnimationSpeed += animationAcceleration * UbiCore.Utils.Singleton<Timer>.Instance.realDeltaTime;
			if (RenderSettings.fogEndDistance >= 300f)
			{
				break;
			}
			yield return StartCoroutine(Timer.WaitForSecondsUnscaled(UbiCore.Utils.Singleton<Timer>.Instance.realDeltaTime));
		}
		RenderSettings.fog = false;
	}

	public IEnumerator FogFadeOut()
	{
		yield return StartCoroutine(Timer.WaitForSecondsUnscaled(StartDelay));
		while (true)
		{
			Color c = Ground.GetComponent<Renderer>().material.GetColor("_TintColor");
			c.a += FadeSpeed * UbiCore.Utils.Singleton<Timer>.Instance.realDeltaTime;
			if (c.a < 1f)
			{
				Ground.GetComponent<Renderer>().material.SetColor("_TintColor", c);
			}
			else if (c.a != 1f)
			{
				c.a = 1f;
				Ground.GetComponent<Renderer>().material.SetColor("_TintColor", c);
			}
			RenderSettings.fogEndDistance = Mathf.Lerp(RenderSettings.fogEndDistance, 1f, AnimationSpeed * UbiCore.Utils.Singleton<Timer>.Instance.realDeltaTime);
			RenderSettings.fogStartDistance = Mathf.Lerp(RenderSettings.fogStartDistance, 0f, AnimationSpeed * 2f * UbiCore.Utils.Singleton<Timer>.Instance.realDeltaTime);
			AnimationSpeed += animationAcceleration;
			if (RenderSettings.fogEndDistance <= 0f)
			{
				break;
			}
			yield return StartCoroutine(Timer.WaitForSecondsUnscaled(UbiCore.Utils.Singleton<Timer>.Instance.realDeltaTime));
		}
		RenderSettings.fog = false;
	}
}
