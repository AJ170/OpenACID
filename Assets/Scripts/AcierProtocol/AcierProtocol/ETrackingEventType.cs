using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "ETrackingEventType")]
	public enum ETrackingEventType
	{
		[ProtoEnum(Name = "AppLaunch", Value = 0)]
		AppLaunch = 0,
		[ProtoEnum(Name = "Session", Value = 1)]
		Session = 1,
		[ProtoEnum(Name = "Screen", Value = 2)]
		Screen = 2,
		[ProtoEnum(Name = "AssetDownload", Value = 3)]
		AssetDownload = 3,
		[ProtoEnum(Name = "ShopScreen", Value = 4)]
		ShopScreen = 4,
		[ProtoEnum(Name = "InviteFriend", Value = 5)]
		InviteFriend = 5,
		[ProtoEnum(Name = "CtaPopupResult", Value = 6)]
		CtaPopupResult = 6,
		[ProtoEnum(Name = "RevivePopup", Value = 7)]
		RevivePopup = 7,
		[ProtoEnum(Name = "PushNotifications", Value = 8)]
		PushNotifications = 8,
		[ProtoEnum(Name = "Gamepad", Value = 9)]
		Gamepad = 9,
		[ProtoEnum(Name = "Sounds", Value = 10)]
		Sounds = 10,
		[ProtoEnum(Name = "ShopBundles", Value = 11)]
		ShopBundles = 11,
		[ProtoEnum(Name = "EventProgression", Value = 12)]
		EventProgression = 12
	}
}
