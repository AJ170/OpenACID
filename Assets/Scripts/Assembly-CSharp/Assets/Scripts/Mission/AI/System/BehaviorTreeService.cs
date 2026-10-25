using Assets.Scripts.Crowd;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using Assets.Scripts.Mission.AI.Helper;
using Assets.Scripts.Mission.AI.NpcBehavior;
using Assets.Scripts.Mission.Logic.Combat;
using SharedBaseLib.Core;
using UbiCore.Debug;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.AI.System
{
	public class BehaviorTreeService : UbiCore.Utils.Singleton<BehaviorTreeService>
	{
		private const int MaxTreeCategoryCount = 3;

		private const int MaxTreeTypeCount = 20;

		private BBDictionary<string, IBehaviorTreeHolder> m_behaviorTrees;

		private BBDictionary<int, IDelegateHolder> m_delegateHolder;

		public BehaviorTreeService()
		{
			Clear();
		}

		public IBehaviorTreeHolder RequestBehaviorTreeHolder(string _assetPath, BehaviorTreeCategory _treeCategory)
		{
			Profiling.Start("BehaviorTreeService::RequestBehaviorTreeHolder()", float.MaxValue);
			IBehaviorTreeHolder value = null;
			if (!m_behaviorTrees.TryGetValue(_assetPath, out value))
			{
				switch (_treeCategory)
				{
				case BehaviorTreeCategory.Npc:
					Profiling.Start("BehaviorTreeCategory.Npc", float.MaxValue);
					value = new NpcTreeHolder();
					Profiling.Stop();
					break;
				case BehaviorTreeCategory.CombatParticipant:
					Profiling.Start("BehaviorTreeCategory.CombatParticipant", float.MaxValue);
					value = new CombatParticipantTreeHolder();
					Profiling.Stop();
					break;
				case BehaviorTreeCategory.Crowd:
					Profiling.Start("BehaviorTreeCategory.Crowd", float.MaxValue);
					value = new CrowdTreeHolder();
					Profiling.Stop();
					break;
				}
				m_behaviorTrees[_assetPath] = value;
				value.Init(RequestDelegateHolder(_treeCategory, value), _assetPath);
			}
			Profiling.Stop();
			return value;
		}

		public void Clear()
		{
			m_behaviorTrees = new BBDictionary<string, IBehaviorTreeHolder>(20);
			m_delegateHolder = new BBDictionary<int, IDelegateHolder>(3);
		}

		private IDelegateHolder RequestDelegateHolder(BehaviorTreeCategory _treeCategory, IBehaviorTreeHolder _treeHolder)
		{
			Profiling.Start("BehaviorTreeService::RequestDelegateHolder()", float.MaxValue);
			IDelegateHolder value = null;
			if (!m_delegateHolder.TryGetValue((int)_treeCategory, out value))
			{
				switch (_treeCategory)
				{
				case BehaviorTreeCategory.Npc:
					Profiling.Start("BehaviorTreeCategory.Npc", float.MaxValue);
					value = new NpcDelegateHolder(_treeHolder as NpcTreeHolder);
					Profiling.Stop();
					break;
				case BehaviorTreeCategory.CombatParticipant:
					Profiling.Start("BehaviorTreeCategory.CombatParticipant", float.MaxValue);
					value = new CombatParticipantDelegateHolder();
					Profiling.Stop();
					break;
				case BehaviorTreeCategory.Crowd:
					Profiling.Start("BehaviorTreeCategory.Crowd", float.MaxValue);
					value = new CrowdDelegateHolder();
					Profiling.Stop();
					break;
				}
				m_delegateHolder[(int)_treeCategory] = value;
			}
			Profiling.Stop();
			return value;
		}

		private bool CheckForSimilarFile(string _path)
		{
			foreach (string key in m_behaviorTrees.Keys)
			{
				if (key.EndsWith(_path))
				{
					return true;
				}
				if (_path.EndsWith(key))
				{
					return true;
				}
			}
			return false;
		}
	}
}
