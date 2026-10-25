using System;
using System.Collections;
using System.Collections.Generic;
using AcierProtocol;
using UnityEngine;

public class ObjectFade : MonoBehaviour
{
	public class Description
	{
		public bool fadeIn;

		public EFadeMode fadeMode;

		public float fadeDuration = 0.4f;

		public Action<GameObject, MonoBehaviour> onFadeDoneCallback;

		public Description()
		{
		}

		public Description(Description _other)
		{
			fadeIn = _other.fadeIn;
			fadeMode = _other.fadeMode;
			fadeDuration = _other.fadeDuration;
			onFadeDoneCallback = _other.onFadeDoneCallback;
		}
	}

	private class MaterialEntry
	{
		public Material mat;

		public Shader originalShader;
	}

	private List<MaterialEntry> m_ControlledMaterials = new List<MaterialEntry>();

	private List<Renderer> m_ControlledRenderers = new List<Renderer>();

	private Description m_Description = new Description();

	private float m_ElapsedTime;

	public bool m_FadeInProgress;

	private static int s_FadeColorId = -1;

	public bool FadeInProgress
	{
		get
		{
			return m_FadeInProgress;
		}
		set
		{
			m_FadeInProgress = value;
		}
	}

	public float FadeDuration
	{
		get
		{
			return m_Description.fadeDuration;
		}
		set
		{
			m_Description.fadeDuration = value;
		}
	}

	public Action<GameObject, MonoBehaviour> OnFadeDoneCallback
	{
		get
		{
			return m_Description.onFadeDoneCallback;
		}
		set
		{
			m_Description.onFadeDoneCallback = value;
		}
	}

	public bool FadeDone
	{
		get
		{
			return !m_FadeInProgress;
		}
	}

	public EFadeMode FadeMode
	{
		get
		{
			return m_Description.fadeMode;
		}
		set
		{
			m_Description.fadeMode = value;
			UpdateMaterialKeywords();
		}
	}

	public static bool CanBeFaded(Material _mat)
	{
		if (_mat != null)
		{
			if (!_mat.shader.name.EndsWith("Fade"))
			{
				return Shader.Find(_mat.shader.name + "Fade") != null;
			}
			return true;
		}
		return false;
	}

	private void Start()
	{
		if (s_FadeColorId < 0)
		{
			s_FadeColorId = Shader.PropertyToID("_FadeColor");
		}
	}

	public void InitFromDescription(Description _desc)
	{
		m_Description = new Description(_desc);
	}

	public Description GetDescription()
	{
		return new Description(m_Description);
	}

	public void Reset(bool _fadeIn, bool _startAnimation)
	{
		s_FadeColorId = Shader.PropertyToID("_FadeColor");
		m_Description.fadeIn = _fadeIn;
		m_ElapsedTime = (_fadeIn ? 0f : m_Description.fadeDuration);
		ResetMaterials();
		CollectAndReplaceMaterials();
		UpdateMaterialKeywords();
		m_FadeInProgress = _startAnimation;
		SetInterpolationValue(_fadeIn ? 0f : 1f);
	}

	[ContextMenu("Fade In")]
	public void FadeIn()
	{
		Reset(true, true);
	}

	[ContextMenu("Fade Out")]
	public void FadeOut()
	{
		Reset(false, true);
	}

	private void UpdateMaterialKeywords()
	{
		if (m_ControlledMaterials == null)
		{
			return;
		}
		int count = m_ControlledMaterials.Count;
		if (m_Description.fadeMode == EFadeMode.Simple_EFadeMode)
		{
			for (int i = 0; count > i; i++)
			{
				m_ControlledMaterials[i].mat.EnableKeyword("SIMPLE_FADE");
				m_ControlledMaterials[i].mat.DisableKeyword("FANCY_FADE");
				m_ControlledMaterials[i].mat.DisableKeyword("RASTER_FADE");
			}
		}
		if (m_Description.fadeMode == EFadeMode.Fancy_EFadeMode)
		{
			for (int j = 0; count > j; j++)
			{
				m_ControlledMaterials[j].mat.DisableKeyword("SIMPLE_FADE");
				m_ControlledMaterials[j].mat.EnableKeyword("FANCY_FADE");
				m_ControlledMaterials[j].mat.DisableKeyword("RASTER_FADE");
			}
		}
		if (m_Description.fadeMode == EFadeMode.Raster_EFadeMode)
		{
			for (int k = 0; count > k; k++)
			{
				m_ControlledMaterials[k].mat.DisableKeyword("SIMPLE_FADE");
				m_ControlledMaterials[k].mat.DisableKeyword("FANCY_FADE");
				m_ControlledMaterials[k].mat.EnableKeyword("RASTER_FADE");
			}
		}
	}

	public void CollectAndReplaceMaterials()
	{
		Renderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<Renderer>();
		m_ControlledMaterials.Clear();
		m_ControlledRenderers.Clear();
		int num = componentsInChildren.Length;
		for (int i = 0; num > i; i++)
		{
			Material[] materials = componentsInChildren[i].materials;
			bool flag = false;
			int num2 = materials.Length;
			for (int j = 0; num2 > j; j++)
			{
				Material material = materials[j];
				if (!material.shader.name.EndsWith("Fade"))
				{
					Shader shader = Shader.Find(material.shader.name + "Fade");
					if (shader != null)
					{
						MaterialEntry materialEntry = new MaterialEntry();
						materialEntry.mat = material;
						materialEntry.originalShader = material.shader;
						material.shader = shader;
						flag = true;
						m_ControlledMaterials.Add(materialEntry);
					}
				}
				if (material.shader.name.EndsWith("AvatarSilhouette"))
				{
					MaterialEntry materialEntry2 = new MaterialEntry();
					materialEntry2.mat = material;
					materialEntry2.originalShader = material.shader;
					material.shader = material.shader;
					flag = true;
					m_ControlledMaterials.Add(materialEntry2);
				}
			}
			if (flag)
			{
				m_ControlledRenderers.Add(componentsInChildren[i]);
			}
		}
	}

	public void ResetMaterials()
	{
		int count = m_ControlledMaterials.Count;
		for (int i = 0; count > i; i++)
		{
			MaterialEntry materialEntry = m_ControlledMaterials[i];
			if (materialEntry.mat != null)
			{
				materialEntry.mat.shader = materialEntry.originalShader;
			}
		}
		m_FadeInProgress = false;
	}

	public void OnDestroy()
	{
		ResetMaterials();
	}

	public void SetInterpolationValue(float _value)
	{
		int count = m_ControlledMaterials.Count;
		for (int i = 0; count > i; i++)
		{
			if (m_ControlledMaterials[i].mat != null)
			{
				Color color = m_ControlledMaterials[i].mat.GetColor(s_FadeColorId);
				color.a = _value;
				m_ControlledMaterials[i].mat.SetColor(s_FadeColorId, color);
			}
		}
		bool flag = _value > 0.05f;
		for (int j = 0; j < m_ControlledRenderers.Count; j++)
		{
			if (m_ControlledRenderers[j].enabled != flag)
			{
				m_ControlledRenderers[j].enabled = flag;
			}
		}
	}

	public void ReenableRenderers()
	{
		for (int i = 0; i < m_ControlledRenderers.Count; i++)
		{
			m_ControlledRenderers[i].enabled = true;
		}
	}

	public void Update()
	{
		if (m_FadeInProgress)
		{
			if (m_Description.fadeIn)
			{
				m_ElapsedTime += Time.deltaTime;
			}
			else
			{
				m_ElapsedTime -= Time.deltaTime;
			}
			float num = m_ElapsedTime / m_Description.fadeDuration;
			SetInterpolationValue(Mathf.Clamp01(num));
			m_FadeInProgress = num < 1f && num > 0f;
			if (!m_FadeInProgress)
			{
				StartCoroutine(EndFading());
			}
		}
	}

	private IEnumerator EndFading()
	{
		float elapsedDelay = 0f;
		while (elapsedDelay < 0.5f)
		{
			elapsedDelay += Time.deltaTime;
			yield return null;
		}
		ResetMaterials();
		if (m_Description.onFadeDoneCallback != null)
		{
			m_Description.onFadeDoneCallback(gameObject, this);
		}
	}
}
