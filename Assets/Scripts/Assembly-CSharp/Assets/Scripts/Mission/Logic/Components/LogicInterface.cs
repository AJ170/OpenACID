using System;
using System.Collections.ObjectModel;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Selection;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using Assets.Scripts.UI.Localization;
using Database;
using SharedBaseLib.Core;
using UbiCore.Debug;
using UbiCore.Messenger;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class LogicInterface : FrameUpdateableComponent, ILogicInterface
	{
		[Flags]
		public enum ECombatEventModifierFlags
		{
			None = 0,
			Critical = 1,
			Deflected = 2,
			FallDamage = 4
		}

		public class ActiveBuff : IActiveBuff
		{
			public int Id { get; private set; }

			public int StartTime { get; private set; }

			public int EndTime { get; private set; }

			public int Duration { get; private set; }

			public EBuffType Type { get; private set; }

			public ActiveBuff(IBuff _buff)
			{
				Id = _buff.GetHashCode();
				StartTime = _buff.StartTurn;
				if (_buff.DurationType == EBuffDurationType.Finite)
				{
					EndTime = _buff.StartTurn + _buff.TurnDuration;
				}
				else
				{
					EndTime = int.MaxValue;
				}
				Duration = _buff.TurnDuration;
				Type = _buff.Type;
			}

			public ActiveBuff(BuffUiEvent _message)
			{
				Id = _message.Id;
				StartTime = _message.StartTime;
				EndTime = _message.EndTime;
				Duration = _message.Duration;
				Type = _message.Type;
			}
		}

		private readonly BBList<ILogicUiEvent> m_EventQueue = new BBList<ILogicUiEvent>();

		private PlayerCharacter m_PlayerCharacter;

		private int m_currentInfoId;

		public bool IsRunning
		{
			get
			{
				return LogicMain.IsRunning;
			}
		}

		public float GameTime
		{
			get
			{
				return LogicMain.CurrentTime;
			}
		}

		public int GameTurn
		{
			get
			{
				return LogicMain.CurrentTurn;
			}
		}

		public float GameTurnProgress
		{
			get
			{
				return Mathf.Clamp01((GameTime - LastGameTurnTime) / TurnDuration);
			}
		}

		public float LastGameTurnTime
		{
			get
			{
				return LogicMain.LastTurnTime;
			}
		}

		public float TurnDuration
		{
			get
			{
				return 0.1f;
			}
		}

		public EMissionType MissionType
		{
			get
			{
				return LogicMain.MissionType;
			}
		}

		public EntityId PlayerId
		{
			get
			{
				return m_PlayerCharacter.Id;
			}
		}

		public string PlayerName
		{
			get
			{
				string text = null;
				if (LogicMain.PlayerEntity.AvatarDefinition.Definition.PortraitDisplayNameLocaKey != 0)
				{
					text = LocalizationManager.Instance.Get(LogicMain.PlayerEntity.AvatarDefinition.Definition.PortraitDisplayNameLocaKey);
				}
				if (!string.IsNullOrEmpty(text))
				{
					return text;
				}
				text = LogicMain.PlayerEntity.AvatarDefinition.Definition.PortraitDisplayName;
				if (!string.IsNullOrEmpty(text))
				{
					return text;
				}
				return LogicMain.PlayerEntity.Definition.EntityDefinition.Name;
			}
		}

		public bool IsPlayerBeingChased
		{
			get
			{
				return m_PlayerCharacter.PlayerProfileCarrier.PlayerProfile.CurrentStateType == EStateType.PlayerProfile_Chase;
			}
		}

		public EMovementSpeed PlayerSpeed
		{
			get
			{
				return m_PlayerCharacter.GetSpeed();
			}
		}

		public EMovementSpeed PlayerChosenSpeed
		{
			get
			{
				if (m_PlayerCharacter.PlayerCharacterStateMachines.PlayerProfile.SpeedIsFixed)
				{
					return PlayerSpeed;
				}
				return m_PlayerCharacter.PlayerCharacterStateMachines.PlayerProfile.ChosenSpeed;
			}
		}

		public int PlayerNotoriety
		{
			get
			{
				float notoriety = m_PlayerCharacter.PlayerStats.Get_float(StatTypes.EStatType.Character_Conspicuous_Notoriety);
				return RoundNotoriety(notoriety);
			}
		}

		public EntityId CurrentInteractionObject
		{
			get
			{
				return m_PlayerCharacter.GetCurrentInteractionObject();
			}
		}

		public EntityId CurrentInteractionNpc
		{
			get
			{
				return m_PlayerCharacter.GetCurrentInteractionNpc();
			}
		}

		public EAlertLevel PlayerAlertLevel
		{
			get
			{
				int playerNotoriety = PlayerNotoriety;
				if ((float)playerNotoriety > 99f)
				{
					return EAlertLevel.Exposed;
				}
				if ((float)playerNotoriety >= 50f)
				{
					return EAlertLevel.Alerted;
				}
				return EAlertLevel.Anonymous;
			}
		}

		public EntityId SelectedEntityId
		{
			get
			{
				EntityId result = UbiCore.Utils.Singleton<SelectionManager>.Instance.SelectedEntityId;
				if (CombatController.Instance.PlayerParticipant != null)
				{
					result = ((CombatController.Instance.PlayerParticipant.NextTarget == null) ? EntityId.InvalidId : CombatController.Instance.PlayerParticipant.NextTarget.Entity.Id);
				}
				return result;
			}
		}

		public Enum<SkillTypes>[] SkillWheel
		{
			get
			{
				return m_PlayerCharacter.Skills.SkillWheel;
			}
		}

		public Enum<SkillTypes> SelectedSkill
		{
			get
			{
				return m_PlayerCharacter.Skills.SelectedSkill;
			}
		}

		public bool Prototype_BuffTimerDisabled { get; set; }

		public override int Priority
		{
			get
			{
				return 36;
			}
		}

		private bool FirstTurn { get; set; }

		public LogicInterface(IEntity _entity)
			: base(_entity)
		{
		}

		public void AddListener<T>(Messenger.EventCallback<T> _handler) where T : ILogicUiEvent
		{
			Messenger.Instance.AddListener(_handler);
		}

		public void RemoveListener<T>(Messenger.EventCallback<T> _handler) where T : ILogicUiEvent
		{
			Messenger.Instance.RemoveListener(_handler);
		}

		public bool IsPlayerSpeedAllowed(EMovementSpeed _speed)
		{
			return _speed == EMovementSpeed.Walk || _speed == EMovementSpeed.Run;
		}

		public void SetPlayerSpeed(EMovementSpeed _speed)
		{
			if (IsPlayerSpeedAllowed(_speed))
			{
				if (m_PlayerCharacter.PlayerCharacterStateMachines.PlayerProfile.SpeedIsFixed)
				{
					m_PlayerCharacter.PlayerCharacterStateMachines.PlayerProfile.OverrideFixedSpeed();
				}
				m_PlayerCharacter.SetSpeed(_speed);
			}
		}

		public int RoundNotoriety(float _notoriety)
		{
			return (!(_notoriety > 0.99f)) ? ((int)(_notoriety * 100f)) : 100;
		}

		public bool Revive()
		{
			return PlayerHelper.Revive();
		}

		public EMapIconType GetMapIconType(EntityId _entityId)
		{
			Entity entity = _entityId.TryGetEntity<Entity>();
			if (entity == null || entity.EntityStats == null)
			{
				return EMapIconType.Invalid_EMapIconType;
			}
			return (EMapIconType)entity.EntityStats.Get_int(StatTypes.EStatType.Entity_MapIconType);
		}

		public IReadonlyHumanoidDef GetHumanoidDef(EntityId _entityId)
		{
			HumanoidDef result = null;
			Character character = _entityId.TryGetEntity<Character>();
			if (character != null)
			{
				result = Main.Instance.GameDB.GetItem<HumanoidDef>(character.CharacterDefinition.Definition.HumanoidGameID);
			}
			return result;
		}

		public IReadonlyCharacterAppearance GetCharacterAppearance(EntityId _entityId)
		{
			ICharacterAppearance result = null;
			Character character = _entityId.TryGetEntity<Character>();
			if (character != null)
			{
				result = character.CharacterAppearance;
			}
			return result;
		}

		public int GetThreatLevel(EntityId _entityId)
		{
			EnemyCharacter enemyCharacter = _entityId.TryGetEntity<EnemyCharacter>();
			if (enemyCharacter == null)
			{
				return 0;
			}
			return enemyCharacter.NpcDefinition.Definition.ThreatLevel;
		}

		public string GetUnitName(EntityId _entityId)
		{
			Character character = _entityId.TryGetEntity<Character>();
			if (character == null)
			{
				return "<Animus Error>";
			}
			return character.GetLocalizedName(CharacterHelper.ECharacterNameType.PortraitWithLevel);
		}

		public bool HasStatValue(EntityId _entityId, StatTypes.EStatType _statType)
		{
			return _entityId.GetEntity<Entity>().DoIfSet((Entity e) => e.EntityStats.HasStat(_statType), false);
		}

		public string GetStatValue_string(EntityId _entityId, StatTypes.EStatType _statType)
		{
			return _entityId.GetEntity<Entity>().DoIfSet((Entity e) => e.EntityStats.GetReadonlyStatValue(_statType).DoIfSet((IReadonlyStatValue stat) => stat.Value_string));
		}

		public int GetStatValue_int(EntityId _entityId, StatTypes.EStatType _statType)
		{
			return _entityId.GetEntity<Entity>().DoIfSet((Entity e) => e.EntityStats.GetReadonlyStatValue(_statType).DoIfSet((IReadonlyStatValue stat) => stat.Value_int));
		}

		public float GetStatValue_float(EntityId _entityId, StatTypes.EStatType _statType)
		{
			return _entityId.GetEntity<Entity>().DoIfSet((Entity e) => e.EntityStats.GetReadonlyStatValue(_statType).DoIfSet((IReadonlyStatValue stat) => stat.Value_float));
		}

		public bool GetStatValue_bool(EntityId _entityId, StatTypes.EStatType _statType)
		{
			return _entityId.GetEntity<Entity>().DoIfSet((Entity e) => e.EntityStats.GetReadonlyStatValue(_statType).DoIfSet((IReadonlyStatValue stat) => stat.Value_bool));
		}

		public float GetNormalizedHealth(EntityId _entityId)
		{
			if (!HasStatValue(_entityId, StatTypes.EStatType.Character_Conflict_Health))
			{
				return 1f;
			}
			EntityStats entityStats = _entityId.Entity.EntityStats;
			return entityStats.Get_float(StatTypes.EStatType.Character_Conflict_Health) / entityStats.Get_float(StatTypes.EStatType.Character_Conflict_ActualMaxHealth);
		}

		public void CharacterInteraction_AcceptOffer(EntityId _npcId)
		{
			m_PlayerCharacter.AssassinInteraction.AcceptOffer(_npcId);
		}

		public void CharacterInteraction_RejectOffer(EntityId _npcId)
		{
			m_PlayerCharacter.AssassinInteraction.RejectOffer(_npcId);
		}

		internal void Combat_KillToken(CombatKillTokenUiEvent.EType _type, int _value)
		{
			m_EventQueue.Add(new CombatKillTokenUiEvent(_type, _value));
		}

		internal void Combat_Hit(Character _source, Character _target, float _delta, ECombatEventModifierFlags _flags)
		{
			m_EventQueue.Add(CombatUiEvent.CreateHitEvent(_source, _target, _delta, _flags));
		}

		internal void Combat_Heal(Character _source, Character _target, float _delta, ECombatEventModifierFlags _flags)
		{
			m_EventQueue.Add(CombatUiEvent.CreateHealEvent(_source, _target, _delta, _flags));
		}

		internal void Combat_Dodge(Character _source, Character _target, bool _missed)
		{
			m_EventQueue.Add(CombatUiEvent.CreateDodgeEvent(_source, _target, _missed));
		}

		internal void Combat_KillMoveDeflect(Character _source, Character _target)
		{
			m_EventQueue.Add(CombatUiEvent.CreateKillMoveDefelectEvent(_source, _target));
		}

		internal void Combat_Timer(Character _target, int _countdown)
		{
			m_EventQueue.Add(CombatUiEvent.CreateTimerEvent(_target, _countdown));
		}

		internal void Combat_SoftCurrency(Character _target, int _coins)
		{
			m_EventQueue.Add(CombatUiEvent.CreateSoftCurrencyEvent(_target, _coins));
		}

		internal void Combat_Enter(Character _source)
		{
			m_EventQueue.Add(CombatUiEvent.CreateEnterEvent(_source));
		}

		internal void Combat_Exit(Character _source)
		{
			m_EventQueue.Add(CombatUiEvent.CreateExitEvent(_source));
		}

		internal void Combat_Select()
		{
			Character target = SelectedEntityId.TryGetEntity<Character>();
			m_EventQueue.Add(CombatUiEvent.CreateSelectTargetEvent(target));
		}

		internal void Combat_PrepareAttack(Character _source, Character _target, float _duration)
		{
			m_EventQueue.Add(CombatUiEvent.CreatePrepareAttack(_source, _target, _duration));
		}

		internal void Combat_MissionGoalLoot(GameEntity _targetEntity, MissionGoalLoot _loot)
		{
			m_EventQueue.Add(CombatUiEvent.CreateMissionGoalLoot(_targetEntity, _loot));
		}

		public bool Has(Enum<SkillTypes> _type)
		{
			return m_PlayerCharacter.Skills.Has(_type);
		}

		public bool CanBeSelected(Enum<SkillTypes> _type)
		{
			return m_PlayerCharacter.Skills.CanBeSelected(_type);
		}

		public bool Select(Enum<SkillTypes> _type)
		{
			return m_PlayerCharacter.Skills.Select(_type);
		}

		public bool CanBeTriggered(Enum<SkillTypes> _type, out ESkillAvailabilityFlags _availabilityFlags)
		{
			return CanBeTriggeredInternal(_type, null, out _availabilityFlags);
		}

		public bool CanBeTriggered(Enum<SkillTypes> _type, EntityId _entityId, out ESkillAvailabilityFlags _availabilityFlags)
		{
			return CanBeTriggeredInternal(_type, new EntityIdSkillPayload(_entityId), out _availabilityFlags);
		}

		public bool CanBeTriggeredOnSelectedEntity(Enum<SkillTypes> _type, out ESkillAvailabilityFlags _availabilityFlags)
		{
			return CanBeTriggered(_type, SelectedEntityId, out _availabilityFlags);
		}

		public bool Trigger(Enum<SkillTypes> _type)
		{
			return TriggerInternal(_type, null);
		}

		public bool Trigger(Enum<SkillTypes> _type, EntityId _entityId)
		{
			return TriggerInternal(_type, new EntityIdSkillPayload(_entityId));
		}

		public bool TriggerOnSelectedEntity(Enum<SkillTypes> _skillType)
		{
			return Trigger(_skillType, SelectedEntityId);
		}

		public bool CanBeAborted(Enum<SkillTypes> _skillType)
		{
			SkillData.IApproachSkillDefinition approachSkillDefinition = _skillType.GetDefinition() as SkillData.IApproachSkillDefinition;
			if (approachSkillDefinition != null && m_PlayerCharacter.AssassinCharacterStateMachines.AssassinApproachingTarget.Reason == approachSkillDefinition.InteractionReason)
			{
				return true;
			}
			return m_PlayerCharacter.Skills.CanBeAborted(_skillType);
		}

		public bool Abort(Enum<SkillTypes> _skillType)
		{
			SkillData.IApproachSkillDefinition approachSkillDefinition = _skillType.GetDefinition() as SkillData.IApproachSkillDefinition;
			if (approachSkillDefinition != null && m_PlayerCharacter.AssassinCharacterStateMachines.AssassinApproachingTarget.Reason == approachSkillDefinition.InteractionReason)
			{
				m_PlayerCharacter.StopApproaching();
				return true;
			}
			return m_PlayerCharacter.Skills.Abort(_skillType);
		}

		public ESkillState GetState(Enum<SkillTypes> _type)
		{
			return m_PlayerCharacter.Skills.GetState(_type);
		}

		public bool GetIsActive(Enum<SkillTypes> _type)
		{
			return m_PlayerCharacter.Skills.GetIsActive(_type);
		}

		public int GetActivityStart(Enum<SkillTypes> _type)
		{
			return m_PlayerCharacter.Skills.GetActivityStart(_type);
		}

		public int GetActivityEnd(Enum<SkillTypes> _type)
		{
			return m_PlayerCharacter.Skills.GetActivityEnd(_type);
		}

		public int GetActivityDuration(Enum<SkillTypes> _type)
		{
			return m_PlayerCharacter.Skills.GetBuffedSkillDuration(_type);
		}

		public bool GetIsCoolingDown(Enum<SkillTypes> _type)
		{
			return m_PlayerCharacter.Skills.GetIsCoolingDown(_type) && !m_PlayerCharacter.Skills.GetIsActive(_type);
		}

		public int GetCoolDownStart(Enum<SkillTypes> _type)
		{
			return m_PlayerCharacter.Skills.GetActivityEnd(_type);
		}

		public int GetCoolDownEnd(Enum<SkillTypes> _type)
		{
			return m_PlayerCharacter.Skills.GetCoolDownEnd(_type);
		}

		public int GetCoolDownDuration(Enum<SkillTypes> _type)
		{
			int buffedCooldownDuration = m_PlayerCharacter.Skills.GetBuffedCooldownDuration(_type);
			int activityDuration = GetActivityDuration(_type);
			return buffedCooldownDuration - activityDuration;
		}

		public GameID GetGameID(Enum<SkillTypes> _type)
		{
			return (!(_type == Enum<SkillTypes>.Invalid)) ? _type.GetDefinition().GameID : GameID.Invalid;
		}

		public IEntityIcon CreateEntityIcon()
		{
			return new EntityIcon();
		}

		public void ReleaseEntityIcon(IEntityIcon _icon)
		{
			((EntityIcon)_icon).Dispose();
		}

		public void AddEntityIcon(EntityId _entityId, IEntityIcon _icon)
		{
			_entityId.GetEntity<IHeadIconCarrier>().DoIfSet(delegate(IHeadIconCarrier c)
			{
				c.HeadIcon.AddCustomIconAttachment((EntityIcon)_icon);
			});
		}

		public void RemoveEntityIcon(EntityId _entityId, IEntityIcon _icon)
		{
			_entityId.GetEntity<IHeadIconCarrier>().DoIfSet(delegate(IHeadIconCarrier c)
			{
				c.HeadIcon.RemoveCustomIconAttachment((EntityIcon)_icon);
			});
		}

		public bool GetDefaultEntityIconVisibility(EntityId _entityId)
		{
			return _entityId.GetEntity<IHeadIconCarrier>().DoIfSet((IHeadIconCarrier c) => c.HeadIcon.IsVisible, false);
		}

		public void SetDefaultEntityIconVisibility(EntityId _entityId, bool _visibility)
		{
			_entityId.GetEntity<IHeadIconCarrier>().DoIfSet(delegate(IHeadIconCarrier c)
			{
				c.HeadIcon.IsVisible = _visibility;
			});
		}

		public IActiveBuff[] GetBuffs(EntityId _entity)
		{
			ReadOnlyCollection<IBuff> sortedBuffs = _entity.GameEntity.As<Character>().Buffs.SortedBuffs;
			IActiveBuff[] array = new IActiveBuff[sortedBuffs.Count];
			for (int i = 0; i < sortedBuffs.Count; i++)
			{
				array[i] = new ActiveBuff(sortedBuffs[i]);
			}
			return array;
		}

		public void WidgetGotPressed(string _path)
		{
			if (UbiCore.Utils.Singleton<MissionManager>.IsAvailable)
			{
				UbiCore.Utils.Singleton<MissionManager>.Instance.Phase.OnWidgetPress(_path);
			}
		}

		public void Tutorial_WidgetHighlight(string _path, bool _enabled)
		{
			m_EventQueue.Add(new TutorialUiEvent(_path, (!_enabled) ? TutorialUiEvent.EType.RemoveHighlight : TutorialUiEvent.EType.SetHighlight));
		}

		public void Tutorial_WidgetVisible(string _path, bool _visible)
		{
			m_EventQueue.Add(new TutorialUiEvent(_path, (!_visible) ? TutorialUiEvent.EType.HideWidget : TutorialUiEvent.EType.ShowWidget));
		}

		public void Tutorial_WidgetBlocked(string _path, bool _blocked)
		{
			m_EventQueue.Add(new TutorialUiEvent(_path, (!_blocked) ? TutorialUiEvent.EType.UnBlockWidget : TutorialUiEvent.EType.BlockWidget));
		}

		public void Tutorial_SetActive(bool _active)
		{
			m_EventQueue.Add(new TutorialUiEvent(string.Empty, (!_active) ? TutorialUiEvent.EType.DeactivateSystem : TutorialUiEvent.EType.ActivateSystem));
		}

		private void UpdateCache()
		{
		}

		private void OnBuffEvent(BuffEvent message)
		{
			if (message.Buff.Type != EBuffType.Invalid)
			{
				m_EventQueue.Add(new BuffUiEvent(message));
			}
		}

		private void OnSkillEvent(UbiCore.Messenger.SkillEvent message)
		{
			m_EventQueue.Add(new Assets.Scripts.Mission.UI.SkillEvent(message));
		}

		private void OnCharacterInteraction(StateChangedMessage<CharacterInteraction> message)
		{
			switch (message.To)
			{
			case EStateType.CharacterInteraction_OfferOpen:
				m_EventQueue.Add(new NpcInteractionEvent(NpcInteractionEvent.ESubType.OfferOpen, message.StateMachine.Entity.Id));
				break;
			case EStateType.CharacterInteraction_OfferAccepted:
				m_EventQueue.Add(new NpcInteractionEvent(NpcInteractionEvent.ESubType.OfferAccepted, message.StateMachine.Entity.Id));
				break;
			case EStateType.CharacterInteraction_OfferRejected:
				m_EventQueue.Add(new NpcInteractionEvent(NpcInteractionEvent.ESubType.OfferRejected, message.StateMachine.Entity.Id));
				break;
			}
		}

		private void OnStatValueChangedEvent(StatValueChangedEvent message)
		{
			if (message.Owner.IsReadyForUpdate)
			{
				switch (message.StatValue.StatType)
				{
				case StatTypes.EStatType.Entity_MapIconType:
					OnEntityMapIconChanged(message.Owner, (EMapIconType)message.StatValue.Value_int);
					break;
				case StatTypes.EStatType.Player_MovementPrevented:
					InvokePlayerStateChangeEvent(m_PlayerCharacter.AssassinCharacterStateMachines.PlayerMain.CurrentStateType, message.StatValue.Value_bool);
					break;
				}
			}
		}

		private void OnEntityMapIconChanged(IEntity _entity, EMapIconType _mapIconType)
		{
			m_EventQueue.Add(new EntityMapIconChangedEvent(_entity, _mapIconType));
		}

		public int OnSendInfo(EInfoEventType _infoType, int _textId = -1, int _headerTextId = 0, StringMapper _stringMapper = null, bool _immediate = false)
		{
			return OnSendInfo(_infoType, _textId, InfoEvent.ETextAnnotationMode.Invalid, _headerTextId, null, _stringMapper, _immediate);
		}

		public int OnSendInfo(EInfoEventType _infoType, int _textId, InfoEvent.ETextAnnotationMode _mode, int _headerTextId = 0, StringMapper _stringMapper = null, bool _immediate = false)
		{
			return OnSendInfo(_infoType, _textId, _mode, _headerTextId, null, _stringMapper, _immediate);
		}

		public int OnSendInfo(EInfoEventType _infoType, int _textId, InfoEvent.ETextAnnotationMode _mode, int _headerTextId, Texture _texture, StringMapper _stringMapper, bool _immediate = false)
		{
			int uniqueInfoId = GetUniqueInfoId();
			OnSendInfo(_infoType, _textId, _mode, _headerTextId, _texture, _stringMapper, uniqueInfoId, _immediate);
			return uniqueInfoId;
		}

		public void OnSendInfo(EInfoEventType _infoType, int _textId, InfoEvent.ETextAnnotationMode _mode, int _headerTextId, StringMapper _stringMapper, int _uniqueInfoId, bool _immediate = false)
		{
			m_EventQueue.Add(new InfoEvent(_infoType, _textId, _mode, _headerTextId, null, _stringMapper, _uniqueInfoId, _immediate));
		}

		public void OnSendInfo(EInfoEventType _infoType, int _textId, InfoEvent.ETextAnnotationMode _mode, int _headerTextId, Texture _texture, StringMapper _stringMapper, int _uniqueInfoId, bool _immediate = false)
		{
			m_EventQueue.Add(new InfoEvent(_infoType, _textId, _mode, _headerTextId, _texture, _stringMapper, _uniqueInfoId, _immediate));
		}

		private void OnEntityRegistered(IEntity _entity)
		{
			m_EventQueue.Add(new EntityRegisteredEvent(_entity.Id, true));
		}

		private void OnEntityUnregistered(IEntity _entity)
		{
			m_EventQueue.Add(new EntityRegisteredEvent(_entity.Id, false));
		}

		private void OnPlayerMainStateChanged(IStateMachine _stateMachine, EStateType _from, EStateType _to)
		{
			InvokePlayerStateChangeEvent(_to, false);
		}

		private void InvokePlayerStateChangeEvent(EStateType _to, bool _switchedMovementPrevented)
		{
			if (m_PlayerCharacter.EntityStats.Get_bool(StatTypes.EStatType.Player_MovementPrevented) && _switchedMovementPrevented)
			{
				m_EventQueue.Add(new PlayerStateChangeEvent(EPlayerState.PreventMovement));
				return;
			}
			switch (_to)
			{
			case EStateType.PlayerMain_ObjectInteraction:
				m_EventQueue.Add(new PlayerStateChangeEvent(EPlayerState.ObjectInteraction));
				break;
			case EStateType.Generic_WaitingForAnim:
				m_EventQueue.Add(new PlayerStateChangeEvent(EPlayerState.Undefined));
				break;
			case EStateType.Generic_WaitingForAnimCancelable:
				m_EventQueue.Add(new PlayerStateChangeEvent(EPlayerState.Undefined));
				break;
			case EStateType.CombatCharacterMain_Roaming:
			case EStateType.PlayerMain_HidingInObjectExitAndMoveTo:
				m_EventQueue.Add(new PlayerStateChangeEvent(EPlayerState.Roaming));
				break;
			case EStateType.CombatCharacterMain_Combat:
				m_EventQueue.Add(new PlayerStateChangeEvent(EPlayerState.Combat));
				break;
			case EStateType.PlayerMain_HidingInObject:
				if (m_PlayerCharacter.GetIsInHaystack())
				{
					m_EventQueue.Add(new PlayerStateChangeEvent(EPlayerState.HidingInHaystack));
				}
				else if (m_PlayerCharacter.GetIsInRooftopGarden())
				{
					m_EventQueue.Add(new PlayerStateChangeEvent(EPlayerState.HidingInRooftopGarden));
				}
				else if (m_PlayerCharacter.GetIsOnBench())
				{
					m_EventQueue.Add(new PlayerStateChangeEvent(EPlayerState.HidingOnBench));
				}
				break;
			case EStateType.PlayerMain_HidingInPatrol:
				m_EventQueue.Add(new PlayerStateChangeEvent(EPlayerState.HidingInPatrol));
				break;
			case EStateType.PlayerMain_NpcInteraction:
				m_EventQueue.Add(new PlayerStateChangeEvent(EPlayerState.NpcInteraction));
				break;
			case EStateType.PlayerMain_BeingChased:
				m_EventQueue.Add(new PlayerStateChangeEvent(EPlayerState.BeingChased));
				break;
			case EStateType.PlayerMain_AssassinationStealth:
				m_EventQueue.Add(new PlayerStateChangeEvent(EPlayerState.Assassination));
				break;
			case EStateType.PlayerMain_AssassinationLeap:
				m_EventQueue.Add(new PlayerStateChangeEvent(EPlayerState.Assassination));
				break;
			case EStateType.PlayerMain_AssassinationAir:
				m_EventQueue.Add(new PlayerStateChangeEvent(EPlayerState.Assassination));
				break;
			case EStateType.CombatCharacterMain_Parcour:
				m_EventQueue.Add(new PlayerStateChangeEvent(EPlayerState.Parcour));
				break;
			}
		}

		public void FireEvents()
		{
			for (int i = 0; i < m_EventQueue.Count; i++)
			{
				Messenger.Instance.Broadcast(m_EventQueue[i]);
			}
			m_EventQueue.ClearIfNotEmpty();
		}

		public void InvokeSkillAvailabilityChanged(Character _source, Enum<SkillTypes> _skillType, bool _available, ESkillAvailabilityFlags _availabilityFlags)
		{
			if (CombatController.IsActive && !_source.GetDeadOrAboutToDie())
			{
				m_EventQueue.Add(CombatUiEvent.CreateSkillAvailabilityEvent(_source, _skillType, _available));
			}
		}

		private bool CanBeTriggeredInternal(Enum<SkillTypes> _type, EntityIdSkillPayload _payload, out ESkillAvailabilityFlags _skillAvailabilityFlags)
		{
			_skillAvailabilityFlags = ESkillAvailabilityFlags.Unknown;
			SkillData.IApproachSkillDefinition approachSkillDefinition = _type.GetDefinition() as SkillData.IApproachSkillDefinition;
			if (!CombatController.IsActive && approachSkillDefinition != null)
			{
				if (m_PlayerCharacter.AssassinCharacterStateMachines.AssassinApproachingTarget.Reason == approachSkillDefinition.InteractionReason)
				{
					return false;
				}
				if (_payload.Id == EntityId.InvalidId)
				{
					_skillAvailabilityFlags = ESkillAvailabilityFlags.NoTargetSelected;
					return false;
				}
				return m_PlayerCharacter.AssassinInteraction.CanApproachNpc(_payload.Id, approachSkillDefinition.InteractionReason) && m_PlayerCharacter.AssassinCharacterStateMachines.AssassinApproachingTarget.EvaluateInteractionReason(_payload.Id, approachSkillDefinition.InteractionReason, out _skillAvailabilityFlags) == approachSkillDefinition.InteractionReason;
			}
			return m_PlayerCharacter.Skills.CanBeTriggered(_type, _payload, out _skillAvailabilityFlags);
		}

		private bool TriggerInternal(Enum<SkillTypes> _type, EntityIdSkillPayload _payload)
		{
			ESkillAvailabilityFlags _skillAvailabilityFlags;
			if (!CanBeTriggeredInternal(_type, _payload, out _skillAvailabilityFlags))
			{
				return false;
			}
			SkillData.IApproachSkillDefinition approachSkillDefinition = _type.GetDefinition() as SkillData.IApproachSkillDefinition;
			if (!CombatController.IsActive && approachSkillDefinition != null)
			{
				Character character = _payload.Id.TryGetEntity<Character>();
				if (character != null && character.IsSelectable())
				{
					UbiCore.Utils.Singleton<SelectionManager>.Instance.Selection = character;
					return m_PlayerCharacter.AssassinInteraction.ApproachNpc(_payload.Id, approachSkillDefinition.InteractionReason);
				}
				return false;
			}
			return m_PlayerCharacter.Skills.Trigger(_type, _payload);
		}

		public override void Attach(EntityWrapperComponent _wrapperComponent)
		{
		}

		public override void Detach()
		{
		}

		public override void OnActivate()
		{
			base.OnActivate();
			m_PlayerCharacter = LogicMain.PlayerEntity;
			IsActive = true;
			FirstTurn = true;
			Messenger.Instance.AddListener<StateChangedMessage<CharacterInteraction>>(OnCharacterInteraction);
			m_PlayerCharacter.PlayerCharacterStateMachines.PlayerMain.StateChanged += OnPlayerMainStateChanged;
			m_PlayerCharacter.Skills.OnSkillEvent += OnSkillEvent;
			Messenger.Instance.AddListener<BuffEvent>(OnBuffEvent);
			Messenger.Instance.AddListener<StatValueChangedEvent>(OnStatValueChangedEvent);
			LogicMain.EntityManager.OnEntityRegistered += OnEntityRegistered;
			LogicMain.EntityManager.OnEntityUnregistered += OnEntityUnregistered;
		}

		public override void OnUnregistered()
		{
			Messenger.Instance.RemoveListener<StateChangedMessage<CharacterInteraction>>(OnCharacterInteraction);
			m_PlayerCharacter.PlayerCharacterStateMachines.PlayerMain.StateChanged -= OnPlayerMainStateChanged;
			m_PlayerCharacter.Skills.OnSkillEvent -= OnSkillEvent;
			Messenger.Instance.RemoveListener<BuffEvent>(OnBuffEvent);
			LogicMain.EntityManager.OnEntityRegistered -= OnEntityRegistered;
			LogicMain.EntityManager.OnEntityUnregistered -= OnEntityUnregistered;
			m_EventQueue.Clear();
			m_PlayerCharacter = null;
		}

		public override void DoFrameUpdate(float _deltaTime)
		{
			Profiling.Start("LogicInterface::DoFrameUpdate()", float.MaxValue);
			if (LogicMain.IsInTurnUpdate && FirstTurn)
			{
				OnFirstTurn();
			}
			UpdateCache();
			base.DoFrameUpdate(_deltaTime);
			Profiling.Stop();
		}

		private void OnFirstTurn()
		{
			InvokePlayerStateChangeEvent(m_PlayerCharacter.PlayerCharacterStateMachines.PlayerMain.CurrentStateType, false);
			FirstTurn = false;
		}

		private int GetUniqueInfoId()
		{
			if (m_currentInfoId == int.MaxValue)
			{
				m_currentInfoId = 0;
			}
			return ++m_currentInfoId;
		}
	}
}
