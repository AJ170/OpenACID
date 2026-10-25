using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using Assets.Scripts.Mission.AI.Helper;

namespace Assets.Scripts.Mission.AI.NpcBehavior
{
	public class NpcTreeHolder : AbstractTreeHolder
	{
		private NpcDelegateHolder m_cachedNpcDelegateHolder;

		public NpcDelegateHolder NpcDelegateHolder
		{
			get
			{
				return m_cachedNpcDelegateHolder;
			}
		}

		public override void Init(IDelegateHolder _delegateHolder, string _assetPath)
		{
			InitInternal(_delegateHolder, _assetPath, 1);
			m_cachedNpcDelegateHolder = _delegateHolder as NpcDelegateHolder;
		}

		public override void UpdateTurn(ITreeRuntimeData _runtimeData)
		{
			PrepareBehaviorComponents(_runtimeData);
			base.UpdateTurn(_runtimeData);
			ResetBehaviorComponents();
		}

		public void OnAttachedAndInitialized(ITreeRuntimeData _runtimeData)
		{
			PrepareBehaviorComponents(_runtimeData);
			BehaviorTree.OnAttachedAndInitialized(_runtimeData);
		}

		public void CancelRunning(ITreeRuntimeData _runtimeData)
		{
			PrepareBehaviorComponents(_runtimeData);
			BehaviorTree.CancelRunning(_runtimeData);
		}

		private void PrepareBehaviorComponents(ITreeRuntimeData _runtimeData)
		{
			NpcRuntimeData runtimeData = _runtimeData as NpcRuntimeData;
			m_cachedNpcDelegateHolder.Knowledge.Prepare(runtimeData);
			m_cachedNpcDelegateHolder.ActionsConditions.Prepare(runtimeData);
		}

		private void ResetBehaviorComponents()
		{
			m_cachedNpcDelegateHolder.Knowledge.Prepare(null);
			m_cachedNpcDelegateHolder.ActionsConditions.Prepare(null);
		}
	}
}
