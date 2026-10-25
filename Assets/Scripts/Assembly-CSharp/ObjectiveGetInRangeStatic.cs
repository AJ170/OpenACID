using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Mission;
using Assets.Scripts.Network.Missions.Mission.Data;
using UbiCore.Utils;
using UnityEngine;

public sealed class ObjectiveGetInRangeStatic : ObjectiveGetInRange<ObjectiveGetInRangeStaticData>
{
	[StaticObjectTypeGuid]
	public int Type;

	public MissionTarget Target;

	private HighlightBehaviour m_portalAsset;

	private StaticObjectData.IStaticObjectDefinition m_entityDefinition;

	protected override Vector3 TargetPosition
	{
		get
		{
			return (!(Target != null)) ? Vector3.zero : Target.transform.position;
		}
	}

	protected override float TargetRange
	{
		get
		{
			return (!(Target != null)) ? 0f : Target.Data.Range;
		}
	}

	private ObjectiveGetInRangeStatic()
	{
	}

	public override void OnActivate()
	{
		base.OnActivate();
		if (Target != null)
		{
			SpawnTarget();
		}
	}

	public override void OnDeactivate()
	{
		UnspawnTaget();
		base.OnDeactivate();
	}

	public override void OnPostRead(MissionBaseData _nodeData)
	{
		base.OnPostRead(_nodeData);
		StaticObjectData.ObjectType objectType = MissionFactory.StaticObjectFromGuid(base.Data.TypeGuid);
		if (objectType != null)
		{
			m_entityDefinition = UbiCore.Utils.Singleton<StaticObjectData>.Instance.GetDefinition(objectType);
			Type = objectType.GameID.Guid;
		}
	}

	public override void OnReadCompleted()
	{
		base.OnReadCompleted();
		Target = GetMissionObject(base.Data.TargetID) as MissionTarget;
	}

	private void SpawnTarget()
	{
		m_portalAsset = MissionFactory.InstantiateStaticObject(m_entityDefinition, Target.transform);
		if (m_portalAsset != null)
		{
			m_portalAsset.Activate(true);
		}
	}

	private void UnspawnTaget()
	{
		if (m_portalAsset != null)
		{
			m_portalAsset.Activate(false);
		}
	}
}
