using System;
using UnityEngine;

public class CameraSpline : MonoBehaviour
{
	[Serializable]
	public class Keyframe
	{
		[SerializeField]
		public float time;

		[SerializeField]
		public Vector3 position;

		public Keyframe(float _time, Vector3 _position)
		{
			time = _time;
			position = _position;
		}
	}

	[SerializeField]
	public Keyframe[] m_keys = new Keyframe[2]
	{
		new Keyframe(0f, Vector3.zero),
		new Keyframe(1f, Vector3.zero)
	};

	private CubicSpline[] m_splines = new CubicSpline[3]
	{
		new CubicSpline(),
		new CubicSpline(),
		new CubicSpline()
	};

	private bool m_isInitialized;

	private float[] m_ts;

	private float[] m_xs;

	private float[] m_ys;

	private float[] m_zs;

	private void Fit()
	{
		if (m_keys.Length == 0)
		{
			m_splines[0].Fit(new float[2] { 0f, 1f }, new float[2]);
			m_splines[1].Fit(new float[2] { 0f, 1f }, new float[2]);
			m_splines[2].Fit(new float[2] { 0f, 1f }, new float[2]);
		}
		else if (m_keys.Length == 1)
		{
			m_splines[0].Fit(new float[2] { 0f, 1f }, new float[2]
			{
				m_keys[0].position.x,
				m_keys[0].position.x
			});
			m_splines[1].Fit(new float[2] { 0f, 1f }, new float[2]
			{
				m_keys[0].position.y,
				m_keys[0].position.y
			});
			m_splines[2].Fit(new float[2] { 0f, 1f }, new float[2]
			{
				m_keys[0].position.z,
				m_keys[0].position.z
			});
		}
		else
		{
			CheckArray(ref m_ts);
			CheckArray(ref m_xs);
			CheckArray(ref m_ys);
			CheckArray(ref m_zs);
			for (int i = 0; i < m_keys.Length; i++)
			{
				m_ts[i] = m_keys[i].time;
				m_xs[i] = m_keys[i].position.x;
				m_ys[i] = m_keys[i].position.y;
				m_zs[i] = m_keys[i].position.z;
			}
			m_splines[0].Fit(m_ts, m_xs);
			m_splines[1].Fit(m_ts, m_ys);
			m_splines[2].Fit(m_ts, m_zs);
		}
	}

	private void CheckArray(ref float[] _ar)
	{
		if (_ar == null || _ar.Length != m_keys.Length)
		{
			_ar = new float[m_keys.Length];
		}
	}

	public Vector3 Eval(float t)
	{
		if (Application.isEditor || !m_isInitialized)
		{
			Fit();
			m_isInitialized = true;
		}
		float num = m_splines[0].Eval(t);
		float num2 = m_splines[1].Eval(t);
		float num3 = m_splines[2].Eval(t);
		if (float.IsNaN(num))
		{
			num = 0f;
		}
		if (float.IsNaN(num2))
		{
			num2 = 0f;
		}
		if (float.IsNaN(num3))
		{
			num3 = 0f;
		}
		return new Vector3(num, num2, num3);
	}
}
