using AcierProtocol;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class PlayerStatsHelper : StatsHelper<PlayerStatsHelper>
	{
		private const float StealthModififiedValueFactor = 0.5f;

		public PlayerStatsHelper()
			: base(new StatTypes.StatsRange(StatTypes.EStatType.__Start_Immutable_Player, StatTypes.EStatType.__End_Immutable_Player), new StatTypes.StatsRange(StatTypes.EStatType.__Start_Mutable_Player, StatTypes.EStatType.__End_Mutable_Player), new StatTypes.StatsRange(StatTypes.EStatType.__Start_Computed_Player, StatTypes.EStatType.__End_Computed_Player))
		{
		}

		public override void InitializeMutable(IStatValueCollection _stats, IInstanceCarrier _instanceCarrier)
		{
			base.InitializeMutable(_stats, _instanceCarrier);
			_stats.Set_bool(StatTypes.EStatType.Player_KillTokenModeActive, false);
			_stats.Set_int(StatTypes.EStatType.Player_KillTokenCount, 0);
			_stats.Set_float(StatTypes.EStatType.Player_KillTokenDecayClock, 0f);
			_stats.Set_bool(StatTypes.EStatType.Player_ProtectedFromDeath, true);
			_stats.Set_int(StatTypes.EStatType.Player_ClimbingHitCount, 0);
			_stats.Set_float(StatTypes.EStatType.Player_LastFallingHeight, 0f);
			_stats.Set_bool(StatTypes.EStatType.Player_SprintInHighProfileOverride, false);
		}

		public override void UpdateComputedTurn(IStatValueCollection _stats, IInstanceCarrier _instanceCarrier, int _deltaTurns)
		{
			EnemyCharacter enemyCharacter = _stats.Get_EntityId(StatTypes.EStatType.Player_ClosestEnemyThreateningOrNormal).TryGetEntity<EnemyCharacter>();
			AssassinCharacter assassinCharacter = _instanceCarrier.Entity.Cast<AssassinCharacter>();
			float _distance;
			int _awareCount;
			int _chasingCount;
			EnemyCharacter enemyCharacter2 = assassinCharacter.ComputeEnemyInformation(out _distance, out _awareCount, out _chasingCount);
			if (enemyCharacter != enemyCharacter2)
			{
				if (enemyCharacter != null)
				{
					enemyCharacter.CanBePooled = true;
				}
				if (enemyCharacter2 != null)
				{
					enemyCharacter2.CanBePooled = false;
				}
			}
			_stats.Set_EntityId(StatTypes.EStatType.Player_ClosestEnemyThreateningOrNormal, (enemyCharacter2 != null) ? enemyCharacter2.Id : EntityId.InvalidId);
			_stats.Set_float(StatTypes.EStatType.Player_ClosestEnemyDistance, (enemyCharacter2 != null) ? _distance : float.MaxValue);
			_stats.Set_float(StatTypes.EStatType.Player_ClosestEnemyDistanceSqr, (enemyCharacter2 != null) ? (_distance * _distance) : float.MaxValue);
			_stats.Set_int(StatTypes.EStatType.Player_AwareEnemies, _awareCount);
			_stats.Set_int(StatTypes.EStatType.Player_ChasingEnemies, _chasingCount);
			Vector3 position = _instanceCarrier.GameEntity.GameObjectProxy.Position;
			bool flag = UbiCore.Utils.Singleton<MissionManager>.Instance.AreaManager.IsInArea(position);
			if (_stats.Get_bool(StatTypes.EStatType.Player_IsInRestrictedArea) != flag)
			{
				_stats.Set_bool(StatTypes.EStatType.Player_IsInRestrictedArea, flag);
				if ((!flag || UbiCore.Utils.Singleton<MissionManager>.Instance.AreaManager.IsInArea(position, EMissionAreaType.Restricted, true)) && _instanceCarrier.GameEntity is PlayerCharacter)
				{
					LogicMain.LogicInterface.OnSendInfo((!flag) ? EInfoEventType.RestrictedAreaLeave : EInfoEventType.RestrictedAreaEnter);
				}
			}
			float incognito = _stats.Get_float(StatTypes.EStatType.Player_ActualIncognito);
			float num = DerivedStatsHelper.StealthModifier(incognito);
			_stats.Set_float(StatTypes.EStatType.Player_StealthModifier, num);
			float maxWaitAfterStareDuration = LogicMain.GameDefinition.MaxWaitAfterStareDuration;
			float value = ApplyStealthModifierInversed(maxWaitAfterStareDuration, num);
			_stats.Set_float(StatTypes.EStatType.Player_EnemyWaitAfterStareDuration, value);
			float maxPushDelay = LogicMain.GameDefinition.MaxPushDelay;
			float value2 = ApplyStealthModifier(maxPushDelay, num);
			_stats.Set_float(StatTypes.EStatType.Player_EnemyPushDelay, value2);
			float notorietyDecreaseDelay = LogicMain.GameDefinition.NotorietyDecreaseDelay;
			_stats.Set_float(StatTypes.EStatType.Player_NotorietyDecreaseDelay, notorietyDecreaseDelay);
			float minNotorietyInterval = LogicMain.GameDefinition.MinNotorietyInterval;
			float maxNotorietyInterval = LogicMain.GameDefinition.MaxNotorietyInterval;
			float slopeNotorietyInterval = LogicMain.GameDefinition.SlopeNotorietyInterval;
			float num2 = ApplyStealthModifier(minNotorietyInterval, maxNotorietyInterval, slopeNotorietyInterval, num);
			float num3 = assassinCharacter.PlayerStats.Get_float(StatTypes.EStatType.Character_Conspicuous_NotorietyIncreaseDelay);
			num2 *= 1f + num3 / 100f;
			_stats.Set_float(StatTypes.EStatType.Player_NotorietyInterval, num2);
			float minNotorietyDecreaseInterval = LogicMain.GameDefinition.MinNotorietyDecreaseInterval;
			float maxNotorietyDecreaseInterval = LogicMain.GameDefinition.MaxNotorietyDecreaseInterval;
			float slopeNotorietyDecreaseInterval = LogicMain.GameDefinition.SlopeNotorietyDecreaseInterval;
			float value3 = ApplyStealthModifierInversed(minNotorietyDecreaseInterval, maxNotorietyDecreaseInterval, num, slopeNotorietyDecreaseInterval);
			_stats.Set_float(StatTypes.EStatType.Player_NotorietyDecreaseInterval, value3);
			_stats.Set_bool(StatTypes.EStatType.Player_IsHiding, assassinCharacter.GetIsInHaystack() || assassinCharacter.GetIsOnBench() || assassinCharacter.GetIsInRooftopGarden() || assassinCharacter.GetIsHidingInPatrol() || assassinCharacter.GetIsStalking());
			_stats.Set_bool(StatTypes.EStatType.Player_Disguise, false);
			_stats.Set_bool(StatTypes.EStatType.Player_EagleVision, false);
			_stats.Copy(StatTypes.EStatType.Player_BaseIncognito, StatTypes.EStatType.Player_ActualIncognito);
			_stats.Set_float(StatTypes.EStatType.Player_EnemyEscapeKillTokenChance, 0f);
			_stats.Set_int(StatTypes.EStatType.Player_GlitchHidingSpots, 0);
			_stats.Copy(StatTypes.EStatType.Player_BaseFearsomeFactor, StatTypes.EStatType.Player_ActualFearsomeFactor);
			_stats.Set_bool(StatTypes.EStatType.Player_DoubleIdentityActive, false);
			_stats.Set_float(StatTypes.EStatType.Player_FallingDamageReductionFactor, 0f);
			_stats.Set_bool(StatTypes.EStatType.Player_WasJustRevived, false);
			_stats.Set_bool(StatTypes.EStatType.Player_Invulnerable, false);
		}

		private static float ApplyStealthModifier(float _max, float _stealthModifier)
		{
			float _min;
			float _remainder;
			float _remainderModified;
			ApplyStealthModifier(_max, _stealthModifier, out _min, out _remainder, out _remainderModified);
			return _min + _remainder - _remainderModified;
		}

		private static float ApplyStealthModifierInversed(float _max, float _stealthModifier)
		{
			float _min;
			float _remainder;
			float _remainderModified;
			ApplyStealthModifier(_max, _stealthModifier, out _min, out _remainder, out _remainderModified);
			return _min + _remainderModified;
		}

		private float ApplyStealthModifier(float _min, float _max, float _stealthModifier, float _slope)
		{
			float _remainder;
			float _remainderModified;
			ApplyStealthModifier(_min, _max, _stealthModifier, _slope, out _remainder, out _remainderModified);
			return Mathf.Max(_min, _min + _remainder - _remainderModified);
		}

		private float ApplyStealthModifierInversed(float _min, float _max, float _stealthModifier, float _slope)
		{
			float _remainder;
			float _remainderModified;
			ApplyStealthModifier(_min, _max, _stealthModifier, _slope, out _remainder, out _remainderModified);
			return Mathf.Min(_max, _min + _remainderModified);
		}

		private static void ApplyStealthModifier(float _max, float _stealthModifier, out float _min, out float _remainder, out float _remainderModified)
		{
			_min = _max * 0.5f;
			ApplyStealthModifier(_min, _max, 1f, _stealthModifier, out _remainder, out _remainderModified);
		}

		private static void ApplyStealthModifier(float _min, float _max, float _slope, float _stealthModifier, out float _remainder, out float _remainderModified)
		{
			_remainder = _max - _min;
			_remainderModified = _remainder / (_slope * _stealthModifier);
		}
	}
}
