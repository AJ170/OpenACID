using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EForeignAssassinType")]
	public enum EForeignAssassinType
	{
		[ProtoEnum(Name = "Hireling", Value = 1)]
		Hireling = 1,
		[ProtoEnum(Name = "StrongestAssassin", Value = 2)]
		StrongestAssassin = 2,
		[ProtoEnum(Name = "SpecificAssassin", Value = 3)]
		SpecificAssassin = 3
	}
}
