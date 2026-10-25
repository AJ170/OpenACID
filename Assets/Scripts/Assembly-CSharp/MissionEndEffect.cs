using System;
using UnityEngine;

public class MissionEndEffect : MonoBehaviour
{
	private Color m_destFogColor;

	private float m_destFogStartDistance;

	private float m_destFogEndDistance;

	private float m_destFogDensity;

	private Color m_origFogColor;

	private float m_origFogStartDistance;

	private float m_origFogEndDistance;

	private float m_origFogDensity;

	private Renderer m_skyRenderer;

	private Material m_skyMaterial;

	private Material m_skyMaterialClone;

	private GameObject m_rays;

	private float m_time;

	private bool m_eventExecuted;

	private int m_fogId;

	public bool ForceStart;

	public bool ForceEnd;

	public bool DisablePlayerFog;

	public MissionEndEffectConf Config { get; set; }

	public bool DoSlowMotion { get; set; }

	public bool AutoDestroy { get; set; }

	public bool Reverse { get; set; }

	public Action<MissionEndEffect> EventAction { get; set; }

	public MissionEndEffectConf SuccessConf { get; private set; }

	public MissionEndEffectConf FailConf { get; private set; }

	public MissionEndEffectConf RevivePendingConf { get; private set; }

	public MissionEndEffectConf ReviveConf { get; private set; }

	public MissionEndEffectConf Revive2Conf { get; private set; }

	public MissionEndEffectConf ReviveFadeoutConf { get; private set; }

	public void Awake()
	{
		SuccessConf = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Effects/Prefabs/MissionEndEffectConf_Success")).GetComponent<MissionEndEffectConf>();
		FailConf = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Effects/Prefabs/MissionEndEffectConf_Fail")).GetComponent<MissionEndEffectConf>();
		RevivePendingConf = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Effects/Prefabs/MissionEndEffectConf_RevivePending")).GetComponent<MissionEndEffectConf>();
		ReviveConf = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Effects/Prefabs/MissionEndEffectConf_Revive")).GetComponent<MissionEndEffectConf>();
		Revive2Conf = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Effects/Prefabs/MissionEndEffectConf_Revive2")).GetComponent<MissionEndEffectConf>();
		ReviveFadeoutConf = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Effects/Prefabs/MissionEndEffectConf_ReviveFadeout")).GetComponent<MissionEndEffectConf>();
	}

	public void OnDestroy()
	{
		if (!SharedMain.IsShuttingDown)
		{
			UnityEngine.Object.Destroy(SuccessConf.gameObject);
			UnityEngine.Object.Destroy(FailConf.gameObject);
			UnityEngine.Object.Destroy(RevivePendingConf.gameObject);
			UnityEngine.Object.Destroy(ReviveConf.gameObject);
			UnityEngine.Object.Destroy(Revive2Conf.gameObject);
			UnityEngine.Object.Destroy(ReviveFadeoutConf.gameObject);
			RenderSettings.fogColor = m_origFogColor;
			RenderSettings.fogDensity = m_origFogDensity;
			RenderSettings.fogEndDistance = m_origFogEndDistance;
			RenderSettings.fogStartDistance = m_origFogStartDistance;
			if (m_skyRenderer != null)
			{
				m_skyRenderer.sharedMaterial = m_skyMaterial;
			}
			m_time = 0f;
			if (m_rays != null)
			{
				UnityEngine.Object.Destroy(m_rays);
			}
			SlowMotionEventManager component = LogicMain.PlayerEntity.GameObject.GetComponent<SlowMotionEventManager>();
			component.ClearSlowMotion();
			TogglePlayerFog(true);
		}
	}

	public void Start()
	{
		m_origFogColor = RenderSettings.fogColor;
		m_origFogStartDistance = RenderSettings.fogStartDistance;
		m_origFogEndDistance = RenderSettings.fogEndDistance;
		m_origFogDensity = RenderSettings.fogDensity;
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
		StartEffect();
	}

	public void StartEffect()
	{
		if (Reverse)
		{
			m_destFogColor = RenderSettings.fogColor;
			m_destFogStartDistance = RenderSettings.fogStartDistance;
			m_destFogEndDistance = RenderSettings.fogEndDistance;
			m_destFogDensity = RenderSettings.fogDensity;
		}
		else
		{
			m_destFogColor = m_origFogColor;
			m_destFogStartDistance = m_origFogStartDistance;
			m_destFogEndDistance = m_origFogEndDistance;
			m_destFogDensity = m_origFogDensity;
		}
		m_time = Config.Duration;
		m_eventExecuted = false;
		base.enabled = true;
		if ((bool)m_skyRenderer)
		{
			m_skyRenderer.sharedMaterial = m_skyMaterialClone;
		}
		if (DisablePlayerFog)
		{
			TogglePlayerFog(false);
		}
		if (m_rays == null)
		{
			m_rays = UnityEngine.Object.Instantiate(Config.RaysPrefab, LogicMain.PlayerEntity.Position, Camera.main.transform.rotation) as GameObject;
			m_rays.transform.parent = base.transform;
			m_rays.name = "Rays";
		}
		Rays component = m_rays.GetComponent<Rays>();
		component.m_color = Config.RaysColor;
		if (Reverse)
		{
			component.EndColor = component.CurrentColor;
		}
		else
		{
			component.EndColor = Color.black;
		}
		component.m_duration = Config.Duration;
		component.m_reverse = Reverse;
		component.m_colorExp = Config.RayExp;
		component.m_affectedBySlowMotion = false;
		component.Start();
		SlowMotionEventManager component2 = LogicMain.PlayerEntity.GameObject.GetComponent<SlowMotionEventManager>();
		component2.ClearSlowMotion();
		if (Config.SlowMotionEventData != null && DoSlowMotion)
		{
			component2.AddSlowMotion(Config.SlowMotionEventData);
		}
		m_fogId = Shader.PropertyToID("_Fog");
	}

	private void TogglePlayerFog(bool _enabled)
	{
		SkinnedMeshRenderer[] componentsInChildren = LogicMain.PlayerEntity.GameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in componentsInChildren)
		{
			for (int j = 0; j < skinnedMeshRenderer.sharedMaterials.Length; j++)
			{
				Material material = skinnedMeshRenderer.sharedMaterials[j];
				if (!_enabled)
				{
					material.EnableKeyword("FOG_OFF");
				}
				else
				{
					material.DisableKeyword("FOG_OFF");
				}
			}
		}
		SkinnedMeshRenderer[] componentsInChildren2 = LogicMain.PlayerEntity.GameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach (SkinnedMeshRenderer skinnedMeshRenderer2 in componentsInChildren2)
		{
			for (int l = 0; l < skinnedMeshRenderer2.sharedMaterials.Length; l++)
			{
				Material material2 = skinnedMeshRenderer2.sharedMaterials[l];
				if (!_enabled)
				{
					material2.EnableKeyword("FOG_OFF");
				}
				else
				{
					material2.DisableKeyword("FOG_OFF");
				}
			}
		}
	}

	private void Update()
	{
		if (ForceStart)
		{
			StartEffect();
			ForceStart = false;
		}
		if (ForceEnd)
		{
			EndEffect();
			ForceEnd = false;
			return;
		}
		if (Config.EventDelay != 0f && Config.Duration - m_time >= Config.EventDelay && EventAction != null && !m_eventExecuted)
		{
			m_eventExecuted = true;
			EventAction(this);
		}
		if (m_time > 0f)
		{
			float num = Mathf.Clamp01(m_time / Config.Duration);
			if (Reverse)
			{
				num = 1f - num;
			}
			float t = 1f - Mathf.Pow(1f - num, Config.SceneExp);
			float a = 1f - Mathf.Pow(1f - num, Config.SkyExp);
			Color color = Color.Lerp(m_destFogColor, Config.SrcFogColor, t);
			color.a = a;
			float num2 = Mathf.Lerp(m_destFogEndDistance, Config.ZoneStart, t);
			RenderSettings.fogColor = color;
			RenderSettings.fogEndDistance = Mathf.Lerp(m_destFogEndDistance, num2 + Config.ZoneWidth, t);
			RenderSettings.fogDensity = Mathf.Lerp(m_destFogDensity, 1f, t);
			RenderSettings.fogStartDistance = Mathf.Lerp(m_destFogStartDistance, num2 - Config.ZoneWidth, t);
			if ((bool)m_skyRenderer)
			{
				m_skyMaterialClone.SetColor(m_fogId, color);
			}
			m_time -= Time.deltaTime / ((!(Time.timeScale > 0f)) ? 1f : Time.timeScale);
		}
		else if (AutoDestroy)
		{
			EndEffect();
		}
	}

	public void EndEffect()
	{
		UnityEngine.Object.DestroyImmediate(base.gameObject);
	}
}
