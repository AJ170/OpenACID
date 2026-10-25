using UnityEngine;

[ExecuteInEditMode]
public class RenderingCharacterLightingSettings : MonoBehaviour
{
	[SerializeField]
	private bool m_useLightprobes = true;

	[Range(0f, 1f)]
	[SerializeField]
	private float m_lightDimmer = 1f;

	[SerializeField]
	private float m_sphericalHarmonicsTerm;

	[SerializeField]
	private float m_desaturationWhenLit = 0.5f;

	[SerializeField]
	private float m_selfshadows = 0.2f;

	[SerializeField]
	private Color m_shadowColorOnEnvironment = Color.grey;

	[SerializeField]
	private Color m_glossiness = Color.magenta;

	[SerializeField]
	private Color m_propGlossiness = Color.magenta;

	[SerializeField]
	private bool m_isAnimus;

	[SerializeField]
	private Cubemap m_specularHiCubemap;

	[SerializeField]
	private Cubemap m_effectCubemap;

	[SerializeField]
	private float m_AoIntensity = 1f;

	[SerializeField]
	private Color m_CharShadowColor = Color.black;

	private bool m_isDirty = true;

	public bool UseLightProbes
	{
		get
		{
			return m_useLightprobes;
		}
		set
		{
			m_isDirty |= m_useLightprobes != value;
			m_useLightprobes = value;
		}
	}

	public float LightDimmer
	{
		get
		{
			return m_lightDimmer;
		}
		set
		{
			m_isDirty = true;
			m_lightDimmer = value;
		}
	}

	public float SphericalHarmonicsTerm
	{
		get
		{
			return m_sphericalHarmonicsTerm;
		}
		set
		{
			m_isDirty = true;
			m_sphericalHarmonicsTerm = value;
		}
	}

	public float DesaturationWhenLit
	{
		get
		{
			return m_desaturationWhenLit;
		}
		set
		{
			m_isDirty = true;
			m_desaturationWhenLit = value;
		}
	}

	public float SelfShadows
	{
		get
		{
			return m_selfshadows;
		}
		set
		{
			m_isDirty = true;
			m_selfshadows = value;
		}
	}

	public Color ShadowColorOnEnvironment
	{
		get
		{
			return m_shadowColorOnEnvironment;
		}
		set
		{
			m_isDirty = true;
			m_shadowColorOnEnvironment = value;
		}
	}

	public Color Glossiness
	{
		get
		{
			return m_glossiness;
		}
		set
		{
			m_isDirty = true;
			m_glossiness = value;
		}
	}

	public Color PropGlossiness
	{
		get
		{
			return m_propGlossiness;
		}
		set
		{
			m_isDirty = true;
			m_propGlossiness = value;
		}
	}

	public bool IsAnimus
	{
		get
		{
			return m_isAnimus;
		}
		set
		{
			m_isDirty |= m_isAnimus != value;
			m_isAnimus = value;
		}
	}

	public Cubemap SpecularHiCubemap
	{
		get
		{
			return m_specularHiCubemap;
		}
		set
		{
			m_isDirty |= m_specularHiCubemap != value;
			m_specularHiCubemap = value;
		}
	}

	public Cubemap EffectCubemap
	{
		get
		{
			return m_effectCubemap;
		}
		set
		{
			m_isDirty |= m_effectCubemap != value;
			m_effectCubemap = value;
		}
	}

	public float AoIntensity
	{
		get
		{
			return m_AoIntensity;
		}
		set
		{
			m_isDirty = true;
			m_AoIntensity = value;
		}
	}

	public Color CharShadowColor
	{
		get
		{
			return m_CharShadowColor;
		}
		set
		{
			m_isDirty = true;
			m_CharShadowColor = value;
		}
	}

	public bool IsDirty
	{
		get
		{
			return m_isDirty;
		}
		set
		{
			m_isDirty = value;
		}
	}

	public void CopyFrom(RenderingCharacterLightingSettings _src)
	{
		UseLightProbes = _src.m_useLightprobes;
		LightDimmer = _src.m_lightDimmer;
		SphericalHarmonicsTerm = _src.m_sphericalHarmonicsTerm;
		DesaturationWhenLit = _src.m_desaturationWhenLit;
		SelfShadows = _src.m_selfshadows;
		ShadowColorOnEnvironment = _src.m_shadowColorOnEnvironment;
		Glossiness = _src.m_glossiness;
		PropGlossiness = _src.m_propGlossiness;
		IsAnimus = _src.m_isAnimus;
		SpecularHiCubemap = _src.m_specularHiCubemap;
		EffectCubemap = _src.m_effectCubemap;
		AoIntensity = _src.m_AoIntensity;
		CharShadowColor = _src.m_CharShadowColor;
	}

	public void Update()
	{
		if (Application.isEditor)
		{
			UpdateShaderVariables();
		}
	}

	public void UpdateShaderVariables(Light _sceneLight = null)
	{
		if (SpecularHiCubemap != null)
		{
			Shader.SetGlobalTexture("_HiSpecCube", SpecularHiCubemap);
		}
		if (EffectCubemap != null)
		{
			Shader.SetGlobalTexture("_EffectCube", EffectCubemap);
		}
		Vector4 vec = new Vector4(SphericalHarmonicsTerm, DesaturationWhenLit, AoIntensity, 0f);
		Shader.SetGlobalVector("_LightParameters", vec);
		Shader.SetGlobalColor("_Glossiness", Glossiness);
		Shader.SetGlobalColor("_PropGlossiness", PropGlossiness);
		Shader.SetGlobalVector("_Shadows", new Vector4(SelfShadows, 1f - SelfShadows, 0f, 0f));
		Shader.SetGlobalColor("_EnvAmbientColor", ShadowColorOnEnvironment);
		Shader.SetGlobalColor("_CharacterShadowColor", CharShadowColor);
		if (_sceneLight == null)
		{
			_sceneLight = RenderingLighting.FindSceneLight();
		}
		if (_sceneLight != null)
		{
			Vector3 forward = _sceneLight.transform.forward;
			Shader.SetGlobalVector("_LightDir", new Vector4(forward.x, forward.y, forward.z, LightDimmer));
		}
		IsDirty = false;
	}
}
