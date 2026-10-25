using System;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

public class DebugGraph
{
	private struct DataPoint
	{
		public float value;

		public int frame;

		public string annotation;

		public int annotationOffset;
	}

	private string m_title;

	private Rect m_areaScreen;

	private Rect m_areaNormalized;

	private Color m_scaleColor;

	private Color m_bgColor = new Color(0f, 0f, 0f, 0.5f);

	private BBRingBuffer<DataPoint>[] m_dataBuffer;

	private Color[] m_dataSetColors;

	private float m_scaleMin;

	private float m_scaleMax = 1f;

	private float m_zeroLine;

	private float m_valueFactor = 1f;

	private bool m_autoRescale;

	private bool m_keepExtremes;

	private Vector2 m_labelSize;

	private string m_unit = string.Empty;

	private float[] m_lastValueAdded;

	private int m_annotationOffset;

	private int m_maxAnnotationRows = 1;

	private static Material s_Material;

	public bool ShowAnnotations { get; set; }

	public DebugGraph(string _title, Rect _areaScreen, Color _scaleColor, Color _dataColor, float _scaleMin, float _scaleMax, int _dataPointCount = 300, int _dataSetCount = 1)
	{
		m_title = _title;
		m_scaleColor = _scaleColor;
		m_areaScreen = _areaScreen;
		m_scaleMin = _scaleMin;
		m_scaleMax = _scaleMax;
		m_maxAnnotationRows = (int)(m_areaScreen.height / 33.3f + 0.5f);
		m_dataBuffer = new BBRingBuffer<DataPoint>[_dataSetCount];
		for (int i = 0; i < _dataSetCount; i++)
		{
			m_dataBuffer[i] = new BBRingBuffer<DataPoint>(_dataPointCount);
		}
		m_dataSetColors = new Color[_dataSetCount];
		for (int j = 0; j < _dataSetCount; j++)
		{
			m_dataSetColors[j] = _dataColor;
		}
		m_lastValueAdded = new float[_dataSetCount];
		m_areaNormalized.x = _areaScreen.x / (float)Screen.width;
		m_areaNormalized.y = _areaScreen.y / (float)Screen.height;
		m_areaNormalized.width = _areaScreen.width / (float)Screen.width;
		m_areaNormalized.height = _areaScreen.height / (float)Screen.height;
		m_valueFactor = 1f / (m_scaleMax - m_scaleMin);
		m_zeroLine = (0f - m_scaleMin) * m_valueFactor;
		m_labelSize = GetLabelWidth(_title);
		m_labelSize.y = GetLabelLineHeight();
	}

	public DebugGraph(string _title, Rect _areaScreen, Color _scaleColor, Color _dataColor, int _dataPointCount = 300, int _dataSetCount = 1, bool _keepExtremes = false)
		: this(_title, _areaScreen, _scaleColor, _dataColor, 0f, 1f, _dataPointCount, _dataSetCount)
	{
		m_autoRescale = true;
		m_keepExtremes = _keepExtremes;
		if (_keepExtremes)
		{
			m_scaleMin = float.MaxValue;
			m_scaleMax = float.MinValue;
		}
	}

	public void SetUnit(string _unit)
	{
		m_unit = _unit.Trim();
		if (m_unit.Length > 0)
		{
			m_title = string.Format("{0} ({1})", m_title, m_unit);
			m_labelSize = GetLabelWidth(m_title);
		}
	}

	public void SetY(float _y)
	{
		m_areaScreen.y = _y;
		m_areaNormalized.y = m_areaScreen.y / (float)Screen.height;
	}

	public float GetHeight()
	{
		return m_areaScreen.height;
	}

	private Vector2 GetLabelWidth(string _text)
	{
		GUIStyle gUIStyle = "Label";
		return gUIStyle.CalcSize(new GUIContent(_text));
	}

	private float GetLabelLineHeight()
	{
		GUIStyle gUIStyle = "Label";
		return gUIStyle.lineHeight;
	}

	public void SetDataSetColor(Color _color, int _dataSetIndex)
	{
		m_dataSetColors[_dataSetIndex] = _color;
	}

	public void OnAddDataPoint(float _value, int _dataSetIndex = 0, string _annotation = null)
	{
		DataPoint value = default(DataPoint);
		value.value = _value;
		value.frame = UbiCore.Utils.Singleton<Timer>.Instance.frameCount;
		value.annotation = _annotation;
		value.annotationOffset = 0;
		if (_annotation != null)
		{
			value.annotationOffset = m_annotationOffset;
			m_annotationOffset++;
			m_annotationOffset %= m_maxAnnotationRows;
		}
		m_dataBuffer[_dataSetIndex].Add(value);
		m_lastValueAdded[_dataSetIndex] = _value;
	}

	private void OnUpdateCurrentScreenArea()
	{
		m_areaScreen.x = m_areaNormalized.x * (float)Screen.width;
		m_areaScreen.y = m_areaNormalized.y * (float)Screen.height;
		m_areaScreen.width = m_areaNormalized.width * (float)Screen.width;
		m_areaScreen.height = m_areaNormalized.height * (float)Screen.height;
	}

	private void OnApplyMaterial()
	{
		CreateLineMaterial();
		s_Material.SetPass(0);
	}

	private void OnInit2DRendering()
	{
		GL.PushMatrix();
		GL.LoadPixelMatrix();
	}

	private void OnDeInit2DRendering()
	{
		GL.PopMatrix();
	}

	private void OnRenderBackground()
	{
		GL.Begin(7);
		GL.Color(m_bgColor);
		GL.Vertex3(m_areaScreen.x + m_areaScreen.width, (float)Screen.height - (m_areaScreen.y + m_areaScreen.height), 0f);
		GL.Vertex3(m_areaScreen.x, (float)Screen.height - (m_areaScreen.y + m_areaScreen.height), 0f);
		GL.Vertex3(m_areaScreen.x, (float)Screen.height - m_areaScreen.y, 0f);
		GL.Vertex3(m_areaScreen.x + m_areaScreen.width, (float)Screen.height - m_areaScreen.y, 0f);
		GL.End();
	}

	private void OnRenderScale()
	{
		GL.Begin(1);
		GL.Color(m_scaleColor);
		GL.Vertex3(m_areaScreen.x, (float)Screen.height - m_areaScreen.y, 0f);
		GL.Vertex3(m_areaScreen.x, (float)Screen.height - (m_areaScreen.y + m_areaScreen.height), 0f);
		GL.Vertex3(m_areaScreen.x, (float)Screen.height - (m_areaScreen.y + m_areaScreen.height) - m_areaScreen.height * m_zeroLine, 0f);
		GL.Vertex3(m_areaScreen.x + m_areaScreen.width, (float)Screen.height - (m_areaScreen.y + m_areaScreen.height) - m_areaScreen.height * m_zeroLine, 0f);
		GL.End();
	}

	private void OnRenderDataPoints()
	{
		GL.Begin(1);
		for (int i = 0; i < m_dataBuffer.Length; i++)
		{
			GL.Color(m_dataSetColors[i]);
			int count = m_dataBuffer[i].Count;
			if (count <= 1)
			{
				continue;
			}
			float num = m_areaScreen.x;
			float num2 = m_areaScreen.width / (float)m_dataBuffer[i].Capacity;
			float num3 = 0f;
			float num4 = 0f;
			if (m_autoRescale)
			{
				if (!m_keepExtremes)
				{
					m_scaleMin = float.MaxValue;
					m_scaleMax = float.MinValue;
				}
				for (int j = 0; count > j; j++)
				{
					num3 = m_dataBuffer[i].GetValue(j).value;
					m_scaleMin = Math.Min(m_scaleMin, num3);
					m_scaleMax = Math.Max(m_scaleMax, num3);
				}
				m_scaleMin = Math.Min(m_scaleMin, 0f);
				m_scaleMax = Math.Max(m_scaleMax, 0f);
				if (m_scaleMax - m_scaleMin == 0f)
				{
					m_scaleMax += 0.1f;
				}
				m_valueFactor = 1f / (m_scaleMax - m_scaleMin);
				m_zeroLine = (0f - m_scaleMin) * m_valueFactor;
			}
			for (int k = 0; k < count; k++)
			{
				num3 = (m_dataBuffer[i].GetValue(k).value - m_scaleMin) * m_valueFactor;
				if (k > 0)
				{
					GL.Vertex3(num, (float)Screen.height - (m_areaScreen.y + m_areaScreen.height - m_areaScreen.height * num4), 0f);
					num += num2;
					GL.Vertex3(num, (float)Screen.height - (m_areaScreen.y + m_areaScreen.height - m_areaScreen.height * num3), 0f);
				}
				num4 = num3;
			}
		}
		GL.End();
	}

	private void OnRenderAnnotationMarkers()
	{
		if (!ShowAnnotations)
		{
			return;
		}
		GL.Begin(1);
		GL.Color(Color.white);
		for (int i = 0; i < m_dataBuffer.Length; i++)
		{
			float num = m_areaScreen.x;
			float num2 = m_areaScreen.width / (float)m_dataBuffer[i].Capacity;
			for (int j = 0; j < m_dataBuffer[i].Count; j++)
			{
				DataPoint value = m_dataBuffer[i].GetValue(j);
				if (value.annotation != null)
				{
					float num3 = (float)value.annotationOffset * 30f;
					float x = (int)(num + 0.5f);
					GL.Vertex3(x, (float)Screen.height - (m_areaScreen.y + num3), 0f);
					GL.Vertex3(x, (float)Screen.height - (m_areaScreen.y + num3 + 10f), 0f);
				}
				num += num2;
			}
		}
		GL.End();
	}

	private void OnRenderAnnotations()
	{
		if (!ShowAnnotations)
		{
			return;
		}
		for (int i = 0; i < m_dataBuffer.Length; i++)
		{
			float num = m_areaScreen.x;
			float num2 = m_areaScreen.width / (float)m_dataBuffer[i].Capacity;
			for (int j = 0; j < m_dataBuffer[i].Count; j++)
			{
				DataPoint value = m_dataBuffer[i].GetValue(j);
				if (value.annotation != null)
				{
					Rect areaScreen = m_areaScreen;
					areaScreen.x = num + 2f;
					areaScreen.y += (float)value.annotationOffset * 30f;
					DebugHelper.DrawSimpleLabel(areaScreen, value.annotation);
				}
				num += num2;
			}
		}
	}

	private void OnDrawTitle()
	{
		Rect areaScreen = m_areaScreen;
		areaScreen.x = areaScreen.x + areaScreen.width - 2f - m_labelSize.x;
		areaScreen.y += 2f;
		DebugHelper.DrawOutlineLabel(areaScreen, m_title);
	}

	private void OnDrawLastValuesAdded()
	{
		Rect areaScreen = m_areaScreen;
		areaScreen.x = areaScreen.x + areaScreen.width - 2f - m_labelSize.x;
		areaScreen.y += GetLabelLineHeight();
		for (int i = 0; i < m_lastValueAdded.Length; i++)
		{
			DebugHelper.DrawOutlineLabel(areaScreen, string.Format("{0:0.00}", m_lastValueAdded[i]), m_dataSetColors[i], Color.black);
			areaScreen.y += GetLabelLineHeight();
		}
	}

	private void OnDrawMinMaxScaleValues()
	{
		Rect areaScreen = m_areaScreen;
		areaScreen.x += 3f;
		DebugHelper.DrawOutlineLabel(areaScreen, string.Format("{0:0.00} {1}", m_scaleMax, m_unit));
		areaScreen.y = areaScreen.y + areaScreen.height - 15f;
		DebugHelper.DrawOutlineLabel(areaScreen, string.Format("{0:0.00} {1}", m_scaleMin, m_unit));
	}

	private static void CreateLineMaterial()
	{
		if (s_Material == null)
		{
			s_Material = new Material("Shader \"Lines/Colored Blended\" {SubShader { Pass {     Blend SrcAlpha OneMinusSrcAlpha     ZWrite Off Cull Off Fog { Mode Off }     BindChannels {      Bind \"vertex\", vertex Bind \"color\", color }} } }");
			s_Material.hideFlags = HideFlags.HideAndDontSave;
			s_Material.shader.hideFlags = HideFlags.HideAndDontSave;
		}
	}

	public void OnRender()
	{
		OnUpdateCurrentScreenArea();
		OnApplyMaterial();
		OnInit2DRendering();
		OnRenderBackground();
		OnRenderScale();
		OnRenderDataPoints();
		OnRenderAnnotationMarkers();
		OnDeInit2DRendering();
		OnRenderAnnotations();
		OnDrawTitle();
		OnDrawLastValuesAdded();
		OnDrawMinMaxScaleValues();
	}
}
