using System;
using Assets.Scripts.Engine;
using UbiCore.Debug;
using UbiCore.Utils;
using UnityEngine;

public class EffectEagleVision : EffectBase
{
	public float EaseInTime = 1f;

	public float EaseOutTime = 1f;

	private BloomSettingsInterface m_OldBloomSettings;

	private BloomSettingsInterface m_EagleVisionBloomSettings;

	public float DefaultDesaturation = 0.25f;

	public float EffectDesaturation = 1f;

	private bool WasActive;

	private bool IsActive = true;

	private AnimationCurve m_EaseInCurve;

	private AnimationCurve m_EaseOutCurve;

	private float m_FadeStart;

	private DesaturationSettings m_DesaturationSettings;

	private BloomSettingsInterface m_BloomSettings;

	private CombinedPostEffect m_Effect;

	private GameObject m_CopyCamGo;

	private Camera m_CopyCam;

	public EffectEagleVision()
	{
		m_EagleVisionBloomSettings = new CustomBloomSettings();
		m_EagleVisionBloomSettings.Iterations = 1;
		m_EagleVisionBloomSettings.Size = 2f;
		m_EagleVisionBloomSettings.Blur = BloomSettings.BlurType.Sgx;
		m_EagleVisionBloomSettings.Downsample = BloomSettings.DownsampleType.None;
		m_EagleVisionBloomSettings.Exponent = Vector4.one;
		m_EagleVisionBloomSettings.Intensity = new Vector4(0.301489f, 0.301489f, 0.301489f, 0.301489f);
		m_EagleVisionBloomSettings.Ordering = BloomSettings.Order.Last;
		m_EagleVisionBloomSettings.BloomResolution = BloomSettings.Resolution.Medium;
		m_EagleVisionBloomSettings.Threshold = new Vector4(0.5024816f, 0.5024816f, 0.5024816f, 0.5024816f);
		UpdateCurves();
	}

	private void UpdateCurves()
	{
		m_EaseInCurve = AnimationCurve.EaseInOut(0f, 0f, EaseInTime, 1f);
		m_EaseOutCurve = AnimationCurve.EaseInOut(0f, 1f, EaseOutTime, 0f);
	}

	private static void CopyBoomSettings(out BloomSettingsInterface _destination, BloomSettingsInterface _source)
	{
		_destination = new CustomBloomSettings();
		_destination.Iterations = _source.Iterations;
		_destination.Size = _source.Size;
		_destination.Blur = _source.Blur;
		_destination.Downsample = _source.Downsample;
		_destination.Exponent = _source.Exponent;
		_destination.Intensity = _source.Intensity;
		_destination.Ordering = _source.Ordering;
		_destination.BloomResolution = _source.BloomResolution;
		_destination.Threshold = _source.Threshold;
	}

	private void LerpBloom(float _value, ref BloomSettingsInterface _settings)
	{
		if (_value < 0.5f)
		{
			_settings.Iterations = m_OldBloomSettings.Iterations;
			_settings.Blur = m_OldBloomSettings.Blur;
			_settings.Downsample = m_OldBloomSettings.Downsample;
			_settings.Ordering = m_OldBloomSettings.Ordering;
			_settings.BloomResolution = m_OldBloomSettings.BloomResolution;
		}
		else
		{
			_settings.Iterations = m_EagleVisionBloomSettings.Iterations;
			_settings.Blur = m_EagleVisionBloomSettings.Blur;
			_settings.Downsample = m_EagleVisionBloomSettings.Downsample;
			_settings.Ordering = m_EagleVisionBloomSettings.Ordering;
			_settings.BloomResolution = m_EagleVisionBloomSettings.BloomResolution;
		}
		_settings.Size = Mathf.Lerp(m_OldBloomSettings.Size, m_EagleVisionBloomSettings.Size, _value);
		_settings.Intensity = Vector4.Lerp(m_OldBloomSettings.Intensity, m_EagleVisionBloomSettings.Intensity, _value);
		_settings.Exponent = Vector4.Lerp(m_OldBloomSettings.Exponent, m_EagleVisionBloomSettings.Exponent, _value);
		_settings.Threshold = Vector4.Lerp(m_OldBloomSettings.Threshold, m_EagleVisionBloomSettings.Threshold, _value);
	}

	private void Start()
	{
		m_DesaturationSettings = UnityEngine.Object.FindObjectOfType(typeof(DesaturationSettings)) as DesaturationSettings;
		m_BloomSettings = UnityEngine.Object.FindObjectOfType(typeof(BloomSettings)) as BloomSettings;
		Camera main = Camera.main;
		m_Effect = ((!(main != null)) ? null : main.GetComponent<CombinedPostEffect>());
		CopyBoomSettings(out m_OldBloomSettings, m_BloomSettings);
		m_EagleVisionBloomSettings.BloomResolution = m_BloomSettings.BloomResolution;
		Helpers.Verify(m_DesaturationSettings != null);
		Helpers.Verify(m_BloomSettings != null);
		Helpers.Verify(m_Effect != null);
		Helpers.Verify(Camera.main != null);
		m_CopyCamGo = new GameObject("EagleVisionCam");
		Transform transform = m_CopyCamGo.transform;
		transform.parent = Camera.main.gameObject.transform;
		transform.localPosition = Vector4.zero;
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector4.one;
		m_CopyCam = m_CopyCamGo.AddComponent<Camera>();
		m_CopyCam.CopyFrom(Camera.main);
		m_CopyCam.clearFlags = CameraClearFlags.Nothing;
		m_CopyCam.enabled = false;
		ConnectDelegate();
		IsActive = true;
		WasActive = false;
		DefaultDesaturation = m_DesaturationSettings.desaturation;
		UpdateCurves();
	}

	public override void OnDestroy()
	{
		SetPostEffectParams(DefaultDesaturation, 0f);
		if (Camera.main != null)
		{
			Vignette component = Camera.main.GetComponent<Vignette>();
			component.SetVignetteColor(Vignette.Type.EagleVision, Color.black, 0f);
		}
		Shader.SetGlobalFloat("_EagleVisionAmount", 0f);
		UnityEngine.Object.Destroy(m_CopyCamGo);
		DisconnectDelegate();
		base.OnDestroy();
	}

	public override void On()
	{
		if (!IsActive)
		{
			Singleton<EffectManager>.Instance.RegisterEffect(this);
			IsActive = true;
			WasActive = false;
			m_FadeStart = UbiCore.Utils.Singleton<Timer>.Instance.realTime;
			DefaultDesaturation = m_DesaturationSettings.desaturation;
		}
	}

	public override void Off()
	{
		if (IsActive || WasActive)
		{
			IsActive = false;
			WasActive = true;
			m_FadeStart = UbiCore.Utils.Singleton<Timer>.Instance.realTime;
		}
	}

	public override void Trigger()
	{
		Singleton<EffectManager>.Instance.RegisterEffect(this);
	}

	private void ConnectDelegate()
	{
		FxBasicBloom.DrawIntoBloomCallback = (FxBasicBloom.TDrawIntoBloomCallback)Delegate.Combine(FxBasicBloom.DrawIntoBloomCallback, new FxBasicBloom.TDrawIntoBloomCallback(DrawIntoBloomCallback2));
	}

	private void DisconnectDelegate()
	{
		FxBasicBloom.DrawIntoBloomCallback = (FxBasicBloom.TDrawIntoBloomCallback)Delegate.Remove(FxBasicBloom.DrawIntoBloomCallback, new FxBasicBloom.TDrawIntoBloomCallback(DrawIntoBloomCallback2));
	}

	private void SetPostEffectParams(float _desaturation, float _amount)
	{
		if (m_DesaturationSettings != null)
		{
			m_DesaturationSettings.desaturation = _desaturation;
		}
		if (m_BloomSettings != null)
		{
			LerpBloom(_amount, ref m_BloomSettings);
		}
		if ((bool)m_Effect)
		{
			m_Effect.Refresh(true);
		}
	}

	private void DrawIntoBloomCallback2(RenderTexture _bloomTexture)
	{
		float value = DoEaseInOut();
		Shader.SetGlobalFloat("_EagleVisionAmount", value);
	}

	private static void ExitRenderingToBloom(ref RenderBuffer rt, ref RenderBuffer rz)
	{
		Graphics.SetRenderTarget(rt, rz);
	}

	private static void EnterRenderingToBloom(RenderTexture _bloomTexture, out RenderBuffer rt, out RenderBuffer rz)
	{
		rt = Graphics.activeColorBuffer;
		rz = Graphics.activeDepthBuffer;
		Graphics.SetRenderTarget(_bloomTexture);
	}

	private float DoEaseInOut()
	{
		float result = 0f;
		if (!IsActive)
		{
			if (WasActive)
			{
				result = DoEaseOut();
			}
		}
		else
		{
			result = 1f;
			if (!WasActive)
			{
				result = DoEaseIn();
			}
		}
		return result;
	}

	private float DoEaseIn()
	{
		float num = m_EaseInCurve.Evaluate(UbiCore.Utils.Singleton<Timer>.Instance.realTime - m_FadeStart);
		if (num > 0.99f)
		{
			WasActive = true;
		}
		float desaturation = DefaultDesaturation * (1f - num) + EffectDesaturation * num;
		Profiling.Start("EffectEagleVision.SetPostEffectParams", float.MaxValue);
		SetPostEffectParams(desaturation, num);
		Profiling.Stop();
		Vignette component = Camera.main.GetComponent<Vignette>();
		component.SetVignetteColor(Vignette.Type.EagleVision, Color.black, num);
		return num;
	}

	private float DoEaseOut()
	{
		float num = m_EaseOutCurve.Evaluate(UbiCore.Utils.Singleton<Timer>.Instance.realTime - m_FadeStart);
		float desaturation = DefaultDesaturation * (1f - num) + EffectDesaturation * num;
		if (Mathf.Approximately(num, 0f))
		{
			WasActive = false;
		}
		SetPostEffectParams(desaturation, num);
		Vignette component = Camera.main.GetComponent<Vignette>();
		component.SetVignetteColor(Vignette.Type.EagleVision, Color.black, num);
		return num;
	}
}
