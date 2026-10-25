using System;
using UnityEngine;

[Serializable]
public class BenchValues
{
	[SerializeField]
	public Vector3 benchCenterOffset = new Vector3(-0.25f, 0f, 0f);

	public void CopyFrom(BenchValues _other)
	{
		benchCenterOffset = _other.benchCenterOffset;
	}
}
