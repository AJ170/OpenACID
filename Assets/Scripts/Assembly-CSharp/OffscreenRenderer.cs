using UnityEngine;

internal class OffscreenRenderer : MonoBehaviour
{
	private const int kMultiBufferingCount = 1;

	public LayerMask m_CullingMask = -1;

	private int m_CurrTextureIndex;

	private Camera m_OffscreenCam;

	private RenderTexture[] m_OffscreenTexture = new RenderTexture[1];

	public Texture OffscreenTexture
	{
		get
		{
			return m_OffscreenTexture[m_CurrTextureIndex];
		}
	}

	public void SetupOffscreenTexture(int _width, int _height, int _depthBits, RenderTextureFormat _format)
	{
		for (int i = 0; i < 1; i++)
		{
			if (m_OffscreenTexture[i] != null)
			{
				m_OffscreenTexture[i].DiscardContents();
				m_OffscreenTexture[i].Release();
			}
			m_OffscreenTexture[i] = new RenderTexture(_width, _height, _depthBits, _format);
		}
	}

	private Camera RetrieveCamera()
	{
		Camera camera = null;
		camera = GetComponent<Camera>();
		if (camera == null)
		{
			camera = GetComponentInChildren<Camera>();
		}
		if (camera == null)
		{
			camera = base.gameObject.AddComponent<Camera>();
		}
		return camera;
	}

	private void ConfigureCamera(Camera _cam, RenderTexture _renderTexture)
	{
		_cam.cullingMask = m_CullingMask;
		_cam.targetTexture = m_OffscreenTexture[0];
		_cam.backgroundColor = new Color(1f, 1f, 1f, 0f);
		_cam.clearFlags = CameraClearFlags.Color;
	}

	public void Start()
	{
		m_OffscreenCam = RetrieveCamera();
		ConfigureCamera(m_OffscreenCam, m_OffscreenTexture[0]);
	}

	public void OnPostRender()
	{
		m_CurrTextureIndex = (m_CurrTextureIndex + 1) % 1;
		m_OffscreenCam.targetTexture = m_OffscreenTexture[m_CurrTextureIndex];
	}
}
