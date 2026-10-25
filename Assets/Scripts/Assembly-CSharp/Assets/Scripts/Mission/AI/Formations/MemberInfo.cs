using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Motion;

namespace Assets.Scripts.Mission.AI.Formations
{
	public class MemberInfo
	{
		public EntityId m_id = EntityId.InvalidId;

		public readonly TargetApproacher m_targetApproacher = new TargetApproacher();

		public bool m_idling;

		public int AddTurn { get; private set; }

		public void Init(EntityId _id)
		{
			m_id = _id;
			m_targetApproacher.Reset();
		}

		public void UpdateAddTurn()
		{
			AddTurn = LogicMain.CurrentTurn;
		}
	}
}
