using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace BB.Mobile.Net
{
	public sealed class UnityTraceAdaptor : MonoBehaviour
	{
		private List<TraceListener> mTraceListeners = new List<TraceListener>();

		private TraceListenerAdapter mConsoleAdapter;

		[SerializeField]
		private bool m_TraceToFile = true;

		[SerializeField]
		private bool m_TraceToConsole = true;

		private void Register(TraceListener traceListener)
		{
			if (object.ReferenceEquals(traceListener, null))
			{
				throw new ArgumentNullException();
			}
			mTraceListeners.Add(traceListener);
			Trace.Listeners.Add(traceListener);
		}

		private void Awake()
		{
			mTraceListeners = new List<TraceListener>();
			if (m_TraceToFile)
			{
				string text = Path.Combine(Application.temporaryCachePath, DateTime.UtcNow.ToFileTimeUtc() + ".trace");
				Register(new TextWriterTraceListener(text));
				UnityEngine.Debug.Log("Trace log file at " + text);
			}
			if (m_TraceToConsole)
			{
				mConsoleAdapter = new TraceListenerAdapter();
				Register(mConsoleAdapter);
			}
		}

		private void OnDestroy()
		{
			Update();
			mConsoleAdapter = null;
			for (int i = 0; i < mTraceListeners.Count; i++)
			{
				using (TraceListener listener = mTraceListeners[i])
				{
					Trace.Listeners.Remove(listener);
				}
			}
			mTraceListeners.Clear();
		}

		private void Update()
		{
			if (!object.ReferenceEquals(mConsoleAdapter, null))
			{
				mConsoleAdapter.ProcessMessages(ProcessTraceMessage);
			}
		}

		private void ProcessTraceMessage(TraceEventType eventType, string message)
		{
			switch (eventType)
			{
			case TraceEventType.Critical:
			case TraceEventType.Error:
				UnityEngine.Debug.LogError(message);
				break;
			case TraceEventType.Warning:
				UnityEngine.Debug.LogWarning(message);
				break;
			default:
				UnityEngine.Debug.Log(message);
				break;
			}
		}
	}
}
