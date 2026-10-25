using UnityEngine;

[ExecuteInEditMode]
public class RenderingEnvironmentLightingSettings : MonoBehaviour
{
	[SerializeField]
	private float m_shininess = 1.7f;

	[SerializeField]
	[HideInInspector]
	private Vector3 m_MobileLightmapMul;

	[SerializeField]
	[HideInInspector]
	private Vector3 m_MobileLightmapAdd;

	private bool m_isDirty = true;

	public float Shininess
	{
		get
		{
			return m_shininess;
		}
		set
		{
			m_isDirty = true;
			m_shininess = value;
		}
	}

	public Vector3 MobileLightmapMul
	{
		get
		{
			return m_MobileLightmapMul;
		}
		set
		{
			m_isDirty = true;
			m_MobileLightmapMul = value;
		}
	}

	public Vector3 MobileLightmapAdd
	{
		get
		{
			return m_MobileLightmapAdd;
		}
		set
		{
			m_isDirty = true;
			m_MobileLightmapAdd = value;
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

	public void CopyFrom(RenderingEnvironmentLightingSettings _src)
	{
		Shininess = _src.m_shininess;
		MobileLightmapMul = _src.m_MobileLightmapMul;
		MobileLightmapAdd = _src.m_MobileLightmapAdd;
	}

	public void Update()
	{
		if (Application.isEditor)
		{
			UpdateShaderVariables();
		}
	}

	public void UpdateShaderVariables()
	{
		Shader.SetGlobalFloat("_Shininess", Shininess);
		Shader.SetGlobalVector("acier_LightmapDecodeMul", MobileLightmapMul);
		Shader.SetGlobalVector("acier_LightmapDecodeAdd", MobileLightmapAdd);
		IsDirty = false;
	}
}
