using System;
using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public interface IStatEnumMapValue : IDataSerializable, IReadonlyStatCollectionValue, IStatCollectionValue, IReadonlyStatValueBase, IStatValueBase, IManagedStatValueBase, IAcierDataSerializable, IAcierDataTemplateSerializable, IAcierDataDeSerializable
	{
		Type KeyType { get; }
	}
}
