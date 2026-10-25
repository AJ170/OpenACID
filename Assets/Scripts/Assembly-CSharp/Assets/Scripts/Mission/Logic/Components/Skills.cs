using System;
using AcierProtocol;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.Core;
using SharedBaseLib.System.Extensions;
using UbiCore.Messenger;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class Skills : FrameUpdateableComponent
	{
		private class SkillInfo
		{
			private readonly Skills m_Skills;

			private static readonly BBList<IEffectInstaceResult> s_EffectInstanceResults = new BBList<IEffectInstaceResult>();

			public Enum<SkillTypes> Type { get; private set; }

			public Enum<SkillTypes> BaseType
			{
				get
				{
					return Definition.BaseSkillType;
				}
			}

			public SkillData.ISkillDefinition Definition { get; private set; }

			public ESkillState State { get; set; }

			public int StartTime { get; private set; }

			public int ActivityEnd { get; private set; }

			public bool IsActive
			{
				get
				{
					return LogicMain.CurrentTurn <= ActivityEnd;
				}
			}

			public int CoolDownEnd { get; private set; }

			public bool IsCoolingDown
			{
				get
				{
					return LogicMain.CurrentTurn <= CoolDownEnd;
				}
			}

			public int BuffedRank
			{
				get
				{
					return Definition.Rank + RankBonus;
				}
			}

			public int RankBonus { get; set; }

			public Enum<SkillTypes> BuffedSkillType
			{
				get
				{
					return Definition.BaseSkillType.Plus(BuffedRank);
				}
			}

			public SkillData.ISkillDefinition BuffedDefinition
			{
				get
				{
					return BuffedSkillType.GetDefinition();
				}
			}

			public Enum<RuleTypes> BuffedRuleType
			{
				get
				{
					return BuffedSkillType.GetDefinition().RuleType;
				}
			}

			public RuleData.IRuleDefinition BuffedRuleDefinition
			{
				get
				{
					return BuffedRuleType.GetDefinition();
				}
			}

			public int BuffedSkillDuration
			{
				get
				{
					return m_Skills.GetBuffedSkillDuration(Type);
				}
			}

			public int BuffedCooldownDuration
			{
				get
				{
					return m_Skills.GetBuffedCooldownDuration(Type);
				}
			}

			public IRule BuffedRule
			{
				get
				{
					return m_Skills.RulesCarrier.Rules.RulesCollection.GetRule(BuffedRuleType);
				}
			}

			public ISkillPayload Payload { get; set; }

			public bool CanBeSelected
			{
				get
				{
					if (Definition.ActivationType != ESkillActivationType.SelectSlot)
					{
						return false;
					}
					return !IsCoolingDown && !IsActive && m_Skills.RulesCarrier.Rules.RulesCollection.GetRule(BuffedRuleType).GetAreCategoryFlagsMatching(m_Skills.RulesCarrier);
				}
			}

			public bool CanBeAborted
			{
				get
				{
					if (State == ESkillState.Triggered)
					{
						return true;
					}
					if (!Definition.Abortable)
					{
						return false;
					}
					return IsActive;
				}
			}

			public bool CanBeRemoved
			{
				get
				{
					return true;
				}
			}

			public SkillInfo(Enum<SkillTypes> _type, Skills _skills)
			{
				Type = _type;
				Definition = UbiCore.Utils.Singleton<SkillData>.Instance.GetDefinition(_type);
				m_Skills = _skills;
				Reset();
			}

			public TPayload GetPayload<TPayload>() where TPayload : class, ISkillPayload
			{
				return Payload as TPayload;
			}

			public bool CanInitiateCombat(ISkillPayload _payload)
			{
				AssassinCharacter assassinCharacter = m_Skills.Entity as AssassinCharacter;
				if (assassinCharacter == null)
				{
					return false;
				}
				IEntityIdSkillPayload entityIdSkillPayload = _payload as IEntityIdSkillPayload;
				if (entityIdSkillPayload == null)
				{
					return false;
				}
				EnemyCharacter enemyCharacter = entityIdSkillPayload.Id.TryGetEntity<EnemyCharacter>();
				if (enemyCharacter == null)
				{
					return false;
				}
				if (enemyCharacter.GetIsDead())
				{
					return false;
				}
				return BuffedDefinition.Categories == ETriggerCategoryFlags.InCombat && (ETriggerCategoryFlags.Roaming | ETriggerCategoryFlags.BeingChased).Include(ETriggerCategoryFlags.NonInteractive).Contains(m_Skills.RulesCarrier.Rules.CurrentTriggerCategoryFlags) && assassinCharacter.AssassinInteraction.CanApproachNpc(entityIdSkillPayload.Id, EInteractionReason.Combat);
			}

			public bool CanBeTriggered(ISkillPayload _payload, Predicate<ITrigger, IRulesCarrier> _predicate, out ESkillAvailabilityFlags _availabilityFlags)
			{
				_availabilityFlags = ESkillAvailabilityFlags.Unknown;
				if (CanInitiateCombat(_payload))
				{
					return true;
				}
				ISkillPayload payload = Payload;
				Payload = _payload;
				bool result = RulesHelper.IsFulfilled(m_Skills.RulesCarrier, BuffedRuleType, _predicate, out _availabilityFlags);
				Payload = payload;
				return result;
			}

			public bool MeetsActivityConditions(ISkillPayload _payload, out ESkillAvailabilityFlags _availabilityFlags)
			{
				ISkillPayload payload = Payload;
				Payload = _payload;
				bool result = RulesHelper.MeetsActivityConditions(m_Skills.RulesCarrier, BuffedRuleType, out _availabilityFlags);
				Payload = payload;
				return result;
			}

			public void Respawn()
			{
				if ((BuffedDefinition.ActivationType & ESkillActivationType.Passives) == 0)
				{
					Abort();
				}
				if (CoolDownEnd > LogicMain.CurrentTurn)
				{
					CoolDownEnd = ActivityEnd + 1;
					m_Skills.InvokeSkillEvent(new SkillEvent(m_Skills.Entity.Id, ESkillEventType.CooldownFinished, Type));
				}
			}

			public bool Abort()
			{
				bool result = false;
				if (State >= ESkillState.RuleApplied)
				{
					bool flag = AbortEffects();
					ActivityEnd = LogicMain.CurrentTurn + 1;
					CoolDownEnd = ((!flag) ? (ActivityEnd + Math.Max(BuffedCooldownDuration - BuffedSkillDuration, 1)) : (LogicMain.CurrentTurn + 2));
					result = true;
				}
				State = ESkillState.Idle;
				return result;
			}

			public void Remove()
			{
				AbortEffects();
				Reset();
				State = ESkillState.Removed;
			}

			public void FrameUpdate()
			{
				ESkillAvailabilityFlags _availabilityFlags;
				switch (State)
				{
				case ESkillState.Selected:
					if (!CanBeSelected)
					{
						m_Skills.Select(Enum<SkillTypes>.Invalid);
					}
					break;
				case ESkillState.Triggered:
					if (!CanBeTriggered(Payload, s_CanTriggerSkillPredicate.Value, out _availabilityFlags))
					{
						m_Skills.InvokeSkillEvent(new SkillEvent(m_Skills.Entity.Id, ESkillEventType.Abort, Type, Math.Max(LogicMain.CurrentTurn - StartTime, 0), _availabilityFlags));
						Reset();
					}
					break;
				case ESkillState.RuleApplied:
				{
					StartTime = LogicMain.CurrentTurn;
					ActivityEnd = StartTime + Math.Max(BuffedSkillDuration, 1);
					CoolDownEnd = Math.Max(StartTime + Math.Max(BuffedCooldownDuration, 1), ActivityEnd + 1);
					State = ESkillState.Active;
					NpcData.NpcType targetNpcType = null;
					IEntityIdSkillPayload payload = GetPayload<IEntityIdSkillPayload>();
					if (payload != null)
					{
						NpcCharacter npcCharacter = payload.Id.TryGetEntity<NpcCharacter>();
						if (npcCharacter != null)
						{
							targetNpcType = npcCharacter.NpcDefinition.Definition.Type;
						}
					}
					m_Skills.InvokeSkillEvent(new SkillEvent(m_Skills.Entity.Id, ESkillEventType.ActivityStart, Type, targetNpcType));
					break;
				}
				case ESkillState.Active:
					if (IsActive)
					{
						if (LogicMain.CurrentTurn - StartTime > 3 && !MeetsActivityConditions(Payload, out _availabilityFlags))
						{
							m_Skills.InvokeSkillEvent(new SkillEvent(m_Skills.Entity.Id, ESkillEventType.Abort, Type, Math.Max(LogicMain.CurrentTurn - StartTime, 0), _availabilityFlags));
							Abort();
						}
					}
					else
					{
						State = ESkillState.Idle;
					}
					break;
				}
				if (LogicMain.IsInTurnUpdate)
				{
					if (LogicMain.CurrentTurn == ActivityEnd)
					{
						m_Skills.InvokeSkillEvent(new SkillEvent(m_Skills.Entity.Id, ESkillEventType.ActivityEnd, Type, Math.Max(LogicMain.CurrentTurn - StartTime, 0)));
					}
					else if (LogicMain.CurrentTurn == CoolDownEnd)
					{
						m_Skills.InvokeSkillEvent(new SkillEvent(m_Skills.Entity.Id, ESkillEventType.CooldownFinished, Type));
					}
				}
			}

			public void Reset()
			{
				State = ESkillState.Idle;
				StartTime = -1;
				CoolDownEnd = -1;
				ActivityEnd = -1;
				Payload = null;
			}

			private bool AbortEffects()
			{
				s_EffectInstanceResults.ClearIfNotEmpty();
				Rules rules = m_Skills.RulesCarrier.Rules;
				for (int i = 0; i <= Definition.MaxRank; i++)
				{
					Enum<RuleTypes> ruleType = Definition.BaseSkillType.Plus(i).GetDefinition().RuleType;
					rules.AbortEffects(ruleType, s_EffectInstanceResults);
				}
				bool result = s_EffectInstanceResults.Count > 0;
				for (int j = 0; j < s_EffectInstanceResults.Count; j++)
				{
					ISkillEffectInstanceResult skillEffectInstanceResult = s_EffectInstanceResults[j] as ISkillEffectInstanceResult;
					if (skillEffectInstanceResult == null || !skillEffectInstanceResult.AllowsSkipCooldown)
					{
						result = false;
						break;
					}
				}
				return result;
			}
		}

		private class CanTriggerSkillPredicate
		{
			public virtual bool Value(ITrigger _trigger, IRulesCarrier _entity)
			{
				ISkillTrigger skillTrigger = _trigger as ISkillTrigger;
				if (skillTrigger != null && skillTrigger.DelayedCheck)
				{
					return false;
				}
				return true;
			}
		}

		private class CanTriggerSkillWithIgnoredTriggersPredicate : CanTriggerSkillPredicate
		{
			private readonly int[] m_IgnoredTriggerTypes;

			public CanTriggerSkillWithIgnoredTriggersPredicate(int[] _ignoredTriggerTypes)
			{
				m_IgnoredTriggerTypes = _ignoredTriggerTypes;
			}

			public override bool Value(ITrigger _trigger, IRulesCarrier _entity)
			{
				if (!base.Value(_trigger, _entity))
				{
					return false;
				}
				for (int i = 0; i < m_IgnoredTriggerTypes.Length; i++)
				{
					if (m_IgnoredTriggerTypes[i] == (int)_trigger.Type)
					{
						return false;
					}
				}
				return true;
			}
		}

		public delegate void SkillEventHandler(SkillEvent _event);

		private readonly BBList<SkillInfo> m_SkillInfos = new BBList<SkillInfo>();

		private readonly BBList<SkillInfo> m_ApproachSkillInfos = new BBList<SkillInfo>();

		private readonly SkillInfo[] m_SkillInfosByType = new SkillInfo[Enum<SkillTypes>.Count];

		private readonly Enum<SkillTypes>[] m_CurrentSkillRankMap = new Enum<SkillTypes>[Enum<SkillTypes>.Count];

		private readonly BBSimpleList<Enum<SkillTypes>> m_SkillSlots = new BBSimpleList<Enum<SkillTypes>>();

		private readonly BBSimpleList<Enum<SkillTypes>> m_SkillWheel = new BBSimpleList<Enum<SkillTypes>>();

		private readonly BBSimpleList<Enum<SkillTypes>> m_SkillSlotsHirelingPassive = new BBSimpleList<Enum<SkillTypes>>();

		private Enum<SkillTypes> m_SelectedType = Enum<SkillTypes>.Invalid;

		private static readonly CanTriggerSkillPredicate s_CanTriggerSkillPredicate;

		private static readonly CanTriggerSkillPredicate s_CanTriggerApproachSkillPredicate;

		private static readonly CanTriggerSkillPredicate s_CanTriggerApproachSkillPredicateSloppy;

		public Enum<SkillTypes>[] SkillSlots
		{
			get
			{
				return m_SkillSlots.ToArray();
			}
		}

		public Enum<SkillTypes>[] SkillSlotsHirelingPassive
		{
			get
			{
				return m_SkillSlotsHirelingPassive.ToArray();
			}
		}

		public Enum<SkillTypes>[] SkillWheel
		{
			get
			{
				return m_SkillWheel.ToArray();
			}
		}

		public SkillData.IApproachSkillDefinition ActiveApproachSkill
		{
			get
			{
				for (int i = 0; i < m_ApproachSkillInfos.Count; i++)
				{
					SkillInfo skillInfo = m_ApproachSkillInfos[i];
					if (skillInfo.State == ESkillState.Triggered || skillInfo.IsActive)
					{
						return (SkillData.ApproachSkillDefinition)skillInfo.Definition;
					}
				}
				return null;
			}
		}

		public override int Priority
		{
			get
			{
				return 33;
			}
		}

		public bool IsBlockingSkillActive
		{
			get
			{
				for (int i = 0; i < m_SkillInfos.Count; i++)
				{
					SkillInfo skillInfo = m_SkillInfos[i];
					if (skillInfo.Definition.Blocking && skillInfo.IsActive)
					{
						return true;
					}
				}
				return false;
			}
		}

		public Enum<SkillTypes> SelectedSkill
		{
			get
			{
				return m_SelectedType;
			}
		}

		public IRulesCarrier RulesCarrier
		{
			get
			{
				return (IRulesCarrier)Entity;
			}
		}

		private SkillInfo SelectedSkillInfo
		{
			get
			{
				if (m_SelectedType == Enum<SkillTypes>.Invalid)
				{
					return null;
				}
				return GetSkillInfo(m_SelectedType);
			}
		}

		public event SkillEventHandler OnSkillEvent
		{
			add
			{
				this.m_OnSkillEvent = (SkillEventHandler)Delegate.Combine(this.m_OnSkillEvent, value);
			}
			remove
			{
				this.m_OnSkillEvent = (SkillEventHandler)Delegate.Remove(this.m_OnSkillEvent, value);
			}
		}

		private event SkillEventHandler m_OnSkillEvent;

		public Skills(IEntity _entity)
			: base(_entity)
		{
		}

		static Skills()
		{
			s_CanTriggerSkillPredicate = new CanTriggerSkillWithIgnoredTriggersPredicate(new int[1] { 15 });
			s_CanTriggerApproachSkillPredicate = new CanTriggerSkillWithIgnoredTriggersPredicate(new int[2] { 16, 21 });
			s_CanTriggerApproachSkillPredicateSloppy = new CanTriggerSkillWithIgnoredTriggersPredicate(new int[4] { 16, 21, 15, 12 });
		}

		public SkillData.IApproachSkillDefinition GetAvailableApproachSkill(ISkillPayload _skillPayload, SkillData.IApproachSkillDefinition _preferredApproachSkillDefinition, bool _strict, out ESkillAvailabilityFlags _availabilityFlags)
		{
			_availabilityFlags = ESkillAvailabilityFlags.Unknown;
			bool flag = _preferredApproachSkillDefinition != null && (_preferredApproachSkillDefinition.ActivationType & ESkillActivationType.Slots) != 0;
			for (int i = 0; i < m_ApproachSkillInfos.Count; i++)
			{
				ESkillAvailabilityFlags _availabilityFlags2 = ESkillAvailabilityFlags.Unknown;
				SkillData.IApproachSkillDefinition approachSkillDefinition = (SkillData.IApproachSkillDefinition)m_ApproachSkillInfos[i].Definition;
				if ((approachSkillDefinition == _preferredApproachSkillDefinition || (!flag && (approachSkillDefinition.ActivationType & ESkillActivationType.Slots) == 0)) && CanBeTriggered(approachSkillDefinition.Type, _skillPayload, (!_strict) ? s_CanTriggerApproachSkillPredicateSloppy : s_CanTriggerApproachSkillPredicate, out _availabilityFlags2))
				{
					return approachSkillDefinition;
				}
				if (approachSkillDefinition == _preferredApproachSkillDefinition)
				{
					_availabilityFlags = _availabilityFlags2;
					break;
				}
			}
			return null;
		}

		public void Respawn()
		{
			for (int i = 0; i < m_SkillInfos.Count; i++)
			{
				SkillInfo skillInfo = m_SkillInfos[i];
				skillInfo.Respawn();
			}
		}

		public override void OnRegistered()
		{
			base.OnRegistered();
			IsActive = true;
			m_SkillSlots.Clear();
			m_SkillSlotsHirelingPassive.Clear();
			m_SkillWheel.Clear();
			for (int i = 0; i < Enum<SkillTypes>.Count; i++)
			{
				m_CurrentSkillRankMap[i] = Enum<SkillTypes>.Invalid;
			}
			IReadonlyStatCollectionValue readonlyStatCollectionValue = Entity.Definition.EntityDefinition.StatValueCollection.GetReadonlyStatCollectionValue(StatTypes.EStatType.Character_Skills);
			for (int j = 0; j < readonlyStatCollectionValue.Count; j++)
			{
				string value = readonlyStatCollectionValue.Get_string(j);
				if (string.IsNullOrEmpty(value))
				{
					m_SkillWheel.Add(Enum<SkillTypes>.Invalid);
					continue;
				}
				Enum<SkillTypes> @enum = readonlyStatCollectionValue.Get_string(j).ToEnum<SkillTypes>();
				SkillInfo skillInfo = new SkillInfo(@enum, this);
				m_SkillInfosByType[(int)@enum] = skillInfo;
				m_SkillInfos.Add(skillInfo);
				if (skillInfo.Definition is SkillData.ApproachSkillDefinition)
				{
					m_ApproachSkillInfos.Add(skillInfo);
				}
				SkillData.ISkillDefinition definition = skillInfo.Definition.BaseSkillType.GetDefinition();
				for (int k = 0; k <= definition.MaxRank; k++)
				{
					m_CurrentSkillRankMap[(int)definition.Type.Plus(k)] = skillInfo.Type;
				}
				switch (UbiCore.Utils.Singleton<SkillData>.Instance.GetDefinition(@enum).ActivationType)
				{
				case ESkillActivationType.TriggerSlot:
				case ESkillActivationType.SelectSlot:
					m_SkillSlots.Add(@enum);
					m_SkillWheel.Add(@enum);
					continue;
				case ESkillActivationType.PassiveHireling:
					m_SkillSlotsHirelingPassive.Add(@enum);
					continue;
				}
				if (definition.ForceSkillWheel)
				{
					m_SkillWheel.Add(@enum);
				}
			}
			m_SkillSlots.Sort(SkillTypeComparison);
			m_SkillSlotsHirelingPassive.Sort(SkillTypeComparison);
			m_ApproachSkillInfos.Sort(SkillInfoComparison);
		}

		public override void OnUnregistered()
		{
			m_SkillInfos.ClearIfNotEmpty();
			for (int i = 0; i < m_SkillInfosByType.Length; i++)
			{
				m_SkillInfosByType[i] = null;
			}
		}

		public int GetTriggeredSlotSkillsCount(Enum<SkillTypes> _ignoredSkill, ESkillActivationType _activationTypeFlags)
		{
			int num = 0;
			for (int i = 0; i < m_SkillInfos.Count; i++)
			{
				SkillInfo skillInfo = m_SkillInfos[i];
				if (!(skillInfo.Type == _ignoredSkill) && (_activationTypeFlags & skillInfo.Definition.ActivationType) != 0 && skillInfo.State == ESkillState.Triggered)
				{
					num++;
				}
			}
			return num;
		}

		public bool Select(Enum<SkillTypes> _type)
		{
			if (_type != Enum<SkillTypes>.Invalid && !CanBeSelected(_type))
			{
				return false;
			}
			if (_type != m_SelectedType)
			{
				SkillInfo selectedSkillInfo = SelectedSkillInfo;
				if (selectedSkillInfo != null)
				{
					selectedSkillInfo.State = ESkillState.Idle;
				}
				m_SelectedType = _type;
				selectedSkillInfo = SelectedSkillInfo;
				if (selectedSkillInfo != null)
				{
					selectedSkillInfo.State = ESkillState.Selected;
				}
				InvokeSkillEvent(new SkillEvent(Entity.Id, ESkillEventType.Selected, m_SelectedType));
			}
			return true;
		}

		public bool Trigger(Enum<SkillTypes> _type, ISkillPayload _payload)
		{
			ESkillAvailabilityFlags _availabilityFlags;
			if (CanBeTriggered(_type, _payload, out _availabilityFlags))
			{
				Select(Enum<SkillTypes>.Invalid);
				SkillInfo skillInfo = GetSkillInfo(_type);
				skillInfo.Reset();
				if (skillInfo.CanInitiateCombat(_payload))
				{
					AssassinApproachingTarget assassinApproachingTarget = ((AssassinCharacter)Entity).AssassinCharacterStateMachines.AssassinApproachingTarget;
					return assassinApproachingTarget.SetTarget(((IEntityIdSkillPayload)_payload).Id, EInteractionReason.Combat, _type);
				}
				skillInfo.State = ESkillState.Triggered;
				skillInfo.Payload = _payload;
				InvokeSkillEvent(new SkillEvent(Entity.Id, ESkillEventType.Triggered, _type));
				return true;
			}
			return false;
		}

		public bool Abort(Enum<SkillTypes> _type)
		{
			if (CanBeAborted(_type))
			{
				SkillInfo skillInfo = GetSkillInfo(_type);
				if (!skillInfo.Abort())
				{
					InvokeSkillEvent(new SkillEvent(Entity.Id, ESkillEventType.Abort, _type, 0, ESkillAvailabilityFlags.Unknown));
				}
				return true;
			}
			return false;
		}

		public bool Remove(Enum<SkillTypes> _type)
		{
			if (CanBeRemoved(_type))
			{
				SkillInfo skillInfo = GetSkillInfo(_type);
				if (skillInfo.State == ESkillState.RuleApplied)
				{
					InvokeSkillEvent(new SkillEvent(Entity.Id, ESkillEventType.ActivityStart, _type));
				}
				m_SkillSlots.Remove(_type);
				m_SkillSlotsHirelingPassive.Remove(_type);
				skillInfo.Remove();
				InvokeSkillEvent(new SkillEvent(Entity.Id, ESkillEventType.Remove, _type));
				return true;
			}
			return false;
		}

		public int GetBuffedSkillDuration(Enum<SkillTypes> _type)
		{
			return GetBuffedStat_int(_type, StatTypes.EStatType.Rule_SkillDuration);
		}

		public int GetBuffedCooldownDuration(Enum<SkillTypes> _type)
		{
			BuffedReadonlyStatValueCollection statValueCollection = GetSkillInfo(_type).BuffedRule.StatValueCollection;
			ISuperBuffsStatsCarrier superBuffsStatsCarrier = Entity as ISuperBuffsStatsCarrier;
			int num = statValueCollection.Get_int(StatTypes.EStatType.Rule_SkillDuration, (ISuperBuffStats)null);
			int num2 = statValueCollection.Get_int(StatTypes.EStatType.Rule_SkillDuration, superBuffsStatsCarrier);
			float num3 = statValueCollection.Get_float(StatTypes.EStatType.Rule_SkillCooldownRelativeDurationReduction, superBuffsStatsCarrier);
			int a = GetBuffedStat_int(_type, StatTypes.EStatType.Rule_SkillCooldownDuration) - num;
			a = Mathf.Max(a, 0);
			a -= Mathf.RoundToInt(num3 * (float)a);
			a = Mathf.Max(a, 1);
			return a + num2;
		}

		public IBuffedStatsCollectionHandle GetBuffedStatsCollectionHandle(Enum<SkillTypes> _type)
		{
			return GetSkillInfo(_type).BuffedRule.StatValueCollection.Apply(Entity as ISuperBuffsStatsCarrier);
		}

		public float GetBuffedStat_float(Enum<SkillTypes> _type, StatTypes.EStatType _statType)
		{
			return GetSkillInfo(_type).BuffedRule.StatValueCollection.Get_float(_statType, Entity as ISuperBuffsStatsCarrier);
		}

		public int GetBuffedStat_int(Enum<SkillTypes> _type, StatTypes.EStatType _statType)
		{
			return GetSkillInfo(_type).BuffedRule.StatValueCollection.Get_int(_statType, Entity as ISuperBuffsStatsCarrier);
		}

		public bool GetBuffedStat_bool(Enum<SkillTypes> _type, StatTypes.EStatType _statType)
		{
			return GetSkillInfo(_type).BuffedRule.StatValueCollection.Get_bool(_statType, Entity as ISuperBuffsStatsCarrier);
		}

		public Enum<SkillTypes> MapToCurrentSkillType(Enum<SkillTypes> _type)
		{
			if (_type == Enum<SkillTypes>.Invalid)
			{
				return Enum<SkillTypes>.Invalid;
			}
			return m_CurrentSkillRankMap[(int)_type];
		}

		public void AddRankBuff(Enum<SkillTypes> _type, int _value)
		{
			Enum<SkillTypes> type = MapToCurrentSkillType(_type);
			GetSkillInfo(type).RankBonus += _value;
		}

		public void RemoveRankBuff(Enum<SkillTypes> _type, int _value)
		{
			Enum<SkillTypes> type = MapToCurrentSkillType(_type);
			GetSkillInfo(type).RankBonus -= _value;
		}

		public bool Has(Enum<SkillTypes> _type)
		{
			SkillInfo skillInfo = GetSkillInfo(_type);
			return skillInfo != null && skillInfo.State != ESkillState.Removed;
		}

		public bool GetIsActive(Enum<SkillTypes> _type)
		{
			return Has(_type) && GetSkillInfo(_type).IsActive;
		}

		public int GetActivityStart(Enum<SkillTypes> _type)
		{
			return GetSkillInfo(_type).StartTime;
		}

		public int GetActivityEnd(Enum<SkillTypes> _type)
		{
			return GetSkillInfo(_type).ActivityEnd;
		}

		public bool GetIsCoolingDown(Enum<SkillTypes> _type)
		{
			return Has(_type) && GetSkillInfo(_type).IsCoolingDown;
		}

		public int GetCoolDownStart(Enum<SkillTypes> _type)
		{
			return GetSkillInfo(_type).StartTime;
		}

		public int GetCoolDownEnd(Enum<SkillTypes> _type)
		{
			return GetSkillInfo(_type).CoolDownEnd;
		}

		public int GetBuffedRank(Enum<SkillTypes> _type)
		{
			return GetSkillInfo(_type).BuffedRank;
		}

		public SkillData.ISkillDefinition GetBuffedSkillDefinition(Enum<SkillTypes> _type)
		{
			SkillInfo skillInfo = GetSkillInfo(_type);
			SkillData.ISkillDefinition result;
			if (skillInfo == null)
			{
				SkillData.ISkillDefinition skillDefinition = null;
				result = skillDefinition;
			}
			else
			{
				result = skillInfo.BuffedDefinition;
			}
			return result;
		}

		public IRule GetBuffedRule(Enum<SkillTypes> _type)
		{
			SkillData.ISkillDefinition buffedSkillDefinition = GetBuffedSkillDefinition(_type);
			if (buffedSkillDefinition == null)
			{
				return null;
			}
			RulesCollection rulesCollection = Entity.Definition.EntityDefinition.Cast<IRulesCollectionCarrier>().RulesCollection;
			return rulesCollection.GetRule(buffedSkillDefinition.RuleType);
		}

		public bool CanBeSelected(Enum<SkillTypes> _type)
		{
			if (!Has(_type))
			{
				return false;
			}
			return GetSkillInfo(_type).CanBeSelected;
		}

		public bool CanBeTriggered(Enum<SkillTypes> _type, ISkillPayload _payload, out ESkillAvailabilityFlags _availabilityFlags)
		{
			return CanBeTriggered(_type, _payload, s_CanTriggerSkillPredicate, out _availabilityFlags);
		}

		public bool CanBeAborted(Enum<SkillTypes> _type)
		{
			if (!Has(_type))
			{
				return false;
			}
			return GetSkillInfo(_type).CanBeAborted;
		}

		public bool CanBeRemoved(Enum<SkillTypes> _type)
		{
			if (!Has(_type))
			{
				return false;
			}
			return GetSkillInfo(_type).CanBeRemoved;
		}

		public ESkillState GetState(Enum<SkillTypes> _type)
		{
			return GetSkillInfo(_type).State;
		}

		public TPayload GetPayload<TPayload>(Enum<SkillTypes> _type) where TPayload : class, ISkillPayload
		{
			return GetSkillInfo(_type).GetPayload<TPayload>();
		}

		public override void DoFrameUpdate(float _deltaTime)
		{
			for (int i = 0; i < m_SkillInfos.Count; i++)
			{
				if (m_SkillInfos[i] != null)
				{
					m_SkillInfos[i].FrameUpdate();
				}
			}
			base.DoFrameUpdate(_deltaTime);
		}

		internal void SetRuleApplied(Enum<SkillTypes> _type)
		{
			SkillInfo skillInfo = GetSkillInfo(_type);
			if (Assets.Scripts.Engine.Helpers.Verify(skillInfo.State == ESkillState.Triggered))
			{
				skillInfo.State = ESkillState.RuleApplied;
			}
		}

		private bool CanBeTriggered(Enum<SkillTypes> _type, ISkillPayload _payload, CanTriggerSkillPredicate _predicate, out ESkillAvailabilityFlags _availabilityFlags)
		{
			_availabilityFlags = ESkillAvailabilityFlags.Unknown;
			if (!Has(_type))
			{
				_availabilityFlags |= ESkillAvailabilityFlags.NotEquipped;
				return false;
			}
			SkillData.ISkillDefinition definition = UbiCore.Utils.Singleton<SkillData>.Instance.GetDefinition(_type);
			if (definition.ActivationType == ESkillActivationType.Passive)
			{
				_availabilityFlags |= ESkillAvailabilityFlags.SkillIsPassive;
				return false;
			}
			if (definition.ActivationType == ESkillActivationType.SelectSlot && SelectedSkill != _type)
			{
				_availabilityFlags |= ESkillAvailabilityFlags.SlotSkillNotSelected;
				return false;
			}
			SkillInfo skillInfo = GetSkillInfo(_type);
			if (skillInfo.IsCoolingDown)
			{
				_availabilityFlags |= ESkillAvailabilityFlags.IsCoolingDown;
				return false;
			}
			if (skillInfo.IsActive)
			{
				_availabilityFlags |= ESkillAvailabilityFlags.IsActive;
				return false;
			}
			AssassinCharacter assassinCharacter = Entity as AssassinCharacter;
			if (assassinCharacter != null && !assassinCharacter.GetIsMovementAllowed())
			{
				PreventMovementExceptionTypes preventMovementExceptionTypes = (PreventMovementExceptionTypes)assassinCharacter.EntityStats.Get_int(StatTypes.EStatType.Player_MovementPreventedException);
				switch (definition.Type.Name)
				{
				default:
				{
					int num;
					if (num == 1)
					{
						if (preventMovementExceptionTypes != PreventMovementExceptionTypes.PreventMovementException_EagleVision)
						{
							return false;
						}
						break;
					}
					return false;
				}
				case "Call_Hireling":
					if (preventMovementExceptionTypes != PreventMovementExceptionTypes.PreventMovementException_Hireling)
					{
						return false;
					}
					break;
				}
			}
			return skillInfo.State == ESkillState.Idle && skillInfo.CanBeTriggered(_payload, _predicate.Value, out _availabilityFlags);
		}

		private SkillInfo GetSkillInfo(Enum<SkillTypes> _type)
		{
			Enum<SkillTypes> @enum = MapToCurrentSkillType(_type);
			if (@enum == Enum<SkillTypes>.Invalid)
			{
				return null;
			}
			return m_SkillInfosByType[(int)@enum];
		}

		private int SkillTypeComparison(Enum<SkillTypes> _left, Enum<SkillTypes> _right)
		{
			return _left.GetDefinition().Priority.CompareTo(_right.GetDefinition().Priority);
		}

		private int SkillInfoComparison(SkillInfo _left, SkillInfo _right)
		{
			return _left.Definition.Priority.CompareTo(_right.Definition.Priority);
		}

		private void InvokeSkillEvent(SkillEvent _event)
		{
			if (this.m_OnSkillEvent != null)
			{
				this.m_OnSkillEvent(_event);
			}
			Messenger.Instance.Broadcast(_event);
		}
	}
}
