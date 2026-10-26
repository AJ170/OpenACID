using System;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace AcierNetwork.AssetBundles
{
	public class AsyncLoadResult<T> : IAsyncResult where T : UnityEngine.Object
	{
		private readonly ManualResetEvent WaitHandle = new ManualResetEvent(false);

		private readonly IEnumerator m_bundleLoadEnum;

		private AssetBundleRequest m_loadAssetEnum;

		private readonly AssetBundleManager.BundleInfo m_Info;

		private readonly string m_Name;

		public object AsyncState { get; internal set; }

		public WaitHandle AsyncWaitHandle
		{
			get
			{
				return WaitHandle;
			}
		}

		public bool CompletedSynchronously
		{
			get
			{
				return false;
			}
		}

		public bool IsCompleted
		{
			get
			{
				if (m_Info.Local)
				{
					// served synchronously from AssetBundleManager.LocalSource in the constructor
					WaitHandle.Set();
					return true;
				}
				if (m_bundleLoadEnum != null && !m_bundleLoadEnum.MoveNext())
				{
					m_loadAssetEnum = m_Info.Bundle.LoadAssetAsync(m_Name, typeof(T));
				}
				if (m_loadAssetEnum != null && m_loadAssetEnum.isDone)
				{
					AsyncState = m_loadAssetEnum.asset;
					AssetBundleManager.Instance.CheckAutoUnload();
					WaitHandle.Set();
					AssetBundleManager.Instance.AutoUnloadEnabled = true;
					return true;
				}
				return false;
			}
		}

		internal AsyncLoadResult(AssetBundleManager.BundleInfo _info, string _resource)
		{
			m_Info = _info;
			m_Name = _resource;
			if (m_Info.Local)
			{
				m_Info.Load();
				AsyncState = AssetBundleManager.LocalSource.LoadAsset(m_Info.Info.URL, m_Name, typeof(T));
				return;
			}
			AssetBundleManager.Instance.AutoUnloadEnabled = false;
			if (m_Info.Unloaded)
			{
				m_bundleLoadEnum = m_Info.LoadAsync();
			}
			else
			{
				m_loadAssetEnum = m_Info.Bundle.LoadAssetAsync(m_Name, typeof(T));
			}
		}
	}
}
