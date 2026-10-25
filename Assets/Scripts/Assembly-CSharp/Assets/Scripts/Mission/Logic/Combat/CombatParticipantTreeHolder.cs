using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using Assets.Scripts.Mission.AI.Helper;

namespace Assets.Scripts.Mission.Logic.Combat
{
	public class CombatParticipantTreeHolder : AbstractTreeHolder
	{
		private CombatParticipantDelegateHolder m_cachedCombatParticipantDelegateHolder;

		public override void Init(IDelegateHolder _delegateHolder, string _assetPath)
		{
			InitInternal(_delegateHolder, _assetPath, -1);
			m_cachedCombatParticipantDelegateHolder = _delegateHolder as CombatParticipantDelegateHolder;
		}

		public override void UpdateTurn(ITreeRuntimeData _runtimeData)
		{
			m_cachedCombatParticipantDelegateHolder.Prepare(_runtimeData as CombatParticipant);
			base.UpdateTurn(_runtimeData);
			m_cachedCombatParticipantDelegateHolder.Prepare(null);
		}
	}
}
