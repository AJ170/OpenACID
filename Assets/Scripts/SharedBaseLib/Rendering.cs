using UbiCore.Utils.Extensions;
using UnityEngine;

public class Rendering : MonoBehaviour
{
	public enum CullMode
	{
		Back = 0,
		Front = 1,
		Off = 2
	}

	public static Rendering Instance;

	[SerializeField]
	private RenderingLighting m_lighting;

	private RenderingBlobShadows m_shadows;

	private RenderingOutlines m_outlines;

	private RenderingSky m_skybox;

	public const float OCCLUSION_SMALLEST_HOLE = 0.25f;

	public Transform Player { get; private set; }

	public RenderingLighting Lighting
	{
		get
		{
			return m_lighting;
		}
	}

	public static RenderingSky RetrieveSkyMesh()
	{
		if (Application.isPlaying && Instance != null)
		{
			return Instance.m_skybox;
		}
		if (!Application.isPlaying)
		{
			return Object.FindObjectOfType<RenderingSky>();
		}
		return null;
	}

	private void Awake()
	{
		Instance = this;
		RenderingSettings.Initialize();
		m_shadows = base.gameObject.AddComponent<RenderingBlobShadows>();
		m_outlines = base.gameObject.AddComponent<RenderingOutlines>();
		m_lighting = base.gameObject.AddComponent<RenderingLighting>();
		Shader.SetGlobalFloat("_EagleVisionAmount", 0f);
		Shader.SetGlobalInt("g_Debug", 0);
	}

	private void Start()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Application.targetFrameRate = 60;
		}
		RenderingSettings.Instance.Apply();
		m_lighting.InitializeTextures();
		m_lighting.InitializeFromScene();
		UpdateTextureResolutionBias();
	}

	private void LateUpdate()
	{
		if (Player != null)
		{
			Shader.SetGlobalMatrix("_Player", Player.localToWorldMatrix);
		}
		UpdateLighting();
	}

	private void UpdateLighting()
	{
		m_lighting.RefreshGlobalsWithInternalSettings();
	}

	public void OnPreInitialize()
	{
		Player = null;
		m_lighting.InitializeFromScene();
	}

	public void OnPostInitialize()
	{
		m_skybox = Object.FindObjectOfType<RenderingSky>();
		if (m_skybox != null)
		{
			m_skybox.Initialize();
		}
		Camera main = Camera.main;
		if (main != null)
		{
			RefreshPostEffects(main);
		}
		GameObject gameObject = GameObject.Find("EnvScenario/MapScenario");
		if (gameObject != null)
		{
			RenderingCharacterLightingSettings renderingCharacterLightingSettings = gameObject.GetComponent<RenderingCharacterLightingSettings>();
			RenderingEnvironmentLightingSettings renderingEnvironmentLightingSettings = gameObject.GetComponent<RenderingEnvironmentLightingSettings>();
			if (renderingCharacterLightingSettings == null)
			{
				Debug.LogWarning("No character light settings found! defaulting to editor lighting.");
				renderingCharacterLightingSettings = m_lighting.LoadDefaultCharacterLightSetup();
			}
			if (renderingEnvironmentLightingSettings == null)
			{
				Debug.LogWarning("No environment light settings found! defaulting to editor lighting.");
				renderingEnvironmentLightingSettings = m_lighting.LoadDefaultEnvironmentLightSetup();
			}
			if (renderingCharacterLightingSettings != null && renderingEnvironmentLightingSettings != null)
			{
				m_lighting.ChangeLightSetup(renderingCharacterLightingSettings, renderingEnvironmentLightingSettings);
			}
		}
		else
		{
			Debug.LogWarning("Could not find UIMapData for mood setup!");
		}
		Shader.WarmupAllShaders();
	}

	private void OnPlayerSpawned(GameObject _player)
	{
		m_outlines.CreateOutlines(_player);
		OnRegisterShadows(_player);
		Player = _player.transform;
	}

	public void OnRegisterShadows(GameObject _character)
	{
		m_shadows.CreateShadows(_character);
	}

	public void OnUnregisterShadows(GameObject _character)
	{
		m_shadows.RemoveShadows(_character);
	}

	public void OnMissionUnload()
	{
		Player = null;
		m_shadows.OnMissionUnload();
		m_lighting.OnMissionUnload();
	}

	public static void RefreshPostEffects(Camera c)
	{
		RefreshPostEffects(c, 0);
	}

	public static void RefreshPostEffects(Camera c, int _upscaleFactor)
	{
		if (CombinedPostEffect.IsSupported())
		{
			ColorCorrectionSettings obj = Object.FindObjectOfType(typeof(ColorCorrectionSettings)) as ColorCorrectionSettings;
			BloomSettings bloomSettings = Object.FindObjectOfType(typeof(BloomSettings)) as BloomSettings;
			DesaturationSettings desaturationSettings = Object.FindObjectOfType(typeof(DesaturationSettings)) as DesaturationSettings;
			LightningEffects lightningEffects = Object.FindObjectOfType(typeof(LightningEffects)) as LightningEffects;
			bool num = obj != null;
			bool flag = bloomSettings != null;
			bool flag2 = desaturationSettings != null;
			bool flag3 = lightningEffects != null;
			bool flag4 = (num || flag || flag2 || flag3) && QualitySettings.GetQualityLevel() >= 2;
			CombinedPostEffect component = c.GetComponent<CombinedPostEffect>();
			if (component == null)
			{
				if (flag4)
				{
					c.gameObject.AddComponent<CombinedPostEffect>();
				}
			}
			else if (!flag4)
			{
				Object.Destroy(component);
			}
		}
		c.gameObject.AddComponentIfNotExist<Vignette>();
		if (_upscaleFactor > 1 && c.gameObject.GetComponent<RenderingUpscaler>() == null)
		{
			c.gameObject.AddComponent<RenderingUpscaler>().SetupLowResRenderTarget(_downscaleFactor: 1f / (float)_upscaleFactor, _fullResWidth: c.pixelWidth, _fullResHeight: c.pixelHeight, _depthBits: 24, _format: RenderTextureFormat.Default);
		}
	}

	public void OnQualitySettingsChanged()
	{
		UpdateTextureResolutionBias();
		Camera main = Camera.main;
		if (main != null)
		{
			RefreshPostEffects(main);
		}
	}

	public static void UpdateTextureResolutionBias()
	{
		float value = 1f / (float)(1 + QualitySettings.masterTextureLimit);
		Shader.SetGlobalFloat("_TextureResBias", value);
	}

	public static void ConfigureLightProbes(GameObject _go, Transform _bone = null)
	{
		Renderer[] componentsInChildren = _go.GetComponentsInChildren<Renderer>(true);
		int num = componentsInChildren.Length;
		for (int i = 0; num > i; i++)
		{
			componentsInChildren[i].useLightProbes = true;
			componentsInChildren[i].probeAnchor = _bone;
		}
	}
}
