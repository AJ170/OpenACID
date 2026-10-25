using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Composites
{
	public class ParallelSequence : AbstractComposite
	{
		public override BBList<BehaviorComponent> Behaviors
		{
			get
			{
				return m_behaviors;
			}
		}

		public ParallelSequence()
		{
			base.Name = "ParallelSequence";
			base.BehaviorType = EBehaviorType.ParallelSequence;
		}

		protected override void BehaveInternal(ITreeRuntimeData _runtimeData, IBehaviorRuntimeData _behaviorRuntimeData)
		{
			int count = m_behaviors.Count;
			for (int i = 0; i < count; i++)
			{
				BehaviorComponent behaviorComponent = m_behaviors[i];
				switch (_behaviorRuntimeData.ReturnCode = behaviorComponent.Behave(_runtimeData).ReturnCode)
				{
				case BehaviorReturnCode.Failure:
					return;
				case BehaviorReturnCode.Running:
					return;
				}
			}
		}
	}
}
