using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public interface IReadonlyStatValue : IReadonlyStatValueBase
	{
		int Value_int { get; }

		float Value_float { get; }

		bool Value_bool { get; }

		string Value_string { get; }

		EntityId Value_EntityId { get; }

		Vector3 Value_Vector3 { get; }

		float Value_number { get; }

		void CopyTo(IStatValueWrapper _wrapper);

		bool HasEqualValue(IStatValueWrapper _statValueWrapper);
	}
}
