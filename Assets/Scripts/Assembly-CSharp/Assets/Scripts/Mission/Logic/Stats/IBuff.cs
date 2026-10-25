using System;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.UI.LogicInterface;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public interface IBuff : ICloneable, IComparable<IBuff>
	{
		EBuffType Type { get; }

		BuffData.IBuffDefinition Definition { get; }

		StatTypes.EStatType StatType { get; }

		EBuffDurationType DurationType { get; }

		int StartTurn { get; }

		int TurnDuration { get; }

		int TurnInterval { get; }

		bool IsRunning { get; }

		int Priority { get; }

		Func<IEntity, IBuff, bool> IsActivePredicate { get; }

		bool GetIsActive(IEntity _carrier);

		bool Apply(IEntity _entity, IStatValueWrapper _wrapper, float _payload);

		void Abort();
	}
}
