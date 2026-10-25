using AcierProtocol;
using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.Network.Missions.Mission.Data;
using Mission;
using UnityEngine;

public abstract class MissionMonoBehaviour : MonoBehaviour, IMissionBase
{
	private static MissionIDMapper s_missionIdMapper;

	protected int MissionLayerIndex = -1;

	public virtual EMapIconType MapIcon
	{
		get
		{
			return MissionData.MapIcon;
		}
		set
		{
			MissionData.MapIcon = value;
		}
	}

	public abstract bool IsActive { get; protected set; }

	public abstract MissionBaseData MissionData { get; protected set; }

	public bool IsDynamic { get; set; }

	public IMissionBase GetMissionObject(long _id)
	{
		MissionIDMapper mapper = GetMapper();
		return mapper.GetMissionObject(_id);
	}

	protected void Register(IMissionBase _missionObject)
	{
		MissionIDMapper mapper = GetMapper();
		mapper.Register(this);
	}

	protected static MissionIDMapper GetMapper()
	{
		if (s_missionIdMapper == null)
		{
			s_missionIdMapper = new MissionIDMapper();
		}
		return s_missionIdMapper;
	}

	public static void Reset()
	{
		s_missionIdMapper = null;
	}

	public abstract bool OnDeserialized();

	public abstract void OnPreActivate();

	public abstract void OnActivate();

	public abstract void OnDeactivate();

	public virtual void OnPostRead(MissionBaseData _nodeData)
	{
	}

	public virtual void OnReadCompleted()
	{
	}
}
