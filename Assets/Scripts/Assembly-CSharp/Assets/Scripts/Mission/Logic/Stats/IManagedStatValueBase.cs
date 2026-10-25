using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public interface IManagedStatValueBase : IDataSerializable, IReadonlyStatValueBase, IStatValueBase, IAcierDataSerializable, IAcierDataTemplateSerializable, IAcierDataDeSerializable
	{
		void Reset();
	}
}
