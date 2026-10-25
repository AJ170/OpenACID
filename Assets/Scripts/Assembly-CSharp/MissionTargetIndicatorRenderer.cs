using Assets.Scripts.Engine.ExtensionMethods;
using Assets.Scripts.UI.Ingame;
using SharedBaseLib.Core;
using SharedBaseLib.System;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

public class MissionTargetIndicatorRenderer : MonoBehaviour
{
	private const float ReferenceScreenHeight = 1536f;

	private const float FadeTime = 0.5f;

	private const string ShaderKeyWord = "TINT_COLOR_ON";

	private const int ReferenceIconSize = 48;

	public static bool ForceInvisible = true;

	private static GameObject Pool;

	private static Object Prefab = null;

	private static BBList<MissionTargetIndicatorRenderer> Instances;

	private static readonly int ColorId = Shader.PropertyToID("_TintColor");

	private float m_alpha = 1f;

	private bool m_dirtyAlpha = true;

	private float m_scale = 1f;

	private IconMaterial m_icon;

	private float m_showTime;

	private bool m_toBeReleased;

	private bool m_ownOverrideValue = !ForceInvisible;

	private float Alpha
	{
		get
		{
			return m_alpha;
		}
		set
		{
			if (value != m_alpha)
			{
				m_dirtyAlpha = true;
				m_alpha = value;
			}
		}
	}

	public Vector3 Offset3D { get; set; }

	public Vector3 Position3D { get; set; }

	public float Scale { get; set; }

	public MissionTargetIndicatorRenderer()
	{
		Offset3D = new Vector3(0f, 1.3f, 0f);
	}

	public static void Preload()
	{
		Prefab = Resources.Load("UI/Combat/MissionTargetIndicator");
		GameObject target = GameObject.FindGameObjectWithTag("UiIconCamera");
		Pool = target.FindDirectChildByName(Constants.IconPoolTag);
		Instances = new BBList<MissionTargetIndicatorRenderer>(5);
		for (int i = 0; i < 5; i++)
		{
			CreateMissionTargetIndicator();
		}
	}

	public static void Unload()
	{
		Prefab = null;
		Instances = null;
		Pool = null;
	}

	public static MissionTargetIndicatorRenderer Get(IconMaterial _icon)
	{
		MissionTargetIndicatorRenderer missionTargetIndicatorRenderer = Instances.Find((MissionTargetIndicatorRenderer _d) => !_d.gameObject.activeSelf && !_d.m_toBeReleased);
		if (missionTargetIndicatorRenderer == null)
		{
			missionTargetIndicatorRenderer = CreateMissionTargetIndicator();
		}
		missionTargetIndicatorRenderer.SetIcon(_icon);
		missionTargetIndicatorRenderer.gameObject.layer = Layers.NonInteractableUiLayer;
		missionTargetIndicatorRenderer.gameObject.SetActive(true);
		missionTargetIndicatorRenderer.m_showTime = UbiCore.Utils.Singleton<Timer>.Instance.realTime;
		missionTargetIndicatorRenderer.Alpha = 0f;
		return missionTargetIndicatorRenderer;
	}

	private static MissionTargetIndicatorRenderer CreateMissionTargetIndicator()
	{
		GameObject gameObject = Object.Instantiate(Prefab) as GameObject;
		gameObject.transform.parent = Pool.transform;
		MissionTargetIndicatorRenderer component = gameObject.GetComponent<MissionTargetIndicatorRenderer>();
		Instances.Add(component);
		return component;
	}

	public void Release()
	{
		m_ownOverrideValue = false;
		m_toBeReleased = true;
		m_showTime = UbiCore.Utils.Singleton<Timer>.Instance.realTime;
		m_dirtyAlpha = true;
	}

	public void SetIcon(IconMaterial _icon)
	{
		if (_icon != m_icon)
		{
			GetComponent<Renderer>().material.DestroyAuto();
			m_icon = _icon;
			if (m_icon != 0)
			{
				GetComponent<Renderer>().material = new Material(UbiCore.Utils.Singleton<IconManager>.Instance.GetMaterial(m_icon));
				GetComponent<Renderer>().material.EnableKeyword("TINT_COLOR_ON");
				UpdateScale();
			}
		}
	}

	private void OnDestroy()
	{
		GetComponent<Renderer>().material.DestroyAuto();
	}

	private void LateUpdate()
	{
		float num = Mathf.Min(UbiCore.Utils.Singleton<Timer>.Instance.realTime - m_showTime, 0.5f);
		float num2 = num / 0.5f;
		if (m_toBeReleased)
		{
			if (num2 >= 1f)
			{
				base.gameObject.SetActive(false);
				return;
			}
			Alpha = 1f - num2;
		}
		else
		{
			Alpha = num2;
		}
		Vector2 vector = new Vector2(Screen.width, Screen.height);
		Vector2 vector2 = vector * 0.5f;
		Vector2 vector3 = vector2 - Vector2.one * 24f;
		Vector3 vector4 = Vector3.Scale(Camera.main.WorldToScreenPoint(Position3D + Offset3D), new Vector3(1f / (float)Camera.main.pixelWidth, 1f / (float)Camera.main.pixelHeight, 1f));
		Vector2 a = new Vector2(vector4.x, vector4.y);
		a -= new Vector2(0.5f, 0.5f);
		a = Vector2.Scale(a, vector);
		if (vector4.z < 0f || a.x < 0f - vector3.x || a.x > vector3.x || a.y < 0f - vector3.y || a.y > vector3.y)
		{
			if (vector4.z < 0f)
			{
				a.x = 0f - a.x;
				a.y = 0f - a.y;
			}
			if (a.sqrMagnitude < 1f)
			{
				a = new Vector2(0f, 0f - vector3.y);
			}
			else
			{
				float num3 = ((a.x == 0f) ? 0f : Mathf.Abs(vector3.x / a.x));
				float num4 = ((a.y == 0f) ? 0f : Mathf.Abs(vector3.y / a.y));
				if (num3 < num4)
				{
					a *= num3;
				}
				else
				{
					a *= num4;
				}
			}
		}
		a += vector2;
		base.transform.position = new Vector3(a.x, a.y, 1f);
		UpdateScale();
		if (ForceInvisible != m_ownOverrideValue)
		{
			m_dirtyAlpha = true;
			m_ownOverrideValue = ForceInvisible;
		}
		SetAlpha((!ForceInvisible) ? Alpha : 0f);
	}

	private void SetAlpha(float _alpha)
	{
		if (m_dirtyAlpha && GetComponent<Renderer>().material.HasProperty(ColorId))
		{
			Vector4 vector = GetComponent<Renderer>().material.GetVector(ColorId);
			vector.w = _alpha;
			GetComponent<Renderer>().material.SetVector(ColorId, vector);
		}
	}

	private void UpdateScale()
	{
		m_scale = (float)Screen.height / 1536f;
		base.transform.localScale = new Vector3(m_scale * 48f * Scale, m_scale * 48f * Scale, 1f);
	}
}
