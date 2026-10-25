using System.Collections.ObjectModel;
using AcierProtocol;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.StateMachines;

namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IReadOnlyMissionResult
	{
		int AssassinationCount { get; }

		int CombatCount { get; }

		int EnemiesKilledInCombatCount { get; }

		int GroundTurnCount { get; }

		int MaxGroundTurnCount { get; }

		int EnterGroundCount { get; }

		int EnterRoofCount { get; }

		int FledCount { get; }

		int EnemiesFledCount { get; }

		int EnemiesKilledCount { get; }

		int EnemiesKilledWithin10MinutesCount { get; }

		int EnemiesShreddedCount { get; }

		int FrightenedTurnCount { get; }

		int CausedSplinterBombBleedingTurnCount { get; }

		int TracedBySeekersTurnCount { get; }

		float LowestBaseHealth { get; }

		float LowestBaseHealthMax { get; }

		float RegularDamageDealtSum { get; }

		int RegularDamageDealtCount { get; }

		int UsedHidingSpotCount { get; }

		int SentCourtesanGroupsCount { get; }

		int UsedSkillsCount { get; }

		int KilledEnemiesBySkillCount { get; }

		int EnemiesOnRooftopsKilledCount { get; }

		int KilledEnemiesFromCivilianCoverCount { get; }

		int EnemiesKilledWithLowPlayerHealthCount { get; }

		int EnemiesKilledWithCombo { get; }

		int EnemiesKilledWithBomb { get; }

		int PlayerDeathsCount { get; }

		int PlayerDiedByOwnBombCount { get; }

		int LostCaptivesCount { get; }

		int SuccessfullyParried { get; }

		int SwitchedTarget { get; }

		int SuccessfullHirelingAttacks { get; }

		int CriticalHitCount { get; }

		int CrowsKilledCount { get; }

		ReadOnlyCollection<int> SucceededChallenges { get; }

		float Playtime { get; }

		long FailMissionID { get; }

		int FailMissionLocaID { get; }

		int TreasureChestsOpenedInMission { get; }

		int MessagesDeliveredInMission { get; }

		int AssassinatedMissionTargets { get; }

		int ConsumedLockPicks { get; }

		int ReviveWithAdColonyCount { get; }

		int GetNotoriety(EAlertLevel _level);

		int GetSkillUsageCount(string _skillName);

		int GetSkillUsageTurnDuration(string _skillName);

		int GetSkillUsageCount(string _skillName, NpcData.NpcType _targetNpcType);

		int GetUsedHotspotCount(EHotspot _hotspot);

		int GetHotspotDuration(EHotspot _hotspot);

		int GetKilledEnemyCount(EStateType _charCondition);

		int GetKilledEnemyCount(EKillType _primaryKillType);
	}
}
