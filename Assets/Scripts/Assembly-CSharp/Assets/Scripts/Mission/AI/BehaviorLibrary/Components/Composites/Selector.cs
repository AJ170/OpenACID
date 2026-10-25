using Assets.Scripts.Mission.AI.BehaviorLibrary.Components.BehaviorRuntimeData;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Composites
{
	public class Selector : AbstractComposite
	{
		public override BBList<BehaviorComponent> Behaviors
		{
			get
			{
				return m_behaviors;
			}
		}

		public Selector()
		{
			base.Name = "Selector";
			base.BehaviorType = EBehaviorType.Selector;
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
					indexRuntimeData.Index++;
					break;
				case BehaviorReturnCode.Success:
					indexRuntimeData.ResetIndex();
					return;
				case BehaviorReturnCode.Running:
					return;
				default:
					indexRuntimeData.Index++;
					break;
				}
			}
			indexRuntimeData.ResetIndex();
			indexRuntimeData.ReturnCode = BehaviorReturnCode.Failure;
		}
	}
}
