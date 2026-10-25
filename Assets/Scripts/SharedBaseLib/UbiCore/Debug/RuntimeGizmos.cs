using System;
using UnityEngine;

namespace UbiCore.Debug
{
	public class RuntimeGizmos
	{
		private static Texture2D m_whiteTexture;

		private static Material m_material;

		private const float TWOPI = (float)Math.PI * 2f;

		public static void DrawWireCube(Vector3 _position, float _size, Color _color)
		{
			InitMaterial();
			GL.PushMatrix();
			GL.MultMatrix(Matrix4x4.TRS(_position, Quaternion.identity, Vector3.one));
			SetMaterial();
			GL.Begin(1);
			GL.Color(_color);
			float num = _size * 0.5f;
			GL.Vertex3(0f - num, 0f - num, 0f - num);
			GL.Vertex3(num, 0f - num, 0f - num);
			GL.Vertex3(0f - num, num, 0f - num);
			GL.Vertex3(num, num, 0f - num);
			GL.Vertex3(0f - num, num, num);
			GL.Vertex3(num, num, num);
			GL.Vertex3(0f - num, 0f - num, num);
			GL.Vertex3(num, 0f - num, num);
			GL.Vertex3(0f - num, 0f - num, 0f - num);
			GL.Vertex3(0f - num, num, 0f - num);
			GL.Vertex3(num, 0f - num, 0f - num);
			GL.Vertex3(num, num, 0f - num);
			GL.Vertex3(num, 0f - num, num);
			GL.Vertex3(num, num, num);
			GL.Vertex3(0f - num, 0f - num, num);
			GL.Vertex3(0f - num, num, num);
			GL.Vertex3(0f - num, 0f - num, 0f - num);
			GL.Vertex3(0f - num, 0f - num, num);
			GL.Vertex3(num, 0f - num, 0f - num);
			GL.Vertex3(num, 0f - num, num);
			GL.Vertex3(num, num, 0f - num);
			GL.Vertex3(num, num, num);
			GL.Vertex3(0f - num, num, 0f - num);
			GL.Vertex3(0f - num, num, num);
			GL.End();
			GL.PopMatrix();
		}

		public static void DrawWireCircle(Vector3 _position, float _radius, Color _color)
		{
			InitMaterial();
			GL.PushMatrix();
			GL.MultMatrix(Matrix4x4.TRS(_position, Quaternion.identity, Vector3.one));
			SetMaterial();
			LineCircle(_radius, _color);
			GL.PopMatrix();
		}

		public static void DrawWireSphere(Vector3 _position, float _radius, Color _color)
		{
			InitMaterial();
			SetMaterial();
			GL.PushMatrix();
			GL.MultMatrix(Matrix4x4.TRS(_position, Quaternion.identity, Vector3.one));
			LineCircle(_radius, _color);
			GL.PopMatrix();
			GL.PushMatrix();
			GL.MultMatrix(Matrix4x4.TRS(_position, Quaternion.LookRotation(Vector3.up, Vector3.right), Vector3.one));
			LineCircle(_radius, _color);
			GL.PopMatrix();
			GL.PushMatrix();
			GL.MultMatrix(Matrix4x4.TRS(_position, Quaternion.LookRotation(Vector3.right, Vector3.forward), Vector3.one));
			LineCircle(_radius, _color);
			GL.PopMatrix();
			GL.PushMatrix();
			Vector3 normal = Camera.current.transform.position - _position;
			Vector3 tangent = ((normal.y < 0.95f) ? Vector3.up : Vector3.right);
			Vector3.OrthoNormalize(ref normal, ref tangent);
			GL.MultMatrix(Matrix4x4.TRS(_position, Quaternion.LookRotation(tangent, normal), Vector3.one));
			LineCircle(_radius, _color);
			GL.PopMatrix();
		}

		public static void DrawLine(Vector3 _from, Vector3 _to, Color _color)
		{
			InitMaterial();
			SetMaterial();
			GL.Begin(1);
			GL.Color(_color);
			GL.Vertex(_from);
			GL.Vertex(_to);
			GL.End();
		}

		public static void DrawSphere(Vector3 _position, float _radius, Color _color)
		{
			InitMaterial();
			GL.PushMatrix();
			GL.MultMatrix(Matrix4x4.TRS(_position, Quaternion.identity, Vector3.one));
			SetMaterial();
			GL.Begin(4);
			GL.Color(_color);
			float num = 0f;
			float num2 = 1f;
			for (int i = 0; i <= 8; i++)
			{
				float f = (float)Math.PI * (float)i / 8f;
				float num3 = Mathf.Sin(f);
				float num4 = Mathf.Cos(f);
				float num5 = 0f;
				float num6 = 1f;
				for (int j = 0; j <= 16; j++)
				{
					float f2 = (float)Math.PI * 2f * (float)j / 16f;
					float num7 = Mathf.Sin(f2);
					float num8 = Mathf.Cos(f2);
					GL.Vertex3(num * num6 * _radius, num2 * _radius, num * num5 * _radius);
					GL.Vertex3(num * num8 * _radius, num2 * _radius, num * num7 * _radius);
					GL.Vertex3(num3 * num6 * _radius, num4 * _radius, num3 * num5 * _radius);
					GL.Vertex3(num3 * num6 * _radius, num4 * _radius, num3 * num5 * _radius);
					GL.Vertex3(num * num8 * _radius, num2 * _radius, num * num7 * _radius);
					GL.Vertex3(num3 * num8 * _radius, num4 * _radius, num3 * num7 * _radius);
					num5 = num7;
					num6 = num8;
				}
				num = num3;
				num2 = num4;
			}
			GL.End();
			GL.PopMatrix();
		}

		public static void DrawQuad(Vector3 _position, float _size, Color _color)
		{
			InitMaterial();
			GL.PushMatrix();
			GL.MultMatrix(Matrix4x4.TRS(_position, Quaternion.identity, Vector3.one));
			SetMaterial();
			GL.Begin(4);
			GL.Color(_color);
			float num = _size * 0.5f;
			GL.Vertex3(0f - num, 0f, num);
			GL.Vertex3(num, 0f, num);
			GL.Vertex3(num, 0f, 0f - num);
			GL.Vertex3(0f - num, 0f, num);
			GL.Vertex3(num, 0f, 0f - num);
			GL.Vertex3(0f - num, 0f, 0f - num);
			GL.End();
			GL.PopMatrix();
		}

		public static void DrawQuad(Vector3 _position, float _size, Texture2D _texture, Color _color)
		{
			InitMaterial(_texture);
			GL.PushMatrix();
			GL.MultMatrix(Matrix4x4.TRS(_position, Quaternion.identity, Vector3.one));
			SetMaterial();
			GL.Begin(4);
			GL.Color(_color);
			float num = _size * 0.5f;
			GL.TexCoord2(0f, 1f);
			GL.Vertex3(0f - num, 0f, num);
			GL.TexCoord2(1f, 1f);
			GL.Vertex3(num, 0f, num);
			GL.TexCoord2(1f, 0f);
			GL.Vertex3(num, 0f, 0f - num);
			GL.TexCoord2(0f, 1f);
			GL.Vertex3(0f - num, 0f, num);
			GL.TexCoord2(1f, 0f);
			GL.Vertex3(num, 0f, 0f - num);
			GL.TexCoord2(0f, 0f);
			GL.Vertex3(0f - num, 0f, 0f - num);
			GL.End();
			GL.PopMatrix();
		}

		public static void DrawQuad(Vector3 _position, float _sizeX, float _sizeY, Texture2D _texture, Color _color)
		{
			InitMaterial(_texture);
			GL.PushMatrix();
			GL.MultMatrix(Matrix4x4.TRS(_position, Quaternion.identity, Vector3.one));
			SetMaterial();
			GL.Begin(4);
			GL.Color(_color);
			GL.TexCoord2(0f, 1f);
			GL.Vertex3(0f, 0f, _sizeY);
			GL.TexCoord2(1f, 1f);
			GL.Vertex3(_sizeX, 0f, _sizeY);
			GL.TexCoord2(1f, 0f);
			GL.Vertex3(_sizeX, 0f, 0f);
			GL.TexCoord2(0f, 1f);
			GL.Vertex3(0f, 0f, _sizeY);
			GL.TexCoord2(1f, 0f);
			GL.Vertex3(_sizeX, 0f, 0f);
			GL.TexCoord2(0f, 0f);
			GL.Vertex3(0f, 0f, 0f);
			GL.End();
			GL.PopMatrix();
		}

		public static void DrawQuadGUI(Rect _rect)
		{
			InitTexture();
			GUI.DrawTexture(_rect, m_whiteTexture, ScaleMode.StretchToFill, true);
		}

		private static void InitMaterial(Texture2D _texture = null)
		{
			if (m_material == null)
			{
				m_material = new Material(Shader.Find("Effects/Alpha"));
				m_material.hideFlags = HideFlags.HideAndDontSave;
				m_material.shader.hideFlags = HideFlags.HideAndDontSave;
			}
			m_material.SetTexture("_MainTex", _texture);
		}

		private static void SetMaterial()
		{
			m_material.SetPass(0);
		}

		private static void InitTexture()
		{
			if (m_whiteTexture == null)
			{
				m_whiteTexture = new Texture2D(1, 1);
				m_whiteTexture.SetPixel(0, 0, Color.white);
				m_whiteTexture.Apply();
			}
		}

		private static void LineCircle(float _radius, Color _color)
		{
			GL.Begin(1);
			GL.Color(_color);
			Vector3 v = new Vector3(_radius, 0f, 0f);
			for (int i = 0; i <= 32; i++)
			{
				GL.Vertex(v);
				float f = (float)Math.PI * 2f * (float)i / 32f;
				v = new Vector3(Mathf.Cos(f) * _radius, 0f, Mathf.Sin(f) * _radius);
				GL.Vertex(v);
			}
			GL.End();
		}
	}
}
