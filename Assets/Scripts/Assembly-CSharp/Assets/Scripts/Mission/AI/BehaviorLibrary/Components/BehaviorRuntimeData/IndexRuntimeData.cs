namespace Assets.Scripts.Mission.AI.BehaviorLibrary.Components.BehaviorRuntimeData
{
	public class IndexRuntimeData : SimpleRuntimeData
	{
		public const short InvalidIndex = -1;

		private readonly short m_resetValue;

		public short Index { get; set; }

		public IndexRuntimeData(bool _restWithInvalid)
		{
			m_resetValue = (short)(_restWithInvalid ? (-1) : 0);
			ResetIndex();
		}

		public override void Reset()
		{
			base.Reset();
			ResetIndex();
		}

		public void ResetIndex()
		{
			Index = m_resetValue;
		}
	}
}
