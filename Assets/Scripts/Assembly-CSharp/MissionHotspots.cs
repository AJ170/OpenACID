using Assets.Scripts.Engine;
using Assets.Scripts.Network.Missions.Mission.Data;
using SharedBaseLib.Core;
using UnityEngine;

public class MissionHotspots : MissionBase<MissionHotspotsData>
{
	public const string RootNodeBenches = "Benches";

	public const string RootNodeHaystacks = "Haystacks";

	public const string RootNodeRoofGardens = "Roof Gardens";

	public bool HotspotIsActive(HotspotMeta _meta)
	{
		if (Helpers.Verify(_meta != null, "Invalid usage - please pass a valid HotspotMeta object. Defaulting to \"IsActive=true\""))
		{
			BBSimpleList<MissionHotspotsData.Hotspot> bBSimpleList = null;
			switch (_meta.Type)
			{
			case eHotspotType.Haystack:
				bBSimpleList = base.Data.Haystacks;
				break;
			case eHotspotType.Bench:
				bBSimpleList = base.Data.Benches;
				break;
			case eHotspotType.RoofGarden:
				bBSimpleList = base.Data.RoofGardens;
				break;
			}
			if (bBSimpleList != null)
			{
				for (int i = 0; i < bBSimpleList.Count; i++)
				{
					if (bBSimpleList[i].UniqueId == _meta.UniqueID)
					{
						return bBSimpleList[i].Active;
					}
				}
			}
		}
		return true;
	}

	public bool HotspotIsActive(GameObject _go)
	{
		if (Helpers.Verify(_go != null, "Invalid usage - please pass a valid GameObject. Defaulting to \"IsActive=true\""))
		{
			return HotspotIsActive(_go.GetComponent<HotspotMeta>());
		}
		return true;
	}

	public override void OnPostRead(MissionBaseData _nodeData)
	{
		base.OnPostRead(_nodeData);
		ApplyHotspotStates("Haystacks");
		ApplyHotspotStates("Benches");
		ApplyHotspotStates("Roof Gardens");
	}

	private void ApplyHotspotStates(string _rootName)
	{
		GameObject gameObject = GameObject.Find(_rootName);
		if (!gameObject)
		{
			return;
		}
		HotspotMeta[] componentsInChildren = gameObject.GetComponentsInChildren<HotspotMeta>();
		if (componentsInChildren != null)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Active = HotspotIsActive(componentsInChildren[i]);
			}
		}
	}
}
