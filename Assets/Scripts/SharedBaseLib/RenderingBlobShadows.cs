using SharedBaseLib.Core;
using UbiCore.Utils.Extensions;
using UnityEngine;

public class RenderingBlobShadows : MonoBehaviour
{
	private string[] sShadowLayers;

	private BBList<GameObject> m_humanoids = new BBList<GameObject>();

	private Material m_bodyMaterial;

	private Material m_footMaterial;

	private int m_shadowLayerMask;

	private bool m_usesRealtimeShadows;

	private void Start()
	{
		sShadowLayers = new string[1] { "StaticCollider_Shadow" };
		string[] array = sShadowLayers;
		foreach (string layerName in array)
		{
			m_shadowLayerMask |= 1 << LayerMask.NameToLayer(layerName);
		}
	}

	private void RefreshUsesRealtimeShadows()
	{
		m_usesRealtimeShadows = SystemInfo.supportsShadows;
		if (m_usesRealtimeShadows)
		{
			m_usesRealtimeShadows &= QualitySettings.shadowDistance > 0f;
		}
	}

	public void CreateShadows(GameObject _go, bool _footShadows = false)
	{
		CreateBlobShadows(_go, _footShadows);
		if (!m_humanoids.Contains(_go))
		{
			m_humanoids.Add(_go);
		}
	}

	public void RemoveShadows(GameObject _go)
	{
		Object.Destroy(_go.FindChildByName("Shadows"));
		if (m_humanoids.Contains(_go))
		{
			m_humanoids.Remove(_go);
		}
	}

	private void CreateBlobShadows(GameObject _go, bool _footShadows = false)
	{
		GameObject gameObject = new GameObject("Shadows");
		CreateMainShadow(_go, gameObject, "Head");
		if (_footShadows)
		{
			CreateFootShadow(_go, gameObject, "LeftFoot");
			CreateFootShadow(_go, gameObject, "RightFoot");
		}
		gameObject.transform.parent = _go.transform;
	}

	private void CreateMainShadow(GameObject _go, GameObject _parent, string _name)
	{
		Transform transform = _go.transform.FindChildRecursive(_name);
		if (transform != null)
		{
			GameObject obj = new GameObject(_name + "Shadow");
			FS_ShadowSimple fS_ShadowSimple = obj.AddComponent<FS_ShadowSimple>();
			if (m_bodyMaterial == null)
			{
				CreateShadowMaterial(ref m_bodyMaterial, "Shadows/main_shadow");
			}
			fS_ShadowSimple.shadowMaterial = m_bodyMaterial;
			fS_ShadowSimple.layerMask = m_shadowLayerMask;
			fS_ShadowSimple.inheritFromTransform = transform.gameObject;
			fS_ShadowSimple.maxProjectionDistance = 3f;
			fS_ShadowSimple.inheritWorldOffset = new Vector3(0f, -0.5f, 0f);
			fS_ShadowSimple.girth = 0.65f;
			fS_ShadowSimple.opacity = 0.9f;
			fS_ShadowSimple.shadowHoverHeight = 0.01f;
			obj.transform.parent = _parent.transform;
		}
	}

	private void CreateFootShadow(GameObject _go, GameObject _parent, string _name)
	{
		Transform transform = _go.transform.FindChildRecursive(_name);
		if (transform != null)
		{
			GameObject obj = new GameObject(_name + "Shadow");
			FS_ShadowSimple fS_ShadowSimple = obj.AddComponent<FS_ShadowSimple>();
			if (m_footMaterial == null)
			{
				CreateShadowMaterial(ref m_footMaterial, "Shadows/foot_shadow");
			}
			fS_ShadowSimple.shadowMaterial = m_footMaterial;
			fS_ShadowSimple.layerMask = m_shadowLayerMask;
			fS_ShadowSimple.inheritFromTransform = transform.gameObject;
			fS_ShadowSimple.maxProjectionDistance = 0.4f;
			fS_ShadowSimple.inheritWorldOffset = new Vector3(0f, 0f, 0f);
			fS_ShadowSimple.girth = 0.4f;
			fS_ShadowSimple.opacity = 0.6f;
			fS_ShadowSimple.shadowHoverHeight = 0.01f;
			obj.transform.parent = _parent.transform;
		}
	}

	private void CreateShadowMaterial(ref Material _material, string _textureName)
	{
		Texture2D texture2D = Resources.Load(_textureName, typeof(Texture2D)) as Texture2D;
		if (texture2D != null)
		{
			_material = new Material(Shader.Find("Hidden/FS_ShadowShader/UnlitAlphaBlend"));
			_material.mainTexture = texture2D;
		}
	}

	private void DeactivateAllShadowCasters()
	{
		Renderer[] array = Object.FindObjectsOfType(typeof(Renderer)) as Renderer[];
		for (int i = 0; i < array.Length; i++)
		{
			array[i].castShadows = false;
		}
	}

	private void OnPreInitialize()
	{
		m_humanoids.Clear();
		RefreshUsesRealtimeShadows();
		DeactivateAllShadowCasters();
	}

	public void OnMissionUnload()
	{
		m_humanoids.Clear();
		m_footMaterial = null;
		m_bodyMaterial = null;
	}

	public void OnQualitySettingsChanged()
	{
		Debug.Log("Quality Settings changed, supportsShadows: " + SystemInfo.supportsShadows.ToString() + " distance: " + QualitySettings.shadowDistance);
		RefreshUsesRealtimeShadows();
		if (m_usesRealtimeShadows)
		{
			m_footMaterial = null;
			m_bodyMaterial = null;
		}
		foreach (GameObject humanoid in m_humanoids)
		{
			Transform transform = humanoid.transform.Find("Shadows");
			if (transform != null)
			{
				Object.Destroy(transform.gameObject);
			}
			CreateShadows(humanoid);
		}
	}
}
