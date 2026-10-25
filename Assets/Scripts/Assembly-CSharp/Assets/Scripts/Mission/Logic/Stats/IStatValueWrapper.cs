using System;
using SharedBaseLib.Serialization;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public interface IStatValueWrapper : IDataSerializable
	{
		int Int { get; set; }

		float Float { get; set; }

		bool Bool { get; set; }

		string String { get; set; }

		EntityId EntityId { get; set; }

		Vector3 Vector3 { get; set; }

		float Number { get; set; }

		Type ValueType { get; }

		IStatValueWrapper Clone();

		void CopyFrom(IStatValueWrapper _wrapper);
	}
}
