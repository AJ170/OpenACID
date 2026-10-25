using System.Collections;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

public class Billboard : MonoBehaviour
{
	public enum eUpdateMode
	{
		Always = 0,
		Visible = 1
	}

	public enum eRotationAxis
	{
		Free = 0,
		X = 1,
		Y = 2,
		Z = 3
	}

	[SerializeField]
	private Transform m_Target;

	[SerializeField]
	private eRotationAxis m_Axis;

	[SerializeField]
	private eUpdateMode m_UpdateMode;

	[SerializeField]
	private int m_fps = 30;

	public Transform Target
	{
		get
		{
			return m_Target;
		}
		set
		{
			m_Target = value;
		}
	}

	public eRotationAxis Axis
	{
		get
		{
			return m_Axis;
		}
		set
		{
			m_Axis = value;
		}
	}

	public eUpdateMode UpdateMode
	{
		get
		{
			return m_UpdateMode;
		}
		set
		{
			m_UpdateMode = value;
		}
	}

	public int Fps
	{
		get
		{
			return m_fps;
		}
		set
		{
			m_fps = value;
		}
	}

	private void OnEnable()
	{
	}

	private void Start()
	{
		StartCoroutine(Co_UpdateRotation());
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	private IEnumerator Co_UpdateRotation()
	{
		while (true)
		{
			if (m_UpdateMode == eUpdateMode.Always || GetComponent<Renderer>().isVisible)
			{
				base.transform.ConstrainedLookAt((!(m_Target != null)) ? Camera.main.transform : m_Target, MapAxis(m_Axis));
			}
			yield return StartCoroutine(Timer.WaitForSecondsUnscaled(1 / m_fps));
		}
	}

	private RotationAxis MapAxis(eRotationAxis _axis)
	{
		switch (_axis)
		{
		case eRotationAxis.X:
			return RotationAxis.XAxis;
		case eRotationAxis.Y:
			return RotationAxis.YAxis;
		case eRotationAxis.Z:
			return RotationAxis.ZAxis;
		case eRotationAxis.Free:
			return RotationAxis.All;
		default:
			return RotationAxis.All;
		}
	}
}
