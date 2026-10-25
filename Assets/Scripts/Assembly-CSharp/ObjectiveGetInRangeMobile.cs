using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.Network.Missions.Mission.Data;
using UnityEngine;

public sealed class ObjectiveGetInRangeMobile : ObjectiveGetInRange<ObjectiveGetInRangeMobileData>
{
	public MissionMonoBehaviour Target;

	private IMissionSpawnEntity m_entity;

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
			return base.Data.Range;
		}
	}

	private ObjectiveGetInRangeMobile()
	{
	}

	public override void OnReadCompleted()
	{
		base.OnReadCompleted();
		IMissionBase missionObject = GetMissionObject(base.Data.TargetID);
		m_entity = missionObject as IMissionSpawnEntity;
		if (m_entity != null)
		{
			Target = (MissionMonoBehaviour)missionObject;
		}
	}

	public override string ToString()
	{
		string text = ObjectiveBase<ObjectiveGetInRangeMobileData>.FormatEntity(Target, 0);
		return text + "\n" + base.ToString();
	}
}
