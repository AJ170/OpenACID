using System.Collections;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.ScreenManager.Layout;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Utils.Layout
{
	[ExecuteInEditMode]
	public class UIGameObjectLayoutUpdate : MonoBehaviour
	{
		[SerializeField]
		private string m_tagNameId = string.Empty;

		[SerializeField]
		private Transform m_transform;

		[SerializeField]
		private UILayoutManager m_uiRefLayoutManager;

		private float m_originalScale;

		public string TagNameId
		{
			get
			{
				return m_tagNameId;
			}
			set
			{
				if (m_tagNameId == value)
				{
					return;
				}
				if (Application.isPlaying)
				{
					if (m_tagNameId == string.Empty)
					{
						m_originalScale = m_transform.localScale.x;
					}
					else if (value == string.Empty)
					{
						m_transform.localScale = new Vector3(m_originalScale, m_originalScale, m_originalScale);
					}
				}
				m_tagNameId = value;
				UpdateDeviceLayout();
			}
		}

		public UILayoutManager LayoutManager
		{
			get
			{
				if (!Application.isPlaying)
				{
					return m_uiRefLayoutManager;
				}
				if (UbiCore.Utils.Singleton<UiMain>.IsAvailable)
				{
					return UbiCore.Utils.Singleton<UiMain>.Instance.LayoutManager;
				}
				return null;
			}
		}

		private void Awake()
		{
			if (m_transform != null)
			{
				m_originalScale = m_transform.localScale.x;
			}
		}

		private void OnStart()
		{
			UpdateDeviceLayout();
		}

		private void OnEnable()
		{
			if (LayoutManager != null)
			{
				EventDelegate.Add(LayoutManager.OnLayoutOptionsChanged, OnDeviceLayoutChanged);
				OnDeviceLayoutChanged();
			}
			else
			{
				StartCoroutine(WaitForValidLayoutManager());
			}
		}

		private void OnDisable()
		{
			if (LayoutManager != null)
			{
				EventDelegate.Add(LayoutManager.OnLayoutOptionsChanged, OnDeviceLayoutChanged);
			}
			StopAllCoroutines();
		}

		private IEnumerator WaitForValidLayoutManager()
		{
			while (LayoutManager == null)
			{
				yield return new WaitForEndOfFrame();
			}
			EventDelegate.Add(LayoutManager.OnLayoutOptionsChanged, OnDeviceLayoutChanged);
			OnDeviceLayoutChanged();
		}

		private void OnDeviceLayoutChanged()
		{
			UpdateDeviceLayout();
		}

		private void UpdateDeviceLayout()
		{
			if (Application.isPlaying && !string.IsNullOrEmpty(m_tagNameId) && LayoutManager != null)
			{
				FamilyLayoutData familyLayoutData = LayoutManager.GetFamilyLayoutData(m_tagNameId);
				if (familyLayoutData != null)
				{
					m_transform.localScale = new Vector3(familyLayoutData.Scale, familyLayoutData.Scale, familyLayoutData.Scale);
				}
				else
				{
					m_transform.localScale = new Vector3(m_originalScale, m_originalScale, m_originalScale);
				}
			}
		}
	}
}
