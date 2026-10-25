using System.Xml;

namespace Assets.Scripts.Mission.AI.BehaviorLibrary.Components
{
	public class TimerComponent
	{
		public float WaitTime { get; set; }

		public float MaxWaitTime { get; set; }

		public TimerComponent()
		{
			MaxWaitTime = -1f;
		}

		public float CalcEndTime()
		{
			if (MaxWaitTime > WaitTime)
			{
				return LogicMain.CurrentTime + LogicMain.Random.Rand(WaitTime, MaxWaitTime);
			}
			return LogicMain.CurrentTime + WaitTime;
		}

		public void Read(XmlReader _reader)
		{
			WaitTime = float.Parse(_reader.GetAttribute("WaitTime"));
			string attribute = _reader.GetAttribute("MaxWaitTime");
			if (attribute != null)
			{
				MaxWaitTime = float.Parse(attribute);
			}
			else
			{
				MaxWaitTime = -1f;
			}
		}
	}
}
