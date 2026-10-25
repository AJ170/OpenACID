namespace Assets.Scripts.Mission.Logic.Components
{
	public abstract class FrameUpdateableComponent : UpdateableComponent, IUpdateable, IFrameUpdateable, IComponent, IUpdateableComponent, IFrameUpdateableComponent
	{
		public virtual float FrameUpdateIntervalSeconds
		{
			get
			{
				return 0f;
			}
		}

		public float FrameUpdateOffsetPercent
		{
			get
			{
				return (float)Entity.Id.Slot / (float)LogicMain.EntityManager.EntityCount;
			}
		}

		public float LastFrameUpdateTime { get; private set; }

		public float NextFrameUpdateTime { get; private set; }

		public int TurnsSinceFrameUpdate
		{
			get
			{
				return this.ComputeTurnsSinceFrameUpdate();
			}
		}

		public float TimeSinceFrameUpdate
		{
			get
			{
				return this.ComputeTimeSinceFrameUpdate();
			}
		}

		protected FrameUpdateableComponent(IEntity _entity)
			: base(_entity)
		{
		}

		public virtual void DoFrameUpdate(float _deltaTime)
		{
			LastFrameUpdateTime = LogicMain.CurrentTime;
			NextFrameUpdateTime = this.ComputeNextFrameUpdateTime();
		}

		public override void OnRegistered()
		{
			base.OnRegistered();
			LastFrameUpdateTime = LogicMain.CurrentTime;
			NextFrameUpdateTime = -1f;
		}

		public void ScheduleFrameUpdate(bool _resetLastFrameUpdateTime)
		{
			NextFrameUpdateTime = -1f;
			if (_resetLastFrameUpdateTime)
			{
				LastFrameUpdateTime = LogicMain.CurrentTime;
			}
		}
	}
}
