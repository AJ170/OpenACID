using System.Globalization;
using AcierProtocol;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.UI.Localization;
using Mission;
using SharedBaseLib.Core;
using UbiCore.Utils;

public class ObjectiveFreeCaptives : ObjectiveBase<ObjectiveFreeCaptivesData>, IObjectiveValue
{
	private int m_value;

	private object m_captiveCount;

	public int Value
	{
		get
		{
			return m_value;
		}
		protected set
		{
			if (m_value != value)
			{
				m_value = value;
				OnValueChanged();
				UpdateDescription();
			}
		}
	}

	private object CaptiveCount
	{
		get
		{
			return m_captiveCount;
		}
		set
		{
			if (value != m_captiveCount)
			{
				m_captiveCount = value;
				UpdateDescription();
			}
		}
	}

	protected override Mission.EObjectiveResult InitialResult
	{
		get
		{
			return Mission.EObjectiveResult.Undefined;
		}
	}

	protected override bool InitialVisualSummaryState
	{
		get
		{
			return false;
		}
	}

	protected override EProgressState InitialVisualTrackerState
	{
		get
		{
			return EProgressState.InProgress;
		}
	}

	public event ValueChangedDelegate ValueChanged;

	private ObjectiveFreeCaptives()
	{
	}

	public override void OnActivate()
	{
		base.OnActivate();
		m_value = 0;
		m_captiveCount = 0;
	}

	protected override void ShowFailMessage()
	{
		base.ShowFailMessage();
		int textId = 7741;
		StringMapper stringMapper = null;
		if (UbiCore.Utils.Singleton<MissionManager>.Instance.MissionMain.Data.ObjectiveType == EObjectiveType.Story_EObjectiveType)
		{
			BBList<Character> allCaptivesDeadOrAlive = GetAllCaptivesDeadOrAlive();
			if (allCaptivesDeadOrAlive.Count == 1)
			{
				textId = 8425;
				StringMapper stringMapper2 = new StringMapper();
				stringMapper2.Add("objective_taget", allCaptivesDeadOrAlive[0].GetLocalizedName(CharacterHelper.ECharacterNameType.Plain));
				stringMapper = stringMapper2;
			}
		}
		LogicMain.LogicInterface.OnSendInfo(EInfoEventType.TextAnnotation, textId, InfoEvent.ETextAnnotationMode.MessageOpen, -1, stringMapper, true);
	}

	public override void FrameUpdateImpl()
	{
		if (IsActive && LogicMain.PlayerEntity != null)
		{
			Value = LogicMain.MissionResult.GetUsedHotspotCount(EHotspot.EHotspot_Captives);
			CaptiveCount = LogicMain.MissionInfo.CaptiveCount;
			UpdateTargetPositions();
			if (LogicMain.MissionResult.LostCaptivesCount > base.Data.MaximumCasualties)
			{
				base.VisualTrackerState = EProgressState.Failed;
				base.VisualSummaryState = false;
				base.Result = Mission.EObjectiveResult.Failed;
				Complete();
			}
			else if (Value == LogicMain.MissionInfo.CaptiveCount)
			{
				base.VisualTrackerState = EProgressState.Fulfilled;
				base.VisualSummaryState = true;
				base.Result = Mission.EObjectiveResult.Fulfilled;
				Complete();
			}
		}
	}

	private void UpdateTargetPositions()
	{
		TargetPositions.ClearIfNotEmpty();
		if (ObjectiveData.MapIcon == EMapIconType.Invalid_EMapIconType)
		{
			return;
		}
		for (int i = 0; i < LogicMain.EntityManager.CrowdCharacterCount; i++)
		{
			CrowdCharacter crowdCharacter = LogicMain.EntityManager.GetCrowdCharacter(i);
			if (crowdCharacter.Patrol != null && crowdCharacter.Patrol.CaptiveComponent != null && !crowdCharacter.Patrol.CaptiveComponent.IsVictimHit && crowdCharacter.GetIsCaptured())
			{
				TargetPositions.Add(crowdCharacter.Position);
			}
		}
	}

	public static BBList<Character> GetAllCaptivesDeadOrAlive()
	{
		BBList<Character> bBList = new BBList<Character>();
		for (int i = 0; i < LogicMain.EntityManager.CrowdCharacterCount; i++)
		{
			CrowdCharacter crowdCharacter = LogicMain.EntityManager.GetCrowdCharacter(i);
			if (crowdCharacter.Patrol != null && crowdCharacter.Patrol.PatrolComponent.MissionData.Data.Type == EPatrolType.CapturedNpcs)
			{
				bBList.Add(crowdCharacter);
			}
		}
		return bBList;
	}

	public override string ToString()
	{
		Assets.Scripts.Mission.MissionInfo missionInfo = LogicMain.MissionInfo;
		return string.Format("{0} of {1}", Value, (missionInfo == null) ? "???" : missionInfo.CaptiveCount.ToString(CultureInfo.InvariantCulture));
	}

	public override StringMapper GetStringMapper()
	{
		StringMapper stringMapper = base.GetStringMapper();
		stringMapper.Add("current", Value);
		stringMapper.Add("count", CaptiveCount);
		stringMapper.Add("maximum_casualties", base.Data.MaximumCasualties);
		stringMapper.Add("current_casualties", LogicMain.MissionResult.LostCaptivesCount);
		return stringMapper;
	}

	protected virtual void OnValueChanged()
	{
		ValueChangedDelegate valueChanged = this.ValueChanged;
		if (valueChanged != null)
		{
			valueChanged();
		}
	}
}
