using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EEffectSpawnerSoundType")]
	public enum EEffectSpawnerSoundType
	{
		[ProtoEnum(Name = "None_EEffectSpawnerSoundType", Value = 0)]
		None_EEffectSpawnerSoundType = 0,
		[ProtoEnum(Name = "Object_EEffectSpawnerSoundType", Value = 1)]
		Object_EEffectSpawnerSoundType = 1,
		[ProtoEnum(Name = "Ground_EEffectSpawnerSoundType", Value = 2)]
		Ground_EEffectSpawnerSoundType = 2,
		[ProtoEnum(Name = "GroundRemove_EEffectSpawnerSoundType", Value = 3)]
		GroundRemove_EEffectSpawnerSoundType = 3
	}
}
