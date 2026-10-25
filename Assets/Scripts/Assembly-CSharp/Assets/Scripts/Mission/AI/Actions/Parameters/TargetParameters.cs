using System.Reflection;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using Assets.Scripts.Mission.AI.NpcBehavior;
using Assets.Scripts.Mission.Logic.Entities;

namespace Assets.Scripts.Mission.AI.Actions.Parameters
{
	public class TargetParameters : ParameterContainer
	{
		public enum ETarget
		{
			StatTarget = 0,
			ConditionTarget = 1,
			EscorteeClosestEnemy = 2
		}

		public ETarget Target { get; set; }

		public TargetParameters()
		{
			Target = ETarget.StatTarget;
			base.ParameterDefinitions = new PropertyInfo[1] { GetType().GetProperty("Target") };
		}

		public GameEntity GetTarget(NpcTreeHolder _npcBehavior, NpcCharacter _character)
		{
			switch (Target)
			{
			case ETarget.StatTarget:
				return _npcBehavior.NpcDelegateHolder.Knowledge.TargetKnowledge.TargetEntity;
			case ETarget.ConditionTarget:
				return _npcBehavior.NpcDelegateHolder.Knowledge.CommonKnowledge.GetVictim();
			case ETarget.EscorteeClosestEnemy:
				return (GameEntity)(((object)_character.Patrol.VictimComponent.GetClosestEnemyInRange()) ?? ((object)LogicMain.PlayerEntity));
			default:
				return null;
			}
		}
	}
}
