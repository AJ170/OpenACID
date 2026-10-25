using System;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.AI.Actions.Parameters;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using Assets.Scripts.Mission.AI.NpcBehavior;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.States;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using SharedBaseLib.System;
using UnityEngine;

namespace Assets.Scripts.Mission.AI.Knowledge
{
	public class CommonKnowledge : NpcBehaviorComponent
	{
		public CommonKnowledge(NpcTreeHolder _npcTreeHolder)
			: base(_npcTreeHolder)
		{
		}

		public bool IsAttached()
		{
			return base.NpcCharacter.IsAttached;
		}

		public bool CanActCommon()
		{
			return !base.NpcTreeHolder.NpcDelegateHolder.Knowledge.CommonKnowledge.IsDead() && !base.NpcTreeHolder.NpcDelegateHolder.Knowledge.CommonKnowledge.IsUnconscious() && !base.NpcTreeHolder.NpcDelegateHolder.Knowledge.CombatKnowledge.InCombat() && !base.NpcTreeHolder.NpcDelegateHolder.Knowledge.TargetKnowledge.TargetDead() && base.NpcCharacter.Patrol != null;
		}

		public bool IsEnemy()
		{
			return base.NpcCharacter.Type == EntityTypes.EType.EnemyCharacter;
		}

		public bool IsDead()
		{
			return base.NpcCharacter.GetDeadOrAboutToDie();
		}

		public bool IsUnconscious()
		{
			return base.NpcCharacter.GetIsUnconscious();
		}

		public bool IsSeducing()
		{
			return base.NpcCharacter.Patrol.IsSeducing();
		}

		public bool IsDisguised()
		{
			return base.NpcCharacter.GetIsDisguised();
		}

		public bool IsInNormalCondition()
		{
			return IsInCondition(EStateType.CharacterCondition_Normal);
		}

		public bool IsJackBoxed()
		{
			return IsInCondition(EStateType.CharacterCondition_JackBoxed) && !InCombatOrExposedInClosedRange();
		}

		public bool IsSeduced()
		{
			return IsInCondition(EStateType.CharacterCondition_Seduced) && !InCombatOrExposedInClosedRange();
		}

		public bool IsGreedy()
		{
			return IsInCondition(EStateType.CharacterCondition_Greedy) && !InCombatOrExposedInClosedRange();
		}

		public bool IsConfused()
		{
			return IsInCondition(EStateType.CharacterCondition_Confused);
		}

		public bool IsLuredBySweetMusic()
		{
			return IsInCondition(EStateType.CharacterCondition_LuredBySweetMusic) && !InCombatOrExposedInClosedRange();
		}

		public bool IsStickyBombPaniced()
		{
			return IsInCondition(EStateType.CharacterCondition_StickyBombPanic);
		}

		public bool IsThreateningVictim()
		{
			return IsInCondition(EStateType.CharacterCondition_ThreateningVictim);
		}

		public bool IsVictimThreatened()
		{
			Assets.Scripts.Mission.Logic.Entities.Patrol patrol = GetVictim().Patrol;
			IVictim victimComponent = patrol.VictimComponent;
			Escort escortComponent = patrol.EscortComponent;
			return victimComponent != null && (victimComponent.AreEnemiesThreatening || (escortComponent != null && escortComponent.ResumeRunning));
		}

		public bool IsThreateningFinalized()
		{
			IVictim victimComponent = GetVictim().Patrol.VictimComponent;
			return victimComponent != null && victimComponent.ThreateningFinished;
		}

		public bool IsVictimAttacked()
		{
			IVictim victimComponent = GetVictim().Patrol.VictimComponent;
			return victimComponent != null && victimComponent.IsVictimAttacked;
		}

		public bool IsVictimHit()
		{
			IVictim victimComponent = GetVictim().Patrol.VictimComponent;
			return victimComponent != null && victimComponent.IsVictimHit;
		}

		public bool HasArrivedAtVictim()
		{
			Assets.Scripts.Mission.Logic.Entities.Patrol patrol = GetVictim().Patrol;
			IVictim victimComponent = patrol.VictimComponent;
			return victimComponent != null && victimComponent.HasArrived(base.NpcCharacter.Id);
		}

		public bool IsFleeingFromCombat()
		{
			return IsFleeingFromCombat(FleeingConditionState.Reason.Fleeing);
		}

		public bool IsWalkingFromCombat()
		{
			return IsFleeingFromCombat(FleeingConditionState.Reason.WalkFromCombat);
		}

		private bool IsFleeingFromCombat(FleeingConditionState.Reason _reason)
		{
			ICharacterConditionCarrier characterConditionCarrier = base.NpcCharacter.CharacterStateMachines as ICharacterConditionCarrier;
			if (characterConditionCarrier == null || characterConditionCarrier.CharacterCondition.CurrentStateType != EStateType.CharacterCondition_Fleeing)
			{
				return false;
			}
			FleeingConditionState.Payload payload = characterConditionCarrier.CharacterCondition.GetPayload<FleeingConditionState.Payload>(EStateType.CharacterCondition_Fleeing);
			return payload.Reason == _reason;
		}

		public bool IsFleeingFromCombatViral()
		{
			bool result = false;
			float enemyFleeFromCombatViralDuration = LogicMain.GameDefinition.EnemyFleeFromCombatViralDuration;
			Buffs buffs = base.NpcCharacter.Buffs;
			int count = buffs.SortedBuffs.Count;
			for (int i = 0; i < count; i++)
			{
				IBuff buff = buffs.SortedBuffs[i];
				if (buff.Type == EBuffType.Fleeing)
				{
					int num = LogicMain.CurrentTurn - buff.StartTurn;
					float num2 = 0.1f * (float)num;
					result = num2 < enemyFleeFromCombatViralDuration;
					break;
				}
			}
			return result;
		}

		private bool IsInCondition(EStateType _condition)
		{
			ICharacterConditionCarrier characterConditionCarrier = base.NpcCharacter.CharacterStateMachines as ICharacterConditionCarrier;
			return characterConditionCarrier != null && characterConditionCarrier.CharacterCondition.CurrentStateType == _condition;
		}

		public bool IsCaptured()
		{
			return IsInCondition(EStateType.CharacterCondition_Captured);
		}

		public bool IsFleeingFromCapture()
		{
			bool result = false;
			if (IsCaptured())
			{
				Assets.Scripts.Mission.Logic.Entities.Patrol patrol = base.NpcCharacter.Patrol;
				if (patrol != null)
				{
					result = patrol.PatrolStats.Get_bool(StatTypes.EStatType.Interaction_Triggered);
				}
			}
			return result;
		}

		public bool IsSilenced()
		{
			EnemyCharacter enemyCharacter = base.NpcCharacter as EnemyCharacter;
			return enemyCharacter != null && enemyCharacter.GetIsSilenced();
		}

		public bool IsApproachedToBeAssasinated()
		{
			return base.NpcCharacter.GetIsApproachedToBeAssasinated();
		}

		public bool IsInInteractionCharacterMainState(IParameterContainer _stateMachineParameters)
		{
			StateMachineParameters stateMachineParameters = _stateMachineParameters as StateMachineParameters;
			return Assets.Scripts.Engine.Helpers.Verify(stateMachineParameters != null) && IsInteractionCharacterMainInState(stateMachineParameters.State);
		}

		public bool IsVictimType(IParameterContainer _victimParameters)
		{
			IVictim victimComponent = GetVictim().Patrol.VictimComponent;
			VictimParameters victimParameters = (VictimParameters)_victimParameters;
			switch (victimParameters.VictimType)
			{
			case EVictimType.Invalid:
				return victimComponent == null;
			case EVictimType.Escortee:
				return victimComponent is Escort;
			case EVictimType.Captive:
				return victimComponent is Captive;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public bool IsApproachedByAssassin(IParameterContainer _victimParameters)
		{
			return base.NpcCharacter.GetIsApproachedByAssassin(((InteractionReasonParameters)_victimParameters).InteractionReason);
		}

		public bool IsInteractionCharacterMainInState(EStateType _state)
		{
			return base.NpcCharacter.DoIfIs((InteractionCharacter c) => c.InteractionCharacterStateMachines.InteractionCharacterMain.CurrentStateType == _state);
		}

		public bool AwarenessLow()
		{
			return CheckAwareness(EStateType.CharacterAwareness_Low);
		}

		public bool AwarenessHigh()
		{
			return CheckAwareness(EStateType.CharacterAwareness_High);
		}

		public bool Alarmed()
		{
			return CheckAwareness(EStateType.CharacterAwareness_Alarmed);
		}

		public bool InCombatOrExposedInClosedRange()
		{
			return base.Knowledge.CombatKnowledge.CombatActive() || TargetExposedInClosedRange();
		}

		public bool TargetExposedInClosedRange()
		{
			TargetKnowledge targetKnowledge = base.Knowledge.TargetKnowledge;
			return Alarmed() && targetKnowledge.IsTargetInRange(EDetectionRange.Close) && !targetKnowledge.TargetHiding();
		}

		public bool IsWithinExtendedRangeOfPatrolPosition()
		{
			float maxDistance = base.NpcCharacter.NpcDefinition.Definition.DetectionDistances[2];
			return IsNearPatrolPosition(maxDistance);
		}

		public bool IsNearPatrolPosition(float _maxDistance)
		{
			NpcCharacter npcCharacter = base.NpcCharacter;
			Assets.Scripts.Mission.Logic.Entities.Patrol patrol = npcCharacter.Patrol;
			if (patrol != null)
			{
				Vector3 _position;
				patrol.PatrolComponent.GetTargetWorldPosition(npcCharacter.Id, out _position);
				return (_position - npcCharacter.Position).magnitude < _maxDistance;
			}
			return false;
		}

		public bool IsInCameraLOS()
		{
			NpcCharacter npcCharacter = base.NpcCharacter;
			Vector3 vector = npcCharacter.Position + Vector3.up * 1f;
			UnityEngine.Camera main = UnityEngine.Camera.main;
			Vector3 position = main.transform.position;
			Vector3 vector2 = vector - position;
			return GeometryUtility.TestPlanesAABB(AcierCameraController.Instance.LastCameraFrustum, new Bounds(vector, new Vector3(1f, 2f, 1f))) && !Physics.Raycast(main.transform.position, vector2.normalized, vector2.magnitude, Layers.StaticCollidersLayersMaskPreset);
		}

		public NpcCharacter GetVictim()
		{
			return GetNpcTargetFromCondition(EStateType.CharacterCondition_ThreateningVictim) ?? base.NpcCharacter;
		}

		public NpcCharacter GetNpcTargetFromCondition(EStateType _characterConditionState)
		{
			return GetTargetFromCondition(_characterConditionState) as NpcCharacter;
		}

		public GameEntity GetTargetFromCondition(EStateType _characterConditionState)
		{
			ICharacterConditionCarrier characterConditionCarrier = base.NpcCharacter.StateMachines as ICharacterConditionCarrier;
			if (characterConditionCarrier == null)
			{
				return null;
			}
			return characterConditionCarrier.CharacterCondition.GetTargetFromCondition(_characterConditionState).GameEntity;
		}

		private bool CheckAwareness(EStateType _checkedStateType)
		{
			EnemyCharacter enemyCharacter = base.NpcCharacter as EnemyCharacter;
			return enemyCharacter != null && enemyCharacter.EnemyCharacterStateMachines.CharacterAwareness.CurrentStateType == _checkedStateType;
		}
	}
}
