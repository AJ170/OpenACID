using Assets.Scripts.Mission.AI.Actions.Parameters;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using Assets.Scripts.Mission.AI.NpcBehavior;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.System;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Mission.AI.Knowledge
{
	public class TargetKnowledge : NpcBehaviorComponent
	{
		public GameEntity TargetEntity
		{
			get
			{
				return base.NpcCharacter.GetTargetEntity();
			}
		}

		public PlayerCharacter TargetAsPlayer
		{
			get
			{
				return base.NpcCharacter.GetTargetAsPlayer();
			}
		}

		public float TargetNotoriety
		{
			get
			{
				return TargetEntity.EntityStats.Get_float(StatTypes.EStatType.Character_Conspicuous_Notoriety);
			}
		}

		public TargetKnowledge(NpcTreeHolder _npcTreeHolder)
			: base(_npcTreeHolder)
		{
		}

		public bool TargetInAlertedOrExposed()
		{
			return TargetAsPlayer.PlayerCharacterStateMachines.PlayerAlertLevel.CurrentStateType >= EStateType.CharacterAlertLevel_Alerted;
		}

		public bool TargetInAlerted()
		{
			return TargetAsPlayer.PlayerCharacterStateMachines.PlayerAlertLevel.CurrentStateType == EStateType.CharacterAlertLevel_Alerted;
		}

		public bool TargetChasedByEnemies()
		{
			return TargetAsPlayer.PlayerCharacterStateMachines.PlayerMain.CurrentStateType == EStateType.PlayerMain_BeingChased;
		}

		public bool TargetHidingInObject()
		{
			return TargetAsPlayer.PlayerCharacterStateMachines.PlayerMain.CurrentStateType == EStateType.PlayerMain_HidingInObject;
		}

		public bool TargetHiding()
		{
			return TargetAsPlayer.GetIsHiding();
		}

		public bool TargetRunning()
		{
			return TargetAsPlayer.GetIsRunning();
		}

		public bool TargetPlayingTask(IParameterContainer _taskParameters)
		{
			Assets.Scripts.Mission.AI.Actions.Parameters.AnimationTaskParameters animationTaskParameters = _taskParameters as Assets.Scripts.Mission.AI.Actions.Parameters.AnimationTaskParameters;
			return TargetPlayingTask(animationTaskParameters.Layer, animationTaskParameters.AnimationTaskType);
		}

		public bool TargetPlayingTask(AnimationTaskManager.ELayer _layer, AnimationTaskType _animationTaskType)
		{
			return TargetAsPlayer.UnityComponentCache.AnimationController.TaskManager.GetCurrentTaskType(_layer) == _animationTaskType;
		}

		public bool TargetInDisguise()
		{
			return TargetAsPlayer.GetIsInDisguise();
		}

		public bool TargetInRestrictedArea()
		{
			return TargetAsPlayer.PlayerStats.Get_bool(StatTypes.EStatType.Player_IsInRestrictedArea);
		}

		public bool TargetDead()
		{
			return TargetAsPlayer.GetIsDead();
		}

		public bool TargetAttackable()
		{
			return CharacterHelper.GetIsAttackable(TargetAsPlayer);
		}

		public bool TargetIsPerformingStrikingAssasination()
		{
			return TargetAsPlayer.GetIsPerformingStrikingAssassination();
		}

		public bool TargetEagleVisionIsActive()
		{
			return TargetAsPlayer.PlayerStats.Get_bool(StatTypes.EStatType.Player_EagleVision);
		}

		public bool TargetMaxAngleVertical(IParameterContainer _maxDeltaAngle)
		{
			AngleParameters maxDeltaAngle = (AngleParameters)_maxDeltaAngle;
			return TargetMaxAngleVertical(maxDeltaAngle);
		}

		private bool TargetMaxAngleVertical(AngleParameters _maxDeltaAngle)
		{
			float num = base.NpcCharacter.EntityStats.Get_float(StatTypes.EStatType.Character_Detection_DeltaAngleVertical);
			return (!_maxDeltaAngle.Absolute) ? (num < _maxDeltaAngle.Angle) : (Mathf.Abs(num) < Mathf.Abs(_maxDeltaAngle.Angle));
		}

		public bool IsTargetInParameterRange(IParameterContainer _range)
		{
			DetectionRangeParameters detectionRangeParameters = _range as DetectionRangeParameters;
			return IsTargetInRange(detectionRangeParameters.Range);
		}

		public bool IsTargetInRange(EDetectionRange _range)
		{
			EDetectionRange eDetectionRange = (EDetectionRange)base.NpcCharacter.EntityStats.Get_int(StatTypes.EStatType.Character_Detection_Range);
			return eDetectionRange <= _range;
		}

		public bool TargetInRangeNotHiding()
		{
			return IsTargetInRange(EDetectionRange.Extended) && !TargetAsPlayer.GetIsHiding();
		}

		public bool IsTargetInDistance(float _maxDistance)
		{
			float num = base.NpcCharacter.EntityStats.Get_float(StatTypes.EStatType.Character_Detection_Distance);
			return num <= _maxDistance;
		}

		public bool IsTargetInParameterDistance(IParameterContainer _distance)
		{
			DistanceParameters distanceParameters = _distance as DistanceParameters;
			return IsTargetInDistance(distanceParameters.Distance);
		}

		public bool IsTargetInCombatDistance()
		{
			EnemyCharacter enemyCharacter = base.NpcCharacter as EnemyCharacter;
			PlayerCharacter targetAsPlayer = TargetAsPlayer;
			Vector3 velocity = targetAsPlayer.UnityComponentCache.NavMeshAgent.velocity;
			Vector3 lhs;
			if (enemyCharacter.IsAttached)
			{
				lhs = enemyCharacter.UnityComponentCache.NavMeshAgent.velocity;
			}
			else
			{
				Vector3 vector = enemyCharacter.NavAgentSurrogate.SteeringTarget - enemyCharacter.Position;
				float magnitude = vector.magnitude;
				if (magnitude > 0.001f)
				{
					vector /= magnitude;
				}
				lhs = enemyCharacter.NavAgentSurrogate.Speed * vector;
			}
			Vector3 normalized = (enemyCharacter.Position - targetAsPlayer.Position).normalized;
			float num = 4f;
			num += (Vector3.Dot(velocity, normalized) - Vector3.Dot(lhs, normalized)) * 2.0933335f;
			num = Mathf.Clamp(num, 1f, 10f);
			if (!IsTargetInDistance(num))
			{
				return false;
			}
			NavMeshPath dummyPath = base.NpcRuntimeData.TargetKnowledge.DummyPath;
			return NavMesh.CalculatePath(enemyCharacter.Position, targetAsPlayer.Position, NavMeshLayers.WalkableLayerMaskPreset, dummyPath) && dummyPath.status == NavMeshPathStatus.PathComplete && dummyPath.CalculateLength() <= num;
		}

		public bool IsTargetInParameterMainState(IParameterContainer _state)
		{
			StateMachineParameters stateMachineParameters = _state as StateMachineParameters;
			return TargetAsPlayer.PlayerMainCarrier.PlayerMain.CurrentStateType == stateMachineParameters.State;
		}
	}
}
