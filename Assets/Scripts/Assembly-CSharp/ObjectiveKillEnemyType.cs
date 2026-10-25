using AcierProtocol;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Mission;
using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.UI.Localization;
using Database;
using Mission;
using SharedBaseLib.Core;
using UbiCore.Messenger;

public class ObjectiveKillEnemyType : ObjectiveBase<ObjectiveKillEnemyTypeData>, IObjectiveValue
{
	[NpcTypeGuid]
	public int Type;

	private readonly BBList<NpcCharacter> m_relevantEntities = new BBList<NpcCharacter>();

	private int m_bodyCount;

	private int m_rolledNameLocaId;

	private int m_rolledPortraitNameLocaId;

	private int m_value;

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
			}
		}
	}

	protected override Mission.EObjectiveResult InitialResult
	{
		get
		{
			return base.Data.ForbidToKill ? Mission.EObjectiveResult.Fulfilled : Mission.EObjectiveResult.Undefined;
		}
	}

	protected override bool InitialVisualSummaryState
	{
		get
		{
			return base.Data.ForbidToKill;
		}
	}

	protected override EProgressState InitialVisualTrackerState
	{
		get
		{
			return base.Data.ForbidToKill ? EProgressState.InProgress : EProgressState.NotStarted;
		}
	}

	public NpcData.NpcType NpcType { get; private set; }

	public event ValueChangedDelegate ValueChanged;

	protected override void ShowFailMessage()
	{
		base.ShowFailMessage();
		LogicMain.LogicInterface.OnSendInfo(EInfoEventType.TextAnnotation, 7745, InfoEvent.ETextAnnotationMode.MessageOpen, -1, null, true);
	}

	public override void FrameUpdateImpl()
	{
		UpdateTargetPositions();
		if (m_bodyCount >= base.Data.KillAmount)
		{
			base.VisualTrackerState = (base.Data.ForbidToKill ? EProgressState.Failed : EProgressState.Fulfilled);
			base.VisualSummaryState = !base.Data.ForbidToKill;
			base.Result = ((!base.Data.ForbidToKill) ? Mission.EObjectiveResult.Fulfilled : Mission.EObjectiveResult.Failed);
			Complete();
		}
	}

	public override void OnActivate()
	{
		for (int i = 0; i < LogicMain.EntityManager.EnemyCharacterCount; i++)
		{
			EnemyCharacter enemyCharacter = LogicMain.EntityManager.GetEnemyCharacter(i);
			RegisterEntity(enemyCharacter);
		}
		Messenger.Instance.AddListener<NPCKillEvent>(OnNPCKilled);
		base.OnActivate();
		UpdateTargetPositions();
		m_bodyCount = 0;
		m_value = 0;
	}

	public override void OnDeactivate()
	{
		for (int num = m_relevantEntities.Count - 1; num >= 0; num--)
		{
			UnregisterEntity(m_relevantEntities[num]);
		}
		LogicMain.EntityManager.OnEntityInitialized -= OnEntityInitialized;
		LogicMain.EntityManager.OnEntityUnregistered -= OnEntityUnregistered;
		Messenger.Instance.RemoveListener<NPCKillEvent>(OnNPCKilled);
		m_relevantEntities.ClearIfNotEmpty();
		base.OnDeactivate();
	}

	public override void OnPostRead(MissionBaseData _nodeData)
	{
		base.OnPostRead(_nodeData);
		LogicMain.EntityManager.OnEntityInitialized += OnEntityInitialized;
		LogicMain.EntityManager.OnEntityUnregistered += OnEntityUnregistered;
		string typeGuid = base.Data.TypeGuid;
		GameID _gameId;
		if (Main.Instance.GameDB.TryFindGameIdFromGuid(typeGuid, out _gameId))
		{
			NpcType = new NpcData.NpcType(_gameId);
			Type = NpcType.GameID.Guid;
			m_rolledNameLocaId = ((!MissionMetaDataManager.IsAvailable || !MissionMetaDataManager.Instance.MissionGoals.RolledNames.ContainsKey(typeGuid)) ? NpcType.Definition.GetLocalizedNameLocaId(CharacterHelper.ECharacterNameType.Plain) : MissionMetaDataManager.Instance.MissionGoals.RolledNames[typeGuid]);
			m_rolledPortraitNameLocaId = ((!MissionMetaDataManager.IsAvailable || !MissionMetaDataManager.Instance.MissionGoals.RolledPortraitNames.ContainsKey(typeGuid)) ? NpcType.Definition.GetLocalizedNameLocaId(CharacterHelper.ECharacterNameType.PortraitWithLevel) : MissionMetaDataManager.Instance.MissionGoals.RolledPortraitNames[typeGuid]);
		}
		else
		{
			NpcType = new NpcData.NpcType(GameID.Invalid);
			Type = GameID.Invalid.Guid;
		}
	}

	public override string ToString()
	{
		string text = string.Empty;
		if (base.Data.ForbidToKill)
		{
			text = "Do not ";
		}
		string arg = "(not set)";
		if (NpcType != null && NpcType.IsValid)
		{
			arg = GameDB.ConvertGUIDToName(NpcType.GameID.Guid);
		}
		return text + string.Format("kill {0} {1}", base.Data.KillAmount, arg);
	}

	protected override void Complete()
	{
		if (IsActive)
		{
			base.Complete();
		}
	}

	public override StringMapper GetStringMapper()
	{
		string value = LocalizationManager.Instance.Get(m_rolledNameLocaId);
		StringMapper stringMapper = base.GetStringMapper();
		stringMapper.Add("killed", m_bodyCount);
		stringMapper.Add("kill_amount", base.Data.KillAmount);
		stringMapper.Add("kill_amount-1", base.Data.KillAmount - 1);
		stringMapper.Add("kill_amount+1", base.Data.KillAmount + 1);
		stringMapper.Add("type_name", value);
		stringMapper.Add("Current", m_bodyCount);
		stringMapper.Add("Threshold", base.Data.KillAmount);
		stringMapper.Add("Type", value);
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

	private void AddToVision(EntityId _entityId)
	{
		if (base.Data.IsVisibleInEagleVision)
		{
			LogicMain.PlayerEntity.Vision.AddEntity(_entityId);
		}
	}

	private void OnEntityInitialized(IEntity _entity)
	{
		NpcCharacter npcCharacter = _entity as NpcCharacter;
		if (npcCharacter != null)
		{
			RegisterEntity(npcCharacter);
		}
	}

	private void RegisterEntity(NpcCharacter _entity)
	{
		if (!base.Data.ForbidToKill && !(_entity.NpcDefinition.Definition.Type != NpcType) && !m_relevantEntities.Contains(_entity))
		{
			_entity.EntityStats.Set_int(StatTypes.EStatType.Entity_DisplayNameLocaKey, m_rolledNameLocaId);
			_entity.EntityStats.Set_int(StatTypes.EStatType.Character_PortraitDisplayNameLocaKey, m_rolledPortraitNameLocaId);
			m_relevantEntities.Add(_entity);
			AddToVision(_entity.Id);
		}
	}

	private void OnEntityUnregistered(IEntity _entity)
	{
		NpcCharacter npcCharacter = _entity as NpcCharacter;
		if (npcCharacter != null && !(npcCharacter.NpcDefinition.Definition.Type != NpcType) && m_relevantEntities.Remove(npcCharacter) && !base.Data.ForbidToKill)
		{
			UnregisterEntity(npcCharacter);
		}
	}

	private void OnNPCKilled(NPCKillEvent _event)
	{
		NpcCharacter logicEntity = _event.LogicEntity;
		if (logicEntity != null && !(logicEntity.NpcDefinition.Definition.Type != NpcType))
		{
			if (!base.Data.ForbidToKill)
			{
				UnregisterEntity(logicEntity);
			}
			m_bodyCount++;
			UpdateDescription();
			UpdateTargetPositions();
		}
	}

	private void RemoveFromVision(EntityId _entityId)
	{
		if (base.Data.IsVisibleInEagleVision)
		{
			LogicMain.PlayerEntity.Vision.RemoveEntity(_entityId);
		}
	}

	private void UnregisterEntity(NpcCharacter _entity)
	{
		if (!base.Data.ForbidToKill)
		{
			RemoveFromVision(_entity.Id);
			m_relevantEntities.Remove(_entity);
		}
	}

	private void UpdateTargetPositions()
	{
		TargetPositions.ClearIfNotEmpty();
		for (int i = 0; i < m_relevantEntities.Count; i++)
		{
			if (m_relevantEntities[i].IsValid && !m_relevantEntities[i].Id.GetEntity<NpcCharacter>().GetIsDead())
			{
				TargetPositions.Add(m_relevantEntities[i].Position);
			}
		}
		Value = base.Data.KillAmount - TargetPositions.Count;
	}
}
