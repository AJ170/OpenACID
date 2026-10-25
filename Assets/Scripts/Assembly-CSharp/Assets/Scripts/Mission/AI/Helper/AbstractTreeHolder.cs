using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using UbiCore.Debug;
using UbiCore.Mission.Ai;
using UnityEngine;
using UnityEngine.Profiling;

namespace Assets.Scripts.Mission.AI.Helper
{
	public abstract class AbstractTreeHolder : IBehaviorTreeHolder, IAIUpdateable
	{
		private const string FallbackBehaviorAssetPath = "AI/BehaviorTrees/DoNothing";

		private bool m_firstTurnUpdate = true;

		public BehaviorTree BehaviorTree { get; private set; }

		public IDelegateHolder DelegateHolder { get; private set; }

		public abstract void Init(IDelegateHolder _delegateHolder, string _assetPath);

		protected void InitInternal(IDelegateHolder _delegateHolder, string _assetPath, int _preStartActionIndex)
		{
			uint monoUsedSize = Profiler.GetMonoUsedSize();
			Profiling.Start("AbstractTreeHolder::Init()", float.MaxValue);
			DelegateHolder = _delegateHolder;
			TextAsset textAsset = (TextAsset)Resources.Load(_assetPath);
			BehaviorTree = new BehaviorTree();
			BehaviorTree.Init(DelegateHolder, textAsset, _preStartActionIndex);
			Resources.UnloadAsset(textAsset);
			Profiling.Stop();
			monoUsedSize = Profiler.GetMonoUsedSize() - monoUsedSize;
		}

		public virtual void UpdateTurn(ITreeRuntimeData _runtimeData)
		{
			if (m_firstTurnUpdate)
			{
				m_firstTurnUpdate = false;
				if (BehaviorTree.PreStartAction != null)
				{
					BehaviorTree.Behave(_runtimeData, BehaviorTree.PreStartAction);
				}
			}
			BehaviorTree.Behave(_runtimeData);
		}
	}
}
