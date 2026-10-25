using System;
using AcierProtocol;
using UnityEngine;

public class SelectionEffectManager : MonoBehaviour
{
	private class BorderElement
	{
		public GameObject TopLeft;

		public GameObject TopRight;

		public GameObject BottomLeft;

		public GameObject BottomRight;

		public Material Material;

		public bool IsVisible;

		public bool FastSelection;

		private float m_visibility;

		private UIWidget m_widget;

		private bool m_cleanupNeeded;

		public UIWidget Widget
		{
			get
			{
				return m_widget;
			}
			set
			{
				m_widget = value;
				m_cleanupNeeded = m_widget != null;
				if (m_widget != null)
				{
					int layer = m_widget.gameObject.layer;
					TopLeft.layer = layer;
					TopRight.layer = layer;
					BottomLeft.layer = layer;
					BottomRight.layer = layer;
				}
			}
		}

		public void Update(SelectionEffectManager _man)
		{
			if ((Widget == null && m_cleanupNeeded) || (Widget != null && !Widget.isVisible))
			{
				DeselectInstant();
			}
			if (!(Widget == null))
			{
				Vector3[] worldCorners = Widget.worldCorners;
				Bounds bounds = new Bounds(worldCorners[0], Vector3.zero);
				for (int i = 1; i < 4; i++)
				{
					bounds.Encapsulate(worldCorners[i]);
				}
				Vector3 center = bounds.center;
				Vector3 vector = new Vector3(bounds.min.x, bounds.max.y, center.z);
				Vector3 vector2 = new Vector3(bounds.max.x, bounds.max.y, center.z);
				Vector3 vector3 = new Vector3(bounds.min.x, bounds.min.y, center.z);
				Vector3 vector4 = new Vector3(bounds.max.x, bounds.min.y, center.z);
				float num = s_uiScale * (_man.CloseSelectionDistance + _man.Progress * _man.FarSelectionDistance);
				vector += (vector - center).normalized * num;
				vector2 += (vector2 - center).normalized * num;
				vector3 += (vector3 - center).normalized * num;
				vector4 += (vector4 - center).normalized * num;
				TopLeft.transform.position = vector;
				TopRight.transform.position = vector2;
				BottomLeft.transform.position = vector3;
				BottomRight.transform.position = vector4;
				float num2 = _man.FadeOutTime;
				if (IsVisible)
				{
					num2 = ((!FastSelection) ? _man.FadeInTime : _man.FadeInTimeFast);
				}
				m_visibility += ((!IsVisible) ? (-1f) : 1f) * Time.unscaledDeltaTime / num2;
				m_visibility = Mathf.Clamp01(m_visibility);
				float num3 = Mathf.SmoothStep(0f, 1f, m_visibility);
				bool flag = num3 > 0.0019607844f;
				if (!IsVisible && !flag)
				{
					Widget = null;
				}
				TopLeft.SetActive(flag);
				TopRight.SetActive(flag);
				BottomRight.SetActive(flag);
				BottomLeft.SetActive(flag);
				if (flag)
				{
					Material.SetColor("_TintColor", new Color(1f, 1f, 1f, num3));
				}
			}
		}

		public void DeselectInstant()
		{
			IsVisible = false;
			FastSelection = false;
			m_visibility = 0f;
			TopLeft.SetActive(false);
			TopRight.SetActive(false);
			BottomRight.SetActive(false);
			BottomLeft.SetActive(false);
			Widget = null;
		}
	}

	private static float s_uiScale = 0.0013020834f;

	public float CloseSelectionDistance = 0.15f;

	public float FarSelectionDistance = 0.5f;

	public GameObject CornerPrefab;

	public float AnimationTime = 1f;

	public int PoolObjectNum = 3;

	public float FadeInTime = 0.3f;

	public float FadeInTimeFast = 0.09f;

	public float FadeOutTime = 0.1f;

	private int m_previousActiveElement = -1;

	private int m_currentlyActiveElement;

	private BorderElement[] m_borderElements;

	private float m_progressClock;

	public float Progress { get; private set; }

	private void Awake()
	{
		if (PoolObjectNum < 2)
		{
			PoolObjectNum = 2;
		}
		m_borderElements = new BorderElement[PoolObjectNum];
		Progress = 0f;
		SetUpElements();
	}

	public void SetUpElements()
	{
		for (int i = 0; i < PoolObjectNum; i++)
		{
			BorderElement borderElement = new BorderElement();
			borderElement.TopLeft = UnityEngine.Object.Instantiate(CornerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			borderElement.TopRight = UnityEngine.Object.Instantiate(CornerPrefab, Vector3.zero, Quaternion.Euler(0f, 0f, -90f)) as GameObject;
			borderElement.BottomRight = UnityEngine.Object.Instantiate(CornerPrefab, Vector3.zero, Quaternion.Euler(0f, 0f, -180f)) as GameObject;
			borderElement.BottomLeft = UnityEngine.Object.Instantiate(CornerPrefab, Vector3.zero, Quaternion.Euler(0f, 0f, -270f)) as GameObject;
			borderElement.TopLeft.SetActive(false);
			borderElement.TopRight.SetActive(false);
			borderElement.BottomRight.SetActive(false);
			borderElement.BottomLeft.SetActive(false);
			borderElement.TopLeft.transform.parent = base.transform;
			borderElement.TopRight.transform.parent = base.transform;
			borderElement.BottomRight.transform.parent = base.transform;
			borderElement.BottomLeft.transform.parent = base.transform;
			borderElement.Material = borderElement.TopLeft.GetComponent<Renderer>().material;
			borderElement.TopRight.GetComponent<Renderer>().sharedMaterial = borderElement.Material;
			borderElement.BottomLeft.GetComponent<Renderer>().sharedMaterial = borderElement.Material;
			borderElement.BottomRight.GetComponent<Renderer>().sharedMaterial = borderElement.Material;
			m_borderElements[i] = borderElement;
		}
	}

	public void SelectElement(UIWidget _rect, bool _fastSelection = false)
	{
		if (m_previousActiveElement != -1)
		{
			m_borderElements[m_previousActiveElement].IsVisible = false;
		}
		if (!(_rect == null))
		{
			m_borderElements[m_currentlyActiveElement].Widget = _rect;
			m_borderElements[m_currentlyActiveElement].IsVisible = true;
			m_borderElements[m_currentlyActiveElement].FastSelection = _fastSelection;
			m_previousActiveElement = m_currentlyActiveElement;
			m_currentlyActiveElement = (m_currentlyActiveElement + 1) % PoolObjectNum;
		}
	}

	public void DeselectElementInstant(UIWidget _rect)
	{
		BorderElement borderElement = m_borderElements.Find((BorderElement _e) => _e.Widget == _rect);
		if (borderElement != null)
		{
			borderElement.DeselectInstant();
		}
	}

	public void DeselectElementsInstant()
	{
		for (int i = 0; i < m_borderElements.Length; i++)
		{
			m_borderElements[i].DeselectInstant();
		}
	}

	public void Update()
	{
		m_progressClock += Time.unscaledDeltaTime / AnimationTime;
		m_progressClock = Mathf.Repeat(m_progressClock, (float)Math.PI * 2f);
		Progress = (Mathf.Sin(m_progressClock) + 1f) / 2f;
		for (int i = 0; i < m_borderElements.Length; i++)
		{
			m_borderElements[i].Update(this);
		}
	}
}
