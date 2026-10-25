using Assets.Scripts.Network.Missions.Mission.Data;
using UnityEngine;

[ExecuteInEditMode]
public abstract class MissionBase<TData> : MissionMonoBehaviour where TData : MissionBaseData, new()
{
	[SerializeField]
	private TData m_missionData;

	public TData Data
	{
		get
		{
			if (m_missionData == null)
			{
				m_missionData = new TData();
			}
			return m_missionData;
		}
		protected set
		{
			m_missionData = value;
			MissionData = value;
		}
	}

	public override bool IsActive { get; protected set; }

	public override MissionBaseData MissionData
	{
		get
		{
			return Data;
		}
		protected set
		{
			m_missionData = (TData)value;
		}
	}

	public void OnEnable()
	{
		Init();
	}

	public override bool OnDeserialized()
	{
		return true;
	}

	public override void OnPreActivate()
	{
	}

	public override void OnActivate()
	{
		IsActive = true;
	}

	public override void OnDeactivate()
	{
		IsActive = false;
	}

	public override void OnPostRead(MissionBaseData _nodeData)
	{
		Data = (TData)_nodeData;
		Register(this);
		base.OnPostRead(_nodeData);
	}

	protected virtual void Init()
	{
	}
}
