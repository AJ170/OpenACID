using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Utilities;
using Database;
using UbiCore.Messenger;

namespace Assets.Scripts.Mission.UI.LogicInterface
{
	public interface ILogicInterface
	{
		bool IsRunning { get; }

		float GameTime { get; }

		int GameTurn { get; }

		float GameTurnProgress { get; }

		float LastGameTurnTime { get; }

		float TurnDuration { get; }

		EMissionType MissionType { get; }

		EntityId PlayerId { get; }

		string PlayerName { get; }

		bool IsPlayerBeingChased { get; }

		EMovementSpeed PlayerSpeed { get; }

		EMovementSpeed PlayerChosenSpeed { get; }

		EAlertLevel PlayerAlertLevel { get; }

		int PlayerNotoriety { get; }

		EntityId CurrentInteractionObject { get; }

		EntityId CurrentInteractionNpc { get; }

		EntityId SelectedEntityId { get; }

		bool Prototype_BuffTimerDisabled { get; set; }

		Enum<SkillTypes>[] SkillWheel { get; }

		Enum<SkillTypes> SelectedSkill { get; }

		void AddListener<T>(Messenger.EventCallback<T> _handler) where T : ILogicUiEvent;

		void RemoveListener<T>(Messenger.EventCallback<T> _handler) where T : ILogicUiEvent;

		bool IsPlayerSpeedAllowed(EMovementSpeed _speed);

		void SetPlayerSpeed(EMovementSpeed _speed);

		int RoundNotoriety(float _notoriety);

		bool Revive();

		EMapIconType GetMapIconType(EntityId _entityId);

		IReadonlyHumanoidDef GetHumanoidDef(EntityId _entityId);

		IReadonlyCharacterAppearance GetCharacterAppearance(EntityId _entityId);

		int GetThreatLevel(EntityId _entityId);

		string GetUnitName(EntityId _entityId);

		bool HasStatValue(EntityId _entityId, StatTypes.EStatType _statType);

		string GetStatValue_string(EntityId _entityId, StatTypes.EStatType _statType);

		int GetStatValue_int(EntityId _entityId, StatTypes.EStatType _statType);

		float GetStatValue_float(EntityId _entityId, StatTypes.EStatType _statType);

		bool GetStatValue_bool(EntityId _entityId, StatTypes.EStatType _statType);

		float GetNormalizedHealth(EntityId _entityId);

		void CharacterInteraction_AcceptOffer(EntityId _npcId);

		void CharacterInteraction_RejectOffer(EntityId _npcId);

		IActiveBuff[] GetBuffs(EntityId _entity);

		bool Has(Enum<SkillTypes> _type);

		bool CanBeSelected(Enum<SkillTypes> _type);

		bool Select(Enum<SkillTypes> _type);

		bool CanBeTriggered(Enum<SkillTypes> _type, out ESkillAvailabilityFlags _availabilityFlags);

		bool CanBeTriggered(Enum<SkillTypes> _type, EntityId _entityId, out ESkillAvailabilityFlags _availabilityFlags);

		bool CanBeTriggeredOnSelectedEntity(Enum<SkillTypes> _type, out ESkillAvailabilityFlags _availabilityFlags);

		bool Trigger(Enum<SkillTypes> _type);

		bool Trigger(Enum<SkillTypes> _type, EntityId _entityId);

		bool TriggerOnSelectedEntity(Enum<SkillTypes> _skillType);

		bool CanBeAborted(Enum<SkillTypes> _skillType);

		bool Abort(Enum<SkillTypes> _skillType);

		ESkillState GetState(Enum<SkillTypes> _type);

		bool GetIsActive(Enum<SkillTypes> _type);

		int GetActivityStart(Enum<SkillTypes> _type);

		int GetActivityEnd(Enum<SkillTypes> _type);

		int GetActivityDuration(Enum<SkillTypes> _type);

		bool GetIsCoolingDown(Enum<SkillTypes> _type);

		int GetCoolDownStart(Enum<SkillTypes> _type);

		int GetCoolDownEnd(Enum<SkillTypes> _type);

		int GetCoolDownDuration(Enum<SkillTypes> _type);

		GameID GetGameID(Enum<SkillTypes> _type);

		IEntityIcon CreateEntityIcon();

		void ReleaseEntityIcon(IEntityIcon _icon);

		void AddEntityIcon(EntityId _entityId, IEntityIcon _icon);

		void RemoveEntityIcon(EntityId _entityId, IEntityIcon _icon);

		bool GetDefaultEntityIconVisibility(EntityId _entityId);

		void SetDefaultEntityIconVisibility(EntityId _entityId, bool _visibility);

		void WidgetGotPressed(string _path);
	}
}
