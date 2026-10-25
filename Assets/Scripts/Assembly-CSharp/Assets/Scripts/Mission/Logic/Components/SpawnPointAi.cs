using UbiCore.Debug;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class SpawnPointAi : TurnUpdateableComponent
	{
		public override int Priority
		{
			get
			{
				return 10;
			}
		}

		public SpawnPointAi(IEntity _entity)
			: base(_entity)
		{
		}

		public override void DoTurnUpdate(int _deltaTurns)
		{
			Profiling.Start("SpawnPointAI::DoUpdate()", float.MaxValue);
			base.DoTurnUpdate(_deltaTurns);
			Profiling.Stop();
		}

		public override void OnRegistered()
		{
			base.OnRegistered();
			IsActive = true;
		}
	}
}
