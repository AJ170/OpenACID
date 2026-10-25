using System;
using Assets.Scripts.Engine.ExtensionMethods;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Utils
{
	[ExecuteInEditMode]
	public abstract class UIPrefabProxy : UIWidget
	{
		public Action OnSetupFinished;

		[SerializeField]
		private GameObject m_prefab;

		private GameObject m_root;

		protected GameObject m_instance;

		public bool IsInitalized
		{
			get
			{
				return m_instance != null && m_prefab != null;
			}
		}

		public GameObject PrefabInstance
		{
			get
			{
				return m_instance;
			}
		}

		public virtual void Init()
		{
			if (!IsInitalized)
			{
				m_instance = AddChild(m_prefab);
				UIWidget uIWidget = m_instance.AddComponentIfNotExist<UIWidget>();
				uIWidget.autoResizeBoxCollider = true;
				uIWidget.depth = base.depth;
				SetTagRecurcive(uIWidget.gameObject, base.gameObject.tag);
				SetLayerRecurcive(uIWidget.gameObject, base.gameObject.layer);
				minWidth = uIWidget.minWidth;
				minHeight = uIWidget.minHeight;
				uIWidget.SetAnchor(base.gameObject, 0, 0, 0, 0);
			}
		}

		protected override void Awake()
		{
			base.Awake();
			if (Application.isPlaying)
			{
				Init();
			}
		}

		protected virtual void OnSetup()
		{
		}

		private void OnDestroy()
		{
			RemoveProxy();
		}

		private GameObject AddChild(GameObject _prefab)
		{
			return NGUITools.AddChild(base.gameObject, _prefab);
		}

		private void RemoveProxy()
		{
			NGUITools.Destroy(m_root);
			NGUITools.Destroy(m_instance);
			m_root = null;
			m_instance = null;
		}

		private static void SetTagRecurcive(GameObject _root, string _tag)
		{
			_root.gameObject.tag = _tag;
			foreach (GameObject child in _root.transform.GetChildren())
			{
				SetTagRecurcive(child.gameObject, _tag);
			}
		}

		private static void SetLayerRecurcive(GameObject _root, int _layer)
		{
			_root.gameObject.layer = _layer;
			foreach (GameObject child in _root.transform.GetChildren())
			{
				SetLayerRecurcive(child.gameObject, _layer);
			}
		}
	}
}
