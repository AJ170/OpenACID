using Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Composites;

namespace Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Decorators
{
	public class Wait : AbstractComposite
	{
		public Wait()
		{
			base.MaxChildCount = 1;
			base.Name = "Wait";
			base.BehaviorType = EBehaviorType.Wait;
		}

		protected override void BehaveInternal(ITreeRuntimeData _runtimeData, IBehaviorRuntimeData _behaviorRuntimeData)
		{
			BehaviorComponent behaviorComponent = m_behaviors[0];
			switch (behaviorComponent.Behave(_runtimeData).ReturnCode)
			{
			case BehaviorReturnCode.Failure:
				_behaviorRuntimeData.ReturnCode = BehaviorReturnCode.Running;
				break;
			case BehaviorReturnCode.Success:
				_behaviorRuntimeData.ReturnCode = BehaviorReturnCode.Success;
				break;
			case BehaviorReturnCode.Running:
				_behaviorRuntimeData.ReturnCode = BehaviorReturnCode.Running;
				break;
			}
		}
	}
}
