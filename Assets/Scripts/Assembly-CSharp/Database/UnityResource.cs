using System;
using System.Collections;
using System.IO;
using AcierNetwork.AssetBundles;
using AssetBundleData;
using SharedBaseLib.Serialization;
using UnityEngine;

namespace Database
{
	public class UnityResource : DataSerializableBase
	{
		private UnityEngine.Object m_object;

		private bool m_internalResource;

		private AssetBundleInfo m_bundleInfo;

		private string m_assetGuid;

		private string m_resourcePath;

		private AsyncLoadResult<UnityEngine.Object> m_asyncLoadResult;

		public UnityResource(AssetBundleInfo _bundleInfo)
		{
			m_bundleInfo = _bundleInfo;
		}

		public UnityResource()
		{
		}

		public void ClearCache()
		{
			m_object = null;
		}

		public T AccessObject<T>() where T : UnityEngine.Object
		{
			return GetObject<T>();
		}

		public T GetObject<T>() where T : UnityEngine.Object
		{
			if (m_object != null)
			{
				return (T)m_object;
			}
			if (string.IsNullOrEmpty(m_resourcePath))
			{
				return (T)null;
			}
			if (m_internalResource)
			{
				return Resources.Load<T>(m_resourcePath);
			}
			if (m_bundleInfo != null)
			{
				m_object = AssetBundleManager.Instance.LoadAsset<UnityEngine.Object>(m_bundleInfo, m_resourcePath);
				return (T)m_object;
			}
			return (T)null;
		}

		public IEnumerator PrepareAsync()
		{
			if (!(m_object == null) || m_asyncLoadResult != null || string.IsNullOrEmpty(m_resourcePath) || m_bundleInfo == null)
			{
				yield break;
			}
			m_asyncLoadResult = AssetBundleManager.Instance.LoadAssetAsync<UnityEngine.Object>(m_bundleInfo, m_resourcePath);
			if (m_asyncLoadResult != null)
			{
				while (!m_asyncLoadResult.IsCompleted)
				{
					yield return 0;
				}
				m_object = (UnityEngine.Object)m_asyncLoadResult.AsyncState;
				m_asyncLoadResult = null;
			}
		}

		protected override void SerializeInternal(DataSerializerBase _s, object _customData = null)
		{
			string empty = string.Empty;
			m_bundleInfo = (AssetBundleInfo)_customData;
			m_resourcePath = _s.SerializeString("ResourcePath", m_resourcePath);
			m_assetGuid = _s.SerializeString("Guid", m_assetGuid);
			int num = m_resourcePath.LastIndexOf("/resources/", StringComparison.InvariantCultureIgnoreCase);
			if (num >= 0)
			{
				m_internalResource = true;
				m_resourcePath = m_resourcePath.Substring(num + 11);
				m_resourcePath = Path.ChangeExtension(m_resourcePath, string.Empty);
				m_resourcePath = m_resourcePath.Remove(m_resourcePath.Length - 1);
			}
			else
			{
				m_internalResource = false;
			}
		}

		public string GetResourcePath()
		{
			return m_resourcePath;
		}

		public bool WrappedObjectIsNull()
		{
			return m_object == null;
		}
	}
}
