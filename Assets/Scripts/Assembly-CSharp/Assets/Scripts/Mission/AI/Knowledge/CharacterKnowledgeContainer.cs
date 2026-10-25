using Assets.Scripts.Mission.AI.NpcBehavior;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.AI.Knowledge
{
	public class CharacterKnowledgeContainer
	{
		private readonly BBList<NpcBehaviorComponent> m_allKnowledge = new BBList<NpcBehaviorComponent>();

		public TargetKnowledge TargetKnowledge { get; private set; }

		public CombatKnowledge CombatKnowledge { get; private set; }

		public CommonKnowledge CommonKnowledge { get; private set; }

		public BBList<NpcBehaviorComponent> AllKnowledge
		{
			get
			{
				return m_allKnowledge;
			}
		}

		public void CreateCommonKnowledge(NpcTreeHolder _npcTreeHolder)
		{
			if (CommonKnowledge == null)
			{
				CommonKnowledge = new CommonKnowledge(_npcTreeHolder);
				m_allKnowledge.Add(CommonKnowledge);
			}
		}

		public void CreateCombatKnowledge(NpcTreeHolder _npcTreeHolder)
		{
			if (CombatKnowledge == null)
			{
				CombatKnowledge = new CombatKnowledge(_npcTreeHolder);
				m_allKnowledge.Add(CombatKnowledge);
			}
		}

		public void CreateTargetKnowledge(NpcTreeHolder _npcTreeHolder)
		{
			if (TargetKnowledge == null)
			{
				TargetKnowledge = new TargetKnowledge(_npcTreeHolder);
				m_allKnowledge.Add(TargetKnowledge);
			}
		}

		public void Prepare(NpcRuntimeData _runtimeData)
		{
			foreach (NpcBehaviorComponent item in m_allKnowledge)
			{
				item.Prepare(_runtimeData);
			}
		}
	}
}
