using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "EEventType")]
	public enum EEventType
	{
		[ProtoEnum(Name = "Event_Shop_Category_Visibility", Value = 0)]
		Event_Shop_Category_Visibility = 0,
		[ProtoEnum(Name = "Event_Shop_Ad_Banner", Value = 1)]
		Event_Shop_Ad_Banner = 1,
		[ProtoEnum(Name = "Event_Shop_Item_Percentage", Value = 2)]
		Event_Shop_Item_Percentage = 2,
		[ProtoEnum(Name = "Event_Shop_Category_Percentage", Value = 3)]
		Event_Shop_Category_Percentage = 3,
		[ProtoEnum(Name = "Event_Shop_Item_Visibility", Value = 4)]
		Event_Shop_Item_Visibility = 4,
		[ProtoEnum(Name = "Event_Shop_Hard_Currency_Payout_Increase", Value = 5)]
		Event_Shop_Hard_Currency_Payout_Increase = 5,
		[ProtoEnum(Name = "Event_Shop_Category_Payout_Increase", Value = 6)]
		Event_Shop_Category_Payout_Increase = 6,
		[ProtoEnum(Name = "Event_Progression_Challenges", Value = 7)]
		Event_Progression_Challenges = 7,
		[ProtoEnum(Name = "Event_Exclusive_Content", Value = 8)]
		Event_Exclusive_Content = 8,
		[ProtoEnum(Name = "Event_Global_Push_Notification", Value = 9)]
		Event_Global_Push_Notification = 9,
		[ProtoEnum(Name = "Event_Flavor", Value = 10)]
		Event_Flavor = 10
	}
}
