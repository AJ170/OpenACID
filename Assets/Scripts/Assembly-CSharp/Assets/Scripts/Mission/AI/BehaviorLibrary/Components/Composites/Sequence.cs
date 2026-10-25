using Assets.Scripts.Mission.AI.BehaviorLibrary.Components.BehaviorRuntimeData;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Composites
{
	public class Sequence : AbstractComposite
	{
		public override BBList<BehaviorComponent> Behaviors
		{
			get
			{
				return m_behaviors;
			}
		}

		public Sequence()
		{
			base.Name = "Sequence";
			base.BehaviorType = EBehaviorType.Sequence;
		}

		protected override void BehaveInternal(ITreeRuntimeData _runtimeData, IBehaviorRuntimeData _behaviorRuntimeData)
		{
			IndexRuntimeData indexRuntimeData = _behaviorRuntimeData as IndexRuntimeData;
			int count = m_behaviors.Count;
			while (indexRuntimeData.Index < count)
			{
				BehaviorComponent behaviorComponent = m_behaviors[indexRuntimeData.Index];
				switch (indexRuntimeData.ReturnCode = behaviorComponent.Behave(_runtimeData).ReturnCode)
				{
				case BehaviorReturnCode.Failure:
					indexRuntimeData.ResetIndex();
					return;
				case BehaviorReturnCode.Success:
					indexRuntimeData.Index++;
					indexRuntimeData.ReturnCode = BehaviorReturnCode.Running;
					break;
				case BehaviorReturnCode.Running:
					return;
				}
			}
			indexRuntimeData.ResetIndex();
			indexRuntimeData.ReturnCode = BehaviorReturnCode.Success;
		}
	}
}
