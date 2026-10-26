using System;
using System.Collections.ObjectModel;
using Assets.Scripts.Audio;
using Assets.Scripts.Engine.ExtensionMethods;
using Assets.Scripts.UI.Ingame;
using Assets.Scripts.UI.NGUIExtentions;
using Assets.Scripts.UI.Utils;
using SharedBaseLib.Core;
using SharedBaseLib.System;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;
using UnityEngine.AI;

public class IconDisplay : MonoBehaviour
{
	public static bool ForceInvisible = true;

	private static GameObject Pool;

	private static UnityEngine.Object Prefab;

	private static BBList<IconDisplay> Instances;

	private static bool NeedInitShaderPropertyIDs = true;

	private static int TintColorId;

	private static int IconId;

	private static int AlphaId;

	private IconAttachment m_iconAttachment;

	private float m_scale = 1f;

	private float m_iconSize = 128f;

	private float m_texSize = 128f;

	private float m_iconAlphaOffsetSize = 30f;

	private float m_texAlphaOffsetSize = 30f;

	private IconMaterial m_icon;

	private bool m_isOnBorder;

	private float m_alpha = 1f;

	private bool m_dirtyAlpha = true;

	private readonly float[] m_fadeScale = new float[2] { 0.7f, 0f };

	private bool m_destroyMaterials;

	private UIPlayAudio m_audioClick;

	private UIPlayAudio m_audioClickDeny;

	private bool m_ownOverrideValue = !ForceInvisible;

	public static ReadOnlyCollection<IconDisplay> ReadOnlyInstances
	{
		get
		{
			return (Instances == null) ? null : Instances.AsReadOnly();
		}
	}

	public IconAttachment IconAttachment
	{
		get
		{
			return m_iconAttachment;
		}
	}

	public bool IsOnBorder
	{
		get
		{
			return m_isOnBorder;
		}
	}

	public float AdditionalScale
	{
		get
		{
			return 1f + m_fadeScale[0] * (1f - Math.Min(LogicMain.CurrentTime - m_fadeScale[1], 0.25f) / 0.25f);
		}
		set
		{
			m_fadeScale[0] = value;
			m_fadeScale[1] = LogicMain.CurrentTime;
		}
	}

	public float IconSize
	{
		get
		{
			return m_iconSize;
		}
	}

	public float Alpha
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

	public override string ToString()
	{
		string empty = string.Empty;
		empty = empty + " IconSize: " + m_iconSize;
		empty = empty + " Pos: " + base.transform.localPosition.ToString();
		return empty + " localScale: " + base.transform.localScale.ToString();
	}

	public static void Preload()
	{
		Prefab = Resources.Load("UI/Combat/IconDisplay");
		if (NeedInitShaderPropertyIDs)
		{
			TintColorId = Shader.PropertyToID("_TintColor");
			IconId = Shader.PropertyToID("_icon");
			AlphaId = Shader.PropertyToID("_alpha");
			NeedInitShaderPropertyIDs = false;
		}
		GameObject target = GameObject.FindGameObjectWithTag("UiIconCamera");
		Pool = target.FindDirectChildByName(Constants.IconPoolTag);
		Instances = new BBList<IconDisplay>(5);
		for (int i = 0; i < 5; i++)
		{
			CreateIconDisplay();
		}
	}

	public static void Unload()
	{
		Prefab = null;
		Instances = null;
		Pool = null;
	}

	public static IconDisplay Get(IconAttachment _target)
	{
		IconDisplay iconDisplay = Instances.Find((IconDisplay _d) => !_d.gameObject.activeSelf);
		if (iconDisplay == null)
		{
			iconDisplay = CreateIconDisplay();
		}
		iconDisplay.gameObject.layer = ((!_target.IsInteractable) ? Layers.NonInteractableUiLayer : Layers.InteractableUiLayer);
		iconDisplay.gameObject.SetActive(true);
		iconDisplay.m_iconAttachment = _target;
		iconDisplay.AdditionalScale = 0f;
		return iconDisplay;
	}

	private static IconDisplay CreateIconDisplay()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Prefab) as GameObject;
		gameObject.transform.parent = Pool.transform;
		IconDisplay component = gameObject.GetComponent<IconDisplay>();
		Instances.Add(component);
		return component;
	}

	public void UpdateInteractableLayer()
	{
		base.gameObject.layer = ((!m_iconAttachment.IsInteractable) ? Layers.NonInteractableUiLayer : Layers.InteractableUiLayer);
	}

	public void Release()
	{
		m_iconAttachment = null;
		base.gameObject.SetActive(false);
		m_dirtyAlpha = true;
	}

	private void DestroyInstancedMaterial()
	{
		if (m_destroyMaterials)
		{
			GetComponent<Renderer>().material.DestroyAuto();
		}
		m_destroyMaterials = false;
		m_alpha = 1f;
		m_dirtyAlpha = true;
	}

	public void SetIcon(IconMaterial _icon)
	{
		if (_icon != m_icon)
		{
			DestroyInstancedMaterial();
			m_icon = _icon;
			GetComponent<Renderer>().sharedMaterial = UbiCore.Utils.Singleton<IconManager>.Instance.GetMaterial(m_icon);
			if (GetComponent<Renderer>().material.HasProperty(IconId))
			{
				Texture texture = GetComponent<Renderer>().material.GetTexture(IconId);
				m_texSize = texture.width;
			}
			else
			{
				m_texSize = UIUtil.ReferenceIconSize;
			}
			UpdateScale();
			UpdateParameters();
		}
	}

	public IconMaterial GetIcon()
	{
		return m_icon;
	}

	public bool OnTap(NavMeshAgent _agent)
	{
		if (m_iconAttachment.OnTap(_agent))
		{
			if (m_audioClick != null)
			{
				m_audioClick.Play();
			}
			return true;
		}
		if (m_audioClickDeny != null)
		{
			m_audioClickDeny.Play();
		}
		return false;
	}

	private void SetAlpha(float _value)
	{
		if (m_dirtyAlpha)
		{
			m_destroyMaterials = true;
			Renderer component = GetComponent<Renderer>();
			// Reading .material makes the instance the renderer's shared material, so copying sharedMaterial into it
			// copied it onto itself: a no-op that Unity 2017 asserts on every frame. Left out.
			component.material.SetColor(TintColorId, new Color(0.5f, 0.5f, 0.5f, Mathf.Clamp01(_value)));
		}
	}

	private void Awake()
	{
		m_audioClick = base.gameObject.AddComponent<UIPlayAudio>();
		m_audioClick.AudioController = UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_navigate;
		m_audioClick.PlayType = UIPlayAudio.EPlayType.StopAndPlay;
		m_audioClickDeny = base.gameObject.AddComponent<UIPlayAudio>();
		m_audioClickDeny.AudioController = UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_deny;
		m_audioClickDeny.PlayType = UIPlayAudio.EPlayType.StopAndPlay;
	}

	private void OnDestroy()
	{
		DestroyInstancedMaterial();
	}

	public void LateUpdate()
	{
		Vector2 vector = new Vector2(Screen.width, Screen.height);
		Vector2 vector2 = vector * 0.5f;
		float num = m_iconSize - m_iconAlphaOffsetSize;
		Vector2 vector3 = vector2 - Vector2.one * (num * 0.5f);
		Vector3 a = Camera.main.WorldToScreenPoint(m_iconAttachment.Position);
		Vector3 vector4 = Vector3.Scale(a, new Vector3(1f / (float)Camera.main.pixelWidth, 1f / (float)Camera.main.pixelHeight, 1f));
		Vector2 a2 = new Vector2(vector4.x, vector4.y);
		a2 -= new Vector2(0.5f, 0.5f);
		a2 = Vector2.Scale(a2, vector);
		Vector3 vector5 = (new Vector3(0f, 0.5f) + m_iconAttachment.Offset2D) * m_texSize * m_scale;
		a2.x += vector5.x;
		a2.y += vector5.y;
		if (vector4.z < 0f || a2.x < 0f - vector3.x || a2.x > vector3.x || a2.y < 0f - vector3.y || a2.y > vector3.y)
		{
			m_isOnBorder = true;
			if (vector4.z < 0f)
			{
				a2.x = 0f - a2.x;
				a2.y = 0f - a2.y;
			}
			if (a2.sqrMagnitude < 1f)
			{
				a2 = new Vector2(0f, 0f - vector3.y);
			}
			else
			{
				float num2 = Mathf.Abs(vector3.x / a2.x);
				float num3 = Mathf.Abs(vector3.y / a2.y);
				if (num2 < num3)
				{
					a2 *= num2;
				}
				else
				{
					a2 *= num3;
				}
			}
		}
		else
		{
			m_isOnBorder = false;
		}
		a2 += vector2;
		base.transform.position = new Vector3(a2.x, a2.y, 1f);
		float num4 = m_iconSize * AdditionalScale;
		base.transform.localScale = new Vector3(num4, num4, 1f);
		if (ForceInvisible != m_ownOverrideValue)
		{
			m_dirtyAlpha = true;
			m_ownOverrideValue = ForceInvisible;
		}
		SetAlpha((!ForceInvisible) ? Alpha : 0f);
	}

	private void UpdateParameters()
	{
		if (GetComponent<Renderer>().material.HasProperty(AlphaId))
		{
			m_alpha = GetComponent<Renderer>().material.GetFloat(AlphaId);
		}
	}

	private void UpdateScale()
	{
		m_scale = UIUtil.CalculateScaleFactorFromDpi();
		m_iconSize = Mathf.RoundToInt(m_texSize * m_scale);
		m_iconSize = Mathf.Max(m_iconSize, 16f);
		m_iconAlphaOffsetSize = Mathf.RoundToInt(m_texAlphaOffsetSize * m_scale);
	}
}
