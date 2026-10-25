using System;
using UnityEngine;

[Serializable]
public class RoofEdgeValues
{
	[SerializeField]
	public float maxUserYawAngle;

	public void CopyFrom(RoofEdgeValues _other)
	{
		maxUserYawAngle = _other.maxUserYawAngle;
	}
}
