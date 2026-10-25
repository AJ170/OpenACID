using System;
using UnityEngine;

public class DynamicSwipe : MonoBehaviour
{
	public float m_skew = 20f;

	public float m_swipeAngle = 45f;

	public Material m_material;

	public float m_AnimationSpeed;

	public Vector2 m_StartOffset;

	public Vector2 m_TargetOffset;

	public Vector4 TFW;

	private AnimateAchievementHighlight a;

	private GameObject m_go;

	public GameObject Container;

	private static Color s_color = new Color(1f, 1f, 1f, 1f);

	public UIWidget m_widget;

	[ContextMenu("InitializeAndPlay")]
	public void InitializeAndPlay()
	{
		if (m_widget != null)
		{
			Initialize(false, m_widget.width, m_widget.height);
			Animate();
		}
	}

	public void Initialize(bool _right, int _width, int _height)
	{
		GenerateMesh(_right, _width, _height);
	}

	public void InitializeDoubleSkew(bool _right, int _width, int _height)
	{
		GenerateMeshDoubleSkew(_right, _width, _height);
	}

	public void ActivateAnimation()
	{
		if (!a.gameObject.activeSelf && a.startOnEnable)
		{
			a.gameObject.SetActive(true);
		}
		else
		{
			StartCoroutine(a.AnimateHighlight(s_color));
		}
	}

	public void Animate()
	{
		StartCoroutine(a.AnimateHighlight(s_color));
	}

	private void GenerateMeshDoubleSkew(bool _right, float _width, float _height)
	{
		if (m_go != null)
		{
			UnityEngine.Object.Destroy(m_go);
		}
		Mesh mesh = new Mesh();
		Vector3[] array = new Vector3[4];
		float num = _height / _width;
		int[] indices = new int[6] { 0, 1, 2, 0, 2, 3 };
		Vector2[] array2 = new Vector2[4]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f)
		};
		Vector3 vector = base.transform.InverseTransformPoint(base.transform.position);
		float num2 = Mathf.Tan(m_skew * ((float)Math.PI / 180f)) * (_height - TFW.x - TFW.z);
		array[0] = new Vector3(vector.x - _width / 2f + num2 + TFW.x, vector.y - _height / 2f + TFW.w, 0f);
		float num3 = num2 / _width;
		array2[0] = new Vector2(num3, 0f);
		array2[1].x += num3;
		array2[2].x += num3;
		num2 = Mathf.Tan(m_skew * ((float)Math.PI / 180f)) * (_height - TFW.x - TFW.z);
		array[3] = new Vector3(vector.x + _width / 2f + num2 - TFW.z, vector.y - _height / 2f + TFW.w, 0f);
		array2[3] = new Vector2(num3 + 1f, 0f);
		array[1] = new Vector3(vector.x - _width / 2f + TFW.x, vector.y + _height / 2f - TFW.y, 0f);
		array[2] = new Vector3(vector.x + _width / 2f - TFW.z, vector.y + _height / 2f - TFW.y, 0f);
		array2[0] = RotatePoint(new Vector2(0.5f, 0.5f), array2[0], m_swipeAngle);
		array2[1] = RotatePoint(new Vector2(0.5f, 0.5f), array2[1], m_swipeAngle);
		array2[2] = RotatePoint(new Vector2(0.5f, 0.5f), array2[2], m_swipeAngle);
		array2[3] = RotatePoint(new Vector2(0.5f, 0.5f), array2[3], m_swipeAngle);
		array2[0] = new Vector2(array2[0].x, array2[0].y * num);
		array2[1] = new Vector2(array2[1].x, array2[1].y * num);
		array2[2] = new Vector2(array2[2].x, array2[2].y * num);
		array2[3] = new Vector2(array2[3].x, array2[3].y * num);
		mesh.vertices = array;
		mesh.SetIndices(indices, MeshTopology.Triangles, 0);
		mesh.uv = array2;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		m_go = new GameObject();
		m_go.layer = base.gameObject.layer;
		m_go.transform.position = base.transform.position;
		m_go.name = "DynamicSwipe";
		MeshFilter meshFilter = m_go.AddComponent<MeshFilter>();
		meshFilter.sharedMesh = mesh;
		MeshRenderer meshRenderer = m_go.AddComponent<MeshRenderer>();
		meshRenderer.material = m_material;
		a = m_go.AddComponent<AnimateAchievementHighlight>();
		a.AnimationSpeed = m_AnimationSpeed;
		a.StartOffset = m_StartOffset;
		a.TargetOffset = m_TargetOffset;
		m_go.transform.parent = Container.transform;
		m_go.transform.localPosition = new Vector3(_width / 2f - num2 / 2f, _height / 2f * -1f, -1f);
		m_go.transform.localScale = Vector3.one;
	}

	private void GenerateMesh(bool _right, float _width, float _height)
	{
		if (m_go != null)
		{
			UnityEngine.Object.Destroy(m_go);
		}
		Mesh mesh = new Mesh();
		Vector3[] array = new Vector3[4];
		float num = _height / _width;
		int[] indices = new int[6] { 0, 1, 2, 0, 2, 3 };
		Vector2[] array2 = new Vector2[4]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f)
		};
		Vector3 vector = base.transform.InverseTransformPoint(base.transform.position);
		if (_right)
		{
			float num2 = Mathf.Tan(m_skew * ((float)Math.PI / 180f)) * (_height - TFW.x - TFW.z);
			array[0] = new Vector3(vector.x - _width / 2f + num2 + TFW.x, vector.y - _height / 2f + TFW.w, 0f);
			float x = num2 / _width;
			array2[0] = new Vector2(x, 0f);
		}
		else
		{
			array[0] = new Vector3(vector.x - _width / 2f + TFW.x, vector.y - _height / 2f + TFW.w, 0f);
		}
		array[1] = new Vector3(vector.x - _width / 2f + TFW.x, vector.y + _height / 2f - TFW.y, 0f);
		array[2] = new Vector3(vector.x + _width / 2f - TFW.z, vector.y + _height / 2f - TFW.y, 0f);
		if (!_right)
		{
			float num2 = Mathf.Tan(m_skew * ((float)Math.PI / 180f)) * (_height - TFW.x - TFW.z);
			array[3] = new Vector3(vector.x + _width / 2f - num2 - TFW.z, vector.y - _height / 2f + TFW.w, 0f);
			float num3 = num2 / _width;
			array2[3] = new Vector2(1f - num3, 0f);
		}
		else
		{
			array[3] = new Vector3(vector.x + _width / 2f - TFW.z, vector.y - _height / 2f + TFW.w, 0f);
		}
		array2[0] = RotatePoint(new Vector2(0.5f, 0.5f), array2[0], m_swipeAngle);
		array2[1] = RotatePoint(new Vector2(0.5f, 0.5f), array2[1], m_swipeAngle);
		array2[2] = RotatePoint(new Vector2(0.5f, 0.5f), array2[2], m_swipeAngle);
		array2[3] = RotatePoint(new Vector2(0.5f, 0.5f), array2[3], m_swipeAngle);
		array2[0] = new Vector2(array2[0].x, array2[0].y * num);
		array2[1] = new Vector2(array2[1].x, array2[1].y * num);
		array2[2] = new Vector2(array2[2].x, array2[2].y * num);
		array2[3] = new Vector2(array2[3].x, array2[3].y * num);
		mesh.vertices = array;
		mesh.SetIndices(indices, MeshTopology.Triangles, 0);
		mesh.uv = array2;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		m_go = new GameObject();
		m_go.layer = base.gameObject.layer;
		m_go.transform.position = base.transform.position;
		m_go.name = "DynamicSwipe";
		MeshFilter meshFilter = m_go.AddComponent<MeshFilter>();
		meshFilter.sharedMesh = mesh;
		MeshRenderer meshRenderer = m_go.AddComponent<MeshRenderer>();
		meshRenderer.material = m_material;
		a = m_go.AddComponent<AnimateAchievementHighlight>();
		a.AnimationSpeed = m_AnimationSpeed;
		a.StartOffset = m_StartOffset;
		a.TargetOffset = m_TargetOffset;
		m_go.transform.parent = Container.transform;
		if (!_right)
		{
			m_go.transform.localPosition = new Vector3(_width / 2f, _height / 2f * -1f, -1f);
		}
		else
		{
			m_go.transform.localPosition = new Vector3(_width / 2f * -1f, _height / 2f * -1f, -1f);
		}
		m_go.transform.localScale = Vector3.one;
	}

	public Vector2 RotatePoint(Vector2 center, Vector2 point, float rotation)
	{
		Vector2 vector = default(Vector2);
		float num = Mathf.Cos(rotation * ((float)Math.PI / 180f));
		float num2 = Mathf.Sin(rotation * ((float)Math.PI / 180f));
		Vector2 vector2 = new Vector2(point.x * num + point.y * num2, (0f - point.x) * num2 + point.y * num);
		return center + vector2;
	}
}
