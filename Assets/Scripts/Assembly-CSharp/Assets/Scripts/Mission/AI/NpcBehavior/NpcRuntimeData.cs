using Assets.Scripts.Mission.AI.Actions;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using Assets.Scripts.Mission.Logic.Entities;

namespace Assets.Scripts.Mission.AI.NpcBehavior
{
	public class NpcRuntimeData : TreeRuntimeData
	{
		public NpcCharacter NpcCharacter { get; private set; }

		public ChaseActions.RuntimeData Chase { get; private set; }

		public RangedCombatActions.RuntimeData RangedCombat { get; private set; }

		public ReactOnTargetActions.RuntimeData ReactOnTarget { get; private set; }

		public SidetrackedActions.RuntimeData Sidetracked { get; private set; }

		public PatrolActions.RuntimeData Patrol { get; private set; }

		public NpcBehaviorComponent.BaseRuntimeData Courtesan { get; private set; }

		public NpcBehaviorComponent.BaseRuntimeData Mission { get; private set; }

		public FleeActions.RuntimeData Flee { get; private set; }

		public NpcBehaviorComponent.BaseRuntimeData TargetKnowledge { get; private set; }

		public void Init(NpcCharacter _npcCharacter)
		{
			NpcCharacter = _npcCharacter;
			Init();
		}

		public override void Init()
		{
			base.Init();
			Chase = new ChaseActions.RuntimeData(this);
			RangedCombat = new RangedCombatActions.RuntimeData(this);
			ReactOnTarget = new ReactOnTargetActions.RuntimeData(this);
			Sidetracked = new SidetrackedActions.RuntimeData(this);
			Patrol = new PatrolActions.RuntimeData(this);
			Courtesan = new NpcBehaviorComponent.BaseRuntimeData(this);
			Mission = new NpcBehaviorComponent.BaseRuntimeData(this);
			Flee = new FleeActions.RuntimeData(this);
			TargetKnowledge = new NpcBehaviorComponent.BaseRuntimeData(this);
		}

		public override void Dispose()
		{
			base.Dispose();
		}
	}
}
