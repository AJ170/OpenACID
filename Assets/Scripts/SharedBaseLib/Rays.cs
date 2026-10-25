using UnityEngine;

public class Rays : MonoBehaviour
{
	[SerializeField]
	public Vector3 m_size = new Vector3(8f, 8f, 8f);

	[SerializeField]
	public Vector3 m_beamDensity = new Vector3(1f, 1f, 1f);

	[SerializeField]
	public float m_duration = 1f;

	[SerializeField]
	public Color m_color = Color.white * 0.5f;

	[SerializeField]
	public bool m_reverse;

	[SerializeField]
	public bool m_affectedBySlowMotion;

	[SerializeField]
	public float m_colorExp = 1f;

	[SerializeField]
	private float m_lifetime;

	private Renderer m_renderer;

	private static int s_idTintColor = -1;

	private static int s_idCustomTime = -1;

	public Color EndColor { get; set; }

	public Color CurrentColor { get; private set; }

	public float PersistentLifeTime { get; set; }

	private void DrawRectangle(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3)
	{
		Gizmos.DrawLine(v0, v1);
		Gizmos.DrawLine(v1, v2);
		Gizmos.DrawLine(v2, v3);
		Gizmos.DrawLine(v3, v0);
	}

	private void OnDrawGizmosSelected()
	{
		Matrix4x4 matrix = Gizmos.matrix;
		Gizmos.matrix = Matrix4x4.TRS(base.transform.position, base.transform.rotation, Vector3.one);
		Gizmos.color = Color.white;
		Gizmos.DrawWireCube(Vector3.zero, m_size);
		Gizmos.matrix = matrix;
	}

	private void Awake()
	{
		s_idTintColor = Shader.PropertyToID("_TintColor");
		s_idCustomTime = Shader.PropertyToID("_CustomTime");
		EndColor = Color.black;
		m_renderer = GetComponent<Renderer>();
	}

	public void Start()
	{
		if (m_renderer.sharedMaterials.Length != 0)
		{
			BuildRays();
		}
		m_lifetime = (m_reverse ? 0f : m_duration);
		float t = Mathf.Pow(Mathf.Clamp01(m_lifetime / m_duration), m_colorExp);
		CurrentColor = Color.Lerp(EndColor, m_color, t);
		m_renderer.material.SetColor(s_idTintColor, CurrentColor);
		m_renderer.enabled = true;
	}

	private void OnDestroy()
	{
		if (m_renderer != null)
		{
			Object.DestroyImmediate(m_renderer.material);
		}
	}

	private void Update()
	{
		if (m_lifetime < 0f)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		float num = Time.deltaTime / ((Time.timeScale > 0f) ? Time.timeScale : 1f);
		m_lifetime += (float)(m_reverse ? 1 : (-1)) * num;
		PersistentLifeTime += (m_affectedBySlowMotion ? Time.deltaTime : num);
		float t = Mathf.Pow(Mathf.Clamp01(m_lifetime / m_duration), m_colorExp);
		CurrentColor = Color.Lerp(EndColor, m_color, t);
		m_renderer.material.SetColor(s_idTintColor, CurrentColor);
		m_renderer.material.SetVector(s_idCustomTime, new Vector4(PersistentLifeTime / 20f, PersistentLifeTime, PersistentLifeTime * 2f, PersistentLifeTime * 3f));
	}

	private void BuildRays()
	{
		MeshFilter component = GetComponent<MeshFilter>();
		if (component.sharedMesh != null)
		{
			return;
		}
		Vector3 size = m_size;
		float x = size.x;
		float y = size.y;
		float z = size.z;
		int num = Mathf.Max((int)(m_beamDensity.x * x), 1);
		int num2 = Mathf.Max((int)(m_beamDensity.y * y), 1);
		int num3 = Mathf.Max((int)(m_beamDensity.z * y), 1);
		int num4 = num + num2 + num3;
		int num5 = num4 * 8;
		Vector3[] array = new Vector3[num5];
		Vector2[] array2 = new Vector2[num5];
		Vector2[] array3 = new Vector2[num5];
		Color32[] array4 = new Color32[num5];
		Vector3[] array5 = new Vector3[num5];
		Vector4[] array6 = new Vector4[num5];
		Vector3 zero = Vector3.zero;
		int num6 = 0;
		for (int i = 0; i < num5; i += 8)
		{
			Vector3 vector = zero;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			if (num6 < num)
			{
				flag = true;
			}
			else if (num6 < num + num2)
			{
				flag2 = true;
			}
			else
			{
				flag3 = true;
			}
			num6++;
			if (flag)
			{
				vector.x += Random.value * x - x * 0.5f;
				vector.z += Random.value * z - z * 0.5f;
			}
			else if (flag2)
			{
				vector.y += Random.value * y - y * 0.5f;
				vector.z += Random.value * z - z * 0.5f;
			}
			else
			{
				vector.x += Random.value * x - x * 0.5f;
				vector.y += Random.value * y - y * 0.5f;
			}
			for (int j = 0; j < 8; j++)
			{
				array[i + j] = vector;
			}
			if (flag)
			{
				array2[i] = (array2[i + 4] = new Vector2(0f, 0f));
				array2[i + 1] = (array2[i + 5] = new Vector2(1f, 0f));
				array2[i + 2] = (array2[i + 6] = new Vector2(0f, 1f));
				array2[i + 3] = (array2[i + 7] = new Vector2(1f, 1f));
				array3[i] = (array3[i + 4] = new Vector2(1f, 0f));
				array3[i + 1] = (array3[i + 5] = new Vector2(1f, 1f));
				array3[i + 2] = (array3[i + 6] = new Vector2(0f, 0f));
				array3[i + 3] = (array3[i + 7] = new Vector2(0f, 1f));
			}
			else
			{
				array2[i] = (array2[i + 4] = new Vector2(0f, 0f));
				array2[i + 1] = (array2[i + 6] = new Vector2(1f, 0f));
				array2[i + 2] = (array2[i + 5] = new Vector2(0f, 1f));
				array2[i + 3] = (array2[i + 7] = new Vector2(1f, 1f));
				array3[i] = (array3[i + 4] = new Vector2(1f, 0f));
				array3[i + 1] = (array3[i + 6] = new Vector2(1f, 1f));
				array3[i + 2] = (array3[i + 5] = new Vector2(0f, 0f));
				array3[i + 3] = (array3[i + 7] = new Vector2(0f, 1f));
			}
			byte g = (byte)(flag ? 255u : 0u);
			for (int k = 0; k < 8; k++)
			{
				array4[i + k].g = g;
			}
			array4[i].a = (array4[i + 1].a = (array4[i + 2].a = (array4[i + 3].a = 127)));
			array4[i + 4].a = (array4[i + 5].a = 0);
			array4[i + 6].a = (array4[i + 7].a = byte.MaxValue);
			Vector3 vector2 = new Vector3(Random.value * 2f - 1f, Random.value * 2f - 1f, Random.value * 2f - 1f);
			for (int l = 0; l < 8; l++)
			{
				array5[i + l] = vector2;
				if (flag3)
				{
					array6[i + l] = new Vector4(0f, 0f, 1f, z * 0.5f);
				}
				else
				{
					array6[i + l] = new Vector4(1f, 0f, 0f, flag ? (y * 0.5f) : (x * 0.5f));
				}
			}
		}
		Mesh mesh = new Mesh();
		mesh.vertices = array;
		mesh.uv = array2;
		mesh.uv2 = array3;
		mesh.normals = array5;
		mesh.tangents = array6;
		mesh.colors32 = array4;
		int num7 = 0;
		num6 = 0;
		int num8 = num4 * 18;
		int[] array7 = new int[num8];
		int num9 = 0;
		while (num7 < num8)
		{
			bool flag4 = num6 < num;
			num6++;
			array7[num7] = num9;
			array7[num7 + 1] = num9 + 2;
			array7[num7 + 2] = num9 + 1;
			array7[num7 + 3] = num9 + 1;
			array7[num7 + 4] = num9 + 2;
			array7[num7 + 5] = num9 + 3;
			if (flag4)
			{
				array7[num7 + 6] = num9 + 4;
				array7[num7 + 7] = num9;
				array7[num7 + 8] = num9 + 5;
				array7[num7 + 9] = num9 + 5;
				array7[num7 + 10] = num9;
				array7[num7 + 11] = num9 + 1;
				array7[num7 + 12] = num9 + 2;
				array7[num7 + 13] = num9 + 6;
				array7[num7 + 14] = num9 + 3;
				array7[num7 + 15] = num9 + 3;
				array7[num7 + 16] = num9 + 6;
				array7[num7 + 17] = num9 + 7;
			}
			else
			{
				array7[num7 + 6] = num9 + 4;
				array7[num7 + 7] = num9 + 5;
				array7[num7 + 8] = num9;
				array7[num7 + 9] = num9;
				array7[num7 + 10] = num9 + 5;
				array7[num7 + 11] = num9 + 2;
				array7[num7 + 12] = num9 + 1;
				array7[num7 + 13] = num9 + 3;
				array7[num7 + 14] = num9 + 6;
				array7[num7 + 15] = num9 + 6;
				array7[num7 + 16] = num9 + 3;
				array7[num7 + 17] = num9 + 7;
			}
			num7 += 18;
			num9 += 8;
		}
		mesh.SetTriangles(array7, 0);
		component.sharedMesh = mesh;
		base.transform.localScale = Vector3.one;
	}
}
