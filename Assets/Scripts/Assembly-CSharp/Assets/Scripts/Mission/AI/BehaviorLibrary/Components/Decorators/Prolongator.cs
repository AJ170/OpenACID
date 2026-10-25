using System.Xml;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components.BehaviorRuntimeData;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Composites;

namespace Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Decorators
{
	public class Prolongator : AbstractComposite, ITimerComponentCarrier
	{
		public BehaviorReturnCode ForcedReturnCode { get; set; }

		public TimerComponent TimerComponent { get; private set; }

		public Prolongator()
		{
			base.Name = "Prolongator";
			base.BehaviorType = EBehaviorType.Prolongator;
			TimerComponent = new TimerComponent();
		}

		protected override void BehaveInternal(ITreeRuntimeData _runtimeData, IBehaviorRuntimeData _behaviorRuntimeData)
		{
			TimerRuntimeData timerRuntimeData = _behaviorRuntimeData as TimerRuntimeData;
			float currentTime = LogicMain.CurrentTime;
			if (timerRuntimeData.EndTime <= 0f)
			{
				BehaviorComponent behaviorComponent = m_behaviors[0];
				switch (behaviorComponent.Behave(_runtimeData).ReturnCode)
				{
				case BehaviorReturnCode.Failure:
					if (_behaviorRuntimeData.ReturnCode == BehaviorReturnCode.Success || _behaviorRuntimeData.ReturnCode == BehaviorReturnCode.Running)
					{
						timerRuntimeData.EndTime = TimerComponent.CalcEndTime();
						_behaviorRuntimeData.ReturnCode = BehaviorReturnCode.Success;
					}
					else
					{
						_behaviorRuntimeData.ReturnCode = BehaviorReturnCode.Failure;
					}
					break;
				case BehaviorReturnCode.Success:
					_behaviorRuntimeData.ReturnCode = BehaviorReturnCode.Success;
					break;
				case BehaviorReturnCode.Running:
					_behaviorRuntimeData.ReturnCode = BehaviorReturnCode.Running;
					break;
				}
			}
			else if (currentTime >= timerRuntimeData.EndTime)
			{
				timerRuntimeData.ReturnCode = BehaviorReturnCode.Failure;
				timerRuntimeData.EndTime = -1f;
			}
			else
			{
				timerRuntimeData.ReturnCode = BehaviorReturnCode.Success;
			}
		}

		public override void Read(XmlReader _reader)
		{
			TimerComponent.Read(_reader);
			base.Read(_reader);
		}
	}
}
