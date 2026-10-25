using UnityEngine;

public class RenderingUpscaler : MonoBehaviour
{
	private RenderTexture m_DownscaledRT;

	private Camera m_MainCamera;

	private RenderingUpscaleCopy m_UpscaleCopy;

	private Camera m_LowResCamera;

	private Vector2 m_Scale = new Vector2(1f, 1f);

	public Vector2 Scale
	{
		get
		{
			return m_Scale;
		}
	}

	public void SetupLowResRenderTarget(int _fullResWidth, int _fullResHeight, float _downscaleFactor, int _depthBits, RenderTextureFormat _format)
	{
		int num = Mathf.NextPowerOfTwo((int)((float)_fullResWidth * _downscaleFactor));
		int num2 = Mathf.NextPowerOfTwo((int)((float)_fullResHeight * _downscaleFactor));
		m_Scale.x = (float)num / (float)_fullResWidth;
		m_Scale.y = (float)num2 / (float)_fullResHeight;
		m_DownscaledRT = new RenderTexture(num, num2, _depthBits, _format);
	}

	public void Start()
	{
		SetupCamera();
	}

	private void SetupCamera()
	{
		m_MainCamera = GetComponent<Camera>();
		if (!(m_MainCamera == null))
		{
			m_MainCamera.targetTexture = null;
			m_MainCamera.ResetAspect();
			float aspect = m_MainCamera.aspect;
			GameObject gameObject = new GameObject();
			gameObject.name = "UpscalerCamera";
			gameObject.transform.parent = base.gameObject.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			m_LowResCamera = gameObject.AddComponent<Camera>();
			m_LowResCamera.cullingMask = m_MainCamera.cullingMask;
			m_LowResCamera.clearFlags = m_MainCamera.clearFlags;
			m_LowResCamera.backgroundColor = m_MainCamera.backgroundColor;
			m_LowResCamera.depth = m_MainCamera.depth;
			m_LowResCamera.targetTexture = m_DownscaledRT;
			m_LowResCamera.aspect = aspect;
			m_MainCamera.cullingMask = 0;
			m_MainCamera.clearFlags = CameraClearFlags.Color;
			m_MainCamera.backgroundColor = Color.black;
			m_MainCamera.depth = m_LowResCamera.depth + 0.01f;
			m_UpscaleCopy = m_MainCamera.gameObject.AddComponent<RenderingUpscaleCopy>();
			m_UpscaleCopy.DownscaledSourceTexture = m_DownscaledRT;
		}
	}
}
