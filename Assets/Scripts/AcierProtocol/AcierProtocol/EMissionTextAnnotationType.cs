using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EMissionTextAnnotationType")]
	public enum EMissionTextAnnotationType
	{
		[ProtoEnum(Name = "Message", Value = 0)]
		Message = 0,
		[ProtoEnum(Name = "PopupConfirm", Value = 1)]
		PopupConfirm = 1,
		[ProtoEnum(Name = "PopupExternal", Value = 2)]
		PopupExternal = 2,
		[ProtoEnum(Name = "MessageExternal", Value = 3)]
		MessageExternal = 3,
		[ProtoEnum(Name = "Image", Value = 4)]
		Image = 4,
		[ProtoEnum(Name = "ImageExternal", Value = 5)]
		ImageExternal = 5
	}
}
