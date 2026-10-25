using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "ESpawnPointKind")]
	public enum ESpawnPointKind
	{
		[ProtoEnum(Name = "Player", Value = 1)]
		Player = 1,
		[ProtoEnum(Name = "EnemyGround", Value = 2)]
		EnemyGround = 2,
		[ProtoEnum(Name = "Cover", Value = 4)]
		Cover = 4,
		[ProtoEnum(Name = "ObjectiveLong", Value = 8)]
		ObjectiveLong = 8,
		[ProtoEnum(Name = "Thieves", Value = 16)]
		Thieves = 0x10,
		[ProtoEnum(Name = "Courtesans", Value = 32)]
		Courtesans = 0x20,
		[ProtoEnum(Name = "Doctor", Value = 64)]
		Doctor = 0x40,
		[ProtoEnum(Name = "EnemyRoof", Value = 128)]
		EnemyRoof = 0x80,
		[ProtoEnum(Name = "ObjectiveMedium", Value = 256)]
		ObjectiveMedium = 0x100,
		[ProtoEnum(Name = "Captives", Value = 512)]
		Captives = 0x200,
		[ProtoEnum(Name = "Tail", Value = 1024)]
		Tail = 0x400
	}
}
