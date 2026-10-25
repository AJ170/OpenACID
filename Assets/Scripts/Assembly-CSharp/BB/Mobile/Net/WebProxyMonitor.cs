using System;
using System.Net;
using UnityEngine;

namespace BB.Mobile.Net
{
	public sealed class WebProxyMonitor : MonoBehaviour
	{
		private static readonly IWebProxy mSystemWebProxy;

		[SerializeField]
		private string[] m_DevelopmentBypassList;

		[SerializeField]
		private bool m_RepairConnection = true;

		static WebProxyMonitor()
		{
			IWebProxy webProxy = WebRequest.GetSystemWebProxy();
			if (!object.ReferenceEquals(webProxy, null))
			{
				string environmentVariable = Environment.GetEnvironmentVariable("no_proxy");
				if (environmentVariable == null)
				{
					environmentVariable = Environment.GetEnvironmentVariable("NO_PROXY");
				}
				if (environmentVariable != null)
				{
					string[] bypassList = environmentVariable.Split(';');
					webProxy = new WebProxyBypassWrapper(webProxy, bypassList);
				}
			}
			mSystemWebProxy = webProxy;
		}

		private void Awake()
		{
			if (m_RepairConnection)
			{
				NetworkMonitor.EnregisterRepairConnection(UpdateWebProxy);
			}
			UpdateWebProxy();
		}

		private bool UpdateWebProxy()
		{
			IWebProxy webProxy = DetectConnectionWebProxy();
			if (object.ReferenceEquals(webProxy, null))
			{
				webProxy = mSystemWebProxy;
			}
			IWebProxy defaultWebProxy = WebRequest.DefaultWebProxy;
			if (object.ReferenceEquals(webProxy, defaultWebProxy))
			{
				return false;
			}
			WebProxy webProxy2 = webProxy as WebProxy;
			if (!object.ReferenceEquals(webProxy2, null))
			{
				WebProxyBypassWrapper webProxyBypassWrapper = defaultWebProxy as WebProxyBypassWrapper;
				IWebProxy webProxy4;
				if (object.ReferenceEquals(webProxyBypassWrapper, null))
				{
					IWebProxy webProxy3 = defaultWebProxy;
					webProxy4 = webProxy3;
				}
				else
				{
					webProxy4 = webProxyBypassWrapper.WebProxy;
				}
				WebProxy webProxy5 = webProxy4 as WebProxy;
				if (!object.ReferenceEquals(webProxy5, null) && webProxy2.Address == webProxy5.Address)
				{
					return false;
				}
			}
			if (!object.ReferenceEquals(m_DevelopmentBypassList, null) && m_DevelopmentBypassList.Length > 0 && !object.ReferenceEquals(webProxy, null))
			{
				webProxy = new WebProxyBypassWrapper(webProxy, m_DevelopmentBypassList);
			}
			WebRequest.DefaultWebProxy = webProxy;
			return true;
		}

		private static WebProxy DetectConnectionWebProxy()
		{
			switch (Application.platform)
			{
			case RuntimePlatform.Android:
			{
				using (AndroidJavaObject androidJavaObject = new AndroidJavaClass("java.lang.System"))
				{
					string text = androidJavaObject.CallStatic<string>("getProperty", new object[1] { "http.proxyHost" });
					if (string.IsNullOrEmpty(text))
					{
						return null;
					}
					string s = androidJavaObject.CallStatic<string>("getProperty", new object[1] { "http.proxyPort" });
					int result;
					WebProxy webProxy = ((!int.TryParse(s, out result)) ? new WebProxy(text) : new WebProxy(text, result));
					string text2 = androidJavaObject.CallStatic<string>("getProperty", new object[1] { "http.nonProxyHosts" });
					if (!string.IsNullOrEmpty(text2))
					{
						string[] array = text2.Split('|');
						if (array.Length > 0)
						{
							webProxy.BypassList = array;
						}
					}
					return webProxy;
				}
			}
			default:
				return null;
			}
		}
	}
}
