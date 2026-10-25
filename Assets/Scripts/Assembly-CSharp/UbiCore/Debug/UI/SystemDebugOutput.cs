using System;
using UbiCore.Utils;
using UnityEngine;

namespace UbiCore.Debug.UI
{
	[RequireComponent(typeof(GUIText))]
	public class SystemDebugOutput : MonoBehaviour
	{
		private int[] m_gcLastCollectionCount = new int[GC.MaxGeneration + 1];

		private bool m_ShowGCStats;

		private bool m_ShowCPUStats;

		private bool m_ShowFreeMemoryPages;

		private bool m_ShowFreeMemoryBytes;

		private bool m_ShowUsedMemoryBytes;

		private bool m_ShowFrameTimeGraph;

		private bool m_ShowFrameTimeGraphAnnotations = true;

		private DebugGraph m_gcCollectionCount;

		private DebugGraph m_frameTime;

		private DebugGraph m_freeMemoryPages;

		private DebugGraph m_freeMemoryBytes;

		private DebugGraph m_usedMemoryBytes;

		private DebugGraph m_cpuUsage;

		private float m_initialGraphOffset = 100f;

		private float m_currentGraphOffsetY;

		private float m_marginBetweenGraphs = 10f;

		private float m_previousRealDeltaTime;

		public bool ShowFrameTimeGraph
		{
			get
			{
				return m_ShowFrameTimeGraph;
			}
			set
			{
				m_ShowFrameTimeGraph = value;
				base.enabled = GetIsEnabled();
			}
		}

		public bool ShowFrameTimeGraphAnnotations
		{
			get
			{
				return m_ShowFrameTimeGraphAnnotations;
			}
			set
			{
				m_ShowFrameTimeGraphAnnotations = value;
				base.enabled = GetIsEnabled();
			}
		}

		public bool ShowGCStats
		{
			get
			{
				return m_ShowGCStats;
			}
			set
			{
				m_ShowGCStats = value;
				base.enabled = GetIsEnabled();
			}
		}

		public bool ShowFreeMemoryPages
		{
			get
			{
				return m_ShowFreeMemoryPages;
			}
			set
			{
				m_ShowFreeMemoryPages = value;
				base.enabled = GetIsEnabled();
			}
		}

		public bool ShowFreeMemoryBytes
		{
			get
			{
				return m_ShowFreeMemoryBytes;
			}
			set
			{
				m_ShowFreeMemoryBytes = value;
				base.enabled = GetIsEnabled();
			}
		}

		public bool ShowUsedMemoryBytes
		{
			get
			{
				return m_ShowUsedMemoryBytes;
			}
			set
			{
				m_ShowUsedMemoryBytes = value;
				base.enabled = GetIsEnabled();
			}
		}

		public bool ShowCPUStats
		{
			get
			{
				return m_ShowCPUStats;
			}
			set
			{
				m_ShowCPUStats = value;
				base.enabled = GetIsEnabled();
			}
		}

		private void Start()
		{
			base.transform.position = new Vector3(0.025f, 0.975f, 0f);
			base.transform.parent = base.transform;
			GetComponent<GUIText>().fontSize = 12;
			GetComponent<GUIText>().font = Resources.Load("UBIGAME TEXT L BOLD 32") as Font;
			m_frameTime = new DebugGraph("Frame-Time", new Rect(10f, 150f, (float)Screen.width - 20f, 200f), Color.white, Color.yellow, 600, 2, true);
			m_frameTime.SetUnit("s");
			m_frameTime.SetDataSetColor(Color.red, 0);
			m_frameTime.SetDataSetColor(Color.green, 1);
			m_gcCollectionCount = new DebugGraph("GC Collections", new Rect(10f, 360f, (float)Screen.width - 20f, 100f), Color.white, Color.yellow, 600, GC.MaxGeneration + 1);
			m_gcCollectionCount.SetUnit("#");
			m_gcCollectionCount.SetDataSetColor(Color.green, 0);
			for (int i = 0; i < GC.MaxGeneration + 1; i++)
			{
				m_gcLastCollectionCount[i] = GC.CollectionCount(i);
			}
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				m_freeMemoryPages = new DebugGraph("Active/Free Memory Pages", new Rect(10f, 470f, (float)Screen.width - 20f, 100f), Color.white, Color.yellow, 0f, 150000f, 600, 2);
				m_freeMemoryPages.SetDataSetColor(Color.red, 0);
				m_freeMemoryPages.SetDataSetColor(Color.green, 1);
				m_freeMemoryBytes = new DebugGraph("Free Memory", new Rect(10f, 580f, (float)Screen.width - 20f, 100f), Color.white, Color.yellow, 0f, 500f, 600);
				m_freeMemoryBytes.SetUnit("MB");
				m_usedMemoryBytes = new DebugGraph("Used Memory", new Rect(10f, 690f, (float)Screen.width - 20f, 100f), Color.white, Color.yellow, 0f, 500f, 600);
				m_usedMemoryBytes.SetUnit("MB");
				m_cpuUsage = new DebugGraph("CPU Usage %", new Rect(10f, 580f, (float)Screen.width - 20f, 100f), Color.white, Color.yellow, 0f, 100f, 600);
			}
		}

		private void OnGUI()
		{
			GUI.depth = -1;
			m_currentGraphOffsetY = m_initialGraphOffset;
			OnRenderFrameTimeGraph();
			OnRenderFreeMemoryPagesGraph();
			OnRenderFreeMemoryBytesGraph();
			OnRenderUsedMemoryBytesGraph();
			OnRenderCPUStatsGraph();
			OnRenderGCStats();
		}

		private bool GetIsEnabled()
		{
			return m_ShowFrameTimeGraph || m_ShowGCStats || m_ShowFreeMemoryPages || m_ShowCPUStats || m_ShowFreeMemoryBytes || m_ShowUsedMemoryBytes;
		}

		private void OnRenderCPUStatsGraph()
		{
			if (ShowCPUStats && Event.current.type == EventType.Repaint)
			{
				m_cpuUsage.SetY(m_currentGraphOffsetY);
				m_currentGraphOffsetY += m_cpuUsage.GetHeight() + m_marginBetweenGraphs;
				m_cpuUsage.OnAddDataPoint(BBProfilerBehavior.GetCPUUsage());
				m_cpuUsage.OnRender();
			}
		}

		private void OnRenderFrameTimeGraph()
		{
			if (!ShowFrameTimeGraph || Event.current.type != EventType.Repaint)
			{
				return;
			}
			string annotation = null;
			m_frameTime.SetY(m_currentGraphOffsetY);
			m_currentGraphOffsetY += m_frameTime.GetHeight() + m_marginBetweenGraphs;
			if (m_previousRealDeltaTime != 0f)
			{
				float num = (UbiCore.Utils.Singleton<Timer>.Instance.realDeltaTime - m_previousRealDeltaTime) / ((UbiCore.Utils.Singleton<Timer>.Instance.realDeltaTime + m_previousRealDeltaTime) / 2f);
				if (!(num >= 0.3f))
				{
				}
			}
			m_previousRealDeltaTime = UbiCore.Utils.Singleton<Timer>.Instance.realDeltaTime;
			m_frameTime.ShowAnnotations = ShowFrameTimeGraphAnnotations;
			m_frameTime.OnAddDataPoint(UbiCore.Utils.Singleton<Timer>.Instance.realDeltaTime, 0, annotation);
			m_frameTime.OnAddDataPoint(UbiCore.Utils.Singleton<Timer>.Instance.deltaTime, 1);
			m_frameTime.OnRender();
		}

		private void OnRenderFreeMemoryPagesGraph()
		{
			if (ShowFreeMemoryPages && Event.current.type == EventType.Repaint)
			{
				m_freeMemoryPages.SetY(m_currentGraphOffsetY);
				m_currentGraphOffsetY += m_freeMemoryPages.GetHeight() + m_marginBetweenGraphs;
				m_freeMemoryPages.OnAddDataPoint(BBProfilerBehavior.GetActiveMemoryPages());
				m_freeMemoryPages.OnAddDataPoint(BBProfilerBehavior.GetFreeMemoryPages(), 1);
				m_freeMemoryPages.OnRender();
			}
		}

		private void OnRenderFreeMemoryBytesGraph()
		{
			if (ShowFreeMemoryBytes && Event.current.type == EventType.Repaint)
			{
				m_freeMemoryBytes.SetY(m_currentGraphOffsetY);
				m_currentGraphOffsetY += m_freeMemoryBytes.GetHeight() + m_marginBetweenGraphs;
				m_freeMemoryBytes.OnAddDataPoint((float)BBProfilerBehavior.GetFreeMemoryBytes() / 1024f / 1024f);
				m_freeMemoryBytes.OnRender();
			}
		}

		private void OnRenderUsedMemoryBytesGraph()
		{
			if (ShowUsedMemoryBytes && Event.current.type == EventType.Repaint)
			{
				m_usedMemoryBytes.SetY(m_currentGraphOffsetY);
				m_currentGraphOffsetY += m_usedMemoryBytes.GetHeight() + m_marginBetweenGraphs;
				m_usedMemoryBytes.OnAddDataPoint((float)BBProfilerBehavior.GetUsedMemoryBytes() / 1024f / 1024f);
				m_usedMemoryBytes.OnRender();
			}
		}

		private void OnRenderGCStats()
		{
			if (ShowGCStats && Event.current.type == EventType.Repaint)
			{
				m_gcCollectionCount.SetY(m_currentGraphOffsetY);
				m_currentGraphOffsetY += m_gcCollectionCount.GetHeight() + m_marginBetweenGraphs;
				for (int i = 0; i < GC.MaxGeneration + 1; i++)
				{
					int num = GC.CollectionCount(i);
					m_gcCollectionCount.OnAddDataPoint(num - m_gcLastCollectionCount[i], i);
					m_gcLastCollectionCount[i] = num;
				}
				m_gcCollectionCount.OnRender();
			}
		}
	}
}
