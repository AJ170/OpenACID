using System.Xml;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components.BehaviorRuntimeData;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Composites;

namespace Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Decorators
{
	public class Counter : AbstractComposite
	{
		public float ResetDelay { get; set; }

		public int MaxCount { get; set; }

		public Counter()
		{
			base.MaxChildCount = 1;
			base.Name = "Counter";
			base.BehaviorType = EBehaviorType.Counter;
		}

		protected override void BehaveInternal(ITreeRuntimeData _runtimeData, IBehaviorRuntimeData _behaviorRuntimeData)
		{
			CounterRuntimeData counterRuntimeData = (CounterRuntimeData)_behaviorRuntimeData;
			if (_behaviorRuntimeData.ReturnCode != BehaviorReturnCode.Running)
			{
				if (LogicMain.CurrentTime >= counterRuntimeData.ResetTime && ResetDelay > 0f)
				{
					counterRuntimeData.Value = 0;
				}
				if (counterRuntimeData.Value < MaxCount)
				{
					counterRuntimeData.Value++;
					counterRuntimeData.ResetTime = LogicMain.CurrentTime + ResetDelay;
					_behaviorRuntimeData.ReturnCode = BehaviorReturnCode.Failure;
				}
				else
				{
					counterRuntimeData.Value = 0;
					BehaviorComponent behaviorComponent = m_behaviors[0];
					_behaviorRuntimeData.ReturnCode = behaviorComponent.Behave(_runtimeData).ReturnCode;
				}
			}
			else
			{
				BehaviorComponent behaviorComponent2 = m_behaviors[0];
				_behaviorRuntimeData.ReturnCode = behaviorComponent2.Behave(_runtimeData).ReturnCode;
			}
		}

		public override void Read(XmlReader _reader)
		{
			MaxCount = int.Parse(_reader.GetAttribute("MaxCount"));
			ResetDelay = float.Parse(_reader.GetAttribute("ResetDelay"));
			base.Read(_reader);
		}
	}
}
