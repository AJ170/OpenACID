using System.Collections;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using UbiCore.Utils;
using UnityEngine;

public class WaypointHighlighting : HighlightBehaviour
{
	public float AnimationSpeed;

	public ParticleSystem digitalArtifacts;

	public ParticleSystem glowSpheres;

	public Transform Helix1;

	public Transform Helix2;

	public Transform Helix3;

	private Material m_GlowSphereMaterial;

	private bool m_WasUsingEagleEye;

	private static Vector4 s_HighlightColor;

	private static Vector4 s_StartColor;

	public override void Activate()
	{
		m_GlowSphereMaterial = glowSpheres.GetComponent<Renderer>().sharedMaterial;
		s_HighlightColor = Highlight.ObjectiveHighlightColor;
		s_StartColor = glowSpheres.startColor;
		Helix1.GetComponent<Renderer>().enabled = true;
		Helix2.GetComponent<Renderer>().enabled = true;
		Helix3.GetComponent<Renderer>().enabled = true;
		m_GlowSphereMaterial.SetColor("_RimColor", s_StartColor);
		digitalArtifacts.Play();
		glowSpheres.Play();
		base.gameObject.SetActive(true);
		StopCoroutine("FadeOut");
		StartCoroutine(FadeIn());
	}

	public override void Deactivate()
	{
		digitalArtifacts.Stop();
		glowSpheres.Stop();
		StopCoroutine("FadeIn");
		StartCoroutine(FadeOut());
	}

	public void Update()
	{
		bool isUsingEagleEye = PlayerHelper.IsUsingEagleEye;
		if (m_WasUsingEagleEye != isUsingEagleEye)
		{
			if (isUsingEagleEye)
			{
				m_GlowSphereMaterial.SetColor("_RimColor", s_HighlightColor);
				m_GlowSphereMaterial.EnableKeyword("FX_HIGHLIGHT");
				m_WasUsingEagleEye = true;
			}
			else
			{
				m_GlowSphereMaterial.SetColor("_RimColor", s_StartColor);
				m_GlowSphereMaterial.DisableKeyword("FX_HIGHLIGHT");
				m_WasUsingEagleEye = false;
			}
		}
	}

	private IEnumerator FadeIn()
	{
		Color newColor = new Color(1f, 1f, 1f, 0f);
		while (true)
		{
			newColor.a += AnimationSpeed * UbiCore.Utils.Singleton<Timer>.Instance.realDeltaTime;
			Helix1.GetComponent<Renderer>().material.SetColor("_TintColor", newColor);
			Helix2.GetComponent<Renderer>().material.SetColor("_TintColor", newColor);
			Helix3.GetComponent<Renderer>().material.SetColor("_TintColor", newColor);
			if (newColor.a >= 1f)
			{
				break;
			}
			yield return StartCoroutine(Timer.WaitForSecondsUnscaled(UbiCore.Utils.Singleton<Timer>.Instance.realDeltaTime));
		}
	}

	private IEnumerator FadeOut()
	{
		Color newColor = new Color(1f, 1f, 1f, 1f);
		while (true)
		{
			newColor.a -= AnimationSpeed * UbiCore.Utils.Singleton<Timer>.Instance.realDeltaTime;
			Helix1.GetComponent<Renderer>().material.SetColor("_TintColor", newColor);
			Helix2.GetComponent<Renderer>().material.SetColor("_TintColor", newColor);
			Helix3.GetComponent<Renderer>().material.SetColor("_TintColor", newColor);
			if (newColor.a <= 0f)
			{
				break;
			}
			yield return StartCoroutine(Timer.WaitForSecondsUnscaled(UbiCore.Utils.Singleton<Timer>.Instance.realDeltaTime));
		}
		Helix1.GetComponent<Renderer>().enabled = false;
		Helix2.GetComponent<Renderer>().enabled = false;
		Helix3.GetComponent<Renderer>().enabled = false;
	}
}
