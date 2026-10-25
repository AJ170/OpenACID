using Assets.Scripts.Mission.AI.NpcBehavior;

namespace Assets.Scripts.Mission.AI.Actions
{
	public class ActionsContainer
	{
		private readonly NpcBehaviorComponent[] m_allActionConditions;

		public ChaseActions ChaseActions { get; private set; }

		public RangedCombatActions RangedCombatActions { get; private set; }

		public PatrolActions PatrolActions { get; private set; }

		public SidetrackedActions SidetrackedActions { get; private set; }

		public MissionActions MissionActions { get; private set; }

		public ReactOnTargetActions ReactOnTargetActions { get; private set; }

		public FleeActions FleeActions { get; private set; }

		public CourtesanActions CourtesanActions { get; private set; }

		public MiscActions MiscActions { get; private set; }

		public ActionsContainer(NpcTreeHolder _npcTreeHolder)
		{
			ChaseActions = new ChaseActions(_npcTreeHolder);
			RangedCombatActions = new RangedCombatActions(_npcTreeHolder);
			MissionActions = new MissionActions(_npcTreeHolder);
			PatrolActions = new PatrolActions(_npcTreeHolder);
			ReactOnTargetActions = new ReactOnTargetActions(_npcTreeHolder);
			SidetrackedActions = new SidetrackedActions(_npcTreeHolder);
			FleeActions = new FleeActions(_npcTreeHolder);
			CourtesanActions = new CourtesanActions(_npcTreeHolder);
			MiscActions = new MiscActions(_npcTreeHolder);
			m_allActionConditions = new NpcBehaviorComponent[9] { ChaseActions, RangedCombatActions, MissionActions, PatrolActions, ReactOnTargetActions, SidetrackedActions, FleeActions, CourtesanActions, MiscActions };
		}

		public void Prepare(NpcRuntimeData _runtimeData)
		{
			int num = m_allActionConditions.Length;
			for (int i = 0; i < num; i++)
			{
				NpcBehaviorComponent npcBehaviorComponent = m_allActionConditions[i];
				npcBehaviorComponent.Prepare(_runtimeData);
			}
		}
	}
}
