using System;
using System.Collections.Generic;
using Assets.Scripts.Audio;
using UnityEngine;

public class Outro : MonoBehaviour
{
	public bool ForceStart;

	public bool ForceEnd;

	public bool Reverse = true;

	public float TargetCameraFov;

	public Vector3 TargetPlayerPosition;

	public Vector3 TargetPlayerForward;

	public float ReachTargetRadius = 3f;

	public GameObject RaysPrefab;

	[SerializeField]
	private Color m_srcFogColor = Color.white;

	[SerializeField]
	private Color m_raysColor = Color.white;

	[SerializeField]
	private float m_duration = 4f;

	[SerializeField]
	private float m_rayExp = 6f;

	[SerializeField]
	private float m_sceneExp = 4f;

	[SerializeField]
	private float m_skyExp = 4f;

	[SerializeField]
	private float m_zoneWidth = 20f;

	[SerializeField]
	private float m_zoneStart = 10f;

	[SerializeField]
	private float m_fadingFogDistance = 500f;

	[SerializeField]
	private float m_fadingFogDamping = 5f;

	private Color m_destFogColor;

	private float m_destFogStartDistance;

	private float m_destFogEndDistance;

	private float m_destFogDensity;

	private Renderer m_skyRenderer;

	private Material m_skyMaterial;

	private Material m_skyMaterialClone;

	private GameObject m_rays;

	private float m_time;

	private bool m_fogFading;

	private bool m_started;

	private int m_fogId;

	private bool m_isCameraActive;

	private bool m_waitForReachTarget;

	private FxColorCorrection m_ColorCorrectionPostFx;

	private bool m_DisableColorCorrection;

	private Animation m_animation;

	private static Outro m_instance;

	public static Outro Instance
	{
		get
		{
			return m_instance;
		}
	}

	public bool IsCameraActive
	{
		get
		{
			return m_isCameraActive;
		}
	}

	public bool IsFinished
	{
		get
		{
			return !m_animation.isPlaying;
		}
	}

	public bool DisableColorCorrectionDuringFadeout
	{
		get
		{
			return m_DisableColorCorrection;
		}
		set
		{
			m_DisableColorCorrection = value;
		}
	}

	public void Awake()
	{
		m_instance = this;
	}

	public void StartOutro()
	{
		m_animation = base.gameObject.GetComponent<Animation>();
		CombinedPostEffect combinedPostEffect = UnityEngine.Object.FindObjectOfType<CombinedPostEffect>();
		if (combinedPostEffect != null)
		{
			m_ColorCorrectionPostFx = combinedPostEffect.FindFx<FxColorCorrection>();
		}
		List<string> list = new List<string>();
		foreach (AnimationState item in m_animation)
		{
			list.Add(item.name);
		}
		if (list.Count > 0)
		{
			m_animation.Play(list[0]);
		}
		for (int i = 1; i < list.Count; i++)
		{
			m_animation.PlayQueued(list[i]);
		}
		AnimationEventListener component = base.gameObject.GetComponent<AnimationEventListener>();
		component.OnEvent = (AnimationEventListener.OnEventDelegate)Delegate.Combine(component.OnEvent, new AnimationEventListener.OnEventDelegate(OnAnimationEvent));
		m_destFogColor = RenderSettings.fogColor;
		m_destFogStartDistance = RenderSettings.fogStartDistance;
		m_destFogEndDistance = RenderSettings.fogEndDistance;
		m_destFogDensity = RenderSettings.fogDensity;
		RenderingSkyMesh renderingSkyMesh = UnityEngine.Object.FindObjectOfType<RenderingSkyMesh>();
		if (renderingSkyMesh != null)
		{
			m_skyRenderer = renderingSkyMesh.GetComponent<Renderer>();
			m_skyMaterial = m_skyRenderer.sharedMaterial;
			m_skyMaterialClone = UnityEngine.Object.Instantiate(m_skyMaterial);
			m_skyMaterialClone.shaderKeywords = new string[1] { "FOG_ON" };
		}
		DirectionalFogColor directionalFogColor = UnityEngine.Object.FindObjectOfType<DirectionalFogColor>();
		if (directionalFogColor != null)
		{
			directionalFogColor.enabled = false;
		}
		AudioManager.Instance.EventManager.OnRegionOutroStarted();
		m_started = true;
	}

	private void OnAnimationEvent(string _desc)
	{
	}

	private void OnWaitForReachTarget()
	{
		m_waitForReachTarget = true;
		foreach (AnimationState item in m_animation)
		{
			item.speed = 0f;
		}
	}

	private void OnIntroMissionCathedralClimbAutomation()
	{
		AudioManager.Instance.EventManager.OnIntroMissionCathedralClimbAutomation();
	}

	private void OnIntroMissionReachCathedralTop()
	{
		AudioManager.Instance.EventManager.OnIntroMissionReachCathedralTop();
	}

	private void OnIntroMissionCathedralClimbWindStart()
	{
		AudioManager.Instance.EventManager.OnIntroMissionCathedralClimbWindStart();
	}

	private void OnStartCamera()
	{
		m_isCameraActive = true;
	}

	private void OnStartFogAnimation()
	{
		StartFogEffect();
	}

	private void OnStartFogFading()
	{
		m_fogFading = true;
	}

	private void StartFogEffect()
	{
		m_time = m_duration;
		if ((bool)m_skyRenderer)
		{
			m_skyRenderer.sharedMaterial = m_skyMaterialClone;
		}
		if (m_rays != null)
		{
			UnityEngine.Object.Destroy(m_rays);
		}
		m_rays = UnityEngine.Object.Instantiate(RaysPrefab, Camera.main.transform.position, Camera.main.transform.rotation) as GameObject;
		m_rays.transform.parent = base.transform;
		m_rays.name = "Rays";
		Rays component = m_rays.GetComponent<Rays>();
		component.m_color = m_raysColor;
		component.m_duration = m_duration;
		component.m_reverse = Reverse;
		component.m_colorExp = m_rayExp;
		component.m_affectedBySlowMotion = false;
		m_fogId = Shader.PropertyToID("_Fog");
		m_fogFading = false;
	}

	private void Update()
	{
		if (ForceStart)
		{
			StartOutro();
			ForceStart = false;
		}
		if (ForceEnd)
		{
			EndOutro();
			EndFogEffect();
			ForceEnd = false;
		}
		if (!m_started)
		{
			return;
		}
		if (m_waitForReachTarget && (LogicMain.PlayerEntity.Position - TargetPlayerPosition).magnitude < ReachTargetRadius)
		{
			foreach (AnimationState item in m_animation)
			{
				item.speed = 1f;
			}
			m_waitForReachTarget = false;
		}
		if (m_time > 0f)
		{
			float num = Mathf.Clamp01(m_time / m_duration);
			if (Reverse)
			{
				num = 1f - num;
			}
			float t = 1f - Mathf.Pow(1f - num, m_sceneExp);
			float a = 1f - Mathf.Pow(1f - num, m_skyExp);
			Color color = Color.Lerp(m_destFogColor, m_srcFogColor, t);
			color.a = a;
			if (m_DisableColorCorrection && m_ColorCorrectionPostFx != null && num > 0.4f)
			{
				m_ColorCorrectionPostFx.Intensity = 1f - num;
			}
			float num2 = Mathf.Lerp(m_destFogEndDistance, m_zoneStart, t);
			RenderSettings.fogColor = color;
			RenderSettings.fogEndDistance = Mathf.Lerp(m_destFogEndDistance, num2 + m_zoneWidth, t);
			RenderSettings.fogDensity = Mathf.Lerp(m_destFogDensity, 1f, t);
			RenderSettings.fogStartDistance = Mathf.Lerp(m_destFogStartDistance, num2 - m_zoneWidth, t);
			if ((bool)m_skyRenderer)
			{
				m_skyMaterialClone.SetColor(m_fogId, color);
			}
			m_time -= Time.deltaTime / ((!(Time.timeScale > 0f)) ? 1f : Time.timeScale);
		}
		else if (m_fogFading)
		{
			float num3 = Time.deltaTime / ((!(Time.timeScale > 0f)) ? 1f : Time.timeScale);
			m_destFogEndDistance = Mathf.Lerp(m_destFogEndDistance, m_fadingFogDistance, num3 / (num3 + 5f));
			RenderSettings.fogEndDistance = m_destFogEndDistance;
		}
	}

	private void EndOutro()
	{
		if (m_ColorCorrectionPostFx != null && m_DisableColorCorrection)
		{
			m_ColorCorrectionPostFx.Intensity = 1f;
		}
		AudioManager.Instance.EventManager.OnRegionOutroEnded();
		m_started = false;
	}

	public void EndFogEffect()
	{
		RenderSettings.fogColor = m_destFogColor;
		RenderSettings.fogDensity = m_destFogDensity;
		RenderSettings.fogEndDistance = m_destFogEndDistance;
		RenderSettings.fogStartDistance = m_destFogStartDistance;
		if (m_skyRenderer != null)
		{
			m_skyRenderer.sharedMaterial = m_skyMaterial;
		}
		m_time = 0f;
		if (m_rays != null)
		{
			UnityEngine.Object.Destroy(m_rays);
		}
	}
}
