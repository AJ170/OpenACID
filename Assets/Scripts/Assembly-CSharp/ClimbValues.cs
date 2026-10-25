using System;
using UnityEngine;

[Serializable]
public class ClimbValues
{
	[SerializeField]
	public float maxUserYawAngle = 60f;

	public void CopyFrom(ClimbValues _other)
	{
		maxUserYawAngle = _other.maxUserYawAngle;
	}
}
