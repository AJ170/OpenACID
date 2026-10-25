using System;
using AcierProtocol;
using UnityEngine;

public class RenderingSettings
{
	private readonly string m_keyShaderLOD = "ShaderLOD";

	private static RenderingSettings ms_MyInstance;

	private int m_shaderLOD;

	public static RenderingSettings Instance
	{
		get
		{
			if (ms_MyInstance == null)
			{
				Initialize();
			}
			return ms_MyInstance;
		}
	}

	public int shaderLOD
	{
		get
		{
			return m_shaderLOD;
		}
		set
		{
			PlayerPrefs.SetInt(m_keyShaderLOD, m_shaderLOD = value);
			Apply();
		}
	}

	public static int FloorShaderLOD(int lod)
	{
		if (lod % 100 != 0)
		{
			return (lod / 100 + 1) * 100;
		}
		return lod;
	}

	public static void Initialize()
	{
		ms_MyInstance = new RenderingSettings();
	}

	public RenderingSettings()
	{
		m_shaderLOD = PlayerPrefs.GetInt(m_keyShaderLOD, int.MaxValue);
		Validate();
		Apply();
	}

	private void Validate()
	{
		if (Array.FindIndex(Enum.GetValues(typeof(EQualityShaderLOD)) as int[], (int o) => o == m_shaderLOD || o == m_shaderLOD + 1) == -1)
		{
			m_shaderLOD = 500;
		}
	}

	public void Apply()
	{
		Shader.globalMaximumLOD = m_shaderLOD;
	}
}
