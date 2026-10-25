using Assets.Scripts.Engine;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components.BehaviorRuntimeData;
using SharedBaseLib.Core;
using UbiCore.Debug;

namespace Assets.Scripts.Mission.AI.BehaviorLibrary.Components
{
	public class TreeRuntimeData : ITreeRuntimeData
	{
		public BBList<BehaviorComponent> LastTicked { get; set; }

		public BBHashSet<BehaviorComponent> LastTickedSet { get; private set; }

		private BBDictionary<BehaviorComponent, IBehaviorRuntimeData> BehaviorRuntimeData { get; set; }

		public TreeRuntimeData()
		{
			LastTicked = new BBList<BehaviorComponent>();
			LastTickedSet = new BBHashSet<BehaviorComponent>();
			BehaviorRuntimeData = new BBDictionary<BehaviorComponent, IBehaviorRuntimeData>();
		}

		public virtual void Init()
		{
			LastTicked.ClearIfNotEmpty();
			LastTickedSet.Clear();
			BehaviorRuntimeData.Clear();
		}

		public virtual void Dispose()
		{
			LastTicked.Clear();
			LastTickedSet.Clear();
		}

		public IBehaviorRuntimeData RequestBehaviorRuntimeData(BehaviorComponent _component)
		{
			Profiling.Start("TreeRuntimeData::RequestBehaviorRuntimeData", float.MaxValue);
			IBehaviorRuntimeData behaviorRuntimeData = null;
			if (BehaviorRuntimeData.ContainsKey(_component))
			{
				behaviorRuntimeData = BehaviorRuntimeData[_component];
			}
			else
			{
				switch (_component.BehaviorType)
				{
				case EBehaviorType.BehaviorAction:
				case EBehaviorType.BehaviorActionParameterized:
				case EBehaviorType.ParallelSelector:
				case EBehaviorType.ParallelSequence:
				case EBehaviorType.Conditional:
				case EBehaviorType.ConditionalParameterized:
				case EBehaviorType.Inverter:
				case EBehaviorType.Enforcer:
				case EBehaviorType.Wait:
					Profiling.Start("SimpleRuntimeData()", float.MaxValue);
					behaviorRuntimeData = new SimpleRuntimeData();
					Profiling.Stop();
					break;
				case EBehaviorType.RandomSelector:
					Profiling.Start("IndexRuntimeData()", float.MaxValue);
					behaviorRuntimeData = new IndexRuntimeData(true);
					Profiling.Stop();
					break;
				case EBehaviorType.Selector:
				case EBehaviorType.Sequence:
				case EBehaviorType.Branch:
					Profiling.Start("IndexRuntimeData()", float.MaxValue);
					behaviorRuntimeData = new IndexRuntimeData(false);
					Profiling.Stop();
					break;
				case EBehaviorType.Counter:
					Profiling.Start("CounterRuntimeData()", float.MaxValue);
					behaviorRuntimeData = new CounterRuntimeData();
					Profiling.Stop();
					break;
				case EBehaviorType.Timer:
					Profiling.Start("TimerRuntimeData()", float.MaxValue);
					behaviorRuntimeData = new TimerRuntimeData();
					Profiling.Stop();
					break;
				case EBehaviorType.Prolongator:
					Profiling.Start("ProlongatorRuntimeData()", float.MaxValue);
					behaviorRuntimeData = new TimerRuntimeData();
					Profiling.Stop();
					break;
				}
				BehaviorRuntimeData[_component] = behaviorRuntimeData;
			}
			Profiling.Stop();
			return behaviorRuntimeData;
		}
	}
}
