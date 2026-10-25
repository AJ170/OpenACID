using Assets.Scripts.Mission.AI.BehaviorLibrary.Components.BehaviorRuntimeData;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Composites;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Conditionals
{
	public class Branch : AbstractComposite
	{
		public override BBList<BehaviorComponent> Behaviors
		{
			get
			{
				return m_behaviors;
			}
		}

		public Branch()
		{
			base.MaxChildCount = 3;
			base.Name = "Branch";
			base.BehaviorType = EBehaviorType.Branch;
		}

		protected override void BehaveInternal(ITreeRuntimeData _runtimeData, IBehaviorRuntimeData _behaviorRuntimeData)
		{
			IndexRuntimeData indexRuntimeData = _behaviorRuntimeData as IndexRuntimeData;
			indexRuntimeData.ReturnCode = BehaviorReturnCode.Failure;
			if (m_behaviors.Count <= 1)
			{
				return;
			}
			if (indexRuntimeData.Index == 0)
			{
				BehaviorComponent behaviorComponent = m_behaviors[0];
				IBehaviorRuntimeData behaviorRuntimeData = behaviorComponent.Behave(_runtimeData);
				if (behaviorRuntimeData.ReturnCode == BehaviorReturnCode.Success && m_behaviors.Count >= 2)
				{
					indexRuntimeData.Index = 1;
				}
				else if (behaviorRuntimeData.ReturnCode == BehaviorReturnCode.Failure && m_behaviors.Count >= 3)
				{
					indexRuntimeData.Index = 2;
				}
			}
			if (indexRuntimeData.Index != 0)
			{
				if (indexRuntimeData.Index < m_behaviors.Count)
				{
					BehaviorComponent behaviorComponent2 = m_behaviors[indexRuntimeData.Index];
					IBehaviorRuntimeData behaviorRuntimeData2 = behaviorComponent2.Behave(_runtimeData);
					indexRuntimeData.ReturnCode = behaviorRuntimeData2.ReturnCode;
				}
				if (indexRuntimeData.ReturnCode != BehaviorReturnCode.Running)
				{
					indexRuntimeData.Index = 0;
				}
			}
		}
	}
}
