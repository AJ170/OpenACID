using System;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public interface IEnumStatValueWrapper : IDataSerializable, IStatValueWrapper
	{
		Type EnumType { get; }
	}
}
