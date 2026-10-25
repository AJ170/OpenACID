using Assets.Scripts.Mission.AI.BehaviorLibrary.Components.BehaviorRuntimeData;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Composites
{
	public class RandomSelector : AbstractComposite
	{
		public override BBList<BehaviorComponent> Behaviors
		{
			get
			{
				return m_behaviors;
			}
		}

		public RandomSelector()
		{
			base.Name = "RandomSelector";
			base.BehaviorType = EBehaviorType.RandomSelector;
		}

		protected override void BehaveInternal(ITreeRuntimeData _runtimeData, IBehaviorRuntimeData _behaviorRuntimeData)
		{
			IndexRuntimeData indexRuntimeData = _behaviorRuntimeData as IndexRuntimeData;
			short index = ((indexRuntimeData.Index < 0 || indexRuntimeData.Index >= m_behaviors.Count) ? ((short)LogicMain.Random.Rand(0, m_behaviors.Count)) : indexRuntimeData.Index);
			BehaviorComponent behaviorComponent = m_behaviors[index];
			switch (indexRuntimeData.ReturnCode = behaviorComponent.Behave(_runtimeData).ReturnCode)
			{
			case BehaviorReturnCode.Failure:
				indexRuntimeData.ResetIndex();
				break;
			case BehaviorReturnCode.Success:
				indexRuntimeData.ResetIndex();
				break;
			case BehaviorReturnCode.Running:
				break;
			}
		}
	}
}
