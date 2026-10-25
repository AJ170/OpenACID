using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public abstract class StateMachineUnityComponentListeningBase<T, TThis> : StateMachineBase<T, TThis> where T : IGameEntity, IStateMachinesCarrier where TThis : StateMachineBase<T, TThis>
	{
		protected StateMachineUnityComponentListeningBase(T _entity)
			: base(_entity)
		{
		}

		protected static void InitStateMachineUnityComponentListeningBase()
		{
			StateMachineBase<T, TThis>.InitStateMachineBase();
		}

		public override void Attach()
		{
			base.Attach();
			if (Entity.UnityComponentCache.EntityWrapper.IsInitialized)
			{
				OnAttachedAndInitialized();
			}
			else
			{
				Entity.UnityComponentCache.EntityWrapper.OnInitialized += EntityWrapperOnInitialized;
			}
		}

		private void EntityWrapperOnInitialized(EntityWrapperComponent _wrapper)
		{
			Entity.UnityComponentCache.EntityWrapper.OnInitialized -= EntityWrapperOnInitialized;
			OnAttachedAndInitialized();
		}

		public override void Detach()
		{
			Entity.UnityComponentCache.EntityWrapper.OnInitialized -= EntityWrapperOnInitialized;
			base.Detach();
		}

		public virtual void OnAttachedAndInitialized()
		{
		}
	}
}
