namespace Assets.Scripts.Mission.Logic.Components
{
	public abstract class TurnAndFrameUpdateableComponent : UpdateableComponent, IUpdateable, ITurnUpdateable, IFrameUpdateable, IComponent, IUpdateableComponent, ITurnUpdateableComponent, IFrameUpdateableComponent
	{
		public int BucketTurnUpdateIntervalTurns
		{
			get
			{
				return this.ComputeBucketTurnUpdateIntervalTurns();
			}
		}

		public virtual int TurnUpdateIntervalTurns
		{
			get
			{
				return 0;
			}
		}

		public int TurnUpdateOffsetTurns
		{
			get
			{
				return (int)Entity.Id.Slot;
			}
		}

		public int LastTurnUpdateTurn { get; private set; }

		public int NextTurnUpdateTurn { get; private set; }

		public int TurnsSinceTurnUpdate
		{
			get
			{
				return this.ComputeTurnsSinceTurnUpdate();
			}
		}

		public float TimeSinceTurnUpdate
		{
			get
			{
				return this.ComputeTimeSinceTurnUpdate();
			}
		}

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

		protected TurnAndFrameUpdateableComponent(IEntity _entity)
			: base(_entity)
		{
		}

		public virtual void DoTurnUpdate(int _deltaTurns)
		{
			LastTurnUpdateTurn = LogicMain.CurrentTurn;
			NextTurnUpdateTurn = this.ComputeNextTurnUpdateTurn();
		}

		public void ScheduleTurnUpdate(bool _resetLastTurnUpdateTurn)
		{
			NextTurnUpdateTurn = LogicMain.CurrentTurn - TurnUpdateOffsetTurns;
			if (_resetLastTurnUpdateTurn)
			{
				LastTurnUpdateTurn = LogicMain.CurrentTurn;
			}
		}

		public virtual void DoFrameUpdate(float _deltaTime)
		{
			LastFrameUpdateTime = LogicMain.CurrentTime;
			NextFrameUpdateTime = this.ComputeNextFrameUpdateTime();
		}

		public void ScheduleFrameUpdate(bool _resetLastFrameUpdateTime)
		{
			NextFrameUpdateTime = -1f;
			if (_resetLastFrameUpdateTime)
			{
				LastFrameUpdateTime = LogicMain.CurrentTime;
			}
		}

		public override void OnRegistered()
		{
			base.OnRegistered();
			LastFrameUpdateTime = LogicMain.CurrentTime;
			NextFrameUpdateTime = -1f;
			LastTurnUpdateTurn = LogicMain.CurrentTurn;
			NextTurnUpdateTurn = LastTurnUpdateTurn + TurnUpdateOffsetTurns % 7;
		}
	}
}
