using System;
using UnityEngine;

public static class MissionAreaPlotter
{
	public static int TextureWidth = 512;

	public static Texture2D CreateAreaOverlay(Vector3[] _areaNodes, UIMapData _mapData, Color _fillColor, Color _otherColor, int _lineShift, int _lineWidth)
	{
		Texture2D texture2D = CreateNewOverlay();
		PlotArea(texture2D, _mapData, _areaNodes, _lineShift, _lineWidth, _fillColor, _otherColor, true);
		return texture2D;
	}

	public static Texture2D AddAreaOverlay(Texture2D _overlay, Vector3[] _areaNodes, UIMapData _mapData, Color _fillColor, Color _otherColor, int _lineShift, int _lineWidth, bool _clearTarget)
	{
		PlotArea(_overlay, _mapData, _areaNodes, _lineShift, _lineWidth, _fillColor, _otherColor, _clearTarget);
		return _overlay;
	}

	private static Color[] Colors(Color _fillColor, Color _fillColorOther, int _blockWidth, int _pixelX, int _pixelY, int _lineShift, int _lineWidth)
	{
		Color[] array = new Color[_blockWidth];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (((i + _pixelX + _pixelY / _lineShift) / _lineWidth % 2 != 0) ? _fillColorOther : _fillColor);
		}
		return array;
	}

	private static Texture2D CreateNewOverlay()
	{
		Texture2D texture2D = new Texture2D(TextureWidth, TextureWidth, TextureFormat.ARGB32, false);
		Clear(texture2D);
		return texture2D;
	}

	public static void Clear(Texture2D _overlay)
	{
		Color[] array = new Color[_overlay.width];
		Color color = new Color(0f, 0f, 0f, 0f);
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = color;
		}
		for (int j = 0; j < _overlay.height; j++)
		{
			_overlay.SetPixels(0, j, _overlay.width, 1, array);
		}
		_overlay.Apply();
	}

	private static void Raster(Vector2[] _poly, Texture2D _overlay, Color _fillColor, Color _otherColor, int _lineShift, int _lineWidth)
	{
		int height = _overlay.height;
		int width = _overlay.width;
		int num = _poly.Length;
		int[] array = new int[num];
		for (int i = 0; i < height; i++)
		{
			int num2 = 0;
			int num3 = num - 1;
			for (int j = 0; j < num; j++)
			{
				if (((double)_poly[j].y < (double)i && (double)_poly[num3].y >= (double)i) || ((double)_poly[num3].y < (double)i && (double)_poly[j].y >= (double)i))
				{
					array[num2++] = (int)(_poly[j].x + ((float)i - _poly[j].y) / (_poly[num3].y - _poly[j].y) * (_poly[num3].x - _poly[j].x));
				}
				num3 = j;
			}
			Array.Sort(array, 0, num2);
			for (int j = 0; j < num2 && array[j] < width; j += 2)
			{
				if (array[j + 1] > 0)
				{
					if (array[j] < 0)
					{
						array[j] = 0;
					}
					if (array[j + 1] > width)
					{
						array[j + 1] = width;
					}
					int blockWidth = array[j + 1] - array[j];
					Color[] colors = Colors(_fillColor, _otherColor, blockWidth, array[j], i, _lineShift, _lineWidth);
					_overlay.SetPixels(array[j], i, blockWidth, 1, colors);
				}
			}
		}
		_overlay.Apply(false, false);
	}

	public static void PlotArea(Texture2D _overlay, UIMapData _mapData, Vector3[] _areaNodes, int _lineShift, int _lineWidth, Color _fillColor, Color _otherFillColor, bool _clearAtStart = false)
	{
		Vector2[] poly = ComputeTextureCorners(_overlay, _mapData, _areaNodes);
		if (_clearAtStart)
		{
			Color[] colors = new Color[_overlay.width];
			for (int i = 0; i < _overlay.height; i++)
			{
				_overlay.SetPixels(0, i, _overlay.width, 1, colors);
			}
		}
		Raster(poly, _overlay, _fillColor, _otherFillColor, _lineShift, _lineWidth);
	}

	private static Vector2[] ComputeTextureCorners(Texture2D _overlay, UIMapData _mapData, Vector3[] _areaNodes)
	{
		Vector2[] array = new Vector2[_areaNodes.Length];
		Vector2 textureSize = new Vector2(_overlay.width, _overlay.height);
		for (int i = 0; i < _areaNodes.Length; i++)
		{
			Vector3 position = _areaNodes[i];
			array[i] = WorldToPixelPosition(_mapData, position, textureSize);
		}
		return array;
	}

	public static Vector2 WorldToPixelPosition(UIMapData _mapData, Vector3 _position, Vector2 _textureSize)
	{
		Vector2 vector = new Vector2(Mathf.Abs(_mapData.m_endPos.x - _mapData.m_startPos.x) / _textureSize.x, Mathf.Abs(_mapData.m_endPos.y - _mapData.m_startPos.y) / _textureSize.y);
		return new Vector2((_position.x - _mapData.m_startPos.x) / vector.x, (_position.z - _mapData.m_startPos.y) / vector.y);
	}

	public static Vector3 PixelToWorldPosition(UIMapData _mapData, Vector2 _position, Vector2 _textureSize)
	{
		Vector3 vector = new Vector2(Mathf.Abs(_mapData.m_endPos.x - _mapData.m_startPos.x) / _textureSize.x, Mathf.Abs(_mapData.m_endPos.y - _mapData.m_startPos.y) / _textureSize.y);
		return new Vector3(_position.x * vector.x + _mapData.m_startPos.x, 0f, _position.y * vector.y + _mapData.m_startPos.y);
	}
}
