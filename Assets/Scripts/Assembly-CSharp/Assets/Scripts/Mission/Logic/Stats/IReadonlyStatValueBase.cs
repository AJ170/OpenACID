using System;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public interface IReadonlyStatValueBase
	{
		Type ValueType { get; }

		StatTypes.EStatType StatType { get; }

		StatTypes.EStatUnit Unit { get; }

		StatTypes.EStatCategory Category { get; }

		bool IsImmutable { get; }

		bool IsMutable { get; }

		bool IsComputed { get; }

		bool IsScaled { get; }

		bool HasDefaultValue { get; }

		StatValueCollection StatValueCollection { get; }

		bool HasEqualValue(IReadonlyStatValueBase _other);

		IReadonlyStatValueBase Clone(StatValueCollection _newStatValueCollection);
	}
}
