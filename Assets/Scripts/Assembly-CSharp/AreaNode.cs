using AcierProtocol;
using Assets.Scripts.Network.Missions.Mission.Data;
using UnityEngine;

public class AreaNode : MissionBase<AreaNodeData>
{
	public override void OnActivate()
	{
		base.OnActivate();
		base.Data.MapIcon = EMapIconType.Invalid_EMapIconType;
	}

	public void OnDrawGizmosSelected()
	{
		MissionMonoBehaviour[] components = base.transform.parent.GetComponents<MissionMonoBehaviour>();
		for (int i = 0; i < components.Length; i++)
		{
			MissionArea missionArea = components[i] as MissionArea;
			if (!(missionArea == null))
			{
				missionArea.OnDrawGizmosSelected();
			}
		}
	}

	public void DrawGizmos()
	{
		Gizmos.color = Color.red;
	}

	public override string ToString()
	{
		return base.transform.position.ToString();
	}
}
