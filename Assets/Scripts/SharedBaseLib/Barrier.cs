using UnityEngine;

public class Barrier : MonoBehaviour
{
	[SerializeField]
	private BarrierSetup m_setup;

	[SerializeField]
	public Vector2 m_size = new Vector2(8f, 4f);

	public Vector2 MaxSize;

	public Transform CachedTransform;

	public Renderer CachedRenderer;

	private const int BARRIER_VERTEX_COUNT = 8;

	private const int BARRIER_INDICES_COUNT = 18;

	private const int BEAM_VERTEX_COUNT = 8;

	private const int BEAM_INDICES_COUNT = 18;

	private void DrawRectangle(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3)
	{
		Gizmos.DrawLine(v0, v1);
		Gizmos.DrawLine(v1, v2);
		Gizmos.DrawLine(v2, v3);
		Gizmos.DrawLine(v3, v0);
	}

	private void OnDrawGizmosSelected()
	{
		if (!(m_setup == null))
		{
			Matrix4x4 matrix = Gizmos.matrix;
			Vector2 trapez = m_setup.m_trapez;
			Vector2 padding = m_setup.m_padding;
			Vector2 vector = CalculateSize();
			Vector3 size = m_setup.m_dimensions + new Vector3(vector.x, vector.y, 0f);
			Gizmos.matrix = Matrix4x4.TRS(base.transform.position, base.transform.rotation, Vector3.one);
			Gizmos.color = Color.red;
			DrawRectangle(new Vector3(vector.x * 0.5f, 0f, 0f), new Vector3((0f - vector.x) * 0.5f, 0f, 0f), new Vector3((0f - vector.x) * 0.5f + trapez.x, vector.y, 0f), new Vector3(vector.x * 0.5f - trapez.y, vector.y, 0f));
			DrawRectangle(new Vector3(vector.x * 0.5f - padding.x, 0f, 0f), new Vector3((0f - vector.x) * 0.5f + padding.y, 0f, 0f), new Vector3((0f - vector.x) * 0.5f + trapez.x + padding.x, vector.y, 0f), new Vector3(vector.x * 0.5f - trapez.y - padding.y, vector.y, 0f));
			Gizmos.color = Color.white;
			Gizmos.DrawWireCube(new Vector3(0f, size.y * 0.5f, size.z * 0.5f), size);
			Gizmos.matrix = matrix;
		}
	}

	public void Build()
	{
		Renderer component = GetComponent<Renderer>();
		if (component.sharedMaterials.Length > 1)
		{
			BuildLevelBorder();
			MaxSize = CalculateSize();
			CachedTransform = base.transform;
			CachedRenderer = component;
		}
	}

	public Vector2 CalculateSize()
	{
		Vector2 size = m_size;
		Vector2 trapez = m_setup.m_trapez;
		Vector2 padding = m_setup.m_padding;
		size.x = Mathf.Max(m_size.x, padding.x + padding.y + trapez.x + trapez.y);
		return size;
	}

	public int CalculateSizeInBytes()
	{
		Vector2 size = m_size;
		Vector3 vector = m_setup.m_dimensions + new Vector3(size.x, size.y, 0f);
		float x = vector.x;
		float y = vector.y;
		int num = Mathf.Max((int)(m_setup.m_beamDensity.x * x), 1);
		int num2 = Mathf.Max((int)(m_setup.m_beamDensity.y * y), 1);
		int num3 = num + num2;
		int num4 = 8 + num3 * 8;
		int num5 = 18 + num3 * 18;
		return num4 * 120 + num5 * 6;
	}

	private void BuildLevelBorder()
	{
		MeshFilter component = GetComponent<MeshFilter>();
		Vector2 trapez = m_setup.m_trapez;
		Vector2 padding = m_setup.m_padding;
		Vector2 size = m_size;
		size.x = Mathf.Max(m_size.x, padding.x + padding.y + trapez.x + trapez.y);
		Vector3 vector = m_setup.m_dimensions + new Vector3(size.x, size.y, 0f);
		float x = vector.x;
		float y = vector.y;
		float z = vector.z;
		int num = Mathf.Max((int)(m_setup.m_beamDensity.x * x), 1);
		int num2 = Mathf.Max((int)(m_setup.m_beamDensity.y * y), 1);
		int num3 = num + num2;
		int num4 = 8 + num3 * 8;
		Vector3[] array = new Vector3[num4];
		Vector3 vector2 = new Vector3((0f - x) * 0.5f, y, 0f);
		Vector3 vector3 = new Vector3((0f - size.x) * 0.5f + trapez.x, size.y, 0f);
		Vector3 vector4 = new Vector3(size.x * 0.5f - trapez.y, size.y, 0f);
		Vector3 vector5 = new Vector3((0f - size.x) * 0.5f, 0f, 0f);
		Vector3 vector6 = new Vector3(size.x * 0.5f, 0f, 0f);
		array[0] = (array[1] = vector3);
		array[2] = (array[3] = vector4);
		array[4] = (array[5] = vector5);
		array[6] = (array[7] = vector6);
		array[1].x += padding.x;
		array[5].x += padding.x;
		array[2].x -= padding.y;
		array[6].x -= padding.y;
		Vector2[] array2 = new Vector2[num4];
		Vector2[] array3 = new Vector2[num4];
		for (int i = 0; i < 8; i++)
		{
			array2[i].x = 0.5f;
			array3[i].x = x;
		}
		array2[4].y = (array3[4].y = (array2[5].y = (array3[5].y = (array2[6].y = (array3[6].y = (array2[7].y = (array3[7].y = 0f)))))));
		array2[0].y = (array3[0].y = (array2[1].y = (array3[1].y = (array2[2].y = (array3[2].y = (array2[3].y = (array3[3].y = 1f)))))));
		Color32[] array4 = new Color32[num4];
		for (int j = 0; j < 8; j++)
		{
			array4[j].r = 0;
			array4[j].g = byte.MaxValue;
		}
		array4[0].b = (array4[3].b = (array4[4].b = (array4[7].b = 0)));
		array4[1].b = (array4[2].b = (array4[5].b = (array4[6].b = byte.MaxValue)));
		Vector3[] array5 = new Vector3[num4];
		Vector4[] array6 = new Vector4[num4];
		for (int k = 0; k < 8; k++)
		{
			array5[k] = Vector3.forward;
			array6[k] = new Vector4(1f, 0f, 0f, x * 0.5f);
		}
		Vector3 vector7 = vector2;
		vector7.x += x * 0.5f;
		vector7.y -= y * 0.5f;
		int num5 = 0;
		for (int l = 8; l < num4; l += 8)
		{
			Vector3 vector8 = vector7;
			bool flag = num5++ < num;
			if (flag)
			{
				vector8.x += Random.value * x - x * 0.5f;
			}
			else
			{
				vector8.y += Random.value * y - y * 0.5f;
			}
			vector8.z += Mathf.Pow(Random.value, m_setup.m_depthExponent) * z;
			for (int m = 0; m < 8; m++)
			{
				array[l + m] = vector8;
			}
			if (flag)
			{
				array2[l] = (array2[l + 4] = new Vector2(0f, 0f));
				array2[l + 1] = (array2[l + 5] = new Vector2(1f, 0f));
				array2[l + 2] = (array2[l + 6] = new Vector2(0f, 1f));
				array2[l + 3] = (array2[l + 7] = new Vector2(1f, 1f));
				array3[l] = (array3[l + 4] = new Vector2(1f, 0f));
				array3[l + 1] = (array3[l + 5] = new Vector2(1f, 1f));
				array3[l + 2] = (array3[l + 6] = new Vector2(0f, 0f));
				array3[l + 3] = (array3[l + 7] = new Vector2(0f, 1f));
			}
			else
			{
				array2[l] = (array2[l + 4] = new Vector2(0f, 0f));
				array2[l + 1] = (array2[l + 6] = new Vector2(1f, 0f));
				array2[l + 2] = (array2[l + 5] = new Vector2(0f, 1f));
				array2[l + 3] = (array2[l + 7] = new Vector2(1f, 1f));
				array3[l] = (array3[l + 4] = new Vector2(1f, 0f));
				array3[l + 1] = (array3[l + 6] = new Vector2(1f, 1f));
				array3[l + 2] = (array3[l + 5] = new Vector2(0f, 0f));
				array3[l + 3] = (array3[l + 7] = new Vector2(0f, 1f));
			}
			byte g = (byte)(flag ? 255u : 0u);
			for (int n = 0; n < 8; n++)
			{
				array4[l + n].g = g;
			}
			array4[l].a = (array4[l + 1].a = (array4[l + 2].a = (array4[l + 3].a = 127)));
			array4[l + 4].a = (array4[l + 5].a = 0);
			array4[l + 6].a = (array4[l + 7].a = byte.MaxValue);
			Vector3 vector9 = new Vector3(Random.value * 2f - 1f, Random.value * 2f - 1f, Random.value * 2f - 1f);
			for (int num6 = 0; num6 < 8; num6++)
			{
				array5[l + num6] = vector9;
				array6[l + num6] = new Vector4(1f, 0f, 0f, flag ? (y * 0.5f) : (x * 0.5f));
			}
		}
		Mesh mesh = new Mesh();
		mesh.vertices = array;
		mesh.uv = array2;
		mesh.uv2 = array3;
		mesh.normals = array5;
		mesh.tangents = array6;
		mesh.colors32 = array4;
		int num7 = 18;
		int[] array7 = new int[num7];
		int num8 = 0;
		array7[num8++] = 0;
		array7[num8++] = 4;
		array7[num8++] = 1;
		array7[num8++] = 1;
		array7[num8++] = 4;
		array7[num8++] = 5;
		array7[num8++] = 1;
		array7[num8++] = 5;
		array7[num8++] = 2;
		array7[num8++] = 2;
		array7[num8++] = 5;
		array7[num8++] = 6;
		array7[num8++] = 2;
		array7[num8++] = 6;
		array7[num8++] = 3;
		array7[num8++] = 3;
		array7[num8++] = 6;
		array7[num8++] = 7;
		mesh.subMeshCount = 2;
		mesh.SetTriangles(array7, 0);
		num8 = 0;
		num5 = 0;
		num7 = num3 * 18;
		array7 = new int[num7];
		int num9 = 8;
		while (num8 < num7)
		{
			bool num10 = num5++ < num;
			array7[num8] = num9;
			array7[num8 + 1] = num9 + 2;
			array7[num8 + 2] = num9 + 1;
			array7[num8 + 3] = num9 + 1;
			array7[num8 + 4] = num9 + 2;
			array7[num8 + 5] = num9 + 3;
			if (num10)
			{
				array7[num8 + 6] = num9 + 4;
				array7[num8 + 7] = num9;
				array7[num8 + 8] = num9 + 5;
				array7[num8 + 9] = num9 + 5;
				array7[num8 + 10] = num9;
				array7[num8 + 11] = num9 + 1;
				array7[num8 + 12] = num9 + 2;
				array7[num8 + 13] = num9 + 6;
				array7[num8 + 14] = num9 + 3;
				array7[num8 + 15] = num9 + 3;
				array7[num8 + 16] = num9 + 6;
				array7[num8 + 17] = num9 + 7;
			}
			else
			{
				array7[num8 + 6] = num9 + 4;
				array7[num8 + 7] = num9 + 5;
				array7[num8 + 8] = num9;
				array7[num8 + 9] = num9;
				array7[num8 + 10] = num9 + 5;
				array7[num8 + 11] = num9 + 2;
				array7[num8 + 12] = num9 + 1;
				array7[num8 + 13] = num9 + 3;
				array7[num8 + 14] = num9 + 6;
				array7[num8 + 15] = num9 + 6;
				array7[num8 + 16] = num9 + 3;
				array7[num8 + 17] = num9 + 7;
			}
			num8 += 18;
			num9 += 8;
		}
		mesh.SetTriangles(array7, 1);
		component.sharedMesh = mesh;
		base.transform.localScale = Vector3.one;
		GetComponent<Renderer>().enabled = true;
	}
}
