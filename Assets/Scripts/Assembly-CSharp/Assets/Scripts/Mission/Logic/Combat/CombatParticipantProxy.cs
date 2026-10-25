using Assets.Scripts.Engine;
using Assets.Scripts.Mission.AI.Helper;
using Assets.Scripts.Mission.AI.System;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.Logic.Combat
{
	public class CombatParticipantProxy : Component
	{
		private CombatParticipant m_nextTarget;

		public CombatParticipant NextTarget
		{
			get
			{
				return m_nextTarget;
			}
			set
			{
				m_nextTarget = value;
				if (IsPlayer)
				{
					LogicMain.LogicInterface.Combat_Select();
				}
			}
		}

		public sealed override int Priority
		{
			get
			{
				return 11;
			}
		}

		public bool IsPlayer { get; private set; }

		public bool IsInCombat { get; set; }

		public Character Character
		{
			get
			{
				return GameEntity.Cast<Character>();
			}
		}

		public CombatParticipant CombatParticipant
		{
			get
			{
				CombatParticipant combatParticipant = null;
				if (IsInCombat)
				{
					int num = CombatController.Instance.ParticipantCount();
					for (int i = 0; i < num; i++)
					{
						if (combatParticipant != null)
						{
							break;
						}
						CombatParticipant participant = CombatController.Instance.GetParticipant(i);
						if (participant.Proxy == this)
						{
							combatParticipant = participant;
						}
					}
				}
				return combatParticipant;
			}
		}

		public CombatParticipantProxy(IEntity _entity)
			: base(_entity)
		{
			IsPlayer = _entity.Type == EntityTypes.EType.PlayerCharacter;
		}

		public bool CanCombatSkillReact(CharacterCondition.EActionAllowed _action)
		{
			CombatParticipant combatParticipant = CombatParticipant;
			if (!IsInCombat || combatParticipant == null)
			{
				return false;
			}
			CharacterCondition characterCondition = combatParticipant.Character.CharacterStateMachines.Cast<ICharacterConditionCarrier>().CharacterCondition;
			return characterCondition.IsActionAllowed(_action);
		}

		public override void OnRegistered()
		{
			base.OnRegistered();
			RequestBehaviorTreeHolder();
		}

		public override void OnUnregistered()
		{
			if (CombatController.IsActive)
			{
				CombatParticipant combatParticipant = CombatParticipant;
				if (combatParticipant != null)
				{
					combatParticipant.ExitCombat();
				}
			}
			base.OnUnregistered();
		}

		public IBehaviorTreeHolder RequestBehaviorTreeHolder()
		{
			NpcCharacter npcCharacter = Character as NpcCharacter;
			string text = null;
			if (npcCharacter != null)
			{
				text = npcCharacter.NpcDefinition.Definition.CombatBehaviorTreePath;
				if (text == string.Empty)
				{
					text = "AI/BehaviorTrees/Combat/Default";
				}
			}
			else
			{
				text = "AI/BehaviorTrees/Combat/Player";
			}
			return UbiCore.Utils.Singleton<BehaviorTreeService>.Instance.RequestBehaviorTreeHolder(text, BehaviorTreeCategory.CombatParticipant);
		}
	}
}
