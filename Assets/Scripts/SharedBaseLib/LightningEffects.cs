using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightningEffects : MonoBehaviour
{
	private AnimationClip[] m_clips;

	public Color lightningColor;

	private ThunderstormSettings m_StormSettings;

	public GameObject[] lightnings;

	private int m_currentLightning;

	private List<Renderer> m_cachedRenderers = new List<Renderer>();

	private static int s_idTintColor;

	private void Awake()
	{
		s_idTintColor = Shader.PropertyToID("_TintColor");
	}

	private void Start()
	{
		if (Application.isPlaying)
		{
			Animation component = GetComponent<Animation>();
			m_clips = new AnimationClip[component.GetClipCount()];
			int num = 0;
			foreach (AnimationState item in component)
			{
				m_clips[num++] = item.clip;
			}
		}
		Reset();
		RefreshRendererCache();
		for (int i = 0; i < lightnings.Length; i++)
		{
			m_cachedRenderers[i].material.EnableKeyword("TINT_COLOR_ON");
			m_cachedRenderers[i].material.DisableKeyword("TINT_COLOR_OFF");
			m_cachedRenderers[i].material.SetColor(s_idTintColor, new Color(0f, 0f, 0f, 0f));
		}
	}

	public void Reset()
	{
		m_StormSettings = Object.FindObjectOfType(typeof(ThunderstormSettings)) as ThunderstormSettings;
	}

	public void newRandomLightning()
	{
		m_currentLightning = Mathf.FloorToInt(Random.Range(0, lightnings.Length));
	}

	private void RefreshRendererCache()
	{
		m_cachedRenderers.Clear();
		for (int i = 0; i < lightnings.Length; i++)
		{
			Renderer item = ((lightnings[i] != null) ? lightnings[i].GetComponent<Renderer>() : null);
			m_cachedRenderers.Add(item);
		}
	}

	private void Update()
	{
		if (!Application.isPlaying)
		{
			Realign();
			RefreshRendererCache();
			return;
		}
		if (!GetComponent<Animation>().isPlaying)
		{
			int num = Random.Range(0, m_clips.Length);
			string animation = m_clips[num].name;
			GetComponent<Animation>().Play(animation);
		}
		if (lightnings[m_currentLightning] != null && m_cachedRenderers[m_currentLightning] != null)
		{
			m_cachedRenderers[m_currentLightning].material.SetColor(s_idTintColor, lightningColor);
		}
		Color thunderColor = new Color(Mathf.Clamp01(lightningColor.r), Mathf.Clamp01(lightningColor.g), Mathf.Clamp01(lightningColor.b), Mathf.Clamp01(lightningColor.a));
		thunderColor.r *= thunderColor.a;
		thunderColor.g *= thunderColor.a;
		thunderColor.b *= thunderColor.a;
		if (m_StormSettings != null)
		{
			m_StormSettings.thunderColor = thunderColor;
		}
	}

	private void Realign()
	{
		for (int i = 0; i < lightnings.Length; i++)
		{
			if (lightnings[i] != null && Camera.current != null)
			{
				lightnings[i].transform.LookAt(Camera.current.transform.position, Vector3.up);
			}
		}
	}
}
