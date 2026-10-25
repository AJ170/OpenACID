namespace Assets.Scripts.Mission.Logic.Components
{
	public abstract class UnityComponentListeningComponent : Component
	{
		protected UnityComponentListeningComponent(IEntity _entity)
			: base(_entity)
		{
		}

		public override void Attach(EntityWrapperComponent _wrapperComponent)
		{
			base.Attach(_wrapperComponent);
			if (base.GameObjectEntity.UnityComponentCache.EntityWrapper.IsInitialized)
			{
				OnAttachedAndInitialized();
			}
			else
			{
				base.GameObjectEntity.UnityComponentCache.EntityWrapper.OnInitialized += EntityWrapperOnInitialized;
			}
		}

		private void EntityWrapperOnInitialized(EntityWrapperComponent wraper)
		{
			base.GameObjectEntity.UnityComponentCache.EntityWrapper.OnInitialized -= EntityWrapperOnInitialized;
			OnAttachedAndInitialized();
		}

		public override void Detach()
		{
			base.GameObjectEntity.UnityComponentCache.EntityWrapper.OnInitialized -= EntityWrapperOnInitialized;
			base.Detach();
		}

		public virtual void OnAttachedAndInitialized()
		{
		}
	}
}
