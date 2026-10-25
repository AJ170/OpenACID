using Assets.Scripts.Network.Missions.Mission.Data;
using UnityEngine;

public class MissionTarget : MissionBase<MissionTargetData>
{
	public Vector3 Position
	{
		get
		{
			return base.transform.position;
		}
	}

	public new static void Reset()
	{
	}

	public void OnDrawGizmos()
	{
		Color green = Color.green;
		green.a = 0.25f;
		Gizmos.color = green;
		Gizmos.DrawSphere(base.transform.position, base.Data.Range);
	}
}
