using System;
using UbiCore.Utils;
using UnityEngine;

internal class MemoryAnalyzer : MonoBehaviour
{
	private float m_memorySize;

	private float m_memorySizePerFrame;

	public void Update()
	{
		float num = GC.GetTotalMemory(false);
		float a = num - m_memorySize;
		m_memorySize = num;
		m_memorySizePerFrame = Mathf.Lerp(m_memorySizePerFrame, Mathf.Max(a, 0f), UbiCore.Utils.Singleton<Timer>.Instance.realDeltaTime / (UbiCore.Utils.Singleton<Timer>.Instance.realDeltaTime + 0.05f));
	}

	public void OnGUI()
	{
		Rect position = new Rect(200f, 80f, 500f, 60f);
		GUI.Label(position, string.Format("Allocated memory: {0}kB (per frame: {1}kB)", (m_memorySize / 1024f).ToString("F2"), (m_memorySizePerFrame / 1024f).ToString("F2")));
	}
}
