using System;
using SharedBaseLib.System;
using UnityEngine;

[Serializable]
public class RenderingLighting : MonoBehaviour
{
	private RenderingCharacterLightingSettings m_CharSettings;

	private RenderingCharacterLightingSettings m_OldCharSettings;

	private RenderingEnvironmentLightingSettings m_EnvSettings;

	private Texture2D m_rampTex;

	private Quaternion m_lightRotation;

	private Quaternion m_lightFlatRotation;

	private int m_idFog;

	private int m_idRampTex;

	public RenderingCharacterLightingSettings CharSettings
	{
		get
		{
			return m_CharSettings;
		}
	}

	public RenderingEnvironmentLightingSettings EnvSettings
	{
		get
		{
			return m_EnvSettings;
		}
	}

	public Light SceneLight { get; private set; }

	public RenderingLighting()
	{
		m_idFog = Shader.PropertyToID("acid_Fog");
		m_idRampTex = Shader.PropertyToID("_RampTex");
	}

	public void InitializeFromScene()
	{
		FetchSceneLight();
	}

	public void InitializeTextures()
	{
		LoadRampTexture();
	}

	public static Light FindSceneLight()
	{
		Light[] array = UnityEngine.Object.FindObjectsOfType<Light>();
		float num = float.MinValue;
		Light result = null;
		Light[] array2 = array;
		foreach (Light light in array2)
		{
			if (light.type == LightType.Directional && (light.cullingMask & (int)Layers.CharacterLayerMask) != 0 && light.intensity > num)
			{
				num = light.intensity;
				result = light;
			}
		}
		return result;
	}

	public void FetchSceneLight()
	{
		SceneLight = null;
		Light light = FindSceneLight();
		if (light != null)
		{
			InitializeFromSceneLight(light);
		}
		if (Application.isPlaying)
		{
			bool flag = SceneLight != null;
		}
	}

	private void InitializeFromSceneLight(Light l)
	{
		SceneLight = l;
		SceneLight.shadows = LightShadows.Hard;
		SceneLight.shadowStrength = 1f;
		if (m_CharSettings != null && m_CharSettings.IsAnimus)
		{
			SceneLight.shadowBias = 0.005f;
		}
		else
		{
			SceneLight.shadowBias = 0.02f;
		}
		m_lightRotation = SceneLight.transform.rotation;
		Vector3 forward = SceneLight.transform.forward;
		forward.y = 0f;
		forward.Normalize();
		m_lightFlatRotation = Quaternion.LookRotation(forward);
	}

	private void LoadRampTexture()
	{
		m_rampTex = (Texture2D)Resources.Load("Character/ramps/character_shading_ramps");
	}

	public RenderingCharacterLightingSettings LoadDefaultCharacterLightSetup()
	{
		GameObject gameObject = Resources.Load("Character/RenderingSetup") as GameObject;
		if (gameObject != null)
		{
			RenderingCharacterLightingSettings component = gameObject.GetComponent<RenderingCharacterLightingSettings>();
			if (component != null)
			{
				return component;
			}
		}
		return null;
	}

	public RenderingEnvironmentLightingSettings LoadDefaultEnvironmentLightSetup()
	{
		GameObject gameObject = Resources.Load("Character/RenderingSetup") as GameObject;
		if (gameObject != null)
		{
			RenderingEnvironmentLightingSettings component = gameObject.GetComponent<RenderingEnvironmentLightingSettings>();
			if (component != null)
			{
				return component;
			}
		}
		return null;
	}

	public void ChangeLightSetup(RenderingCharacterLightingSettings _charSettings, RenderingEnvironmentLightingSettings _envSettings)
	{
		m_CharSettings = _charSettings;
		m_EnvSettings = _envSettings;
	}

	public void RefreshGlobalsWithInternalSettings()
	{
		if (SceneLight == null || m_OldCharSettings != m_CharSettings)
		{
			FetchSceneLight();
		}
		if (m_CharSettings != null && m_EnvSettings != null)
		{
			RefreshGlobals(m_CharSettings, m_EnvSettings);
		}
		m_OldCharSettings = m_CharSettings;
	}

	public void RefreshGlobals(RenderingCharacterLightingSettings _charSettings, RenderingEnvironmentLightingSettings _envSettings)
	{
		if (m_rampTex != null)
		{
			Shader.SetGlobalTexture(m_idRampTex, m_rampTex);
		}
		Vector4 vec = new Vector4(0f, float.MaxValue, 0f);
		if (RenderSettings.fog)
		{
			vec.x = RenderSettings.fogStartDistance;
			vec.y = RenderSettings.fogEndDistance;
			vec.z = RenderSettings.fogDensity;
		}
		Shader.SetGlobalVector(m_idFog, vec);
		_charSettings.UpdateShaderVariables(SceneLight);
		_envSettings.UpdateShaderVariables();
	}

	public void OnMissionUnload()
	{
		m_CharSettings = null;
	}
}
