using UnityEngine;

public class FxColorCorrection : BasePostFx
{
	private Texture2D m_lutTexture;

	private float m_Intensity = 1f;

	private ColorCorrectionSettings m_Settings;

	private int m_LutTexId;

	private int m_LutTranslateScaleId;

	private int m_ColorCorrectionIntensityId;

	public Texture2D lutTexture
	{
		get
		{
			return m_lutTexture;
		}
	}

	public float Intensity
	{
		get
		{
			return m_Intensity;
		}
		set
		{
			m_Intensity = Mathf.Clamp01(value);
		}
	}

	public FxColorCorrection()
	{
		m_LutTexId = Shader.PropertyToID("_LutTex");
		m_LutTranslateScaleId = Shader.PropertyToID("_LutTranslateScale");
		m_ColorCorrectionIntensityId = Shader.PropertyToID("_ColorCorrectionIntensity");
	}

	public override void Reset(bool _skipExpensive)
	{
		if (m_Settings == null || !_skipExpensive)
		{
			m_Settings = Object.FindObjectOfType(typeof(ColorCorrectionSettings)) as ColorCorrectionSettings;
		}
		IsActive = m_Settings != null;
		bool flag = m_Settings == null;
		if (!_skipExpensive && IsActive)
		{
			RefreshTexture(m_Settings);
		}
	}

	public override void ExecuteOrSkip(Material _ComboPass)
	{
		if (IsActive && m_Settings != null)
		{
			if (m_lutTexture != null)
			{
				float num = m_lutTexture.width;
				float num2 = num - 1f;
				_ComboPass.SetTexture(m_LutTexId, m_lutTexture);
				_ComboPass.SetVector(m_LutTranslateScaleId, new Vector4(0.5f / num, 1f / num * num2));
			}
			if (m_Intensity < 0.999f)
			{
				_ComboPass.EnableKeyword("COLORCORRECT_FADING_ON");
				_ComboPass.DisableKeyword("COLORCORRECT_ON");
				_ComboPass.SetFloat(m_ColorCorrectionIntensityId, m_Intensity);
			}
			else
			{
				_ComboPass.EnableKeyword("COLORCORRECT_ON");
				_ComboPass.DisableKeyword("COLORCORRECT_FADING_ON");
			}
		}
		else
		{
			_ComboPass.DisableKeyword("COLORCORRECT_ON");
			_ComboPass.DisableKeyword("COLORCORRECT_FADING_ON");
		}
	}

	private void RefreshTexture(ColorCorrectionSettings settings)
	{
		if (m_lutTexture != null)
		{
			Object.DestroyImmediate(m_lutTexture);
		}
		int lutSize = (int)settings.lutSize;
		float num = (float)lutSize - 1f;
		Color[] array = new Color[lutSize * lutSize];
		float num2 = 1f / num;
		m_lutTexture = new Texture2D(lutSize, lutSize, TextureFormat.RGBA32, false);
		m_lutTexture.wrapMode = TextureWrapMode.Clamp;
		m_lutTexture.filterMode = FilterMode.Bilinear;
		for (int i = 0; i < lutSize; i++)
		{
			for (int j = 0; j < lutSize; j++)
			{
				float num3 = (float)i * num2;
				float num4 = (float)j * num2;
				float num5 = settings.red.Evaluate(num3);
				float num6 = settings.green.Evaluate(num4);
				float num7 = settings.blue.Evaluate(num4);
				float num8 = settings.global.Evaluate(num3);
				float num9 = settings.global.Evaluate(num4);
				num5 += num8 - num3;
				num6 += num9 - num4;
				num7 += num9 - num4;
				int num10 = i + j * lutSize;
				float r = num5;
				float g = num6;
				float num11 = num7;
				array[num10] = new Color(r, g, num11, num11);
			}
		}
		m_lutTexture.SetPixels(array);
		m_lutTexture.Apply();
	}
}
