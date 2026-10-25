using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "ECustomLootTemplateType")]
	public enum ECustomLootTemplateType
	{
		[ProtoEnum(Name = "LockPick", Value = 1)]
		LockPick = 1
	}
}
