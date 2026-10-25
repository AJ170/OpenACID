using System;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Actions;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Conditionals;
using Assets.Scripts.Mission.AI.Combat;
using Assets.Scripts.Mission.Logic.Combat.AI.Parameters;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Combat
{
	public class CombatParticipantDelegateHolder : IDelegateHolder
	{
		public static class ParameterHelper
		{
			public static readonly string[] ParameterTypeNames;

			public static readonly BBDictionary<int, string[]> DelegateMap;

			public static readonly BBDictionary<int, string[]> ParameterMap;

			private static readonly string[] m_actionParameters;

			private static readonly string[] m_conditionalParameters;

			static ParameterHelper()
			{
				m_actionParameters = new string[Enum.GetValues(typeof(EActionParameterizedType)).Length];
				m_actionParameters[0] = EParametersType.FloatParameters.ToString();
				m_actionParameters[1] = EParametersType.ActionQueuedTypeParameters.ToString();
				m_actionParameters[2] = EParametersType.BoolParameters.ToString();
				m_actionParameters[3] = EParametersType.BoolParameters.ToString();
				m_actionParameters[4] = EParametersType.FloatParameters.ToString();
				m_actionParameters[5] = EParametersType.FloatParameters.ToString();
				m_actionParameters[6] = EParametersType.FloatParameters.ToString();
				m_conditionalParameters = new string[Enum.GetValues(typeof(EConditionalParameterizedType)).Length];
				m_conditionalParameters[0] = EParametersType.CompareIntParameters.ToString();
				m_conditionalParameters[1] = EParametersType.CompareIntParameters.ToString();
				m_conditionalParameters[2] = EParametersType.ActiveCombatBehaviorParameters.ToString();
				m_conditionalParameters[6] = EParametersType.CompareFloatParameters.ToString();
				m_conditionalParameters[4] = EParametersType.CompareFloatParameters.ToString();
				m_conditionalParameters[5] = EParametersType.CompareFloatParameters.ToString();
				m_conditionalParameters[7] = EParametersType.CompareFloatParameters.ToString();
				m_conditionalParameters[3] = EParametersType.ActiveCombatBehaviorParameters.ToString();
				m_conditionalParameters[8] = EParametersType.FloatParameters.ToString();
				m_conditionalParameters[9] = EParametersType.CompareIntParameters.ToString();
				m_conditionalParameters[10] = EParametersType.FloatParameters.ToString();
				ParameterTypeNames = Enum.GetNames(typeof(EParametersType));
				ParameterMap = new BBDictionary<int, string[]>
				{
					{ 2, m_actionParameters },
					{ 9, m_conditionalParameters }
				};
				DelegateMap = new BBDictionary<int, string[]>
				{
					{
						1,
						Enum.GetNames(typeof(EActionType))
					},
					{
						2,
						Enum.GetNames(typeof(EActionParameterizedType))
					},
					{
						8,
						Enum.GetNames(typeof(EConditionalType))
					},
					{
						9,
						Enum.GetNames(typeof(EConditionalParameterizedType))
					}
				};
			}

			public static IParameterContainer CreateParameterContainer(EParametersType parametersType)
			{
				switch (parametersType)
				{
				case EParametersType.CompareFloatParameters:
					return new CompareFloatParameters();
				case EParametersType.CompareIntParameters:
					return new CompareIntParameters();
				case EParametersType.FloatParameters:
					return new FloatParameters();
				case EParametersType.IntParameters:
					return new IntParameters();
				case EParametersType.BoolParameters:
					return new BoolParameters();
				case EParametersType.RandomFloatParameters:
					return new RandomFloatParameters();
				case EParametersType.StringParameters:
					return new StringParameters();
				case EParametersType.ActiveCombatBehaviorParameters:
					return new ActiveCombatBehaviorParameters();
				case EParametersType.ActionQueuedTypeParameters:
					return new ActionQueuedTypeParameters();
				default:
					return null;
				}
			}
		}

		private BehaviorAction.DelegateType[] m_actionDelegates;

		private BehaviorAction.CancelDelegateType[] m_actionCancelDelegates;

		private BehaviorAction.AttachDelegateType[] m_actionAttachDelegates;

		private BehaviorActionParameterized.DelegateType[] m_actionParameterizedDelegates;

		private BehaviorActionParameterized.CancelDelegateType[] m_actionParameterizedCancelDelegates;

		private BehaviorActionParameterized.AttachDelegateType[] m_actionParameterizedAttachDelegates;

		private Conditional.DelegateType[] m_conditionalDelegates;

		private ConditionalParameterized.DelegateType[] m_conditionalParameterizedDelegates;

		private CombatParticipant CombatParticipant { get; set; }

		public CombatParticipantDelegateHolder()
		{
			CreateActionDelegates();
			CreateActionParameterizedDelegates();
			CreateConditionalDelegates();
			CreateConditionalParameterizedDelegates();
		}

		private BehaviorReturnCode BT_Flee(BehaviorReturnCode _lastreturncode)
		{
			if (_lastreturncode == BehaviorReturnCode.NotCalled)
			{
				CombatParticipant.WantsToFlee = true;
			}
			if (!CombatParticipant.WantsToFlee)
			{
				return BehaviorReturnCode.Failure;
			}
			if (CombatParticipant.IsInCombat)
			{
				return BehaviorReturnCode.Running;
			}
			return BehaviorReturnCode.Success;
		}

		private BehaviorReturnCode BT_QueueAction(BehaviorReturnCode _lastreturncode)
		{
			return QueueAction(CombatParticipant.EActionQueuedType.Attack, _lastreturncode);
		}

		private BehaviorReturnCode BT_CancelAction(BehaviorReturnCode _lastreturncode)
		{
			CombatParticipant.CancelAction();
			return BehaviorReturnCode.Success;
		}

		private BehaviorReturnCode BT_SetParry(IParameterContainer _container, BehaviorReturnCode _lastreturncode)
		{
			CombatParticipant.EnableParry = (_container as BoolParameters).Value;
			return BehaviorReturnCode.Success;
		}

		private BehaviorReturnCode BT_EnableDodge(BehaviorReturnCode _lastreturncode)
		{
			CombatParticipant.EnableDodge = true;
			return BehaviorReturnCode.Success;
		}

		private BehaviorReturnCode BT_SetParryKillMove(IParameterContainer _container, BehaviorReturnCode _lastreturncode)
		{
			CombatParticipant.EnableParryKillMove = (_container as BoolParameters).Value;
			return BehaviorReturnCode.Success;
		}

		private BehaviorReturnCode BT_Cooldown(IParameterContainer _container, BehaviorReturnCode _lastreturncode)
		{
			if (_lastreturncode != BehaviorReturnCode.Running)
			{
				CombatParticipant.SchedulerCanAttack = false;
				CombatParticipant.SchedulerCooldownMultiplier = (_container as FloatParameters).Value;
				CombatParticipant.SchedulerCooldownStartTime = LogicMain.CurrentTime;
				CombatParticipant.SchedulerPriority = CombatParticipant.Entity.EntityStats.Get_float(StatTypes.EStatType.Character_Conflict_BaseSchedulerPriorityInitial);
			}
			return (!CombatParticipant.SchedulerCanAttack) ? BehaviorReturnCode.Running : BehaviorReturnCode.Success;
		}

		private void BT_Cooldown_Cancel(IParameterContainer _container)
		{
			CombatParticipant.SchedulerCanAttack = true;
		}

		private BehaviorReturnCode BT_SetGlobalSchedulerCooldownMultiplier(IParameterContainer _container, BehaviorReturnCode _lastreturncode)
		{
			CombatParticipant.GlobalSchedulerCooldownMultiplier = (_container as FloatParameters).Value;
			return BehaviorReturnCode.Success;
		}

		private BehaviorReturnCode BT_SetGlobalFleeChanceMultiplier(IParameterContainer _container, BehaviorReturnCode _lastreturncode)
		{
			CombatParticipant.GlobalFleeChanceMultiplier = (_container as FloatParameters).Value;
			return BehaviorReturnCode.Success;
		}

		private BehaviorReturnCode BT_ConsumeAllyWasAttacked(BehaviorReturnCode _lastreturncode)
		{
			float globalAllyGotAttackedTime = CombatParticipant.CombatController.GlobalAllyGotAttackedTime;
			if (globalAllyGotAttackedTime + CombatParticipant.CombatController.EnemyBehaviourAllyFlagMaxAge < LogicMain.CurrentTime)
			{
				return BehaviorReturnCode.Failure;
			}
			return BT_ConsumeAllyTimeInternal();
		}

		private BehaviorReturnCode BT_ConsumeAllyWasComboKilled(BehaviorReturnCode _lastreturncode)
		{
			float globalAllyGotComboKilledTime = CombatParticipant.CombatController.GlobalAllyGotComboKilledTime;
			if (globalAllyGotComboKilledTime + CombatParticipant.CombatController.EnemyBehaviourAllyFlagMaxAge < LogicMain.CurrentTime)
			{
				return BehaviorReturnCode.Failure;
			}
			return BT_ConsumeAllyTimeInternal();
		}

		private BehaviorReturnCode BT_ConsumeAllyTimeInternal()
		{
			CombatParticipant.CombatController.GlobalAllyGotAttackedTime = float.MinValue;
			CombatParticipant.CombatController.GlobalAllyGotComboKilledTime = float.MinValue;
			return BehaviorReturnCode.Success;
		}

		private BehaviorReturnCode BT_ConsumeQueuedAllyDied(BehaviorReturnCode _lastreturncode)
		{
			if (CombatParticipant.AllyDiedFlag <= 0)
			{
				return BehaviorReturnCode.Failure;
			}
			CombatParticipant.AllyDiedFlag--;
			return BehaviorReturnCode.Success;
		}

		private BehaviorReturnCode BT_ConsumeQueuedAllyFled(BehaviorReturnCode _lastreturncode)
		{
			if (CombatParticipant.AllyFledFlag <= 0)
			{
				return BehaviorReturnCode.Failure;
			}
			CombatParticipant.AllyFledFlag--;
			return BehaviorReturnCode.Success;
		}

		private BehaviorReturnCode BT_QueueActionWithWarning(IParameterContainer _container, BehaviorReturnCode _lastreturncode)
		{
			if (_lastreturncode != BehaviorReturnCode.Running)
			{
				float value = (_container as FloatParameters).Value;
				float num = CombatParticipant.Entity.EntityStats.Get_float(StatTypes.EStatType.Character_Conflict_BaseAttackWarningDuration) * value;
				LogicMain.LogicInterface.Combat_PrepareAttack(CombatParticipant.Proxy.CombatParticipant.Character, (CombatParticipant.NextTarget == null) ? null : CombatParticipant.NextTarget.Character, num);
				CombatParticipant.WarningEndTime = LogicMain.CurrentTime + num;
			}
			return QueueAction(CombatParticipant.EActionQueuedType.Attack, _lastreturncode);
		}

		private BehaviorReturnCode BT_QueueAction(IParameterContainer _container, BehaviorReturnCode _lastreturncode)
		{
			CombatParticipant.EActionQueuedType value = (_container as ActionQueuedTypeParameters).Value;
			return QueueAction(value, _lastreturncode);
		}

		private bool BT_IsIdle()
		{
			return CombatParticipant.ActiveBehaviour.BehaviourType == ECombatBehaviourType.Idle;
		}

		private bool BT_IsTaunting()
		{
			return CombatParticipant.ActiveBehaviour.BehaviourType == ECombatBehaviourType.Taunt;
		}

		private bool BT_IsAttacked()
		{
			return CombatParticipant.IsAttackedFlag;
		}

		private bool BT_AttackedByKillMove()
		{
			return CombatParticipant.LastAttacker != null && CombatParticipant.LastAttacker.AttackWasKillMove;
		}

		private bool BT_AttackedByComboCount(IParameterContainer _container)
		{
			return CombatParticipant.LastAttacker != null && (_container as CompareIntParameters).Evaluate(CombatParticipant.LastAttacker.AttackComboCount);
		}

		private bool BT_ActionQueued()
		{
			return CombatParticipant.ActionQueued;
		}

		private bool BT_DodgeRoll()
		{
			return BT_DodgeRollInternal(1f, true);
		}

		private bool BT_DodgeRollNoFeint(IParameterContainer _container)
		{
			float value = (_container as FloatParameters).Value;
			return BT_DodgeRollInternal(value, false);
		}

		private bool BT_DodgeRollInternal(float _dodgeFactor, bool _applyFeint)
		{
			if (CombatParticipant.LastAttacker == null)
			{
				return false;
			}
			if (CombatParticipant.LastAttacker.AttackWasKillMove)
			{
				return false;
			}
			CharacterCondition characterCondition = CombatParticipant.Character.CharacterStateMachines.Cast<ICharacterConditionCarrier>().CharacterCondition;
			if (!characterCondition.IsAnimationAllowed(CharacterCondition.EAnimAllowed.Dodge))
			{
				return false;
			}
			if (!CombatHelper.DecideToDodge(CombatParticipant.LastAttacker, CombatParticipant, _dodgeFactor, _applyFeint))
			{
				return false;
			}
			return true;
		}

		private bool BT_FleeRoll(IParameterContainer _container)
		{
			return CombatHelper.DecideToFlee(CombatParticipant, (_container as FloatParameters).Value);
		}

		private bool BT_TargetActiveCombatBehavior(IParameterContainer _container)
		{
			CombatParticipant combatParticipant = CombatParticipant.CurrentTarget ?? CombatParticipant.NextTarget;
			if (combatParticipant == null)
			{
				return false;
			}
			ActiveCombatBehaviorParameters activeCombatBehaviorParameters = _container as ActiveCombatBehaviorParameters;
			return combatParticipant.ActiveBehaviour.BehaviourType == activeCombatBehaviorParameters.Value;
		}

		private bool BT_EnemyCount(IParameterContainer _container)
		{
			return (_container as CompareIntParameters).Evaluate((!CombatParticipant.IsPlayer) ? 1 : (CombatParticipant.CombatController.ParticipantCount() - 1));
		}

		private bool BT_AllyCount(IParameterContainer _container)
		{
			return (_container as CompareIntParameters).Evaluate((!CombatParticipant.IsPlayer) ? (CombatParticipant.CombatController.ParticipantCount() - 2) : 0);
		}

		private bool BT_ActiveCombatBehavior(IParameterContainer _container)
		{
			ActiveCombatBehaviorParameters activeCombatBehaviorParameters = _container as ActiveCombatBehaviorParameters;
			return CombatParticipant.ActiveBehaviour.BehaviourType == activeCombatBehaviorParameters.Value;
		}

		private bool BT_RandomChance(IParameterContainer _container)
		{
			CompareFloatParameters compareFloatParameters = _container as CompareFloatParameters;
			return compareFloatParameters.Evaluate(LogicMain.Random.RandF());
		}

		private bool BT_TargetFacingAngle(IParameterContainer _container)
		{
			CombatParticipant combatParticipant = CombatParticipant.CurrentTarget ?? CombatParticipant.NextTarget;
			if (combatParticipant == null)
			{
				return false;
			}
			Vector3 to = -(combatParticipant.GameObject.transform.position - CombatParticipant.GameObject.transform.position);
			to.y = 0f;
			CompareFloatParameters compareFloatParameters = _container as CompareFloatParameters;
			return compareFloatParameters.Evaluate(Vector3.Angle(combatParticipant.GameObject.transform.forward, to));
		}

		private bool BT_HPPercent(IParameterContainer _container)
		{
			float v = CombatParticipant.Entity.EntityStats.Get_float(StatTypes.EStatType.Character_Conflict_Health) / CombatParticipant.Entity.EntityStats.Get_float(StatTypes.EStatType.Character_Conflict_BaseHealth);
			CompareFloatParameters compareFloatParameters = _container as CompareFloatParameters;
			return compareFloatParameters.Evaluate(v);
		}

		private bool BT_HPPercentTarget(IParameterContainer _container)
		{
			CombatParticipant combatParticipant = CombatParticipant.CurrentTarget ?? CombatParticipant.NextTarget;
			if (combatParticipant == null)
			{
				return false;
			}
			float v = combatParticipant.Entity.EntityStats.Get_float(StatTypes.EStatType.Character_Conflict_Health) / combatParticipant.Entity.EntityStats.Get_float(StatTypes.EStatType.Character_Conflict_BaseHealth);
			CompareFloatParameters compareFloatParameters = _container as CompareFloatParameters;
			return compareFloatParameters.Evaluate(v);
		}

		private BehaviorReturnCode QueueAction(CombatParticipant.EActionQueuedType _actionQueuedType, BehaviorReturnCode _lastreturncode)
		{
			if (_lastreturncode != BehaviorReturnCode.Running && !CombatParticipant.QueueAction(_actionQueuedType))
			{
				return BehaviorReturnCode.Failure;
			}
			return (!CombatParticipant.ActionQueued) ? BehaviorReturnCode.Success : BehaviorReturnCode.Running;
		}

		public void Prepare(CombatParticipant _combatParticipant)
		{
			CombatParticipant = _combatParticipant;
		}

		public BehaviorAction.DelegateType GetActionDelegate(string _name)
		{
			return m_actionDelegates[ConvertActionStringToIndex(_name)];
		}

		public BehaviorAction.CancelDelegateType GetActionCancelDelegate(string _name)
		{
			return m_actionCancelDelegates[ConvertActionStringToIndex(_name)];
		}

		public BehaviorAction.AttachDelegateType GetActionAttachDelegate(string _name)
		{
			return m_actionAttachDelegates[ConvertActionStringToIndex(_name)];
		}

		public BehaviorActionParameterized.DelegateType GetActionParameterizedDelegate(string _name)
		{
			return m_actionParameterizedDelegates[ConvertActionParameterizedStringToIndex(_name)];
		}

		public BehaviorActionParameterized.CancelDelegateType GetActionParameterizedCancelDelegate(string _name)
		{
			return m_actionParameterizedCancelDelegates[ConvertActionParameterizedStringToIndex(_name)];
		}

		public BehaviorActionParameterized.AttachDelegateType GetActionParameterizedAttachDelegate(string _name)
		{
			return m_actionParameterizedAttachDelegates[ConvertActionParameterizedStringToIndex(_name)];
		}

		public Conditional.DelegateType GetConditionalDelegate(string _name)
		{
			return m_conditionalDelegates[ConvertConditionalStringToIndex(_name)];
		}

		public ConditionalParameterized.DelegateType GetConditionalParameterizedDelegate(string _name)
		{
			return m_conditionalParameterizedDelegates[ConvertConditionalParameterizedStringToIndex(_name)];
		}

		public IParameterContainer CreateActionParameterContainer(int _index)
		{
			string[] array = ParameterHelper.ParameterMap[2];
			return CreateParameterContainer(array[_index]);
		}

		public IParameterContainer CreateConditionalParameterContainer(int _index)
		{
			string[] array = ParameterHelper.ParameterMap[9];
			return CreateParameterContainer(array[_index]);
		}

		public IParameterContainer CreateParameterContainer(string _parameterName)
		{
			EParametersType parametersType = (EParametersType)(int)Enum.Parse(typeof(EParametersType), _parameterName);
			return ParameterHelper.CreateParameterContainer(parametersType);
		}

		public string ConvertActionIndexToString(int _index)
		{
			return ((EActionType)_index).ToString();
		}

		public string ConvertActionParameterizedIndexToString(int _index)
		{
			return ((EActionParameterizedType)_index).ToString();
		}

		public string ConvertConditionalIndexToString(int _index)
		{
			return ((EConditionalType)_index).ToString();
		}

		public string ConvertConditionalParameterizedIndexToString(int _index)
		{
			return ((EConditionalParameterizedType)_index).ToString();
		}

		public int ConvertActionStringToIndex(string _name)
		{
			return (int)Enum.Parse(typeof(EActionType), _name);
		}

		public int ConvertActionParameterizedStringToIndex(string _name)
		{
			return (int)Enum.Parse(typeof(EActionParameterizedType), _name);
		}

		public int ConvertConditionalStringToIndex(string _name)
		{
			return (int)Enum.Parse(typeof(EConditionalType), _name);
		}

		public int ConvertConditionalParameterizedStringToIndex(string _name)
		{
			return (int)Enum.Parse(typeof(EConditionalParameterizedType), _name);
		}

		public string[] GetParameterTypeNames()
		{
			return ParameterHelper.ParameterTypeNames;
		}

		public string[] GetDelegateNames(EBehaviorType _behaviorType)
		{
			if (ParameterHelper.DelegateMap.ContainsKey((int)_behaviorType))
			{
				return ParameterHelper.DelegateMap[(int)_behaviorType];
			}
			return null;
		}

		public string[] GetParameterNames(EBehaviorType _behaviorType)
		{
			if (ParameterHelper.ParameterMap.ContainsKey((int)_behaviorType))
			{
				return ParameterHelper.ParameterMap[(int)_behaviorType];
			}
			return null;
		}

		public BehaviorAction.AttachDelegateType GetDefaultAttachDelegate()
		{
			return m_actionAttachDelegates[0];
		}

		private void CreateActionDelegates()
		{
			m_actionDelegates = new BehaviorAction.DelegateType[Enum.GetValues(typeof(EActionType)).Length];
			m_actionCancelDelegates = new BehaviorAction.CancelDelegateType[Enum.GetValues(typeof(EActionType)).Length];
			m_actionAttachDelegates = new BehaviorAction.AttachDelegateType[Enum.GetValues(typeof(EActionType)).Length];
			m_actionDelegates[0] = BT_Flee;
			m_actionDelegates[1] = BT_QueueAction;
			m_actionDelegates[2] = BT_CancelAction;
			m_actionDelegates[3] = BT_EnableDodge;
			m_actionDelegates[4] = BT_ConsumeAllyWasAttacked;
			m_actionDelegates[5] = BT_ConsumeAllyWasComboKilled;
			m_actionDelegates[6] = BT_ConsumeQueuedAllyDied;
			m_actionDelegates[7] = BT_ConsumeQueuedAllyFled;
		}

		private void CreateActionParameterizedDelegates()
		{
			m_actionParameterizedDelegates = new BehaviorActionParameterized.DelegateType[Enum.GetValues(typeof(EActionParameterizedType)).Length];
			m_actionParameterizedCancelDelegates = new BehaviorActionParameterized.CancelDelegateType[Enum.GetValues(typeof(EActionParameterizedType)).Length];
			m_actionParameterizedAttachDelegates = new BehaviorActionParameterized.AttachDelegateType[Enum.GetValues(typeof(EActionParameterizedType)).Length];
			m_actionParameterizedDelegates[0] = BT_QueueActionWithWarning;
			m_actionParameterizedDelegates[1] = BT_QueueAction;
			m_actionParameterizedDelegates[2] = BT_SetParry;
			m_actionParameterizedDelegates[3] = BT_SetParryKillMove;
			m_actionParameterizedDelegates[5] = BT_SetGlobalSchedulerCooldownMultiplier;
			m_actionParameterizedDelegates[6] = BT_SetGlobalFleeChanceMultiplier;
			m_actionParameterizedDelegates[4] = BT_Cooldown;
			m_actionParameterizedCancelDelegates[4] = BT_Cooldown_Cancel;
		}

		private void CreateConditionalDelegates()
		{
			m_conditionalDelegates = new Conditional.DelegateType[Enum.GetValues(typeof(EConditionalType)).Length];
			m_conditionalDelegates[4] = BT_ActionQueued;
			m_conditionalDelegates[2] = BT_IsAttacked;
			m_conditionalDelegates[3] = BT_AttackedByKillMove;
			m_conditionalDelegates[0] = BT_IsIdle;
			m_conditionalDelegates[1] = BT_IsTaunting;
			m_conditionalDelegates[5] = BT_DodgeRoll;
		}

		private void CreateConditionalParameterizedDelegates()
		{
			m_conditionalParameterizedDelegates = new ConditionalParameterized.DelegateType[Enum.GetValues(typeof(EConditionalParameterizedType)).Length];
			m_conditionalParameterizedDelegates[0] = BT_AllyCount;
			m_conditionalParameterizedDelegates[1] = BT_EnemyCount;
			m_conditionalParameterizedDelegates[2] = BT_ActiveCombatBehavior;
			m_conditionalParameterizedDelegates[4] = BT_HPPercent;
			m_conditionalParameterizedDelegates[5] = BT_HPPercentTarget;
			m_conditionalParameterizedDelegates[6] = BT_RandomChance;
			m_conditionalParameterizedDelegates[7] = BT_TargetFacingAngle;
			m_conditionalParameterizedDelegates[3] = BT_TargetActiveCombatBehavior;
			m_conditionalParameterizedDelegates[8] = BT_FleeRoll;
			m_conditionalParameterizedDelegates[9] = BT_AttackedByComboCount;
			m_conditionalParameterizedDelegates[10] = BT_DodgeRollNoFeint;
		}
	}
}
