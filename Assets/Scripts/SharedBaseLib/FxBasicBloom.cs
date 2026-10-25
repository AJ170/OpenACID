using UnityEngine;

public class FxBasicBloom : BasePostFx
{
	public delegate void TDrawIntoBloomCallback(RenderTexture _bloomTexture);

	private BloomSettings m_Settings;

	private Material m_MatIteration;

	private Material m_MatIsolation;

	private RenderTexture m_BloomRt;

	private int m_ParameterId;

	private int m_BloomTexId;

	private int m_ThresholdId;

	private int m_IntensityId;

	private int m_ExponentId;

	public static TDrawIntoBloomCallback DrawIntoBloomCallback { get; set; }

	public FxBasicBloom()
	{
		m_ParameterId = Shader.PropertyToID("_Parameter");
		m_BloomTexId = Shader.PropertyToID("_BloomTex");
		m_ThresholdId = Shader.PropertyToID("_Threshold");
		m_IntensityId = Shader.PropertyToID("_Intensity");
		m_ExponentId = Shader.PropertyToID("_Exponent");
	}

	public override void Reset(bool _skipExpensive)
	{
		if (m_Settings == null || !_skipExpensive)
		{
			m_Settings = Object.FindObjectOfType(typeof(BloomSettings)) as BloomSettings;
		}
		if (m_MatIteration == null || !_skipExpensive)
		{
			m_MatIteration = new Material(Shader.Find("Hidden/LqBlurIteration"));
		}
		if (m_MatIsolation == null || !_skipExpensive)
		{
			m_MatIsolation = new Material(Shader.Find("Hidden/LqBlurIsolation"));
		}
		IsActive = m_Settings != null;
	}

	public override void Prepare(RenderTexture _src)
	{
		float num = ((m_Settings.resolution == BloomSettings.Resolution.Low) ? 0.25f : ((m_Settings.resolution == BloomSettings.Resolution.Medium) ? 0.5f : 1f));
		m_MatIteration.SetFloat(m_ParameterId, m_Settings.blurSize * num);
		m_MatIsolation.SetVector(m_ThresholdId, m_Settings.threshold);
		m_MatIsolation.SetVector(m_IntensityId, m_Settings.intensity);
		m_MatIsolation.SetVector(m_ExponentId, m_Settings.exponent);
		int num2 = ((m_Settings.resolution == BloomSettings.Resolution.Low) ? 8 : ((m_Settings.resolution == BloomSettings.Resolution.Medium) ? 4 : 2));
		int width = _src.width / num2;
		int height = _src.height / num2;
		m_BloomRt = RenderTexture.GetTemporary(width, height, 0, _src.format);
		m_BloomRt.filterMode = FilterMode.Bilinear;
		if (m_Settings.downsampleType == BloomSettings.DownsampleType._4Tap)
		{
			m_MatIsolation.EnableKeyword("DOWNSAMPLE_ON");
			m_MatIsolation.DisableKeyword("DOWNSAMPLE_OFF");
		}
		else
		{
			m_MatIsolation.EnableKeyword("DOWNSAMPLE_OFF");
			m_MatIsolation.DisableKeyword("DOWNSAMPLE_ON");
		}
		Graphics.Blit(_src, m_BloomRt, m_MatIsolation);
		if (DrawIntoBloomCallback != null)
		{
			DrawIntoBloomCallback(m_BloomRt);
		}
		// The PS Vita GPU is an SGX: always use the variant without dependent texture reads.
		if (m_Settings.blurType == BloomSettings.BlurType.Sgx || Application.platform == RuntimePlatform.PSP2)
		{
			m_MatIteration.EnableKeyword("SGX_ON");
		}
		else
		{
			m_MatIteration.DisableKeyword("SGX_ON");
		}
		BloomSettings.BlurType blurType = m_Settings.blurType;
		for (int i = 0; i < m_Settings.blurIterations; i++)
		{
			m_MatIteration.SetFloat(m_ParameterId, m_Settings.blurSize * num + (float)i * 1f);
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, _src.format);
			temporary.filterMode = FilterMode.Bilinear;
			Graphics.Blit(m_BloomRt, temporary, m_MatIteration, 0);
			RenderTexture.ReleaseTemporary(m_BloomRt);
			m_BloomRt = temporary;
			temporary = RenderTexture.GetTemporary(width, height, 0, _src.format);
			temporary.filterMode = FilterMode.Bilinear;
			Graphics.Blit(m_BloomRt, temporary, m_MatIteration, 1);
			RenderTexture.ReleaseTemporary(m_BloomRt);
			m_BloomRt = temporary;
		}
	}

	public override void ExecuteOrSkip(Material _ComboPass)
	{
		_ComboPass.DisableKeyword("BLOOM_FIRST");
		_ComboPass.DisableKeyword("BLOOM_LAST");
		if (IsActive && m_Settings != null)
		{
			if (m_Settings.order == BloomSettings.Order.First)
			{
				_ComboPass.EnableKeyword("BLOOM_FIRST");
			}
			else
			{
				_ComboPass.EnableKeyword("BLOOM_LAST");
			}
			_ComboPass.SetTexture(m_BloomTexId, m_BloomRt);
		}
	}

	public override void ReleaseTemporaries()
	{
		if (m_BloomRt != null)
		{
			RenderTexture.ReleaseTemporary(m_BloomRt);
		}
	}
}
