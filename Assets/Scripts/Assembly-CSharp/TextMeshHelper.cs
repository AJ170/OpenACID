using System;
using UnityEngine;

public class TextMeshHelper : MonoBehaviour
{
	public TextMesh TextMeshDisplay;

	public int MaxCharPerLine;

	public float OffsetPerLine = 0.06f;

	public float MinLineHeight = 0.08f;

	public Color TextColor = Color.white;

	private float m_oldCartesianHeight;

	private float m_oldBoundsHeight;

	public void SetText(string _text)
	{
		TextMeshDisplay.text = FormatString(_text);
	}

	public float GetCartesianHeight()
	{
		Renderer component = TextMeshDisplay.GetComponent<Renderer>();
		float y = component.bounds.size.y;
		if ((double)Math.Abs(m_oldBoundsHeight - y) > 0.0001)
		{
			string text = TextMeshDisplay.text;
			TextMeshDisplay.text = "qÛ";
			float y2 = component.bounds.size.y;
			TextMeshDisplay.text = text;
			int num = (int)Math.Floor(y / y2 + 0.5f);
			m_oldBoundsHeight = y;
			m_oldCartesianHeight = Math.Max(OffsetPerLine * (float)num, MinLineHeight);
		}
		return m_oldCartesianHeight;
	}

	private string FormatString(string _text)
	{
		if (MaxCharPerLine <= 0)
		{
			return _text;
		}
		string text = "\r\n";
		string text2 = string.Empty;
		int num = 0;
		string[] array = _text.Split(' ');
		for (int i = 0; i < array.Length; i++)
		{
			bool flag = true;
			string text3 = array[i];
			num += text3.Length + 1;
			if (num <= MaxCharPerLine)
			{
				if (text3.EndsWith(text))
				{
					num = 0;
				}
				if (!text3.StartsWith(text))
				{
					text2 = text2 + " " + text3;
					flag = false;
				}
			}
			if (flag)
			{
				text2.TrimEnd(text.ToCharArray());
				if (!text3.StartsWith(text))
				{
					text2 += text;
				}
				text2 += text3;
				num = text3.Length;
			}
		}
		if (string.IsNullOrEmpty(text2))
		{
			return _text;
		}
		return text2.Trim();
	}

	public void Update()
	{
		if (!(TextMeshDisplay == null) && !(TextMeshDisplay.color == TextColor))
		{
			TextMeshDisplay.color = TextColor;
		}
	}
}
