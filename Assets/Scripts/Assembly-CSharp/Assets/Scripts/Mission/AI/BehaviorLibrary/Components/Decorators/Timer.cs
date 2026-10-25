using System.Xml;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components.BehaviorRuntimeData;

namespace Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Decorators
{
	public class Timer : BehaviorComponent, IHasCancelDelegate, ITimerComponentCarrier
	{
		public TimerComponent TimerComponent { get; private set; }

		public Timer()
		{
			base.Name = "Timer";
			base.BehaviorType = EBehaviorType.Timer;
			TimerComponent = new TimerComponent();
		}

		protected override void BehaveInternal(ITreeRuntimeData _runtimeData, IBehaviorRuntimeData _behaviorRuntimeData)
		{
			TimerRuntimeData timerRuntimeData = _behaviorRuntimeData as TimerRuntimeData;
			if (_behaviorRuntimeData.ReturnCode != BehaviorReturnCode.Running)
			{
				timerRuntimeData.EndTime = TimerComponent.CalcEndTime();
			}
			if (LogicMain.CurrentTime >= timerRuntimeData.EndTime)
			{
				timerRuntimeData.ReturnCode = BehaviorReturnCode.Success;
			}
			else
			{
				timerRuntimeData.ReturnCode = BehaviorReturnCode.Running;
			}
		}

		public override void Read(XmlReader _reader)
		{
			base.Read(_reader);
			TimerComponent.Read(_reader);
		}

		public void Cancel(IBehaviorRuntimeData _runtimeData)
		{
			_runtimeData.ReturnCode = BehaviorReturnCode.NotCalled;
		}
	}
}
