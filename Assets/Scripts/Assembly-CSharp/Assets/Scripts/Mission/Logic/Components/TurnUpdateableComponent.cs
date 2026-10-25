namespace Assets.Scripts.Mission.Logic.Components
{
	public abstract class TurnUpdateableComponent : UpdateableComponent, IUpdateable, ITurnUpdateable, IComponent, IUpdateableComponent, ITurnUpdateableComponent
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

		protected TurnUpdateableComponent(IEntity _entity)
			: base(_entity)
		{
		}

		public virtual void DoTurnUpdate(int _deltaTurns)
		{
			LastTurnUpdateTurn = LogicMain.CurrentTurn;
			NextTurnUpdateTurn = this.ComputeNextTurnUpdateTurn();
		}

		public override void OnRegistered()
		{
			base.OnRegistered();
			LastTurnUpdateTurn = LogicMain.CurrentTurn;
			NextTurnUpdateTurn = LastTurnUpdateTurn + TurnUpdateOffsetTurns % 7;
		}

		public void ScheduleTurnUpdate(bool _resetLastTurnUpdateTurn)
		{
			NextTurnUpdateTurn = LogicMain.CurrentTurn - TurnUpdateOffsetTurns;
			if (_resetLastTurnUpdateTurn)
			{
				LastTurnUpdateTurn = LogicMain.CurrentTurn;
			}
		}
	}
}
