namespace Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Actions
{
	public abstract class AbstractBehaviorAction : BehaviorComponent, IHasDelegate, IHasCancelDelegate, IHasAttachDelegate
	{
		public abstract string DelegateEnumEntry { get; protected set; }

		public virtual int DelegateEnumIndex { get; set; }

		public virtual void Cancel(IBehaviorRuntimeData _runtimeData)
		{
			_runtimeData.Reset();
		}

		public abstract void Attach();
	}
}
