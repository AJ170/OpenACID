using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using SharedBaseLib.System;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public static class CharacterDetectionStatsComputationHelper
	{
		private static readonly int s_raycastLayerMask;

		static CharacterDetectionStatsComputationHelper()
		{
			s_raycastLayerMask = Layers.StaticCollidersLayersMaskPreset;
		}

		public static void UpdateDetectionStats(NpcCharacter _entity, Character _target)
		{
			IStatValue statValue = _entity.EntityStats.GetStatValue(StatTypes.EStatType.Character_Detection_Range);
			EDetectionRange value_int = (EDetectionRange)statValue.Value_int;
			EDetectionRange eDetectionRange = EDetectionRange.FarAway;
			if (!_entity.GetIsDead())
			{
				Vector3 headPosition = _entity.GetHeadPosition();
				Vector3 headPosition2 = _target.GetHeadPosition();
				Vector3 direction = headPosition2 - headPosition;
				float magnitude = direction.magnitude;
				if (value_int == EDetectionRange.Close || magnitude < _entity.NpcDefinition.Definition.DetectionDistances[2])
				{
					IStatValue statValue2 = _entity.EntityStats.GetStatValue(StatTypes.EStatType.Character_Detection_Distance);
					IStatValue statValue3 = _entity.EntityStats.GetStatValue(StatTypes.EStatType.Character_Detection_DeltaAngle);
					IStatValue statValue4 = _entity.EntityStats.GetStatValue(StatTypes.EStatType.Character_Detection_DeltaAngleVertical);
					statValue2.Value_float = magnitude;
					statValue3.Value_float = Mathf.DeltaAngle(_entity.Rotation.eulerAngles.y, direction.AngleZX());
					float f = direction.y / magnitude;
					statValue4.Value_float = Mathf.Asin(f) * 57.29578f;
					bool flag = _entity.GetIsInCombat();
					bool flag2 = _entity.GetIsInCombat();
					bool targetIsStalking = IsTargetStalking(_target);
					for (int num = 2; num >= 0; num--)
					{
						EDetectionRange eDetectionRange2 = (EDetectionRange)LogicConstants.DetectionRangeValues[num];
						float num2 = CalcAudioDistance(_entity, _target, eDetectionRange2, targetIsStalking);
						float num3 = CalcRangeDistance(_entity, eDetectionRange2, targetIsStalking);
						if (magnitude > num2 && magnitude > num3)
						{
							break;
						}
						if (magnitude <= num2 || (Mathf.Abs(statValue3.Value_float) <= CalcDetectionAngle(_entity, eDetectionRange2) && Mathf.Abs(statValue4.Value_float) < CalcDetectionAngleVertical(_entity, eDetectionRange2)))
						{
							if (!flag)
							{
								flag = true;
								Vector3 vector = headPosition;
								Vector3 vector2 = headPosition2;
								Vector3 direction2 = vector2 - vector;
								flag2 = !Physics.Raycast(vector, direction2, direction2.magnitude, s_raycastLayerMask);
							}
							if (!flag2)
							{
								break;
							}
							eDetectionRange = eDetectionRange2;
						}
					}
					if (eDetectionRange <= EDetectionRange.Normal)
					{
						IStatValue statValue5 = _entity.EntityStats.GetStatValue(StatTypes.EStatType.Character_Detection_NormalRangeEnterTurn);
						statValue5.Value_int = ((value_int <= EDetectionRange.Normal) ? (-1) : LogicMain.CurrentTurn);
						if (eDetectionRange == EDetectionRange.Close)
						{
							IStatValue statValue6 = _entity.EntityStats.GetStatValue(StatTypes.EStatType.Character_Detection_CloseRangeEnterTurn);
							statValue6.Value_int = ((value_int <= EDetectionRange.Close) ? (-1) : LogicMain.CurrentTurn);
						}
					}
				}
			}
			if (eDetectionRange != value_int)
			{
				statValue.Value_int = (int)eDetectionRange;
			}
		}

		public static float CalcDetectionAngle(NpcCharacter _entity, EDetectionRange _detectionRange)
		{
			return _entity.NpcDefinition.Definition.DetectionAngles[(int)_detectionRange];
		}

		public static float CalcDetectionAngleVertical(NpcCharacter _entity, EDetectionRange _detectionRange)
		{
			return _entity.NpcDefinition.Definition.DetectionAnglesVertical[(int)_detectionRange];
		}

		public static float CalcRangeDistance(NpcCharacter _entity, EDetectionRange _detectionRange, bool _targetIsStalking)
		{
			float result = _entity.NpcDefinition.Definition.DetectionDistances[(int)_detectionRange];
			if (_targetIsStalking && _detectionRange == EDetectionRange.Close)
			{
				result = _entity.NpcDefinition.Definition.CloseRangeDistanceStalking;
			}
			return result;
		}

		public static float CalcAudioDistance(NpcCharacter _entity, Character _target, EDetectionRange _detectionRange, bool _targetIsStalking)
		{
			if (_detectionRange != 0)
			{
				return 0f;
			}
			EInteractionReason eInteractionReason = EInteractionReason.Invalid;
			IPlayerAlertlevelCarrier playerAlertlevelCarrier = _target.StateMachines as IPlayerAlertlevelCarrier;
			if (playerAlertlevelCarrier != null)
			{
				eInteractionReason = playerAlertlevelCarrier.PlayerAlertLevel.RegisteredInteraction;
			}
			if (eInteractionReason == EInteractionReason.Invalid)
			{
				AssassinCharacter assassinCharacter = _target as AssassinCharacter;
				if (assassinCharacter != null)
				{
					switch (assassinCharacter.AssassinCharacterStateMachines.PlayerMain.CurrentStateType)
					{
					case EStateType.CombatCharacterMain_Combat:
						eInteractionReason = EInteractionReason.Combat;
						break;
					case EStateType.PlayerMain_AssassinationLeap:
						eInteractionReason = EInteractionReason.AssassinationLeap;
						break;
					case EStateType.PlayerMain_AssassinationStealth:
						eInteractionReason = EInteractionReason.AssassinationStealth;
						break;
					case EStateType.PlayerMain_AssassinationAir:
						eInteractionReason = EInteractionReason.AssassinationAir;
						break;
					}
				}
			}
			float num = 1f;
			switch (eInteractionReason)
			{
			case EInteractionReason.AssassinationLeap:
			case EInteractionReason.AssassinationAir:
				num = _entity.NpcDefinition.Definition.CloseRangeFactorStrikingAssasination;
				break;
			case EInteractionReason.AssassinationStealth:
			case EInteractionReason.AssassinationStealthFallback:
				if (!IsTargetStalking(_target))
				{
					num = _entity.NpcDefinition.Definition.CloseRangeFactorStealthAssasination;
				}
				break;
			case EInteractionReason.Combat:
				num = _entity.NpcDefinition.Definition.CloseRangeFactorCombat;
				break;
			default:
				if (_targetIsStalking)
				{
					num = _entity.NpcDefinition.Definition.CloseRangeFactorStalking;
				}
				break;
			}
			return num * _entity.NpcDefinition.Definition.CloseRangeAudioDistance;
		}

		public static bool IsTargetStalking(Character _target)
		{
			AssassinCharacter assassinCharacter = _target as AssassinCharacter;
			return assassinCharacter != null && assassinCharacter.GetIsStalking();
		}
	}
}
