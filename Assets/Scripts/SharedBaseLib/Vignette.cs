using System;
using UnityEngine;

public class Vignette : MonoBehaviour
{
	public enum Type
	{
		Hit = 0,
		CritHealth = 1,
		EagleVision = 2,
		Debug = 3,
		Count = 4
	}

	private Transform m_VignetteTransform;

	private Renderer m_Renderer;

	private readonly Color[] m_colors = new Color[4];

	private readonly Color m_invisible = new Color(0f, 0f, 0f, 0f);

	public Color VignetteColor { get; private set; }

	private Mesh CreateVignetteMesh()
	{
		new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
		Color32 color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		Mesh mesh = new Mesh();
		Vector3[] array = new Vector3[8];
		Vector2[] array2 = new Vector2[8];
		Color32[] array3 = new Color32[8];
		array[0] = new Vector3(-1f, -1f, 0f);
		array[1] = new Vector3(0f, -1f, 0f);
		array[2] = new Vector3(1f, -1f, 0f);
		array[3] = new Vector3(-1f, 0f, 0f);
		array[4] = new Vector3(1f, 0f, 0f);
		array[5] = new Vector3(-1f, 1f, 0f);
		array[6] = new Vector3(0f, 1f, 0f);
		array[7] = new Vector3(1f, 1f, 0f);
		array3[0] = color;
		array3[1] = color;
		array3[2] = color;
		array3[3] = color;
		array3[4] = color;
		array3[5] = color;
		array3[6] = color;
		array3[7] = color;
		array2[0] = new Vector2(0f, 1f);
		array2[1] = new Vector2(1f, 1f);
		array2[2] = new Vector2(0f, 1f);
		array2[3] = new Vector2(0f, 0f);
		array2[4] = new Vector2(0f, 0f);
		array2[5] = new Vector2(0f, 1f);
		array2[6] = new Vector2(1f, 1f);
		array2[7] = new Vector2(0f, 1f);
		int[] triangles = new int[12]
		{
			0, 3, 1, 1, 4, 2, 3, 5, 6, 4,
			6, 7
		};
		mesh.vertices = array;
		mesh.colors32 = array3;
		mesh.uv = array2;
		mesh.subMeshCount = 1;
		mesh.SetTriangles(triangles, 0);
		return mesh;
	}

	private GameObject CreateVignette()
	{
		GameObject obj = new GameObject("Vignette");
		obj.transform.parent = base.transform;
		MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
		obj.AddComponent<MeshFilter>().sharedMesh = CreateVignetteMesh();
		meshRenderer.material = Resources.Load("HUD/Materials/CameraEffects") as Material;
		m_Renderer = meshRenderer;
		return obj;
	}

	private void Start()
	{
		m_VignetteTransform = CreateVignette().transform;
	}

	private void Update()
	{
		Color color = m_invisible;
		float num = 0f;
		for (int i = 0; i < m_colors.Length; i++)
		{
			Color color2 = m_colors[i];
			num += color2.a;
			Color color3 = color;
			Color color4 = color2;
			color = color3 + color4 * color4.a;
		}
		if (num > 0.0019607844f)
		{
			color /= num;
		}
		else
		{
			color = m_invisible;
		}
		VignetteColor = color;
		if (VignetteColor.a > 0.003921569f)
		{
			Camera component = GetComponent<Camera>();
			Transform vignetteTransform = m_VignetteTransform;
			m_Renderer.material.SetColor("_TintColor", VignetteColor);
			m_VignetteTransform.gameObject.SetActive(true);
			if ((bool)component)
			{
				float num2 = component.nearClipPlane + 0.1f;
				float num3 = Mathf.Tan(component.fieldOfView * ((float)Math.PI / 180f) * 0.5f) * num2;
				m_VignetteTransform.localPosition = new Vector3(0f, 0f, num2);
				m_VignetteTransform.localScale = new Vector3(num3 * component.aspect, num3, 1f);
				m_VignetteTransform.localRotation = Quaternion.identity;
			}
		}
		else
		{
			m_VignetteTransform.gameObject.SetActive(false);
		}
	}

	public void SetVignetteColor(Type _type, Color _color, float _intensity)
	{
		m_colors[(int)_type] = _color;
		m_colors[(int)_type].a *= _intensity;
	}
}
