using UnityEngine;

public class DebugHelper
{
	private static GUIStyle s_style = new GUIStyle();

	private static Rect s_rc;

	private static Rect s_tmp;

	public static void DrawOutlineLabel(Rect _rc, string _text)
	{
		DrawOutlineLabel(_rc, _text, Color.white, Color.black);
	}

	public static void DrawOutlineLabel(Rect _rc, string _text, Color _innerColor, Color _outerColor)
	{
		s_style.normal.textColor = _outerColor;
		float xMin = _rc.xMin;
		float yMin = _rc.yMin;
		for (int i = -1; i < 2; i++)
		{
			for (int j = -1; j < 2; j++)
			{
				if (i != 0 && j != 0)
				{
					s_tmp.x = xMin + (float)i;
					s_tmp.y = yMin + (float)j;
					s_tmp.width = _rc.width;
					s_tmp.height = _rc.height;
					GUI.Label(s_tmp, _text, s_style);
				}
			}
		}
		s_style.normal.textColor = _innerColor;
		s_style.fontSize = 30;
		s_rc.x = xMin;
		s_rc.y = yMin;
		s_rc.width = _rc.width;
		s_rc.height = _rc.height;
		GUI.Label(s_rc, _text, s_style);
	}

	public static void DrawSimpleLabel(Rect _rc, string _text)
	{
		GUI.Label(_rc, _text, s_style);
	}
}
