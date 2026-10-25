using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public interface IStatValue : IReadonlyStatValueBase, IStatValueBase, IReadonlyStatValue
	{
		new int Value_int { get; set; }

		new float Value_float { get; set; }

		new bool Value_bool { get; set; }

		new string Value_string { get; set; }

		new EntityId Value_EntityId { get; set; }

		new Vector3 Value_Vector3 { get; set; }

		new float Value_number { get; set; }

		float GetPayload();

		void CopyFrom(IStatValueWrapper _wrapper);
	}
}
