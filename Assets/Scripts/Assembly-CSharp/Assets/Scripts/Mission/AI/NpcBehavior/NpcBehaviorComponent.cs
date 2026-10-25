using Assets.Scripts.Mission.AI.Knowledge;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Motion;
using UnityEngine;

namespace Assets.Scripts.Mission.AI.NpcBehavior
{
	public class NpcBehaviorComponent
	{
		public class BaseRuntimeData
		{
			public NpcRuntimeData NpcRuntimeData { get; private set; }

			public TargetApproacher TargetApproacher { get; private set; }

			public NavMeshPath DummyPath { get; private set; }

			public BaseRuntimeData(NpcRuntimeData _npcRuntimeData)
			{
				NpcRuntimeData = _npcRuntimeData;
				TargetApproacher = new TargetApproacher();
				DummyPath = new NavMeshPath();
			}
		}

		protected NpcTreeHolder NpcTreeHolder { get; private set; }

		protected NpcRuntimeData NpcRuntimeData { get; private set; }

		protected NpcCharacter NpcCharacter
		{
			get
			{
				return NpcRuntimeData.NpcCharacter;
			}
		}

		protected CharacterKnowledgeContainer Knowledge
		{
			get
			{
				return NpcTreeHolder.NpcDelegateHolder.Knowledge;
			}
		}

		protected NpcBehaviorComponent(NpcTreeHolder _npcTreeHolder)
		{
			NpcTreeHolder = _npcTreeHolder;
		}

		public virtual void Prepare(NpcRuntimeData _runtimeData)
		{
			NpcRuntimeData = _runtimeData;
		}
	}
}
