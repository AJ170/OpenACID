using System.Collections;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.ScreenManager.Layout;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Utils
{
	[ExecuteInEditMode]
	public class UIRootLayoutUpdate : MonoBehaviour
	{
		[SerializeField]
		private string m_tagNameId = string.Empty;

		private int m_originalMinScreenHeight;

		private UIRoot m_uiRoot;

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
				if (Application.isPlaying && UIRefRoot != null)
				{
					if (m_tagNameId == string.Empty)
					{
						m_originalMinScreenHeight = UIRefRoot.manualHeight;
					}
					else if (value == string.Empty)
					{
						UIRefRoot.manualHeight = m_originalMinScreenHeight;
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
					return null;
				}
				if (UbiCore.Utils.Singleton<UiMain>.IsAvailable)
				{
					return UbiCore.Utils.Singleton<UiMain>.Instance.LayoutManager;
				}
				return null;
			}
		}

		private UIRoot UIRefRoot
		{
			get
			{
				if (m_uiRoot == null)
				{
					m_uiRoot = GetComponent<UIRoot>();
				}
				return m_uiRoot;
			}
		}

		private void Awake()
		{
			if (UIRefRoot != null)
			{
				m_originalMinScreenHeight = UIRefRoot.manualHeight;
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
					UIRefRoot.manualHeight = familyLayoutData.FontData.FontSize;
				}
				else
				{
					UIRefRoot.manualHeight = m_originalMinScreenHeight;
				}
			}
		}
	}
}
