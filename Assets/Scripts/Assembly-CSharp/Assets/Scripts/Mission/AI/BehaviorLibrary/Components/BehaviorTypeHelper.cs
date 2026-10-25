using System;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Actions;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Composites;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Conditionals;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Decorators;

namespace Assets.Scripts.Mission.AI.BehaviorLibrary.Components
{
	public static class BehaviorTypeHelper
	{
		private static readonly Type[] BehaviorTypes;

		static BehaviorTypeHelper()
		{
			BehaviorTypes = new Type[Enum.GetValues(typeof(EBehaviorType)).Length];
			BehaviorTypes[1] = typeof(BehaviorAction);
			BehaviorTypes[2] = typeof(BehaviorActionParameterized);
			BehaviorTypes[3] = typeof(ParallelSelector);
			BehaviorTypes[4] = typeof(ParallelSequence);
			BehaviorTypes[5] = typeof(RandomSelector);
			BehaviorTypes[6] = typeof(Selector);
			BehaviorTypes[7] = typeof(Sequence);
			BehaviorTypes[8] = typeof(Conditional);
			BehaviorTypes[9] = typeof(ConditionalParameterized);
			BehaviorTypes[11] = typeof(Counter);
			BehaviorTypes[12] = typeof(Inverter);
			BehaviorTypes[15] = typeof(Wait);
			BehaviorTypes[13] = typeof(Timer);
			BehaviorTypes[14] = typeof(Enforcer);
			BehaviorTypes[10] = typeof(Branch);
			BehaviorTypes[16] = typeof(Prolongator);
		}

		public static Type GetBehaviorType(EBehaviorType _typeEnum)
		{
			return BehaviorTypes[(int)_typeEnum];
		}
	}
}
