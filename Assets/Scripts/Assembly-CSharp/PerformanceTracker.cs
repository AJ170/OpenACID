using System.Collections;
using SharedBaseLib.Core;
using UnityEngine;

public class PerformanceTracker : MonoBehaviour
{
	private const float kMinReportFrequency = 30f;

	private const float kSampleFrequency = 1f;

	private const int kRawFrameTimeRingBufferSize = 128;

	private const int kAvgFrameTimeRingBufferSize = 64;

	private const int kLastFramesRingBufferSize = 50;

	private const int kSecondsToMS = 1000;

	private const int kMaxPlausibleFrameTime = 200;

	private const int kMinPlausibleFrameTime = 10;

	private BBRingBuffer<float> m_vFrameTimes = new BBRingBuffer<float>(128);

	private BBRingBuffer<float> m_vSmoothFrameTimes = new BBRingBuffer<float>(64);

	public float MinSmoothedFrameTime { get; private set; }

	public float MaxSmoothedFrameTime { get; private set; }

	public float AvgSmoothedFrameTime { get; private set; }

	private static float GetAvgFrameTime(BBRingBuffer<float> _vFrameTimes)
	{
		float num = 0f;
		for (int i = 0; i < _vFrameTimes.Count; i++)
		{
			num += _vFrameTimes[i];
		}
		return num / Mathf.Max(1f, _vFrameTimes.Count);
	}

	private static float GetMinimum(BBRingBuffer<float> _vFrameTimes)
	{
		float num = float.MaxValue;
		for (int i = 0; i < _vFrameTimes.Count; i++)
		{
			num = Mathf.Min(num, _vFrameTimes[i]);
		}
		return num;
	}

	private static float GetMaximum(BBRingBuffer<float> _vFrameTimes)
	{
		float num = float.MinValue;
		for (int i = 0; i < _vFrameTimes.Count; i++)
		{
			num = Mathf.Max(num, _vFrameTimes[i]);
		}
		return num;
	}

	private void Start()
	{
		Restart();
		StartCoroutine(UpdateFrameTime());
	}

	public void Restart()
	{
		m_vFrameTimes.Clear();
		m_vSmoothFrameTimes.Clear();
		MinSmoothedFrameTime = 0f;
		MaxSmoothedFrameTime = 0f;
		AvgSmoothedFrameTime = 0f;
	}

	private void Update()
	{
		float num = Time.deltaTime * 1000f;
		if (num <= 200f && num >= 10f)
		{
			m_vFrameTimes.Add(num);
		}
	}

	private IEnumerator UpdateFrameTime()
	{
		while (true)
		{
			float currAvgFrameTime = GetAvgFrameTime(m_vFrameTimes);
			m_vSmoothFrameTimes.Add(currAvgFrameTime);
			MinSmoothedFrameTime = GetMinimum(m_vSmoothFrameTimes);
			MaxSmoothedFrameTime = GetMaximum(m_vSmoothFrameTimes);
			AvgSmoothedFrameTime = GetAvgFrameTime(m_vSmoothFrameTimes);
			yield return new WaitForSeconds(1f);
		}
	}
}
