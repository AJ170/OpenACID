using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class EveryplayHudCamera : MonoBehaviour
{
	private const int EPSR = 1162892114;

	private bool subscribed;

	private bool readyForRecording;

	private IntPtr renderEventPtr = IntPtr.Zero;

	private void Awake()
	{
		Subscribe(true);
		readyForRecording = Everyplay.IsReadyForRecording();
	}

	private void OnDestroy()
	{
		Subscribe(false);
	}

	private void OnEnable()
	{
		Subscribe(true);
	}

	private void OnDisable()
	{
		Subscribe(false);
	}

	private void Subscribe(bool subscribe)
	{
		if (!subscribed && subscribe)
		{
			Everyplay.ReadyForRecording += ReadyForRecording;
		}
		else if (subscribed && !subscribe)
		{
			Everyplay.ReadyForRecording -= ReadyForRecording;
		}
		subscribed = subscribe;
	}

	private void ReadyForRecording(bool ready)
	{
		if (ready && renderEventPtr == IntPtr.Zero)
		{
			renderEventPtr = EveryplayGetUnityRenderEventPtr();
		}
		readyForRecording = ready;
	}

	private void OnPreRender()
	{
		if (readyForRecording && renderEventPtr != IntPtr.Zero)
		{
			GL.IssuePluginEvent(renderEventPtr, 1162892114);
		}
	}

	[DllImport("everyplay")]
	private static extern IntPtr EveryplayGetUnityRenderEventPtr();
}
