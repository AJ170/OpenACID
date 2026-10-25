using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EQualityShaderLOD")]
	public enum EQualityShaderLOD
	{
		[ProtoEnum(Name = "None_0", Value = 0)]
		None_0 = 0,
		[ProtoEnum(Name = "VertexLit_100", Value = 100)]
		VertexLit_100 = 100,
		[ProtoEnum(Name = "Diffuse_200", Value = 200)]
		Diffuse_200 = 200,
		[ProtoEnum(Name = "Bumped_300", Value = 300)]
		Bumped_300 = 300,
		[ProtoEnum(Name = "BumpedSpecular_400", Value = 400)]
		BumpedSpecular_400 = 400,
		[ProtoEnum(Name = "Maximum_500", Value = 500)]
		Maximum_500 = 500
	}
}
