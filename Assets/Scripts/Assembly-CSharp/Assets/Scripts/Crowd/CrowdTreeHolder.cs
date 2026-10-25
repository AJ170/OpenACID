using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using Assets.Scripts.Mission.AI.Helper;

namespace Assets.Scripts.Crowd
{
	public class CrowdTreeHolder : AbstractTreeHolder
	{
		private CrowdDelegateHolder m_cachedCrowdDelegateHolder;

		public override void Init(IDelegateHolder _delegateHolder, string _assetPath)
		{
			InitInternal(_delegateHolder, _assetPath, -1);
			m_cachedCrowdDelegateHolder = _delegateHolder as CrowdDelegateHolder;
		}

		public override void UpdateTurn(ITreeRuntimeData _runtimeData)
		{
			m_cachedCrowdDelegateHolder.Prepare(_runtimeData as CrowdBehaviorRuntimeData);
			base.UpdateTurn(_runtimeData);
			m_cachedCrowdDelegateHolder.Prepare(null);
		}
	}
}
