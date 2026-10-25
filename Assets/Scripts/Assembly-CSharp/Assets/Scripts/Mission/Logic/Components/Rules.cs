using System.Collections.Generic;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class Rules : FrameUpdateableComponent
	{
		private class RuleInfo
		{
			public BBList<IEffectInstance> EffectInstances { get; private set; }

			public int LastApplicationTurn { get; set; }

			public RuleInfo()
			{
				EffectInstances = new BBList<IEffectInstance>();
				LastApplicationTurn = -2;
			}
		}

		private readonly RuleInfo[] m_RuleInfos = new RuleInfo[Enum<RuleTypes>.Count];

		private readonly BBList<IEffectInstance> m_EffectInstances = new BBList<IEffectInstance>();

		private ETriggerCategoryFlags m_CurrentTriggerCategoryFlags = ETriggerCategoryFlags.Other.Include(ETriggerCategoryFlags.None);

		private int m_CurrentTriggerCategoryFlagsUpdateTurn;

		private static readonly BBList<IEffectInstance> s_ReturnedEffectInstances = new BBList<IEffectInstance>();

		public IRulesCarrier RulesCarrier
		{
			get
			{
				return (IRulesCarrier)Entity;
			}
		}

		public ETriggerCategoryFlags CurrentTriggerCategoryFlags
		{
			get
			{
				if (m_CurrentTriggerCategoryFlagsUpdateTurn != LogicMain.CurrentTurn)
				{
					m_CurrentTriggerCategoryFlags = GetCurrentTriggerCategoryFlags();
					m_CurrentTriggerCategoryFlagsUpdateTurn = LogicMain.CurrentTurn;
				}
				return m_CurrentTriggerCategoryFlags;
			}
		}

		public RulesCollection RulesCollection
		{
			get
			{
				return Entity.Cast<IDefinitionCarrier>().Definition.EntityDefinition.Cast<IRulesCollectionCarrier>().RulesCollection;
			}
		}

		public override int Priority
		{
			get
			{
				return 32;
			}
		}

		public Rules(IEntity _entity)
			: base(_entity)
		{
		}

		public override void OnRegistered()
		{
			base.OnRegistered();
			IsActive = true;
			for (int i = 0; i < m_RuleInfos.Length; i++)
			{
				m_RuleInfos[i] = new RuleInfo();
			}
		}

		public override void DoFrameUpdate(float _deltaTime)
		{
			IEffectInstance effectInstance = null;
			s_ReturnedEffectInstances.ClearIfNotEmpty();
			RulesHelper.ApplyRules((IRulesCarrier)Entity, LogicMain.IsInTurnUpdate ? ETriggerFrequency.Turnwise : ETriggerFrequency.Framewise, s_ReturnedEffectInstances);
			for (int i = 0; i < s_ReturnedEffectInstances.Count; i++)
			{
				effectInstance = s_ReturnedEffectInstances[i];
				m_EffectInstances.Add(effectInstance);
				RuleInfo ruleInfo = m_RuleInfos[(int)effectInstance.Effect.Owner.Type];
				ruleInfo.EffectInstances.Add(effectInstance);
				ruleInfo.LastApplicationTurn = LogicMain.CurrentTurn;
			}
			for (int j = 0; j < m_EffectInstances.Count; j++)
			{
				effectInstance = m_EffectInstances[j];
				if (effectInstance.DemandAbortAll)
				{
					AbortEffects(effectInstance.Effect.Owner.Type, null);
					j = 0;
				}
				else if (!effectInstance.Update(RulesCarrier))
				{
					m_EffectInstances.RemoveAt(j);
					j--;
					m_RuleInfos[(int)effectInstance.Effect.Owner.Type].EffectInstances.Remove(effectInstance);
				}
			}
			base.DoFrameUpdate(_deltaTime);
		}

		public void AbortEffects(Enum<RuleTypes> _type, IList<IEffectInstaceResult> _results)
		{
			BBList<IEffectInstance> effectInstances = m_RuleInfos[(int)_type].EffectInstances;
			for (int i = 0; i < effectInstances.Count; i++)
			{
				effectInstances[i].Abort(RulesCarrier);
				if (_results != null && effectInstances[i].Result != null)
				{
					_results.Add(effectInstances[i].Result);
				}
				m_EffectInstances.Remove(effectInstances[i]);
			}
			effectInstances.ClearIfNotEmpty();
		}

		public int GetLastApplicationTurn(Enum<RuleTypes> _type)
		{
			return m_RuleInfos[(int)_type].LastApplicationTurn;
		}

		private ETriggerCategoryFlags GetCurrentTriggerCategoryFlags()
		{
			ETriggerCategoryFlags eTriggerCategoryFlags = ETriggerCategoryFlags.None;
			bool isWaitingForAnim = RulesCarrier.GetIsWaitingForAnim();
			switch ((!isWaitingForAnim) ? RulesCarrier.PlayerMainCarrier.PlayerMain.CurrentStateType : RulesCarrier.PlayerMainCarrier.PlayerMain.GetDelayedNextState(RulesCarrier.PlayerMainCarrier.PlayerMain.CurrentStateType))
			{
			case EStateType.CombatCharacterMain_Roaming:
				eTriggerCategoryFlags |= ETriggerCategoryFlags.Roaming;
				break;
			case EStateType.CombatCharacterMain_Combat:
				eTriggerCategoryFlags |= ETriggerCategoryFlags.InCombat;
				break;
			case EStateType.PlayerMain_HidingInObject:
				eTriggerCategoryFlags |= ETriggerCategoryFlags.HidingInObject;
				break;
			case EStateType.PlayerMain_HidingInPatrol:
				eTriggerCategoryFlags |= ETriggerCategoryFlags.HidingInPatrol;
				break;
			case EStateType.PlayerMain_BeingChased:
				eTriggerCategoryFlags |= ETriggerCategoryFlags.BeingChased;
				break;
			case EStateType.CombatCharacterMain_Parcour:
				eTriggerCategoryFlags |= ETriggerCategoryFlags.Parcour;
				break;
			default:
				eTriggerCategoryFlags |= ETriggerCategoryFlags.Other;
				break;
			}
			AssassinCharacter assassinCharacter = Entity as AssassinCharacter;
			if (assassinCharacter != null && assassinCharacter.GetIsStalking())
			{
				eTriggerCategoryFlags |= ETriggerCategoryFlags.HidingInStalkingZone;
			}
			if (isWaitingForAnim)
			{
				eTriggerCategoryFlags |= ETriggerCategoryFlags.NonInteractive;
			}
			ISkillsCarrier skillsCarrier = RulesCarrier as ISkillsCarrier;
			if (skillsCarrier == null || skillsCarrier.Skills.GetTriggeredSlotSkillsCount(SkillTypes.Invalid, ESkillActivationType.All) == 0)
			{
				eTriggerCategoryFlags |= ETriggerCategoryFlags.NoSkillFired;
			}
			if (RulesCarrier.EntityStats.Get_int(StatTypes.EStatType.Character_BirthTurn) != LogicMain.CurrentTurn)
			{
				eTriggerCategoryFlags |= ETriggerCategoryFlags.NoBirth;
			}
			return eTriggerCategoryFlags;
		}
	}
}
